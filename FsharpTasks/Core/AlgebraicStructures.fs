/// Collection of algebraic structures
/// (semiring and semigroup) implemented as `interface-like` records
module Core.AlgebraicStructures

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