open Core.Matrix
open Core.MatrixIO

open Core.AlgebraicStructures

let intSemigroupPO = { Multiply = (+); LE = (<=) }

/// Separator is a string character
/// that will be used to separate
/// the elements of the matrix
/// after the matrix is written/read to/from the file
let private _sep = " " // use <space> as separator

let private _dataFolderName = "data"

let simpleFloydWarshall inFilename outFilename =
    let matrix =
        (getFullPathToFile _dataFolderName inFilename)
        |> readMatrixFromFile _sep (int)
        |> array2D
        |> Matrix

    let res =
        MatrixFs.floydWarshallInSemigroup intSemigroupPO matrix

    if res.IsNone then
        Console.error "Error: Floyd Warshall returns unexpected result"
    else
        Console.msg "Initial matrix was ["

        matrix
        |> MatrixFs.toArray2D
        |> matrixToString _sep
        |> Console.msg'

        Console.msg "]\n Floyd Warshall is:\n["

        Console.msg'
            (res.Value
             |> MatrixFs.toArray2D
             |> matrixToString _sep)

        Console.msg "]"
        // if everything is ok -> save to file
        res.Value
        |> MatrixFs.toArray2D
        |> saveMatrixToFile _sep (getFullPathToFile _dataFolderName outFilename)

//  -> Задание:
//      2.3) Для матриц над произвольным полукольцом посчитать APSP
//          (Длина кратчайшего пути между всеми парами вершин).
//          Результат записать аналогично №2.1.
[<EntryPoint>]
let main _ =
    Console.info "Running Task 2.3 ..."
    simpleFloydWarshall "task_2.3_input.txt" "task_2.3_output.txt"
    Console.ok "\nFinished."
    0 // return an integer exit code
