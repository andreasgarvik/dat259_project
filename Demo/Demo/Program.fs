// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp
// Define a function to construct a message to print

open CPN
open Multiset
open TPC
open Evaluation

let rec loop (currentMarking: Marking) =
    let nextMarking = step currentMarking
    if (dead currentMarking nextMarking)
        then
            printfn $"Dead Marking \n {nextMarking}"
            0
    else
        if (stop nextMarking)
            then
                printfn $"Marking \n {nextMarking}"
                0
        else
            printfn $"Marking \n {nextMarking}"
            loop(nextMarking)


[<EntryPoint>]
let main _ =
    printfn $"Marking \n {initialMarking}"
    loop(initialMarking)