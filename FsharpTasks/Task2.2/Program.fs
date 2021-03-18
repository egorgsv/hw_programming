open System
open System.IO
open System.Diagnostics
open System.Text

open Core.MatrixIO
open Core.Matrix
open Core.Algebras

// C#-версия генератора
/// Provides simple way to use C# pdf-generator from F#
module CsharpToFsharpMiddleware =
    open TransitiveClosure // open related C#-project
    open Matrix.AlgebraicStructures // C# extended boolean type

    /// Converts types (System.Boolean [][] -> Matrix.AlgebraicStructures.Boolean [][])
    let private cast matrixOfSystemBool =
        let rowsCount = Array.length matrixOfSystemBool
        let columnsCount = Array.length matrixOfSystemBool.[0]

        [| for i in 0 .. rowsCount - 1 do
            [| for j in 0 .. columnsCount - 1 do
                Matrix.AlgebraicStructures.Boolean(matrixOfSystemBool.[i].[j]) |] |]

    /// Calls C# pdf generator
    let internal useCsharpPDFGeneration directory filename matrixArr closureArr =
        let _matrixArr = cast matrixArr
        let _closureArr = cast closureArr

        let dotCode =
            PdfGenerator.GetDotCode(_matrixArr, _closureArr)

        PdfGenerator.GeneratePDF((getFullPathToFile directory filename), dotCode)

// Это F#-версия генератора pdf
module PDFGeneratorFsharp =
    let internal getDotCode (matrix: bool [,]) (transClosureMatrix: bool [,]) =
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

    let internal generatePDF resFilename (dot: string) =
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

/// Calls F# pdf generator
let useFsharpPDFGeneration directory filename matrixArr2D closureArr2D =
    closureArr2D
    |> PDFGeneratorFsharp.getDotCode matrixArr2D
    |> PDFGeneratorFsharp.generatePDF (getFullPathToFile directory filename)

let private _dataFolderName = "data"

//  -> Задание:
//      2.2) Посчитать транзитивное замыкание булевой матрицы.
//          Результат работы - pdf-файл графа с ребрами,
//          принадлежащими транизтивному замыканию.
[<EntryPoint>]
let main argv =
    Console.info "Running Task 2.2 ..."

    let path =
        "task_2.2_input.txt"
        |> getFullPathToFile _dataFolderName

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

    let transClosure =
        matrix
        |> MatrixFs.findTransitiveClosure Boolean.booleanSemiring

    Console.complete "Transitive closure computed!"

    Console.info "Generating PDF-file via `dot` at folder:"
    Console.msg' (Path.Combine((getBaseDir).FullName, _dataFolderName))

    // both version gives the same result; this is kind of a "PoC" that F#-version works
    // ==> so we could use any of F# or C# pdf-generation method
    CsharpToFsharpMiddleware.useCsharpPDFGeneration
        _dataFolderName
        "task2-2_graphviz_csharp"
        (MatrixFs.toArray matrix)
        (MatrixFs.toArray transClosure)

    useFsharpPDFGeneration
        _dataFolderName
        "task2-2_graphviz_fsharp"
        (MatrixFs.toArray2D matrix)
        (MatrixFs.toArray2D transClosure)

    Console.ok "\nAll done."
    0 // return an integer exit code
