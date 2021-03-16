/// Collection of algebraic structures
/// (semiring and semigroup) implemented as `interface-like` records
module Core.AlgebraicStructures

(*
    quote (page 9): https://fsharp.org/specs/component-design-guidelines/fsharp-design-guidelines-v14.pdf
    -----------------------------------------------------------------------------------------------------
    => In F#there are a number of ways to represent a dictionary of operations, such as using tuples of
    functions or records of functions. In general, we recommend you use interface types
    for this purpose.
                                    ["oop-style"]
    <------------------------------------------->
        type ICounter =                         >
            abstract Increment : unit -> unit   >
            abstract Decrement : unit -> unit   >
            abstract Value : int                >
    <------------------------------------------->
    => In preference to:
                        ["records-style"]
    <----------------------------------->
        type CounterOps =               >
            { Increment: unit -> unit   >
              Decrement : unit -> unit  >
              GetValue : unit -> int }  >
    <----------------------------------->
*)

/// Semiring
type Semiring<'a> =
    { IdentityElement: 'a
      Add: ('a -> 'a -> 'a)
      Multiply: ('a -> 'a -> 'a) }

let isZeroInSemiring (sr: Semiring<'a>) element = sr.IdentityElement = element

/// Semigroup with partial order
type SemigroupPO<'a> =
    { Multiply: ('a -> 'a -> 'a)
      LE: ('a -> 'a -> bool) }