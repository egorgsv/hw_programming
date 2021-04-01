module Core.MatrixIO

open System.IO

/// Returns base directory
let getBaseDir =
    let baseDirectory = __SOURCE_DIRECTORY__
    Directory.GetParent(baseDirectory)

/// Get full path to file with data
let getFullPathToFile subdir filename =
    let dir = getBaseDir
    Path.Combine(dir.FullName, subdir + "\\" + filename)

/// Converts matrix (array2D) to a nicely formatted string using custom casting function
let matrixToStringCustom (separator: string) castFunc (matrix: 'a [,]) =
    let mutable acc = ""
    for i = 0 to matrix.GetUpperBound(0) do
        for j = 0 to matrix.GetUpperBound(1) do
            acc <- acc + (castFunc matrix.[i, j]) + separator

        acc <- acc + "\n"

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
    |> Array.map (fun line -> line.Trim().Split separator |> Array.map (parseF))

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

/// Provides colorful console output
module Console =

    open System

    let log =
        let lockObj = obj ()

        fun color s ->
            lock lockObj (fun _ ->
                Console.ForegroundColor <- color
                printfn "%s" s
                Console.ResetColor())

    let complete = log ConsoleColor.Magenta
    let ok = log ConsoleColor.Green
    let info = log ConsoleColor.Cyan
    let warn = log ConsoleColor.Yellow
    let error = log ConsoleColor.Red
    let msg = log ConsoleColor.DarkCyan
    let msg' = log ConsoleColor.DarkYellow