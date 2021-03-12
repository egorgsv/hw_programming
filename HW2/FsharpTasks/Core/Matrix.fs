module Core.Matrix

open Core.AlgebraicStructures

/// Matrix implementation matrix [N x M]
type MatrixFs<'a when 'a: (static member Zero: 'a) and 'a: (static member (+): 'a * 'a -> 'a)> =
    | ZeroMatrix of N: int * M: int * T: 'a
    | Matrix of 'a [,]

    /// Returns the number of rows of this matrix
    member inline m.Rows =
        match m with
        | ZeroMatrix (rows, _, _) -> rows
        | Matrix m -> m.GetLength 0

    /// Returns the number of columns of this matrix
    member inline m.Columns =
        match m with
        | ZeroMatrix (_, columns, _) -> columns
        | Matrix m -> m.GetLength 1

    /// Converts this matrix into a one dimensional sequence,
    /// scanning columns from left to right and rows from top to bottom
    member inline m.ToSeq() =
        match m with
        | ZeroMatrix (N, M, _) -> Array2D.zeroCreate N M |> Seq.cast<'a>
        | Matrix m -> m |> Seq.cast<'a>

    /// Returns an enumerator
    /// which can be used to
    /// iterate through this matrix
    member inline m.GetEnumerator() = m.ToSeq().GetEnumerator()

    // Accessing to matrix elements by index
    member inline m.Item
        with get (i, j) =
            match m with
            | Matrix m -> m.[i, j]
            | ZeroMatrix _ -> LanguagePrimitives.GenericZero<'a>

/// Module contains matrix operations implementation
module MatrixFs =
    let inline flatten arr2D = arr2D |> Seq.cast<'a>

    let inline transpose' (arr2D: 'a [,]) =
        let N, M =
            arr2D |> Array2D.length1, arr2D |> Array2D.length2

        Array2D.init M N (fun row column -> arr2D.[column, row])

    let inline toArray2D matrix =
        match matrix with
        | Matrix m -> m
        | ZeroMatrix (rows, columns, _) -> Array2D.zeroCreate rows columns

    let inline toArray matrix =
        let arr2D = matrix |> toArray2D

        [| for i = 0 to matrix.Rows - 1 do
            yield
                [| for j = 0 to matrix.Columns - 1 do
                    yield arr2D.[i, j] |] |]

    /// Returns transposed matrix
    let inline transpose (matrix: MatrixFs<'a>) =
        match matrix with
        | ZeroMatrix (rows, columns, T) -> ZeroMatrix(columns, rows, T)
        | Matrix m -> m |> transpose' |> Matrix

    let inline getColumn columnIndex matrix =
        let xs = matrix |> toArray2D

        xs.[*, columnIndex..columnIndex]
        |> flatten
        |> Seq.toArray

    let inline getRow rowIndex matrix =
        let xs = matrix |> toArray2D

        xs.[rowIndex..rowIndex, *]
        |> flatten
        |> Seq.toArray

    /// Multiplication in semiring
    let inline multiplyInSemiring (sr: Semiring<'a>) (matA: MatrixFs<'a>) (matB: MatrixFs<'a>) =
        if (matA.Columns <> matB.Rows) then
            invalidArg "matB"
            <| "Cannot multiply matrices of different dimensions"

        let N', M' = matA.Rows, matB.Columns

        match matA, matB with
        | Matrix _, ZeroMatrix (_, M, T) -> ZeroMatrix(N', M, T)
        | ZeroMatrix (N, _, T), Matrix _ -> ZeroMatrix(N, M', T)
        | ZeroMatrix (N, _, T), ZeroMatrix (_, M, _) -> ZeroMatrix(N, M, T)
        | Matrix A, Matrix B ->
            let multiplyInSemiring x y = sr.Multiply x y

            let multiplyElements =
                (fun i j ->
                    [| 0 .. (matB.Rows - 1) |]
                    |> Array.sumBy (fun k -> multiplyInSemiring A.[i, k] B.[k, j]))

            Array2D.init N' M' multiplyElements |> Matrix

    /// Floyd-Warshall algorithm in semigroup with partial order
    let inline floydWarshallInSemigroup (sg: SemigroupPO<'a>) (matrix: MatrixFs<'a>) =
        match matrix with
        | ZeroMatrix _ -> None
        | Matrix m ->
            let rows = matrix |> toArray
            let columns = matrix |> transpose |> toArray

            let folderMin state (x, y) =
                let func = sg.Multiply x y
                if sg.LE func state then func else state

            let inner columns row =
                let zipWithRow xs = Array.zip row xs

                let statePairs =
                    (zipWithRow << Array.map zipWithRow) columns

                let processPair (state, pairs) = Array.fold folderMin state <| pairs

                statePairs |> Array.map processPair

            if m.GetLength 0 = m.GetLength 1 then
                let partialMapper = inner columns

                Array.map partialMapper rows
                |> array2D
                |> Matrix
                |> Some
            else
                None

    let inline createRandomSquareMatrix (size: int) (nextRandom: unit -> 'a) =
        (fun _ _ -> nextRandom ())
        |> Array2D.init size size
        |> Matrix