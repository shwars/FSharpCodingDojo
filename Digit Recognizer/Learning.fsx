// Простейшие концепции
let удвоить x = x*2
let удвоить_вещ x = x*2.

удвоить 1
удвоить (удвоить 1)
(удвоить >> удвоить) 1
1 |> удвоить |> удвоить
удвоить <| 1 |> удвоить

// Рекурсия
let rec solve f a b =
   let c = (b+a)/2.
   if (b-a)<0.001 then c
   else 
     if f(a)*f(c)<0. then solve f a c
     else solve f c b

solve (fun x -> -1.+x*x-sin x) -3. 3.

// Абстрагирование рекурсии

let rec state_rec init trans pred =
     if pred init then init
     else state_rec (trans init) trans pred

let solve f a b = state_rec (a,b)
                    (fun (a,b) ->
                       let c = (a+b)/2.
                       if f(a)*f(c)<0. then (a,c) else (c,b))
                    (fun (a,b) -> b-a<0.001)
                  |> fst

solve (fun x -> -1.+x*x-sin x) -3. 3.

// Учимся считывать файл

// Открываем библиотеки
// Чтобы код дальше работал, надо выполнить эти строчки в F# Interactive
// (выделить эти строчки и нажать Alt-Enter)
open System
open System.IO

// Функция для чтения файла как массива строк
let read fn = File.ReadAllLines(fn)
read @"d:\books\wap_1.txt"

// Три абстракции данных к F#: массивы, списки, последовательности
let array = [|1;2;3;4;5|]
let list = [1;2;3;4;5]
let sq = {1..5}

Array.length array
List.length list
Seq.length list

list |> Seq.length


// Базовые функции работы с данными
[1..5] |> List.map (fun x->x*2)
[1..5] |> List.filter (fun x->x%2=0)
[1..5] |> List.fold (fun acc x -> (if acc="" then "" else acc+",")+x.ToString()) ""

// Обрабатываем книгу:
let book = read @"d:\books\wap_1.txt"

// Выделяем массив слов, длиннее 3 символов
let words=
 book |> Array.map (fun s -> s.Split([|' ';';';'.';',';'!';'-';'!';'"'|]))
      |> Array.concat
      |> Array.filter (fun s -> s.Length>3)

// Какое самое длинное слово
words |> Array.maxBy(fun s -> s.Length)

// Распределение слов по длине
words |> Seq.groupBy (fun s -> s.Length)
      |> Seq.map (fun (n,s) -> (n, Seq.length s))
      |> Seq.sortBy fst


// Загружаем библиотеку визуализации
#load @"d:\winapp\lib\fsharp\FSharpChart-0.2\FSharpChart.fsx"
#load @"d:\winapp\lib\fsharp\FSharpChart-0.2\FSharpChartAutoDisplay.fsx"

open System.Drawing
open Samples.Charting
open Samples.Charting.ChartStyles
open System.Windows.Forms.DataVisualization.Charting

words |> Seq.groupBy (fun s -> s.Length)
      |> Seq.map (fun (n,s) -> (n, Seq.length s))
      |> Seq.sortBy fst
      |> FSharpChart.Bar

// Строим частотный словарь
words |> Seq.groupBy (fun s -> s)
      |> Seq.map (fun (n,s) -> (n, Seq.length s))
      |> Seq.sortBy (fun (n,s) -> -s)
      |> Seq.take 5
      |> FSharpChart.Bar






