module Tokens =
    type 'a multiset = (int * 'a) list

    let emptyms = []

    let (^) a b = [ (a, b) ]
    let (+) ams bms = List.append ams bms

// colour set / type definitions for the CPN model

let Wn = 2

type Worker =
    | W of int
    static member all() = [ for i in 1 .. Wn -> W(i) ]

type Vote =
    | Yes
    | No

type WorkerxVote = Worker * Vote

type Decision =
    | Abort
    | Commit

type WorkerxDecision = Worker * Decision


// should variables be declared or just derived from the arc expressions
// via the use of compiler internals and traversal of the AST?

// generate code by embedding the F# interactive?
// multi-sets - to be used as markings and result of evaluating expressions

// use classes for structs

module CPN =

    type Colset = Colset of string
    type Var = Var of string
    type Expr = Expr of string
    type Guard = Guard of string
    type Marking = Marking of string

    type Place(name: string, colset: Colset, initialmark: Marking) =
        member this.name = name
        member this.colset = colset
        member this.initialmark = initialmark

    // just to check inheritance - argument for using classes instead of records
    type QueuePlace(name: string, colset: Colset, initialmark: Marking) =
        inherit Place(name, colset, initialmark)

    type Transition(name: string, vars: Var list, guard: Guard option) =
        member this.name = name
        member this.guard = guard
        member this.vars = vars

    type ArcDirection =
        | PT
        | TP // TODO: add double arcs later

    // hellere bruke arv her til de ulike Arcs
    type Arc(place: Place, transition: Transition, expr: Expr, dir: ArcDirection) =
        member this.expr = expr
        member this.place = place
        member this.transition = transition
        member this.dir = dir

    type Module(name: string, places: Place list, transitions: Transition list, arcs: Arc list) =
        member this.name = name
        member this.places = places
        member this.transitions = transitions
        member this.arcs = arcs

    let getPlaces (m: Module) = m.places

// type Place ()= { name : string; colset : string; initialmark : string}
// type transition = {name : string; guard : string}
// type arc = with PTarc of place * transition

// syntax checking phase by generating text code from text and use type system

// bruke mellomliggende abstrakt klasse for plasser of transitions som så
// konkretisere videre via arv basert på generert kode
// spåesisalier transiton med bindings type, variable
// speaisliere plass med current marking?
// record type for merkning basert på plasser?

// TODO: implement CPN module generator
module CPNModule =

    let generate (m: CPN.Module) = ""

// Worker module - generated from syntactical information

// first generate a worker syntactical module

// based on this generate binding and occurrence functions?

module Worker =

    let places =
        new CPN.Place("Idle", CPN.Colset("Worker"), CPN.Marking("Worker.all()"))

    // will this place type be needed?
    type Place =
        | Idle of Worker Tokens.multiset
        | WaitingDecision of CPN.Place

    type Marking =
        { Idle: Worker Tokens.multiset
          CanCommit: Worker Tokens.multiset
          Votes: WorkerxVote Tokens.multiset
          Waitingdecision: Worker Tokens.multiset
          Decision: WorkerxDecision Tokens.multiset
          Acknowledge: Worker Tokens.multiset }

    type ReceiveCanCommit_binding = { w: Worker option; vote: Vote option }

    type ReceiveDecision_binding =
        { w: Worker option
          decision: Decision option }

    type Transition =
        | ReceiveCanCommit of ReceiveCanCommit_binding
        | ReceiveDecision of ReceiveDecision_binding


    let execute (transition: Transition) (marking: Marking) = marking
// enabled : Transition ->
// type transition = {name : string; guard : }
// https://en.wikibooks.org/wiki/F_Sharp_Programming/Advanced_Data_Structures
// type 'a ms = (int * 'a) list with
//     static member empty = ([] : 'a list

open Tokens

let x = 1 ^ 2
let v = W(1)
let vs = Worker.all ()
let ms = emptyms

let mss : string multiset = (1 ^ "xx") + (1 ^ "yy")
