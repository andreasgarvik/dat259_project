type multiset<'a> = list<int * 'a>

let (^) a b = [ (a, b) ]
let (+) ams bms = List.append ams bms

let ms : multiset<string> = (1 ^ "hello") + (1 ^ "cpn")
