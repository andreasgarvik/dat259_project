namespace TPC

open CPN

module Representation =
    let coordinatorIdle = {
        name = "Coordinator Idle"
        colset = "Unit"
        initialMarking = "()"
    }
    let workerIdle = {
        name = "Worker Idle"
        colset = "Worker"
        initialMarking = "Worker.all()"
    }
    let canCommit = {
        name = "CanCommit"
        colset = "Worker"
        initialMarking = ""
    }
    let waitingVotes = {
        name = "Waiting Votes"
        colset = "()"
        initialMarking = ""
    }
    let votes = {
        name = "Votes"
        colset = "WorkerxVote"
        initialMarking = ""
    }
    let collectedVotes = {
        name = "Collected Votes"
        colset = "WorkerxVotes"
        initialMarking = "[]"
    }
    let waitingDecision = {
        name = "Waiting Decision"
        colset = "Worker"
        initialMarking = ""
    }
    let decision = {
        name = "Decision"
        colset = "WorkerxDecision"
        initialMarking = ""
    }
    let waitingAcknowledgements = {
        name = "Waiting Acknowledgements"
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
        name = "Send CanCommit"
        vars = []
        guard = None
    }
    let receiveCanCommit = {
        name = "Receive CanCommit"
        vars = [ "w"; "vote"]
        guard = None
    }

    let receiveDecision = {
        name = "Receive Decision"
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
            transaction = sendCanCommit
            expr = "1'()"
            direction = PT
        }
        {
            place = workerIdle
            transaction = receiveCanCommit
            expr = "w"
            direction = PT
        }
        {
            place = waitingDecision
            transaction = receiveCanCommit
            expr = "if vote = Yes then 1`w else empty"
            direction = TP
        }
        {
            place = waitingDecision
            transaction = receiveDecision
            expr = "w"
            direction = PT
        }
        {
            place = workerIdle
            transaction = receiveDecision
            expr = "w"
            direction = TP
        }
        {
            place = workerIdle
            transaction = receiveCanCommit
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