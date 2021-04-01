module Core.Random

open System
open Core.Algebras.ExtendedReal

/// Implemented random generators for each type
module RandomGen =

    let private rand = Random()

    let getNextInteger () =
        rand.Next(Int32.MinValue, Int32.MaxValue)

    let getNextBoolean () = rand.Next() % 2 = 0

    let getNextReal () =
        if getNextBoolean ()
        then rand.NextDouble() * Double.MaxValue
        else (-1.) * rand.NextDouble() * Double.MaxValue

    let getNextExtendedReal () =
        let cases = [ "real"; "inf"; "NaN" ]

        match cases.[rand.Next cases.Length] with
        | "real" -> getNextReal () |> RealNumber
        | "inf" -> Infinity
        | "NaN" -> IndeterminateForm
        | _ ->
            "failed to generate next extended real; unknown identifier"
            |> failwith