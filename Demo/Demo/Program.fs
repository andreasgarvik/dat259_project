// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp
// Define a function to construct a message to print

open TPC

[<EntryPoint>]
let main _ =
    let initialMarking = Evaluation.initialMarking
    printfn $"{initialMarking}"
    let nextMarking = Evaluation.nextMarking
    printfn $"{nextMarking}"
    0 // return an integer exit code