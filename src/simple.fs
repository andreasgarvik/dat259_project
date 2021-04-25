// First need a way to represent the model
// Code should be in the CPN module/file
// Should be able to represent all possible cpn models

// Second need a way to represent the state of the model
// Code should be in a State module/file

// Third need a way to advance or reverse the state
// Code should be in the State module/file??

// Possible ways to represent state
// 1. In the objects if CPN model consists of objects, immunatillity??
// 2. Have a list where every next entry is the calculated state of the previous entry
// - val nextState : State -> State