namespace Paginator

module CustomTypes = 
    type IntMore0Less65535Exclsv(pValue) = 
        let IsValid x  =
              if x > 0 
              then x
              else failwith "Out of range"

        member this.Value = IsValid pValue
        static member op_Explicit(source: int) =
            IntMore0Less65535Exclsv(source)

    type Person = {lat:int; long:int}

    type PaginatorState = 
         {NumberOfPages : IntMore0Less65535Exclsv
          CurrentPage   : IntMore0Less65535Exclsv
          ItemsPerPage  : IntMore0Less65535Exclsv
          PagesToSkip   : IntMore0Less65535Exclsv

          TotalNumberOfItemsInDB : int

          IsValidLeft      : bool
          IsValidLeftMore  : bool
          IsValidRight     : bool
          IsValidRightMore : bool

          DbData : List<int>

          PagesToShow : string }

open CustomTypes
              
module PaginatorScope =
    open System

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

          PagesToShow ="2" }
    
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
    