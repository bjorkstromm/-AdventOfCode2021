type Operation =
    | Forward
    | Down
    | Up

type Instruction = {
    Operation : Operation
    Steps : int
}

type Position = {
    X : int
    Depth : int
}

let parseInstructions (xs : seq<string>) =
    let parseInstruction (str : string) =
        match str.Split(' ') with
        | [|op ; step|] ->
            match (op, (System.Int32.TryParse step)) with
            | ("forward", (true, s)) -> Some { Operation = Forward; Steps = s }
            | ("down", (true, s)) -> Some { Operation = Down; Steps = s }
            | ("up", (true, s)) -> Some { Operation = Up; Steps = s }
            | _ -> None
        | _ -> None

    xs
    |> Seq.map parseInstruction
    |> Seq.choose id
    |> Seq.toList

let getInstructions filename =
    System.IO.File.ReadAllLines filename
    |> parseInstructions

let processInstructions instructions =
    let processor current instruction =
        match instruction.Operation with
        | Forward -> { current with X = current.X + instruction.Steps }
        | Down -> { current with Depth = current.Depth + instruction.Steps }
        | Up -> { current with Depth = current.Depth - instruction.Steps }

    instructions
    |> Seq.fold processor { X = 0; Depth = 0 }

let getPosition filename =
    filename
    |> getInstructions
    |> processInstructions
    |> (fun i -> i.X * i.Depth)