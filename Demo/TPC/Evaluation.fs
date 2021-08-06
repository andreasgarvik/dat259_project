namespace TPC

open CPN
open Multiset

module Evaluation =

    // From definitions
    let W = 2
    type Worker = int
    type Vote = Yes | No
    type WorkerVote = Worker * Vote
    type Decision = Abort | Commit
    type WorkerDecision = Worker * Decision

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

    type SendCanCommit_Binding = unit
    type ReceiveCanCommit_Binding = { w : Worker; vote : Vote }
    type CollectOneVote_Binding = { workerVote: WorkerVote }
    type AllVoteCollected_Binding = { votes : WorkerVote list; }
    type ReceiveDecision_Binding = { workerDecision : WorkerDecision }
    type ReceiveAcknowledgements_Binding = unit

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
            then [SendCanCommit ()]
        else []
    let receiveCanCommitEnablingW1 (marking: Marking) =
        if (1^1 <= marking.CanCommit) && (1^1 <= marking.WorkerIdle)
            then [ReceiveCanCommit { w = 1; vote = Yes }; ReceiveCanCommit { w = 1; vote = No }]
        else []
    let receiveCanCommitEnablingW2 (marking: Marking) =
        if (1^2 <= marking.CanCommit) && (1^2 <= marking.WorkerIdle)
            then [ReceiveCanCommit { w = 2; vote = Yes }; ReceiveCanCommit { w = 2; vote = No }]
        else []

    let collectOneVoteEnablingW1Yes (marking: Marking) =
        if 1^(1,Yes) <= marking.Votes
            then [CollectOneVote { workerVote = (1,Yes) }]
        else []
    let collectOneVoteEnablingW1No (marking: Marking) =
        if 1^(1,No) <= marking.Votes
            then [CollectOneVote { workerVote = (1,No) }]
        else []
    let collectOneVoteEnablingW2Yes (marking: Marking) =
        if 1^(2,Yes) <= marking.Votes
            then [CollectOneVote { workerVote = (2,Yes) }]
        else []
    let collectOneVoteEnablingW2No (marking: Marking) =
        if 1^(2,No) <= marking.Votes
            then [CollectOneVote { workerVote = (2,No) }]
        else []
    let allVoteCollectedEnablingYesYes (marking: Marking) =
        if ([(1,Yes); (2,Yes)] |> List.sort) = (toList marking.CollectedVotes |> List.sort)
            then [AllVoteCollected { votes = [(1,Yes); (2,Yes)] }]
        else []
    let allVoteCollectedEnablingYesNo (marking: Marking) =
        if ([(1,Yes); (2,No)] |> List.sort) = (toList marking.CollectedVotes |> List.sort)
            then [AllVoteCollected { votes = [(1,Yes); (2,No)] }]
        else []
    let allVoteCollectedEnablingNoYes (marking: Marking) =
        if ([(1,No); (2,Yes)] |> List.sort) = (toList marking.CollectedVotes |> List.sort)
            then [AllVoteCollected { votes = [(1,No); (2,Yes)] }]
        else []
    let allVoteCollectedEnablingNoNo (marking: Marking) =
        if ([(1,No); (2,No)] |> List.sort) = (toList marking.CollectedVotes |> List.sort)
            then [AllVoteCollected { votes = [(1,No); (2,No)] }]
        else []
    let receiveDecisionEnabledW1Commit (marking: Marking) =
        if 1^(1,Commit) <= marking.Decision
            then [ReceiveDecision { workerDecision = (1,Commit) }]
        else []
    let receiveDecisionEnabledW2Commit (marking: Marking) =
        if 1^(2,Commit) <= marking.Decision
            then [ReceiveDecision { workerDecision = (2,Commit) }]
        else []
    let receiveDecisionEnabledW1Abort (marking: Marking) =
        if 1^(1,Abort) <= marking.Decision
            then [ReceiveDecision { workerDecision = (1,Abort) }]
        else []
    let receiveDecisionEnabledW2Abort (marking: Marking) =
        if 1^(2,Abort) <= marking.Decision
            then [ReceiveDecision { workerDecision = (2,Abort) }]
        else []
    let receiveAcknowledgementsEnabling (marking: Marking) =
        if (
            (marking.WaitingAcknowledge |> isEmpty |> not) && // This is temp to get my poor logic to work, might not ne needed
            (toList marking.WaitingAcknowledge |> List.fold (fun acc w -> acc + (1^w)) empty ) <= marking.Acknowledge)
            then [ReceiveAcknowledgements ()]
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
        WorkerIdle = (1^1) + (1^2)
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
        @ collectOneVoteEnablingW1Yes marking
        @ collectOneVoteEnablingW1No marking
        @ collectOneVoteEnablingW2Yes marking
        @ collectOneVoteEnablingW2No marking
        @ allVoteCollectedEnablingYesYes marking
        @ allVoteCollectedEnablingYesNo marking
        @ allVoteCollectedEnablingNoYes marking
        @ allVoteCollectedEnablingNoNo marking
        @ receiveDecisionEnabledW1Commit marking
        @ receiveDecisionEnabledW2Commit marking
        @ receiveDecisionEnabledW1Abort marking
        @ receiveDecisionEnabledW2Abort marking
        @ receiveAcknowledgementsEnabling marking

    // let occurence (binding : Binding) (state : Marking) = state (marking)
    // Global occurrence function: binding state (marking) -> state (marking)
    // executes the enabled bindings

    let occurrence (marking: Marking) (binding: Binding)  =
        printfn $"Occurrence of binding {binding}"
        match binding with
        | SendCanCommit _ -> {
            marking with
                CoordinatorIdle = marking.CoordinatorIdle - (1^()) // Good enough with empty here: CoordinatorIdle = empty ?
                CanCommit = (1^1) + (1^2)
                WaitingVotes = 1^() }
        | ReceiveCanCommit b -> {
            marking with
                CanCommit = marking.CanCommit - (1^b.w)
                Votes = marking.Votes + (1^(b.w,b.vote))
                WaitingDecision = marking.WaitingDecision + if (b.vote = Yes) then 1^b.w else empty}
        | CollectOneVote b -> {
            marking with
                Votes = marking.Votes - (1^b.workerVote)
                CollectedVotes = 1^b.workerVote :: (toList marking.CollectedVotes) }
        | AllVoteCollected b -> {
            marking with
                WaitingVotes = marking.WaitingVotes - (1^())
                CollectedVotes = 1^[]
                WaitingAcknowledge = 1^(List.filter (fun (_,vote) -> vote = Yes) b.votes |> List.map fst)
                Decision =
                    let yesWorkers = List.filter (fun (_,vote) -> vote = Yes) b.votes |> List.map fst
                    let decision = if ((yesWorkers |> List.length) = W) then Commit else Abort
                    List.fold (fun acc w ->  acc + (1^(w, decision))) empty yesWorkers }
        | ReceiveDecision b -> {
            marking with
                Decision = marking.Decision - (1^b.workerDecision)
                Acknowledge = marking.Acknowledge + (1^fst b.workerDecision)
                WaitingDecision = empty }
        | ReceiveAcknowledgements _ -> {
            marking with
                WaitingAcknowledge = empty // Good enough with empty here or pass what to remove from enable binding function ?
                Acknowledge = empty
                CoordinatorIdle = 1^()
        }

    let randomEnabledBinding enabledBindings = List.item (System.Random().Next(0, List.length enabledBindings)) enabledBindings

    let step (marking: Marking) =
        let enabledBindings = enabling marking
        printfn $"Enabled Bindings {enabledBindings}"
        match enabledBindings.Length with
            | 0 -> marking
            | _ -> enabledBindings |> randomEnabledBinding |> occurrence marking
    let stop (marking: Marking) = 1^() <= marking.CoordinatorIdle