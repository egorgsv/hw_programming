module Core.Algebras

open System
open Core.AlgebraicStructures

/// Defines boolean algebra
module Boolean =
    let booleanSemigroupWithPO =
        { Multiply = (&&)
          LE = fun x y -> x || not y }

    let booleanSemiring =
        { IdentityElement = false
          Add = (||)
          Multiply = (&&) }

    let toSymbol x = if x then "true " else "false"

    let fromSymbol symbol =
        match symbol with
        | "true " -> true
        | "false" -> false
        | _ ->
            FormatException("Boolean values must only be represented as symbols: 'true ', 'false'.")
            |> raise

/// Defines integer algebra
module Integer =
    let integerSemigroupWithPO = { Multiply = (+); LE = (<=) }

    let integerSemiring =
        { IdentityElement = 0
          Add = (+)
          Multiply = (*) }

    let toSymbol = string
    let fromSymbol (symbol: string) = symbol |> int

/// Defines real algebra
module Real =
    let realSemigroupWithPO: SemigroupPO<float> = { Multiply = (+); LE = (<=) }

    let realSemiring =
        { IdentityElement = 0.0
          Add = (+)
          Multiply = (*) }

    let toSymbol = string
    let fromSymbol (symbol: string) = symbol |> float
    let epsilon = 0.00001
    let inline (=!) x y = abs (x - y) < epsilon

/// Defines an extension of real type
/// (with infinity, indeterminate form incl.)
module ExtendedReal =
    open Real

    type ExtendedReal =
        | Infinity
        | IndeterminateForm
        | RealNumber of x: float

    let internal add x y =
        match x, y with
        | Infinity, _
        | _, Infinity -> Infinity
        | IndeterminateForm, _
        | _, IndeterminateForm -> IndeterminateForm
        | RealNumber x, RealNumber y -> RealNumber(x + y)

    let internal reverse x =
        match x with
        | RealNumber x -> RealNumber -x
        | _ -> x

    let internal multiply x y =
        match x, y with
        | Infinity, Infinity -> Infinity
        | IndeterminateForm, _
        | _, IndeterminateForm -> IndeterminateForm
        | Infinity, RealNumber x
        | RealNumber x, Infinity -> if x =! 0.0 then IndeterminateForm else Infinity
        | RealNumber x, RealNumber y -> RealNumber(x * y)

    let internal le x y =
        match x, y with
        | Infinity, Infinity -> true
        | IndeterminateForm, _
        | _, IndeterminateForm -> false
        | Infinity, _ -> false
        | _, Infinity -> true
        | RealNumber x, RealNumber y -> x <= y

    let inline (=!) x y =
        match x, y with
        | Infinity, Infinity -> true
        | IndeterminateForm, _
        | _, IndeterminateForm -> false
        | Infinity, _
        | _, Infinity -> false
        | RealNumber x, RealNumber y -> x =! y

    let extendedRealSemigroupWithPO = { Multiply = add; LE = le }

    let extendedRealSemiring =
        { IdentityElement = RealNumber 0.0
          Add = add
          Multiply = multiply }

    let toSymbol x =
        match x with
        | IndeterminateForm -> "[?]"
        | Infinity -> "[inf]"
        | RealNumber x -> string x

    let fromSymbol symbol =
        if symbol = "[?]" then IndeterminateForm
        elif symbol = "[inf]" then Infinity
        else (RealNumber << float) symbol