module Core.Tests.MultiplicationInSemiring

open NUnit.Framework
open FsUnit

open Core.Matrix
open Core.AlgebraicStructures

let semiringInt = {IdentityElement = 0; Add = (+); Multiply = (*)}
let semiringFloat = {IdentityElement = 0.0; Add = (+); Multiply = (*)}

[<Test>]
let ``matrix multiplied by itself in a semiring of integers equal to its square`` () =
    let matrix =
        [|  [| 1; 2 |]
            [| 1; 2 |] |]
        |> array2D
        |> Matrix
    
    let result =
        matrix
        |> MatrixFs.multiplyInSemiring semiringInt
        <| matrix
        |> MatrixFs.toArray
    
    should equal result
        [|  [| 3; 6 |]
            [| 3; 6 |] |]