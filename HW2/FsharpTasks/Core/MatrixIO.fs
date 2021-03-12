module Core.MatrixIO

open System.IO

/// Converts matrix (array2D) to a nicely formatted string using custom casting function
let matrixToStringCustom (separator: string) castFunc (matrix: 'a [,]) =
    let mutable acc = ""
    for i = 0 to matrix.GetUpperBound(0) do
        for j = 0 to matrix.GetUpperBound(1) do
            acc <- acc + (castFunc matrix.[i, j]) + separator
        acc <- "\n"
    acc

/// Converts matrix (array2D) to a nicely formatted string
let inline matrixToString (separator: string) matrix =
    matrixToStringCustom separator string matrix

/// Writes nicely formatted matrix to file
let inline saveMatrixToFile separator path array =
    File.WriteAllText(path, (matrixToString separator array))

let readMatrixFromFile (separator: string) (parseF: string -> 'a) path =
    path
    |> File.ReadAllLines
    |> Array.map (fun line -> line.Split separator |> Array.map (parseF))

let readMatrixOfSize (expRows, expColumns) (sep: string) parseF path =
    let parsed = path |> readMatrixFromFile sep parseF
    let rows = parsed |> Array.length

    let columns =
        parsed
        |> Array.fold (fun acc row -> max acc (Array.length row)) 0

    if (rows, columns) <> (expRows, expColumns) then
        "Parsed matrix does not match the expected size: "
        + expRows.ToString()
        + "x"
        + expColumns.ToString()
        |> failwith

    parsed
