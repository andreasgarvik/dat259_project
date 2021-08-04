// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp
// Define a function to construct a message to print

open TPC
open Evaluation

let rec loop (marking: Marking, n: int) =
    let marking = step marking
    if (stop marking)
        then
            printfn $"{marking}"
            0
    else
        printfn $"{marking}"
        loop(marking, n+1)


[<EntryPoint>]
let main _ =
    printfn $"{initialMarking}"
    loop(initialMarking, 0)