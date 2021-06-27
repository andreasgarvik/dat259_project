namespace TPC.Evaluations

open CPN

module Worker =

    // From declarations:
    let Wn = 2
    type Worker = W of int with // range as in index colour set?
        static member all () = [for i in 1 .. Wn -> W(i)]

    type Vote = Yes | No
    type WorkerVote = Worker * Vote

    type Decision = Abort | Commit
    type WorkerDecision = Worker * Decision

    // From syntax:
    // let ReceiveCanCommit = {
    //     name = "Receive CanCommit"
    //     vars = [ "w"; "vote"]
    //     guard = None
    // }
    // let ReceiveDecision = {
    //     name = "Receive Decision"
    //     vars = [ "w"; "decision"]
    //     guard = None
    // }

    type Transition = Transition of string
    //    static member enabled(Marking) -> Binding list
    //    syntactic analyse..
    //    static member occurrence(Marking) -> Marking
    //    syntactic analyse..

    // State
    type Marking = {
        WorkerIdle : Worker Multiset
        CanCommit : Worker Multiset;
        Votes : WorkerVote Multiset;
        WaitingDecision : Worker Multiset;
        Decision : WorkerDecision Multiset;
        Acknowledge : Worker Multiset;
    }

    type ReceiveCanCommit_Binding = { w: Worker option; vote : Vote option } // tilhører en transition
    type ReceiveDecision_Binding = { w: Worker option; decision : Decision option } // transition i constructor

    // Semantisk transition
    // Binding = Union over transitioner
    type Binding = | ReceiveCanCommit of ReceiveCanCommit_Binding
                   | ReceiveDecision of ReceiveDecision_Binding

    // Enablings function
    // Global enablings function state -> enabled bindings (Markings -> Binding list)
    // gå over alle transitioner

    // Global occurrence function: binding state (marking) -> state (marking)
    // let occurence (bining : Binding) (state : Marking) = state (marking)

    let initialMarking = {
        WorkerIdle =
    }