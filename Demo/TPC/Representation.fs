namespace TPC

open CPN

module Representation =
    let coordinatorIdle = {
        name = "CoordinatorIdle"
        colset = "Unit"
        initialMarking = "()"
    }
    let workerIdle = {
        name = "WorkerIdle"
        colset = "Worker"
        initialMarking = "Worker.all()"
    }
    let canCommit = {
        name = "CanCommit"
        colset = "Worker"
        initialMarking = ""
    }
    let waitingVotes = {
        name = "WaitingVotes"
        colset = "()"
        initialMarking = ""
    }
    let votes = {
        name = "Votes"
        colset = "WorkerxVote"
        initialMarking = ""
    }
    let collectedVotes = {
        name = "CollectedVotes"
        colset = "WorkerxVotes"
        initialMarking = "[]"
    }
    let waitingDecision = {
        name = "WaitingDecision"
        colset = "Worker"
        initialMarking = ""
    }
    let decision = {
        name = "Decision"
        colset = "WorkerxDecision"
        initialMarking = ""
    }
    let waitingAcknowledgements = {
        name = "WaitingAcknowledgements"
        colset = "Workers"
        initialMarking = ""
    }
    let acknowledge = {
        name = "Acknowledge"
        colset = "Worker"
        initialMarking = ""
    }
    let Places = [
        coordinatorIdle
        workerIdle
        canCommit
        waitingVotes
        votes
        collectedVotes
        waitingDecision
        decision
        waitingAcknowledgements
        acknowledge
    ]

    let sendCanCommit = {
        name = "SendCanCommit"
        vars = []
        guard = None
    }
    let receiveCanCommit = {
        name = "ReceiveCanCommit"
        vars = [ "w"; "vote"]
        guard = None
    }

    let receiveDecision = {
        name = "ReceiveDecision"
        vars = [ "w"; "decision"]
        guard = None
    }

    let Transition = [
        sendCanCommit
        receiveCanCommit
        receiveDecision
    ]

    let Arcs = [
        {
            place = coordinatorIdle
            transition = sendCanCommit
            expr = "1'()"
            direction = PT
        }
        {
            place = workerIdle
            transition = receiveCanCommit
            expr = "w"
            direction = PT
        }
        {
            place = waitingDecision
            transition = receiveCanCommit
            expr = "if vote = Yes then 1`w else empty"
            direction = TP
        }
        {
            place = waitingDecision
            transition = receiveDecision
            expr = "w"
            direction = PT
        }
        {
            place = workerIdle
            transition = receiveDecision
            expr = "w"
            direction = TP
        }
        {
            place = workerIdle
            transition = receiveCanCommit
            expr = "if vote = No then 1`w else empty"
            direction = TP
        }
    ]

    let Worker = {
        name = "Worker"
        places = Places
        transitions = Transition
        arcs = Arcs
    }