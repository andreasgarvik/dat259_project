namespace CPN

// let place = ["Worker Idle"; "Worker"; "Worker.all()"]

// let place = {
//      name = "Worker Idle"
//      colset = "Worker"
//      initialMarking = "Worker.all()"
// }

type Place = {
    name : string
    colset : string
    initialMarking : string
}

// let transition = ["Receive CanCommit"; ["w", "vote"] ]

// let transition = {
//      name = "Receive CanCommit"
//      vars = ["w", "vote"]
// }

type Transition = {
    name : string
    vars : string list
    guard : string option
}

type Direction =
    | PT
    | TP

// let arc  = ["Worker Idle"; "Receive CanCommit"; "w"; "PT" ]

// let arc  = {
//      place = { name = "Worker Idle"; colset = "Worker"; initialMarking = "Worker.all()" }
//      transaction = { name = "Receive CanCommit"; vars = ["w", "vote"] }
//      expr = "w"
//      direction = "PT"
// }

type Arc = {
    place : Place
    transaction : Transition
    expr : string
    direction : Direction
}

// let Worker = {
//    name = "Worker"
//    places = Places
//    transitions = Transition
//    arcs = Arcs
// }

type Module = {
    name : string
    places : Place list
    transitions : Transition list
    arcs : Arc list
}