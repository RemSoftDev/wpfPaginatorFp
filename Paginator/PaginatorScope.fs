namespace Paginator

module PaginatorScope =
    open System
    open Paginator.Types
    
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
        let tmp = uint32 ((pState.CurrentPage.Value - 1us) * pState.ItemsPerPage.Value) 
        {pState with LeftIndex = tmp}

    let private GetRightIndex pState = 
        let tmp = uint32 (pState.CurrentPage.Value * pState.ItemsPerPage.Value)
        {pState with RightIndex = tmp }

    let  NextState pState =
         pState
         |> _GetNumberOfPages  
         |> _IsValidLeft  
         |> _IsValidLeftMore  
         |> _IsValidRight  
         |> _IsValidRightMore
         |> GetLeftIndex
         |> GetRightIndex
   
    let Init (pCurrentPage     : IntMore0Less65535Exclsv, 
              pItemsPerPageList: IntMore0Less65535Exclsv, 
              pPagesToSkipList : IntMore0Less65535Exclsv, 
              pTotalNumberOfItemsInDB : int) =

         NextState   {NumberOfPages  = IntMore0Less65535Exclsv(1)
                      CurrentPage    = pCurrentPage
                      ItemsPerPage   = pItemsPerPageList
                      PagesToSkip    = pPagesToSkipList

                      TotalNumberOfItemsInDB = pTotalNumberOfItemsInDB
                      LeftIndex = 1u
                      RightIndex = uint32 pItemsPerPageList.Value

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

    type PaginatorState with
       member this.GoRight() = 
            NextState {this with CurrentPage = IntMore0Less65535Exclsv (this.CurrentPage.Value + 1us)}

    type state = { TotalItems : uint16; ItemsPerPage : uint16; CurrentIndex : uint16; }

    let init totalItems itemsPerPage = { TotalItems = totalItems; ItemsPerPage = itemsPerPage; CurrentIndex = 0us; }

    let nextPage state =
        if state.CurrentIndex + state.ItemsPerPage = state.TotalItems 
        then state
        else { state with CurrentIndex = state.CurrentIndex + state.ItemsPerPage }
  
    let goRight ((count, perPage), (currentPage, (currentItems : int list))) =
        let nextItemIndex = currentPage * perPage - 1
        ((count, perPage), (currentPage + 1, (if count = currentItems.[perPage - 1] then failwith "Error!" else [nextItemIndex..(nextItemIndex + perPage - 1)])))

    init 10us 2us 
    |> nextPage
    |> nextPage
    |> printf "%A"

    ((10, 2), (1, [0;1]))
    |> goRight
    |> goRight
    |> printf "%A"