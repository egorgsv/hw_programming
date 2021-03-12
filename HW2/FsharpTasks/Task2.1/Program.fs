module MatrixProcessor

open System
open Argu

open Core.AlgebraicStructures
open Core.Matrix
open Core.Matrix
open Core.Matrix
open Core.MatrixIO
open Core.Maybe

(*
    -> Задание:
        2.1) Реализовать умножение матриц над произвольным полукольцом.
            Матрицы брать из текстового файла и записать в текстовый файл.
*)

type TaskID =
    | MultiplyMatrices
    | ShortestPath
    | TRC

type Argument = 
    | [<Mandatory>] Task of task : TaskID
    | [<MainCommand; ExactlyOnce; Last>] Matrices of paths : string list
    with
    interface IArgParserTemplate with
        member s.Usage =
            match s with
            | Task _ -> "specify what is to be done. (TRC | APSP | MUL)"
            | Matrices _ -> "specify paths to matrices."

let parser = ArgumentParser.Create<Argument>(programName = "MatrixProcessor")

let maybe = MaybeBuilder ()

[<EntryPoint>]
let main argv =
    let m = ZeroMatrix(10, 10, 0)
    m.[1, 1] |> printfn "%A"
    0
