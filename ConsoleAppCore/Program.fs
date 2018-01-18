// Learn more about F# at http://fsharp.org

open System
open Paginator.PaginatorScope
open Paginator.Types

[<EntryPoint>]
let main argv =
    printfn "Hello World from F#!"
    let sdf = Init (IntMore0Less65535Exclsv 1)  (IntMore0Less65535Exclsv 10) (IntMore0Less65535Exclsv 2) 100u
    let sdfa = sdf|>GoRight

    let rangeTest testValue mid size =
        match testValue with
        | var1 when var1 >= mid - size/2 && var1 <= mid + size/2 -> printfn "The test value is in range. %d" var1
        | _ -> printfn "The test value is out of range."

    rangeTest 10 20 5
    rangeTest 11 20 10
    rangeTest 12 20 40
    0 // return an integer exit code
