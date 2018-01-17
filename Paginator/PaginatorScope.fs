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
        let divide = System.Math.Round((pState.TotalNumberOfItemsInDB/pState.ItemsPerPage.Value) |> double, MidpointRounding.AwayFromZero) |> int;
        {pState with NumberOfPages = IntMore0Less65535Exclsv divide}

    let private _IsValidLeft pState = 
        {pState with IsValidLeft = pState.CurrentPage.Value > 1}

    let private _IsValidLeftMore pState = 
        {pState with IsValidLeftMore = pState.CurrentPage.Value - pState.PagesToSkip.Value > 0}

    let private _IsValidRight pState = 
        {pState with IsValidRight = pState.CurrentPage.Value < pState.NumberOfPages.Value}

    let private _IsValidRightMore pState = 
        {pState with IsValidRightMore = pState.CurrentPage.Value + pState.PagesToSkip.Value < pState.NumberOfPages.Value}

    let  NextState pState =
         pState
         |> _GetTotalNumberOfItemsInDB  
         |> _GetNumberOfPages  
         |> _IsValidLeft  
         |> _IsValidLeftMore  
         |> _IsValidRight  
         |> _IsValidRightMore  
    