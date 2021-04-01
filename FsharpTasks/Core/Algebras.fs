module Core.Algebras

open System
open Core.AlgebraicStructures

/// Compares floating point numbers with limited precision.
/// Returns `true` if two numbers are equal
/// with the specified precision,
/// otherwise returns `false`
let compareLimitedPrecision x y (epsilon: float) = abs (x - y) < epsilon

/// Defines boolean algebra
module Boolean =

    let booleanSemigroupWithPO =
        { Multiply = (&&)
          LE = fun x y -> x || not y }

    let booleanSemiring =
        { IdentityElement = false
          Add = (||)
          Multiply = (&&) }

    let toWord x = if x then "true " else "false"

    let fromWord word =
        match word with
        | "true " -> true
        | "false" -> false
        | _ ->
            "Boolean values must only be represented as strings: 'true ', 'false'."
            |> FormatException
            |> raise

/// Defines integer algebra
module Integer =
    let integerSemigroupWithPO = { Multiply = (+); LE = (<=) }

    let integerSemiring =
        { IdentityElement = 0
          Add = (+)
          Multiply = (*) }

    let toWord = string
    let fromWord (word: string) = word |> int

/// Defines real algebra
module Real =
    let realSemigroupWithPO: SemigroupPO<float> = { Multiply = (+); LE = (<=) }

    let realSemiring =
        { IdentityElement = 0.0
          Add = (+)
          Multiply = (*) }

    let toWord = string
    let fromWord (word: string) = word |> float
    let defaultEpsilon = 0.00001

    let compareReal eps =
        fun x y -> compareLimitedPrecision x y eps

/// Defines an extension of real type
/// (with infinity, indeterminate form incl.)
module ExtendedReal =
    open Real // use Real implementation

    type ExtendedReal =
        | IndeterminateForm
        | Infinity
        | RealNumber of x: float

    let internal add x y =
        match x, y with
        | IndeterminateForm, _
        | _, IndeterminateForm -> IndeterminateForm
        | Infinity, _
        | _, Infinity -> Infinity
        | RealNumber x, RealNumber y -> RealNumber(x + y)

    /// In abstract algebra,
    /// the idea of an inverse element
    /// generalises the concepts of negation (sign reversal)
    let internal inverseElement x =
        match x with
        | RealNumber x -> RealNumber -x
        | _ -> x

    let internal multiply x y epsilon =
        match x, y with
        | IndeterminateForm, _
        | _, IndeterminateForm -> IndeterminateForm
        | Infinity, Infinity -> Infinity
        | Infinity, RealNumber x
        | RealNumber x, Infinity -> if compareReal x 0.0 epsilon then IndeterminateForm else Infinity
        | RealNumber x, RealNumber y -> RealNumber(x * y)

    let internal lessOrEqual x y =
        match x, y with
        | IndeterminateForm, _
        | _, IndeterminateForm -> false
        | Infinity, Infinity -> true
        | Infinity, _ -> false
        | _, Infinity -> true
        | RealNumber x, RealNumber y -> x <= y

    let compareLimitedPrecision' x y epsilon =
        match x, y with
        | IndeterminateForm, _
        | _, IndeterminateForm -> false
        | Infinity, Infinity -> true
        | Infinity, _
        | _, Infinity -> false
        | RealNumber x, RealNumber y -> compareLimitedPrecision x y epsilon

    let extRealSemigroupWithPO = { Multiply = add; LE = lessOrEqual }

    let extRealSemiring eps =
        { IdentityElement = RealNumber 0.0
          Add = add
          Multiply = fun x y -> multiply x y eps }

    let toWord x =
        match x with
        | IndeterminateForm -> "[ ? ]"
        | Infinity -> "[inf]"
        | RealNumber x -> string x

    let fromWord word =
        if word = "[ ? ]" then IndeterminateForm
        elif word = "[inf]" then Infinity
        else (RealNumber << float) word