namespace CPN

// Do a static analysis of the representation, syntax error?

// Represent the state of the CPN Model:
// - Markings (multiset) on places
// - Enable transitions based on markings (marking on places on arcs satisfies transitions)
// - Types?

// C# map
// type 'a multiset = (int * 'a) list
// let emptyms = []
// let (^) a b = [(a,b)]
// let (+) ams bms = List.append ams bms