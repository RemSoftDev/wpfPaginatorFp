// Learn more about F# at http://fsharp.org

open System
open Paginator.PaginatorScope
open Paginator.Types

[<EntryPoint>]
let main argv =
    printfn "Hello World from F#!"
    let sdf = Init (IntMore0Less65535Exclsv 1)  (IntMore0Less65535Exclsv 10) (IntMore0Less65535Exclsv 2) 100
    let sdfa = sdf|>GoRight
    let vc = sdfa.NumberOfPages.Value
    let zz = 1-2 > 0
    0 // return an integer exit code
