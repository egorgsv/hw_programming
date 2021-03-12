module Core.Maybe

/// Maybe expression builder
type MaybeBuilder() =
    member _.Bind(x, f) =
        match x with
        | None -> None
        | Some x -> f x

    member _.Return(x) = Some x
