namespace CPN
type Multiset<'a when 'a: comparison> = private { multiset: Map<'a, int> }

module Multiset =
    let empty = { multiset = Map.empty }
    let (^) x c = { multiset = (Map.add c x Map.empty) }
    let (+) ams bms = { multiset = (Map.fold (fun ms c x ->
        match Map.tryFind c ms with
        | Some x' -> Map.add c (x+x') ms
        | None -> Map.add c x ms) ams.multiset bms.multiset ) }
    let (-) ams bms = { multiset = (Map.fold (fun ms c x ->
        match Map.tryFind c ms with
        | Some x' -> Map.add c (x'-x) ms
        | None -> Map.add c x ms) ams.multiset bms.multiset ) }
    let (<=) ams bms = Map.forall (fun c x ->
        match Map.tryFind c bms.multiset with
        | Some x' -> x <= x'
        | None -> false ) ams.multiset