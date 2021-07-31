namespace TPC

open CPN
open Multiset

module Evaluation =

    // From definitions:
    type Worker = W of int with // range as in index colour set?
        static member empty = [] // Should this be here?
        static member all = [for i in 1 .. 2 -> W(i)]

    type Vote = Yes | No
    type WorkerVote = WorkerVote of Worker * Vote with
        static member empty = []

    type Decision = Abort | Commit
    type WorkerDecision = WorkerDecision of  Worker * Decision with
        static member empty = []

    // State
    type Marking = {
        CoordinatorIdle : Unit Multiset
        WorkerIdle : Worker Multiset
        CanCommit : Worker Multiset
        Votes : WorkerVote Multiset
        WaitingDecision : Worker Multiset
        Decision : WorkerDecision Multiset
        Acknowledge : Worker Multiset
    }

    // From syntax:
    // let ReceiveCanCommit = {
    //      name = "Receive CanCommit"
    //      vars = [ "w"; "vote"]
    //      guard = None
    // }

    // let ReceiveDecision = {
    //     name = "Receive Decision"
    //     vars = [ "w"; "decision"]
    //     guard = None
    // }

    // type Transition =
    //      for computing the enabled bindings of the transition in a given marking
    //      static member enabled(Marking) -> Binding list
    //      syntactic analyse..
    //      for computing the marking resulting from an occurrence of the transition in the given marking.
    //      static member occurrence(Marking) -> Marking
    //      syntactic analyse..


    //let patternBindingBasis (transition: Transition) =

    type ReceiveCanCommit_Binding = { w : Worker option; vote : Vote option }

    // Do some partial binding, combining and merging stuff...

    // Bindings in first marking: No enabled ??
    let receiveCanCommit_BindingOfFirstMarking1 = { w = Some(W(1)); vote = None }
    let receiveCanCommit_BindingOfFirstMarking2 = { w = Some(W(2)); vote = None }

    // Bindings in second marking: All enabled ??

    // From the fact that Vote is a constraint data type
    let receiveCanCommit_BindingOfSecondMarking1 = { w = Some(W(1)); vote = Some(Yes) }
    let receiveCanCommit_BindingOfSecondMarking2 = { w = Some(W(1)); vote = Some(No) }
    let receiveCanCommit_BindingOfSecondMarking3 = { w = Some(W(2)); vote = Some(Yes) }
    let receiveCanCommit_BindingOfSecondMarking4 = { w = Some(W(2)); vote = Some(No) }

    type ReceiveDecision_Binding = { w: Worker; decision : Decision option }

    type SendCanCommit_Binding = { unit : unit } //??

    // let sendCanCommit_EnabledBindingOfFistMarking = { unit = () }

    // Semantic transition
    // Binding = Union of transitions
    type Transition = | SendCanCommit of SendCanCommit_Binding
                      | ReceiveCanCommit of ReceiveCanCommit_Binding
                      | ReceiveDecision of ReceiveDecision_Binding

    let sendCanCommitEnabling (marking: Marking) = if marking.CoordinatorIdle <= (empty) then [SendCanCommit { unit = () }] else []
    let t = (1^"2") + (1^"2")

    // let workerIdle_receiveCanCommit {
    //     place = workerIdle
    //     transaction = receiveCanCommit
    //     expr = "w"
    //     direction = PT
    // }

    //let partialBind (arc : Arc) =

    let enabling (transition: Transition, marking: Marking) =

    // Enabling function
    // Global enabling function (state -> enabled bindings) (Markings -> Binding list)
    // goes through all transitions

    // Global occurrence function: binding state (marking) -> state (marking)
    // let occurence (binding : Binding) (state : Marking) = state (marking)

    let firstMarking = {
        CoordinatorIdle = Multiset.create [()]
        WorkerIdle = Multiset.create Worker.all
        CanCommit = Multiset.empty
        Votes = Multiset.create WorkerVote.empty
        WaitingDecision = Multiset.create Worker.empty
        Decision = Multiset.create WorkerDecision.empty
        Acknowledge = Multiset.create Worker.empty
    }

    let secondMarking = {
        CoordinatorIdle = Multiset.create []
        WorkerIdle = Multiset.create Worker.all
        CanCommit = Multiset.create Worker.all
        Votes = Multiset.create WorkerVote.empty
        WaitingDecision = Multiset.create Worker.empty
        Decision = Multiset.create WorkerDecision.empty
        Acknowledge = Multiset.create Worker.empty
    }