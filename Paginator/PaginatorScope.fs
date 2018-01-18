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
        let tmp = if pState.CurrentPage.Value > pState.PagesToSkip.Value && pState.CurrentPage.Value - pState.PagesToSkip.Value > 0us
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
    
    let private IsValidItemsPerPage pState = 
        if uint32 pState.ItemsPerPage.Value < pState.TotalNumberOfItemsInDB
            then pState
        else
            failwith "Incorect number of items per page"

    let private IsValidPagesToSkip pState = 
        if uint32 pState.ItemsPerPage.Value < pState.TotalNumberOfItemsInDB
            then pState
        else
            failwith "Incorect number of pages to skip"

    let private  NextState pState =
         pState
         |> IsValidItemsPerPage  
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
        match pState.IsValidRight with 
        | true -> NextState {pState with CurrentPage = IntMore0Less65535Exclsv (pState.CurrentPage.Value + 1us)}
        | false -> failwith "Cant change page to GoRight"      

    let GoLeft pState = 
        match pState.IsValidLeft with 
        | true -> NextState {pState with CurrentPage = IntMore0Less65535Exclsv (pState.CurrentPage.Value - 1us)}
        | false -> failwith "Cant change page to GoLeft"           

    let GoRightMore pState = 
          match pState.IsValidRightMore with 
        | true -> NextState {pState with CurrentPage = IntMore0Less65535Exclsv (pState.CurrentPage.Value + pState.PagesToSkip.Value)}
        | false -> failwith "Cant change page to GoRightMore"     
        
    let GoLeftMore pState = 
        match pState.IsValidLeftMore with 
        | true -> NextState {pState with CurrentPage = IntMore0Less65535Exclsv (pState.CurrentPage.Value - pState.PagesToSkip.Value)}  
        | false -> failwith "Can't change page to GoLeftMore"     
         