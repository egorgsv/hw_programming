// Matrix implementation tests
module Core.Tests.SimpleMatrixTests

open NUnit.Framework // test framework
open FsUnit // make tests more 'functional'

// test basic matrix functionality
open Core.Matrix
open Core.MatrixIO

/// Separator is a string character
/// that will be used to separate
/// the elements of the matrix
/// after the matrix is written/read to/from the file
let private _sep = " " // <space> as separator

/// specified test filenames
let private fileA, fileB, fileC =
    getFullPathToFile "data" "test_matrix_a.txt",
    getFullPathToFile "data" "test_matrix_b.txt",
    getFullPathToFile "data" "test_matrix_c.txt"

[<Test>]
let ``array2d unchanged after conversion to MatrixFs`` () =
    let mat = array2D [| [| 1; 2 |]; [| 1; 2 |] |]

    mat
    |> Matrix
    |> MatrixFs.toArray2D
    |> should equal mat

[<Test>]
let ``transposed matrix is correct`` () =
    let mat = array2D [| [| 42; 1 |]; [| 0; 33 |] |]
    let transposed = array2D [| [| 42; 0 |]; [| 1; 33 |] |]

    transposed
    |> Matrix
    |> MatrixFs.transpose
    |> MatrixFs.toArray2D
    |> should equal mat

[<Test>]
let ``transposed non-square matrix is correct`` () =
    let mat =
        array2D [| [| 1; 1; 1 |]
                   [| 2; 2; 2 |]
                   [| 3; 3; 3 |]
                   [| 4; 4; 4 |] |]

    let transposed =
        array2D [| [| 1; 2; 3; 4 |]
                   [| 1; 2; 3; 4 |]
                   [| 1; 2; 3; 4 |] |]

    transposed
    |> Matrix
    |> MatrixFs.transpose
    |> MatrixFs.toArray2D
    |> should equal mat

[<Test>]
let ``reading, changing, then writing matrix to file`` () =
    let inline transpose' xs =
        xs
        |> Matrix
        |> MatrixFs.transpose
        |> MatrixFs.toArray2D

    // read matrix from file;
    // -> matrix from "test_matrix_a.txt" should look like:
    //      [| [| 42; 1 |]; [| 0; 33 |] |]
    let matA =
        array2D (readMatrixFromFile _sep (int) fileA)

    // transpose, cast to float, then save simple matrix to "test_matrix_b.txt";
    // -> [| [| 42.0; 0.0 |]; [| 1.0; 33.0 |] |]
    matA
    |> transpose'
    |> Array2D.map (float)
    |> saveMatrixToFile _sep fileB

    // read "test_matrix_b.txt", transpose and compare
    // to initial matrix from fileA (cast `matA` to float first)
    let matB =
        array2D (readMatrixFromFile _sep (float) fileB)
        |> transpose' // [| [| 42.0; 1.0 |]; [| 0.0; 33.0 |] |]

    matA |> Array2D.map (float) |> should equal matB
