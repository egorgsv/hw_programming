module CLI

open Argu // better argument parsing library

open Core.Algebras
open Core.Random
open Core.Matrix
open Core.MatrixIO
open System.IO

/// Defines matrix elements types supported by CLI
type MatrixElementsTypes =
    | RealType
    | ExtendedRealType
    | BooleanType
    | IntegerType

/// Defines arguments supported by CLI
type CliArguments =
    | [<Mandatory; AltCommandLine("-t")>] ElementType of MatrixElementsTypes
    | [<Mandatory; AltCommandLine("-s")>] Size of int
    | [<Mandatory; AltCommandLine("-p")>] Path of string
    | [<Mandatory; AltCommandLine("-c")>] Count of int
    interface IArgParserTemplate with
        member s.Usage =
            match s with
            | ElementType _ ->
                "specify type of matrix elements. (BooleanType | IntegerType | RealType | ExtendedRealType)"
            | Size _ -> "specify size of matrix."
            | Path _ -> "specify where matrix should be created."
            | Count _ -> "specify count of matrices to create."

/// Sets argument parser
let private parser =
    ArgumentParser.Create<CliArguments>(programName = "MatrixGeneratorCLI")

let toStr elemT =
    match elemT with
    | BooleanType -> "boolean"
    | IntegerType -> "integer"
    | RealType -> "real"
    | ExtendedRealType -> "extended_real"

[<EntryPoint>]
let main argv =
    Console.msg "Running `Matrix Generator CLI` (Task 3)"

    try
        let parsed = parser.ParseCommandLine argv

        let elemType, size, path, count =
            parsed.GetResult ElementType, parsed.GetResult Size, parsed.GetResult Path, parsed.GetResult Count

        let range = { 1 .. count }

        let path =
            Path.Combine [| path
                            (toStr elemType)
                            (string size) |]

        path |> Directory.CreateDirectory |> ignore
        Console.info "Directory created:"
        Console.msg' path
        let fullPath = Path.Combine(path, "matrix")

        let action x =
            let inline genThenSave rand toWord =
                let matStr =
                    rand
                    |> MatrixFs.createRandomSquareMatrix size
                    |> MatrixFs.toArray2D
                    |> matrixToStringCustom " " toWord

                File.WriteAllText((fullPath + string x), matStr)

            match elemType with
            | BooleanType ->
                Boolean.toWord
                |> genThenSave RandomGen.getNextBoolean
            | IntegerType ->
                Integer.toWord
                |> genThenSave RandomGen.getNextInteger
            | RealType -> Real.toWord |> genThenSave RandomGen.getNextReal
            | ExtendedRealType ->
                ExtendedReal.toWord
                |> genThenSave RandomGen.getNextExtendedReal

            Console.info ("Creating matrix of type: " + toStr elemType)

        Seq.iter action range
    with err -> eprintfn "%s" err.Message

    Console.complete "\nFinished."
    0