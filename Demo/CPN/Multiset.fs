namespace CPN

open CPN

type Multiset<'a when 'a: comparison> = private { multiset: Map<'a, int> }

module Multiset =
    let empty = { multiset = Map.empty }
    let isEmpty ms = Map.isEmpty ms.multiset
    let (^) x c = { multiset = (Map.add c x Map.empty) }
    let (+) ams bms = { multiset = (Map.fold (fun ms c x ->
        match Map.tryFind c ms with
        | Some x' -> Map.add c (x+x') ms
        | None -> Map.add c x ms) ams.multiset bms.multiset ) }
    let (<=) ams bms = Map.forall (fun c x ->
        match Map.tryFind c bms.multiset with
        | Some x' -> x <= x'
        | None -> false ) ams.multiset
    let (-) ams bms =
        if bms <= ams
        then { multiset = (Map.fold (fun ms c x ->
            match Map.tryFind c ms with
            | Some x' ->
                         let n = x'-x
                         match n with
                         | 0 -> Map.remove c ms
                         | _ -> Map.add c n ms
            | None -> Map.add c x ms) ams.multiset bms.multiset ) }
        else invalidOp "Cannot perform the operation with the given arguments"
    let toList ms = if not (Map.isEmpty ms.multiset)
                        then Map.toList ms.multiset |> List.head |> fst
                    else []