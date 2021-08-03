namespace TPC

open CPN
open Multiset

module Evaluation =

    // From definitions
    type Worker = W of int
    type Vote = Yes | No
    type WorkerVote = WorkerVote of Worker * Vote
    type Decision = Abort | Commit
    type WorkerDecision = WorkerDecision of  Worker * Decision

    // State
    [<StructuralEquality;StructuralComparison>]
    type Marking = {
        CoordinatorIdle : Unit Multiset
        WorkerIdle : Worker Multiset
        CanCommit : Worker Multiset
        WaitingVotes : Unit Multiset
        Votes : WorkerVote Multiset
        CollectedVotes : WorkerVote list Multiset
        WaitingDecision : Worker Multiset
        Decision : WorkerDecision Multiset
        WaitingAcknowledge : Worker list Multiset
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

    // Do some partial binding, combining and merging stuff...

    // Bindings in first marking: No enabled ??
    //let receiveCanCommit_BindingOfFirstMarking1 = { w = Some(W(1)); vote = None }
    //let receiveCanCommit_BindingOfFirstMarking2 = { w = Some(W(2)); vote = None }

    // Bindings in second marking: All enabled ??

    // From the fact that Vote is a constraint data type
    //let receiveCanCommit_BindingOfSecondMarking1 = { w = Some(W(1)); vote = Some(Yes) }
    //let receiveCanCommit_BindingOfSecondMarking2 = { w = Some(W(1)); vote = Some(No) }
    //let receiveCanCommit_BindingOfSecondMarking3 = { w = Some(W(2)); vote = Some(Yes) }
    //let receiveCanCommit_BindingOfSecondMarking4 = { w = Some(W(2)); vote = Some(No) }

    type SendCanCommit_Binding = { unit : unit } // Should this be a multiset? { unit : Multiset unit }
    type ReceiveCanCommit_Binding = { w : Worker; vote : Vote option }
    type CollectOneVote_Binding = { unit : unit; w : Worker; vote : Vote }
    type AllVoteCollected_Binding = { unit : unit; votes : Vote list; guard : Vote list }
    type ReceiveDecision_Binding = { w: Worker; decision : Decision option }
    type ReceiveAcknowledgements_Binding = { w: Worker; decision : Decision option }

    // let sendCanCommit_EnabledBindingOfFistMarking = { unit = () }

    // Semantic transition
    // Binding = Union of transitions
    type Binding = | SendCanCommit of SendCanCommit_Binding
                   | ReceiveCanCommit of ReceiveCanCommit_Binding
                   | CollectOneVote of CollectOneVote_Binding
                   | AllVoteCollected of AllVoteCollected_Binding
                   | ReceiveDecision of ReceiveDecision_Binding
                   | ReceiveAcknowledgements of ReceiveAcknowledgements_Binding

    let sendCanCommitEnabling (marking: Marking) =
        if 1^() <= marking.CoordinatorIdle
            then [SendCanCommit { unit = () }]
        else []
    let receiveCanCommitEnablingW1 (marking: Marking) =
        if (1^W(1) <= marking.CanCommit) && (1^W(1) <= marking.WorkerIdle)
            then [ReceiveCanCommit { w = W(1); vote = None }]
        else []
    let receiveCanCommitEnablingW2 (marking: Marking) =
        if (1^W(2) <= marking.CanCommit) && (1^W(2) <= marking.WorkerIdle)
            then [ReceiveCanCommit { w = W(2); vote = None }]
        else []

    let CollectOneVoteEnablingW1Yes (marking: Marking) =
        if marking.Votes <= 1^(WorkerVote(W(1),Yes))
            then [CollectOneVote { unit = (); w = W(1); vote = Yes }]
        else []
    let CollectOneVoteEnablingW1No (marking: Marking) =
        if marking.Votes <= 1^(WorkerVote(W(1),No))
            then [CollectOneVote { unit = (); w = W(1); vote = No }]
        else []
    let CollectOneVoteEnablingW2Yes (marking: Marking) =
        if marking.Votes <= 1^(WorkerVote(W(2),Yes))
            then [CollectOneVote { unit = (); w = W(2); vote = Yes }]
        else []
    let CollectOneVoteEnablingW2No (marking: Marking) =
        if marking.Votes <= 1^(WorkerVote(W(2),No))
            then [CollectOneVote { unit = (); w = W(2); vote = No }]
        else []

    // let workerIdle_receiveCanCommit {
    //     place = workerIdle
    //     transaction = receiveCanCommit
    //     expr = "w"
    //     direction = PT
    // }

    //let partialBind (arc : Arc) =

    let initialMarking = {
        CoordinatorIdle = 1^()
        WorkerIdle = (1^W(1)) + (1^W(2))
        CanCommit = empty
        WaitingVotes = empty
        Votes = empty
        CollectedVotes = 1^[]
        WaitingDecision = empty
        Decision = empty
        WaitingAcknowledge = empty
        Acknowledge = empty
    }

    // let enabling (transition: Transition, marking: Marking)
    // Global enabling function (state -> enabled bindings) (Markings -> Binding list)
    // goes through all transitions

    let enabling (marking: Marking) =
        sendCanCommitEnabling marking
        @ receiveCanCommitEnablingW1 marking
        @ receiveCanCommitEnablingW2 marking

    // let occurence (binding : Binding) (state : Marking) = state (marking)
    // Global occurrence function: binding state (marking) -> state (marking)
    // executes the enabled bindings

    let occurrence (marking: Marking) (binding: Binding)  =
        match binding with
        | SendCanCommit b -> { marking with CoordinatorIdle = marking.CoordinatorIdle - (1^b.unit); CanCommit = (1^W(1)) + (1^W(2)); WaitingVotes = 1^b.unit  }
        | ReceiveCanCommit b -> { marking with CanCommit = marking.CanCommit - (1^b.w); Votes = marking.Votes + (1^WorkerVote(b.w,Yes)) }
        | CollectOneVote b -> marking
        | AllVoteCollected b -> marking
        | ReceiveDecision b -> marking
        | ReceiveAcknowledgements b -> marking

    let step (marking: Marking) =
        let enabledBindings = enabling marking
        match enabledBindings.Length with
            | 0 -> marking
            | _ -> List.fold occurrence marking (List.take 1 enabledBindings)