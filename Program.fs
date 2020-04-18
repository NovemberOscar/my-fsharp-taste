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


open System.Net
open System
open System.IO

let fetchUrl callback url =
    let req = WebRequest.Create(Uri(url))
    use resp = req.GetResponse()
    use stream = resp.GetResponseStream()
    use reader = new IO.StreamReader(stream)
    callback reader url

let myCallback (reader: IO.StreamReader) url =
    let html = reader.ReadToEnd()
    printfn "Downloaded %s. Fist 100 is: \n%s" url (html.Substring(0, 100))
    html

let exampledotcom = fetchUrl myCallback "http://example.com"

let curriedFetchUrl = fetchUrl myCallback

let kernelorgsign = curriedFetchUrl "https://cdn.kernel.org/pub/linux/kernel/v5.x/linux-5.6.tar.sign"

let kernelSignUrls =
    [ "https://cdn.kernel.org/pub/linux/kernel/v5.x/linux-5.6.2.tar.sign"
      "https://cdn.kernel.org/pub/linux/kernel/v5.x/linux-5.5.15.tar.sign"
      "https://cdn.kernel.org/pub/linux/kernel/v5.x/linux-5.4.30.tar.sign" ]

kernelSignUrls
|> List.map curriedFetchUrl
|> printfn "fetched kernel.org signs = %A"

// Algebraic types

type IntAndBool =
    { intPart: int
      boolType: bool } // "product" type

type IntOrBool =
    | IntChoice of int
    | BoolChoice of bool // "sum/union" type

// Pattern matching for flow of control
let rec iter list = // Loops are generally done using recursion
    match list with
    | [] -> printfn ""
    | first :: rest ->
        printfn "%d" first
        iter rest

iter [ 1; 2; 3 ]

// Pattern matching with union types

type Shape =
    | Circle of radius: int
    | Rectangle of height: int * width: int
    | Point of x: int * y: int
    | Polygon of pointList: (int * int) list

let draw shape =
    match shape with
    | Circle rad -> printfn "The circle has a radius of %d" rad
    | Rectangle(height, width) -> printfn "The rectangle is %d high by %d wide" height width
    | Polygon points -> printfn "The polygon is made of these points %A" points
    | _ -> printfn "I don't recognize this shape"

[ Circle(10)
  Rectangle(4, 5)
  Point(2, 3)
  Polygon
      ([ (1, 2)
         (2, 3) ]) ]
|> List.iter draw


// Using functions to extract boilerplate code
let product n = [ 1 .. n ] |> List.fold (fun productSoFar y -> productSoFar * y) 1

product 10 |> printfn "product 10 = %d"

let sumOfOdds n =
    [ 1 .. n ]
        |> List.fold (fun sumSoFar x ->
                if x % 2 = 0 then sumSoFar
                else sumSoFar + x) 0

let alternatingSum n =
    let f (isNeg, sumSoFar) x =
        if isNeg then (false, sumSoFar - x)
        else (true, sumSoFar + x)
    [ 1 .. n ] |> List.fold f (true, 0) |> snd

let sumOfSquaresFold n = [ 1 .. n ] |> List.fold (fun sumSoFar x -> sumSoFar + (x * x)) 0

type NameAndSize =
    { Name: string
      Size: int }

let maxNameAndSize (list: List<NameAndSize>) =
    let innerMaxNameAndSize first rest =
        rest |> List.fold (fun maxSoFar x ->
                           if maxSoFar.Size < x.Size then x
                           else maxSoFar) first
    match list with
    | [] -> None
    | first :: rest -> Some(innerMaxNameAndSize first rest)

let maxNameAndSizeBuiltIn (list: List<NameAndSize>) =
    match list with
    | [] -> None
    | _ -> Some(list |> List.maxBy (fun item -> item.Size))

// Using functions as building blocks

let add2 x = x + 2
let mult3 x = x * 3

[ 1 .. 10 ] |> List.map add2 |> printfn "%A"

[ 1 .. 10 ] |> List.map mult3 |> printfn "%A"

let mult2ThenMult3 = add2 >> mult3

[ 1 .. 10 ] |> List.map mult2ThenMult3 |> printfn "%A"

let logMsg msg x =
    printf "%s%i" msg x
    x //without linefeed

let logMsgN msg x =
    printfn "%s%i" msg x
    x //with linefeed

let mult3ThenSquareLogged =
    logMsg "before="
    >> mult3
    >> logMsg " after mult3="
    >> square
    >> logMsgN " result="

mult3ThenSquareLogged 5 |> printfn "%A"

[ 1 .. 10 ] |> List.map mult3ThenSquareLogged |> printfn "%A"


// DSLs

type DateScale =
    | Hour
    | Hours
    | Day
    | Days
    | Week
    | Weeks

type DateDirection =
    | Ago
    | Hence

let getDate interval scale direction =
    let absHours =
        match scale with
        | Hour | Hours -> 1 * interval
        | Day | Days -> 24 * interval
        | Week | Weeks -> 7 * 24 * interval

    let signedHours =
        match direction with
        | Ago -> -1 * absHours
        | Hence -> absHours

    System.DateTime.Now.AddHours(float signedHours)

getDate 5 Days Ago |> printfn "5 Days Ago = %A"
getDate 2 Hour Hence |> printfn "2 Hour Hence = %A"

type FluentShape =
    { label: string
      color: string
      onClick: FluentShape -> FluentShape }

let defaultShape =
    { label = ""
      color = ""
      onClick = fun shape -> shape }

let click (shape: FluentShape) = shape.onClick shape

let display shape =
    printfn "My label=%s and my color=%s" shape.label shape.color
    shape

let setLabel label shape = { shape with FluentShape.label = label }

let setColor color shape = { shape with FluentShape.color = color }

let setClickAction action shape = { shape with FluentShape.onClick = shape.onClick >> action }

let setRedBox = setColor "red" >> setLabel "box"

let setBlueBox = setRedBox >> setColor "blue"

let changeColorOnClick color = setColor color |> setClickAction

let redBox = defaultShape |> setRedBox
let blueBox = defaultShape |> setBlueBox

redBox
    |> display
    |> changeColorOnClick "green"
    |> click
    |> display
    |> ignore

blueBox
    |> display
    |> setClickAction (setLabel "box2" >> setColor "green")
    |> click
    |> display
    |> ignore
    
let rainbow =
    ["red";"orange";"yellow";"green";"blue";"indigo";"violet"]

let showRainbow =
    rainbow
    |> List.map (fun color -> setColor color >> display)
    |> List.reduce (>>)


defaultShape
    |> showRainbow
    |> ignore

type Address = { Street: string; City: string; }   
type Customer = { ID: int; Name: string; Address: Address}

let customer1 = { ID = 1; Name = "Bob"; 
      Address = {Street="123 Main"; City="NY" } }

let { Name=name1 } = customer1 
printfn "The customer is called %s" name1

type FileErrorReason = 
    | FileNotFound of string
    | UnauthorizedAccess of string * System.Exception
    | Unexpected of string
    
let readFile filePath =
    try
        use sr = new System.IO.StreamReader(filePath:string)
        Ok(sr.ToString())
    with
        | :? System.IO.FileNotFoundException as ex -> Error ex.Message
        | :? System.Security.SecurityException as ex -> Error ex.Message
        | :? System.Exception as ex -> Error ex.Message

readFile "helloword.txt" |> printfn "%A"

// Using the type system to ensure correct code

type EmailAddress = EmailAddress of string

let sendEmail (EmailAddress email) =
    printfn "sent an email to %s" email
    
let aliceEmail = EmailAddress "alice@example.com"
sendEmail aliceEmail

// sendEmail "bob@example.com"   //error

[<Measure>]
type cm

[<Measure>] 
type inches

[<Measure>] 
type feet =
   static member toInches(feet : float<feet>) : float<inches> = 
      feet * 12.0<inches/feet>

let meter = 100.0<cm>
let yard = 3.0<feet>

let yardInInches = feet.toInches(yard)

// yard + meter  // Error

[<Measure>] 
type GBP


[<Measure>] 
type USD

let gbp10 = 10.0<GBP>
let usd10 = 10.0<USD>
//gbp10 + gbp10             // allowed: same currency
//gbp10 + usd10             // not allowed: different currency
//gbp10 + 1.0               // not allowed: didn't specify a currency
gbp10 + 1.0<_>            // allowed using wildcard


[<EntryPoint>]
let main argv =
    printfn "Hello World from F#!"
    0 // return an integer exit code
