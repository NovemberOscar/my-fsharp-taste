// Learn more about F# at http://fsharp.org

open System

// Variables
let myInt = 5
let myFloat = 3.15
let myString = "String"

// Lists
let twoToFive = [ 2; 3; 4; 5 ]
let oneToFive = 1 :: twoToFive
let zeroToFive = [ 0; 1 ] @ oneToFive

// Functions

let inline square x = x * x

square 1.14 |> printfn "square 1.14 = %f"
square 3 |> printfn "square 3 = %d"

let add x y = x + y

add 2 3 |> printfn "add 2 3 = %d"

let evens list =
    let isEven x = x % 2 = 0
    List.filter isEven list

evens oneToFive |> printfn "evens oneToFive = %A"

let SumOfSquaresTo100 = List.sum (List.map square [ 1 .. 100 ])

SumOfSquaresTo100 |> printfn "SumOfSquaresTo100 = %d"

let SumOfSquaresTo100Piped =
    [ 1 .. 100 ]
    |> List.map square
    |> List.sum

SumOfSquaresTo100Piped |> printfn "SumOfSquaresTo100 = %d"

let SumOfSquaresTo100Lambda =
    [ 1 .. 100 ]
    |> List.map (fun x -> x * x)
    |> List.sum

SumOfSquaresTo100Lambda |> printfn "SumOfSquaresTo100Lambda = %d"


// Pattern Matching

let simplePatternMatch =
    let x = "a"
    match x with
    | "a" -> printfn "x is a"
    | "b" -> printfn "x is b"
    | _ -> printfn "x is something else"

let validValue: int Option = Some 99
let invalidVaue: Option<int> = None

let optionPatternMatch input =
    match input with
    | Some v -> printfn "input is an int %d" v
    | None -> printfn "input is a null"

optionPatternMatch validValue
optionPatternMatch invalidVaue

// Complex data types

let twoTuple = 1, 2
let threeTuple = "a", 2, true

type Person =
    { First: string
      Last: string }

let person1 =
    { First = "john"
      Last = "doe" }

type Temp =
    | DegreesC of float
    | DegreesF of float

let tempInF = DegreesF 98.6
let tempInC = DegreesC 36.5

type Employee =
    | Worker of Person
    | Manager of Employee list

let jdoe =
    { First = "John"
      Last = "Doe" }

let worker = Worker jdoe

// Printfn

printfn "Printing an int %i, a float %f, a bool %b" 1 2.0 true
printfn "A string %s, and something generic %A" "hello" [ 1; 2; 3; 4 ]
printfn "twoTuple=%A,\nPerson=%A,\nTemp=%A,\nEmployee=%A" twoTuple person1 tempInF worker

// Sorting with F#

let rec qSort list =
    match list with
    | [] -> []
    | firstElm :: otherElms ->
        let smallerElms =
            otherElms
            |> List.filter (fun e -> e < firstElm)
            |> qSort

        let largerElms =
            otherElms
            |> List.filter (fun e -> e >= firstElm)
            |> qSort

        smallerElms @ [ firstElm ] @ largerElms

qSort [ 1; 5; 23; 18; 9; 1; 3 ] |> printfn "qSort -> %A"

let rec NewQSort =
    function
    | [] -> []
    | first :: rest ->
        let smaller, larger = List.partition ((>=) first) rest
        NewQSort smaller @ [ first ] @ NewQSort larger

NewQSort [ 1; 5; 23; 18; 9; 1; 3 ] |> printfn "NewQSort -> %A"


[<EntryPoint>]
let main argv =
    printfn "Hello World from F#!"
    0 // return an integer exit code
