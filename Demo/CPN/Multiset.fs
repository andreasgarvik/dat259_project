namespace CPN

open System.Collections.Generic

type 't Multiset = private Multiset of Dictionary<int, 't>

module Multiset =