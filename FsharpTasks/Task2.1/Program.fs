module MatrixProcessor

open System
open Core.Algebras
open Core.Matrix
open Core.MatrixIO

/// Separator is a string character
/// that will be used to separate
/// the elements of the matrix
/// after the matrix is written/read to/from the file
let private _sep = " " // use <space> as separator

let private _dataFolderName = "data"

let simpleMultiplication inFile outFile =
    let mat =
        (getFullPathToFile _dataFolderName inFile)
        |> readMatrixFromFile _sep (int)
        |> array2D
        |> Matrix

    let res =
        MatrixFs.multiplyInSemiring Integer.integerSemiring mat mat

    Console.msg "Matrix product of \n["

    mat
    |> MatrixFs.toArray2D
    |> matrixToString _sep
    |> Console.msg'

    Console.msg "]\n multiplied by itself is:\n["

    res
    |> MatrixFs.toArray2D
    |> matrixToString _sep
    |> Console.msg'

    Console.msg "]"

    res
    |> MatrixFs.toArray2D
    |> saveMatrixToFile _sep (getFullPathToFile _dataFolderName outFile)

//  -> Задание:
//      2.1) Реализовать умножение матриц над произвольным полукольцом.
//          Матрицы брать из текстового файла и записать в текстовый файл.
[<EntryPoint>]
let main _argv =
    Console.info "Running Task 2.1 ..."
    simpleMultiplication "task_2.1_input.txt" "task_2.1_output.txt"
    Console.ok "\nAll done."
    0
