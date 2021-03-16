open System
open System.IO
open System.Diagnostics
open System.Text

open Core.MatrixIO
open Core.Matrix
open Core.Algebras

// Это вместо С#-генератора, он работает в точности также
module PDFGeneratorFsharp =
    let getDotCode (matrix: bool [,]) (transClosureMatrix: bool [,]) =
        let stringBuilder = StringBuilder()
        stringBuilder.AppendLine("digraph G {") |> ignore

        (fun i j x ->
            if x then
                let str =
                    i.ToString()
                    + " -> "
                    + j.ToString()
                    + " ["
                    + if matrix.[i, j] then "" else "color=red"
                    + "];"

                stringBuilder.Append(str) |> ignore)

        |> Array2D.iteri
        <| transClosureMatrix

        stringBuilder.AppendLine("}") |> ignore
        stringBuilder.ToString()

    let generatePDF resFilename (dot: string) =
        let dotFile = resFilename + ".dot"
        using (File.CreateText(dotFile)) (fun writer -> writer.Write(dot))

        using (new Process()) (fun proc ->
            proc.StartInfo.FileName <-
                if Environment.OSVersion.Platform = PlatformID.Win32NT
                then "dot.exe"
                else "dot"

            proc.StartInfo.Arguments <- "-Tpdf -o" + resFilename + ".pdf" + " " + dotFile
            proc.Start() |> ignore

            while not proc.HasExited do
                proc.Refresh())

        File.Delete(dotFile)

// 2.2) Посчитать транзитивное замыкание булевой матрицы.
//         Результат работы - pdf-файл графа с ребрами,
//         принадлежащими транизтивному замыканию.

[<EntryPoint>]
let main argv =
    let path =
        getFullPathToFile "data" "task_2.2_input.txt"

    let parseBool =
        fun str ->
            match str with
            | "true" -> true
            | "false" -> false
            | _ -> failwith "parsing error"

    let matrix =
        readMatrixFromFile " " parseBool path
        |> array2D
        |> Matrix

    let res =
        matrix
        |> MatrixFs.findTransitiveClosure Boolean.booleanSemiring
        |> MatrixFs.toArray2D

    let dot =
        res
        |> PDFGeneratorFsharp.getDotCode (MatrixFs.toArray2D matrix)

    dot
    |> PDFGeneratorFsharp.generatePDF (getFullPathToFile "data" "task2-2_graphviz")

    printf "All done."
    0 // return an integer exit code
