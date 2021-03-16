module MatrixProcessor

open Core.Algebras
open Core.Matrix
open Core.MatrixIO
open Core.Maybe

//  -> Задание:
//      2.1) Реализовать умножение матриц над произвольным полукольцом.
//      Матрицы брать из текстового файла и записать в текстовый файл.

let private maybe = MaybeBuilder()

/// Separator is a string character
/// that will be used to separate
/// the elements of the matrix
/// after the matrix is written/read to/from the file
let private _sep = " " // use <space> as separator

let private simpleFunctionalityTest =
    let mat =
        [| [| 1; 2; 3 |]
           [| 1; 4; 9 |]
           [| 1; 8; 27 |] |]
        |> array2D
        |> Matrix

    let res =
        MatrixFs.multiplyInSemiring Integer.integerSemiring mat mat

    let matStr =
        mat
        |> MatrixFs.toArray2D
        |> matrixToString _sep

    let resStr =
        res
        |> MatrixFs.toArray2D
        |> matrixToString _sep

    printfn "Matrix product of \n[\n%s]\n multiplied by itself is:\n[\n%s]\n" matStr resStr

    res
    |> MatrixFs.toArray2D
    |> saveMatrixToFile _sep (getFullPathToFile "data" "task_2.1_output.txt")

[<EntryPoint>]
let main _argv =
    simpleFunctionalityTest
    0
