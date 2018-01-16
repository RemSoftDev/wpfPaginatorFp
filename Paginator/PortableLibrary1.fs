﻿namespace Paginator

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
        { NumberOfPages : IntMore0Less65535Exclsv
          CurrentPage   : IntMore0Less65535Exclsv
          ItemsPerPage  : IntMore0Less65535Exclsv
          PagesToSkip   : IntMore0Less65535Exclsv

          TotalNumberOfItemsInDB : int

          IsValidLeft      : bool
          IsValidLeftMore  : bool
          IsValidRight     : bool
          IsValidRightMore : bool

          DbData : string

          PagesToShow : string }

open CustomTypes
              
module PaginatorScope =
    let Init (pCurrentPage:IntMore0Less65535Exclsv, 
              pItemsPerPageList: IntMore0Less65535Exclsv, 
              pPagesToSkipList: IntMore0Less65535Exclsv, 
              pDbData: string) =
         {NumberOfPages = IntMore0Less65535Exclsv(1)
          CurrentPage    = IntMore0Less65535Exclsv(1)
          ItemsPerPage   = IntMore0Less65535Exclsv(1)
          PagesToSkip    = IntMore0Less65535Exclsv(1)

          TotalNumberOfItemsInDB =1

          IsValidLeft      = false
          IsValidLeftMore  = false
          IsValidRight     = false
          IsValidRightMore = false

          DbData ="1"

          PagesToShow ="2" }
    

