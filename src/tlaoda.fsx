type IStack<'a> =
    // abstract static New: unit -> IStack<'a>
    abstract Push : 'a -> IStack<'a>
    abstract Pop : unit -> IStack<'a>
    abstract Top : 'a Option

type private Representation<'a> =
    | New
    | Push of 'a * Representation<'a>

type Stack<'a when 'a: equality> private (repr: Representation<'a>) =

    static member New() = New |> Stack

    member __.Push(x: 'a) = Push(x, repr) |> Stack

    member __.Pop() =
        match repr with
        | New -> New
        | Push (_, s) -> s
        |> Stack

    member __.Top : 'a Option =
        match repr with
        | New -> None
        | Push (x, _) -> Some x

    member private __.repr = repr

    override __.Equals(obj) =
        match obj with
        | :? (Stack<'a>) as x -> x.repr.Equals(repr)
        | _ -> false

    override __.GetHashCode() = repr.GetHashCode()
