// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp
// Define a function to construct a message to print

open CPN
open Multiset
open TPC
open Evaluation

let rec loop (marking: Marking) =
    let marking = step marking
    if (stop marking)
        then
            printfn $"Marking \n {marking}"
            0
    else
        printfn $"Marking \n {marking}"
        loop(marking)


[<EntryPoint>]
let main _ =
    printfn $"Marking \n {initialMarking}"
    loop(initialMarking)