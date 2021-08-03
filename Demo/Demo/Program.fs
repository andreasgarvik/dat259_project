// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp
// Define a function to construct a message to print

open TPC

let rec loop (marking: Evaluation.Marking) =
    let newMarking = Evaluation.step marking
    if (newMarking = marking)
        then 0
    else
        printfn $"{newMarking}"
        loop(newMarking)


[<EntryPoint>]
let main _ =
    let initialMarking = Evaluation.initialMarking
    printfn $"{initialMarking}"
    loop(Evaluation.initialMarking)