namespace Paginator

module PaginatorScope =
    open System
    open Paginator.Types
    
    let private GetNumberOfPages pState =
        let tmp = System.Math.Round( double pState.TotalNumberOfItemsInDB/double pState.ItemsPerPage.Value, MidpointRounding.AwayFromZero) |> uint16;
        {pState with NumberOfPages = IntMore0Less65535Exclsv tmp}

    let private IsValidLeft pState = 
        {pState with IsValidLeft = pState.CurrentPage.Value > 1us}

    let private IsValidLeftMore pState = 
        let tmp = if pState.PagesToSkip.Value > pState.CurrentPage.Value 
                    then false 
                  elif pState.CurrentPage.Value - pState.PagesToSkip.Value > 0us
                    then true
                  else false
       
        {pState with IsValidLeftMore = tmp}

    let private IsValidRight pState = 
        {pState with IsValidRight = pState.CurrentPage.Value < pState.NumberOfPages.Value}

    let private IsValidRightMore pState = 
        {pState with IsValidRightMore = pState.CurrentPage.Value + pState.PagesToSkip.Value < pState.NumberOfPages.Value}

    let private GetLeftIndex pState = 
        let tmp = ((pState.CurrentPage.Value - 1us) * pState.ItemsPerPage.Value) |> uint32
        {pState with LeftIndexInclsv  = tmp}

    let private GetRightIndex pState = 
        let tmp = (pState.CurrentPage.Value * pState.ItemsPerPage.Value  - 1us) |> uint32
        {pState with RightIndexInclsv  = tmp }

    let private  NextState pState =
         pState
         |> GetNumberOfPages  
         |> IsValidLeft  
         |> IsValidLeftMore  
         |> IsValidRight  
         |> IsValidRightMore
         |> GetLeftIndex
         |> GetRightIndex
   
    let Init pCurrentPage
             pItemsPerPageList
             pPagesToSkipList
             pTotalNumberOfItemsInDB =

         NextState   {NumberOfPages  = IntMore0Less65535Exclsv(1)
                      CurrentPage    = pCurrentPage
                      ItemsPerPage   = pItemsPerPageList
                      PagesToSkip    = pPagesToSkipList

                      TotalNumberOfItemsInDB = pTotalNumberOfItemsInDB
                      LeftIndexInclsv  = 0u
                      RightIndexInclsv  = uint32 (pItemsPerPageList.Value - 1us)

                      IsValidLeft      = false
                      IsValidLeftMore  = false
                      IsValidRight     = false
                      IsValidRightMore = false}

    let GoRight pState = 
        NextState {pState with CurrentPage = IntMore0Less65535Exclsv (pState.CurrentPage.Value + 1us)}

    let GoLeft pState = 
        NextState {pState with CurrentPage = IntMore0Less65535Exclsv (pState.CurrentPage.Value - 1us)}

    let GoRightMore pState = 
        NextState {pState with CurrentPage = IntMore0Less65535Exclsv (pState.CurrentPage.Value + pState.PagesToSkip.Value)}

    let GoLeftMore pState = 
        NextState {pState with CurrentPage = IntMore0Less65535Exclsv (pState.CurrentPage.Value - pState.PagesToSkip.Value)}   