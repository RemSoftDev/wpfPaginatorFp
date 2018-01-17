// Learn more about F# at http://fsharp.org

open System
open Paginator.PaginatorScope
open Paginator.CustomTypes

[<EntryPoint>]
let main argv =
    printfn "Hello World from F#!"
    let sdf = Init( IntMore0Less65535Exclsv 1,  IntMore0Less65535Exclsv 2, IntMore0Less65535Exclsv 3, [0..110])
    let sdfa = sdf|>NextState
    let vc = sdfa.NumberOfPages.Value
    0 // return an integer exit code
