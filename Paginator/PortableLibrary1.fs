namespace Paginator

exception InnerError of string

type IntMore0Less65535Exclsv(pValue) = 
    
    member this.Value = 
        if pValue > 0 || pValue< 65535
        then pValue
        else raise(InnerError("qwe"))
        

