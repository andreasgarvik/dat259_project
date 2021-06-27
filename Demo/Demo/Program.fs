// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp
// Define a function to construct a message to print
let from whom =
    $"from %s{whom}"

[<EntryPoint>]
let main _ =
    let message = from "F#" // Call the function
    printfn $"Hello world %s{message}"
    0 // return an integer exit code