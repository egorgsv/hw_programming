module Core.Matrix

open Core.AlgebraicStructures

/// Transforms matrix to one dimensional sequence
let flattenArray2D array2D =
    seq {
        for x in [ 0 .. (Array2D.length1 array2D) - 1 ] do
            for y in [ 0 .. (Array2D.length2 array2D) - 1 ] do
                yield array2D.[x, y]
    }

/// Matrix implementation matrix [N x M]
type MatrixFs<'a> =
    | ZeroMatrix of N: int * M: int * T: 'a
    | Matrix of 'a [,]

    /// Returns the number of rows of this matrix
    member m.Rows =
        match m with
        | ZeroMatrix (rows, _, _) -> rows
        | Matrix m -> Array2D.length1 m

    /// Returns the number of columns of this matrix
    member m.Columns =
        match m with
        | ZeroMatrix (_, columns, _) -> columns
        | Matrix m -> Array2D.length2 m

    /// Converts this matrix into a one dimensional sequence,
    /// scanning columns from left to right and rows from top to bottom
    member m.ToSeq() =
        match m with
        | ZeroMatrix (N, M, _) -> Array2D.zeroCreate N M |> Seq.cast<'a>
        | Matrix m -> m |> Seq.cast<'a>

    /// Returns an enumerator
    /// which can be used to
    /// iterate through this matrix
    member m.GetEnumerator() = m.ToSeq().GetEnumerator()

/// Module contains matrix operations implementation
module MatrixFs =
    let private flatten' arr2D = arr2D |> Seq.cast<'a>

    let private transpose' (arr2D: 'a [,]) =
        let N, M =
            Array2D.length1 arr2D, Array2D.length2 arr2D

        Array2D.init M N (fun row column -> arr2D.[column, row])

    let toArray2D matrix =
        match matrix with
        | Matrix m -> m
        | ZeroMatrix (rows, columns, _) -> Array2D.zeroCreate rows columns

    let toArray matrix =
        let arr2D = matrix |> toArray2D

        [| for i = 0 to matrix.Rows - 1 do
            yield
                [| for j = 0 to matrix.Columns - 1 do
                    yield arr2D.[i, j] |] |]

    // arr.[i, j] |> printfn "%A"
    // matr |> MatixFs.getItem integerSR (i, j) |> printfn "%A"
    let getItem (sr: Semiring<'a>) (i, j) matrix =
        match matrix with
        | Matrix m -> m.[i, j]
        | ZeroMatrix _ -> sr.IdentityElement

    /// Returns transposed matrix
    let transpose (matrix: MatrixFs<'a>) =
        match matrix with
        | ZeroMatrix (rows, columns, T) -> ZeroMatrix(columns, rows, T)
        | Matrix m -> m |> transpose' |> Matrix

    let getColumn columnIndex matrix =
        let xs = matrix |> toArray2D

        xs.[*, columnIndex..columnIndex]
        |> flatten'
        |> Seq.toArray

    let getRow rowIndex matrix =
        let xs = matrix |> toArray2D

        xs.[rowIndex..rowIndex, *]
        |> flatten'
        |> Seq.toArray

    // Task 2.1
    /// Multiplication in semiring
    let multiplyInSemiring (sr: Semiring<'a>) (matA: MatrixFs<'a>) (matB: MatrixFs<'a>) =
        let rowsA, colsA = matA.Rows, matA.Columns
        let rowsB, colsB = matB.Rows, matB.Columns
        // check matrix compatibility
        if colsA <> rowsB
        then invalidArg "matB" "Cannot multiply matrices of different dimensions"

        match matA, matB with
        | Matrix _, ZeroMatrix (_, colsB, T) -> ZeroMatrix(rowsA, colsB, T)
        | ZeroMatrix (rowsA, _, T), Matrix _ -> ZeroMatrix(rowsA, colsB, T)
        | ZeroMatrix (rowsA, _, T), ZeroMatrix (_, colsB, _) -> ZeroMatrix(rowsA, colsB, T)
        | Matrix arrA, Matrix arrB ->
            let result =
                Array2D.create rowsA colsB sr.IdentityElement

            for i in 0 .. rowsA - 1 do
                for j in 0 .. colsB - 1 do
                    for k in 0 .. colsA - 1 do
                        result.[i, j] <- sr.Add result.[i, j] (sr.Multiply arrA.[i, k] arrB.[k, j])

            result |> Matrix

    // [Important!] This is not a part of task 4;
    //      just a little bit faster multiplication
    //
    /// Multiplication in semiring
    /// using fsharp async & parallel computing
    let multiplyParallelInSemiring (sr: Semiring<'a>) (matA: MatrixFs<'a>) (matB: MatrixFs<'a>) =
        let rowsA, colsA = matA.Rows, matA.Columns
        let rowsB, colsB = matB.Rows, matB.Columns
        // check matrix compatibility
        if colsA <> rowsB
        then invalidArg "matB" "Cannot multiply matrices of different dimensions"

        match matA, matB with
        | Matrix _, ZeroMatrix (_, colsB, T) -> ZeroMatrix(rowsA, colsB, T)
        | ZeroMatrix (rowsA, _, T), Matrix _ -> ZeroMatrix(rowsA, colsB, T)
        | ZeroMatrix (rowsA, _, T), ZeroMatrix (_, colsB, _) -> ZeroMatrix(rowsA, colsB, T)
        | Matrix arrA, Matrix arrB ->
            let result =
                Array2D.create rowsA colsB sr.IdentityElement

            [ for i in 0 .. rowsA - 1 ->
                async {
                    for j in 0 .. colsB - 1 do
                        for k in 0 .. colsA - 1 do
                            result.[i, j] <- sr.Add result.[i, j] (sr.Multiply arrA.[i, k] arrB.[k, j])
                } ]
            |> Async.Parallel
            |> Async.RunSynchronously
            |> ignore

            result |> Matrix

    // Task 2.2
    /// Find transitive closure
    let findTransitiveClosure sr matrix =
        match matrix with
        | ZeroMatrix _ -> failwith "Could not find transitive closure in zero matrix"
        | Matrix m ->
            let pattern =
                m
                |> Array2D.map (fun x -> not <| isZeroInSemiring sr x)

            let matrix' =
                pattern
                |> Array2D.mapi (fun i j v -> (i = j) || v)
                |> Matrix

            seq { 0 .. matrix.Rows - 1 }
            |> Seq.fold (fun matrixOfDegreeK _ -> multiplyParallelInSemiring sr matrixOfDegreeK matrix') matrix'

    // Task 2.3
    /// Floyd-Warshall algorithm in semigroup with partial order
    let floydWarshallInSemigroup sg matrix =
        match matrix with
        | ZeroMatrix _ -> None
        | Matrix m ->
            let rowsArr = matrix |> toArray
            let columnsArr = matrix |> transpose |> toArray

            let folderMin state (x, y) =
                let func = sg.Multiply x y
                if sg.LE func state then func else state

            let inner columnsArr rowArr =
                let zipWithRow xs = Array.zip rowArr xs

                let statePairs =
                    (zipWithRow << Array.map zipWithRow) columnsArr

                let processPair (state, pairs) = Array.fold folderMin state <| pairs
                statePairs |> Array.map processPair

            if m.GetLength 0 = m.GetLength 1 then
                let partialMapper = inner columnsArr

                Array.map partialMapper rowsArr
                |> array2D
                |> Matrix
                |> Some
            else
                None

    /// Generates random square matrix of specified size
    let createRandomSquareMatrix (size: int) (nextRandom: unit -> 'a) =
        (fun _ _ -> nextRandom ())
        |> Array2D.init size size
        |> Matrix

    /// Generates random square matrix
    /// of specified dimensions [rows count x columns count]
    let createRandomMatrix rows columns (nextRandom: unit -> 'a) =
        (fun _ _ -> nextRandom ())
        |> Array2D.init rows columns
        |> Matrix
