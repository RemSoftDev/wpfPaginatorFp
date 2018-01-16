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
    
    let private _NextState (pPaginatorState : PaginatorState) =
        pPaginatorState