open System

type NormalPlace = { name: string }
type CustomPlace = { name: string; custom: string }

type Place =
    | NormalPlace of NormalPlace
    | CustomPlace of CustomPlace

let np : NormalPlace = { name = "Name of NormalPlace" }

let cp : CustomPlace =
    { name = "Name of CustomPlace"
      custom = "Custom" }

let l : list<Place> = [ NormalPlace np; CustomPlace cp ]

let getName p =
    match p with
    | NormalPlace { name = n } -> n
    | CustomPlace { name = n; custom = c } -> n

for p in l do
    Console.WriteLine(getName p)
