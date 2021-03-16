// Task 2.1 tests
module Core.Tests.MultiplicationInSemiring

open NUnit.Framework // test framework
open FsUnit // make tests more 'functional'

// test matrix multiplication (for each of [int, real, ..] semiring)
open Core.Matrix
open Core.AlgebraicStructures

let semiringInt =
    { IdentityElement = 0
      Add = (+)
      Multiply = (*) }

let semiringBool =
    { IdentityElement = true
      Add = (||)
      Multiply = (&&) }

let semiringReal =
    { IdentityElement = 0.0
      Add = (+)
      Multiply = (*) }

[<Test>]
let ``matrix (integer semiring) multiplied by itself equal to its square`` () =
    let matrix =
        [| [| 1; 2; 3 |]
           [| 6; 5; 4 |]
           [| 7; 8; 9 |] |]
        |> array2D
        |> Matrix

    let result =
        matrix |> MatrixFs.multiplyInSemiring semiringInt
        <| matrix

    [| [| 34; 36; 38 |]
       [| 64; 69; 74 |]
       [| 118; 126; 134 |] |]
    |> should equal (result |> MatrixFs.toArray)

[<Test>]
let ``simple matrix multiplication (integer semiring)`` () =
    let matrix =
        [| [| 1; 2; 3 |]; [| 1; 2; 3 |] |]
        |> array2D
        |> Matrix

    let result =
        matrix |> MatrixFs.multiplyInSemiring semiringInt
        <| ([| [| 1; 1 |]; [| 2; 2 |]; [| 3; 3 |] |]
            |> array2D
            |> Matrix)

    [| [| 14; 14 |]; [| 14; 14 |] |]
    |> should equal (result |> MatrixFs.toArray)


[<Test>]
let ``simple matrix multiplication (boolean semiring)`` () =
    let matrix =
        [| [| true; false; true |]
           [| true; false; true |] |]
        |> array2D
        |> Matrix

    let result =
        matrix |> MatrixFs.multiplyInSemiring semiringBool
        <| ([| [| true; true |]
               [| false; false |]
               [| true; true |] |]
            |> array2D
            |> Matrix)

    [| [| true; true |]; [| true; true |] |]
    |> should equal (result |> MatrixFs.toArray)

[<Test>]
let ``simple matrix multiplication (real semiring)`` () =
    let matrix =
        [| [| 1.0; 0.0; 1.0 |]
           [| 0.0; 1.0; 0.0 |] |]
        |> array2D
        |> Matrix

    let result =
        matrix |> MatrixFs.multiplyInSemiring semiringReal
        <| ([| [| 1.0; 0.0 |]
               [| 0.0; 1.0 |]
               [| 1.0; 0.0 |] |]
            |> array2D
            |> Matrix)

    [| [| 2.0; 0.0 |]; [| 0.0; 1.0 |] |]
    |> should equal (result |> MatrixFs.toArray)