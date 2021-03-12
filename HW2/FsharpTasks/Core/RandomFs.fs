module Core.RandomFs

open System
open Core.Algebras.ExtendedReal

/// Implemented random generators for each type
module RandomGen =

    let internal rand = Random()

    let getNextInteger () =
        rand.Next(Int32.MinValue, Int32.MaxValue)

    let getNextBoolean () = rand.Next() % 2 = 0

    let getNextReal () =
        if getNextBoolean ()
        then rand.NextDouble() * Double.MaxValue
        else (-1.) * rand.NextDouble() * Double.MaxValue

    let getNextExtendedReal () =
        let cases = [ "real"; "inf"; "nan" ]

        match cases.[rand.Next cases.Length] with
        | "real" -> getNextReal () |> RealNumber
        | "inf" -> Infinity
        | "nan" -> IndeterminateForm
        | _ -> "fail to generate next extended real" |> failwith