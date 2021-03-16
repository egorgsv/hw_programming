module Core.Tests.FloydWarshallTests

open NUnit.Framework
open FsUnit

open Core.AlgebraicStructures
open Core.Matrix

let sgInt = { Multiply = (+); LE = (<=) }

[<Test>]
let ``Floyd-Warshall simple test`` () =
    let matrix =
        [| [| 0; 9; 2 |]
           [| 1; 0; 4 |]
           [| 2; 4; 0 |] |]
        |> array2D
        |> Matrix

    let expected =
        [| [| 0; 6; 2 |]
           [| 1; 0; 3 |]
           [| 2; 4; 0 |] |]
        |> array2D
        |> Matrix

    let res =
        MatrixFs.floydWarshallInSemigroup sgInt matrix

    res |> should equal (Some expected)

[<Test>]
let ``Floyd-Warshall with wrong arguments`` () =
    let res =
        [| [| 1; 2; 3 |]; [| 4; 5; 6 |] |]
        |> array2D
        |> Matrix
        |> MatrixFs.floydWarshallInSemigroup sgInt

    res |> should equal None