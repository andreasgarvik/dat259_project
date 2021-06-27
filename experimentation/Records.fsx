module CPN =

    type Multiset<'a> = list<int * 'a>

    let (^) a b = [ (a, b) ]
    let (+) ams bms = List.append ams bms

    type Colset = Colset of string

    type InitialMarking = InitialMarking of string

    // Can colset be interfered from initial marking?
    type Place =
        { name: string
          colset: Colset
          initialMarking: InitialMarking }

    type Transaction = { name: string }

    type Expression = Expression of string

    type Direction =
        | PT
        | TP

    type Arc =
        { place: Place
          transaction: Transaction
          expression: Expression
          direction: Direction }

    type Module =
        { name: string
          places: list<Place>
          transactions: list<Transaction>
          arcs: list<Arc> }

    // A CPN Model consists of one or more modules
    type Model = { name: string; modules: list<Module> }

module State =
    type Marking = list<CPN.Model>

let place : CPN.Place =
    { name = "Start"
      colset = CPN.Colset "INT"
      initialMarking = CPN.InitialMarking "1" }

let transaction : CPN.Transaction = { name = "Add" }

let arc1 : CPN.Arc =
    { place = place
      transaction = transaction
      expression = CPN.Expression "n"
      direction = CPN.PT }

let arc2 : CPN.Arc =
    { place = place
      transaction = transaction
      expression = CPN.Expression "n+1"
      direction = CPN.TP }

let addloop : CPN.Module =
    { name = "addloop"
      places = [ place ]
      transactions = [ transaction ]
      arcs = [ arc1; arc2 ] }

let simple : CPN.Model =
    { name = "simple"
      modules = [ addloop ] }

let marking : State.Marking = [ simple ]
