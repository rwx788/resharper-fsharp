// ${COMPLETE_ITEM:A}
module Module

[<RequireQualifiedAccess>]
type U =
    | A of int * int

    static member P =
        { new System.IDisposable with
            member this.Dispose() =
                match A(1, 2) with
                | {caret}
        }
