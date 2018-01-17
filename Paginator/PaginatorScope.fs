namespace Paginator

module PaginatorScope =
    open System
    open Paginator.Types

    let Init (pCurrentPage     : IntMore0Less65535Exclsv, 
              pItemsPerPageList: IntMore0Less65535Exclsv, 
              pPagesToSkipList : IntMore0Less65535Exclsv, 
              pDbData          : List<int>) =

         {NumberOfPages  = IntMore0Less65535Exclsv(1)
          CurrentPage    = pCurrentPage
          ItemsPerPage   = pItemsPerPageList
          PagesToSkip    = pPagesToSkipList

          TotalNumberOfItemsInDB = 1

          IsValidLeft      = false
          IsValidLeftMore  = false
          IsValidRight     = false
          IsValidRightMore = false

          DbData = pDbData

          PagesToShow ="1" }
    
    let private _GetTotalNumberOfItemsInDB pState = 
        {pState with TotalNumberOfItemsInDB = pState.DbData.Length}

    let private _GetNumberOfPages pState =
        let divide = System.Math.Round( double pState.TotalNumberOfItemsInDB/double pState.ItemsPerPage.Value, MidpointRounding.AwayFromZero) |> uint16;
        {pState with NumberOfPages = IntMore0Less65535Exclsv divide}

    let private _IsValidLeft pState = 
        {pState with IsValidLeft = pState.CurrentPage.Value > 1us}

    let private _IsValidLeftMore pState = 
        {pState with IsValidLeftMore = pState.CurrentPage.Value - pState.PagesToSkip.Value > 0us}

    let private _IsValidRight pState = 
        {pState with IsValidRight = pState.CurrentPage.Value < pState.NumberOfPages.Value}

    let private _IsValidRightMore pState = 
        {pState with IsValidRightMore = pState.CurrentPage.Value + pState.PagesToSkip.Value < pState.NumberOfPages.Value}

    let private GetLeftIndex pState = 
        ((pState.CurrentPage.Value - 1us) * pState.ItemsPerPage.Value) |> uint32

    let private GetRightIndex pState = 
        {pState with IsValidLeft = pState.CurrentPage.Value > 1us}

    let private GetDataStartEndIndex pState = 
        {pState with IsValidLeft = pState.CurrentPage.Value > 1us}

    let  NextState pState =
         pState
         |> _GetTotalNumberOfItemsInDB  
         |> _GetNumberOfPages  
         |> _IsValidLeft  
         |> _IsValidLeftMore  
         |> _IsValidRight  
         |> _IsValidRightMore  
    