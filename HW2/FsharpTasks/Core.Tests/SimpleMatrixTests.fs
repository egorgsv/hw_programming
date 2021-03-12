module Core.Tests.SimpleMatrixTests

open NUnit.Framework
open FsUnit

open Core.Matrix

[<Test>]
let ``array2d unchanged after conversion to MatrixFs`` () =
    let mat = [| [| 1; 2 |]; [| 1; 2 |] |]
    mat
    |> array2D
    |> Matrix
    |> MatrixFs.toArray
    |> should equal mat
    
[<Test>]
let ``transposed matrix is correct`` () =
    let mat = [| [| 42; 1 |]; [| 0; 33 |] |]
    [| [| 42; 0 |]; [| 1; 33 |] |]
    |> array2D
    |> Matrix
    |> MatrixFs.transpose
    |> MatrixFs.toArray
    |> should equal mat