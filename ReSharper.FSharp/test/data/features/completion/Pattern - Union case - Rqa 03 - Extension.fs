// ${COMPLETE_ITEM:A}
module Module

[<RequireQualifiedAccess>]
type U =
    | A of int * int

type U with
    static member P =
        match A(1, 2) with
        | {caret}
