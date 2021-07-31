// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp
// Define a function to construct a message to print

open CPN
open Multiset


[<EntryPoint>]
let main _ =
    let ms = 1^"Hello"
    printfn $"{ms}"
    let msplus = ms + ((1^"Hello") + (2^"World"))
    printfn $"{msplus}"
    let msminus = msplus - ((1^"Hello") + (1^"World"))
    printfn $"{msminus}"
    let mscompareTrue = msminus <= ((1^"Hello") + (1^"World"))
    printfn $"{mscompareTrue}"
    let mscompareFalse = msminus <= (1^"Hello")
    printfn $"{mscompareFalse}"
    0 // return an integer exit code