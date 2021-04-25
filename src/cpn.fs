type multiset<'a> = list<int * 'a>

let (^) a b = [ (a, b) ]
let (+) ams bms = List.append ams bms

type colset = colset of string 
type marking = marking of string
type Place = {name: string; colset: colset; initialMarking: marking }

