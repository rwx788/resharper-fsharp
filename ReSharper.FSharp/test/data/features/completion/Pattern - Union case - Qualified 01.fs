// ${COMPLETE_ITEM:Bb}
module Module

[<RequireQualifiedAccess>]
type U =
    | A
    | Bb of int

match U.A with
| U.B{caret}
