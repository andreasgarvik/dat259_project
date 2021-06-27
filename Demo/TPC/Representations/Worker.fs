namespace TPC.Representations

open CPN

module Worker =
    let WorkerIdle = {
        name = "Worker Idle"
        colset = "Worker"
        initialMarking = "Worker.all()"
    }

    let WaitingDecision = {
        name = "Waiting Decision"
        colset = "Worker"
        initialMarking = ""
    }

    let Places = [WorkerIdle; WaitingDecision]

    let ReceiveCanCommit = {
        name = "Receive CanCommit"
        vars = [ "w"; "vote"]
        guard = None
    }

    let ReceiveDecision = {
        name = "Receive Decision"
        vars = [ "w"; "decision"]
        guard = None
    }

    let Transition = [ReceiveCanCommit; ReceiveDecision]

    let Arcs = [
        {
            place = WorkerIdle
            transaction = ReceiveCanCommit
            expr = "w"
            direction = PT
        }
        {
            place = WaitingDecision
            transaction = ReceiveCanCommit
            expr = "if vote = Yes then 1`w else empty"
            direction = TP
        }
        {
            place = WaitingDecision
            transaction = ReceiveDecision
            expr = "w"
            direction = PT
        }
        {
            place = WorkerIdle
            transaction = ReceiveDecision
            expr = "w"
            direction = TP
        }
        {
            place = WorkerIdle
            transaction = ReceiveCanCommit
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