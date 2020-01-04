namespace Aspnet2.Controllers

open System
open System.Collections.Generic
open System.Linq
open System.Threading.Tasks
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open Aspnet2
open Aspnet2.Models

[<ApiController>]
[<Route("api/[controller]")>]
type MyWork01Controller (logger : ILogger<MyWork01Controller>) =
    inherit ControllerBase()

    let summaries = [| "Freezing"; "Bracing"; "Chilly"; "Cool"; "Mild"; "Warm"; "Balmy"; "Hot"; "Sweltering"; "Scorching" |]

    [<HttpGet>]
    member __.Get() : MyWork01Model =
        let a = MyWork01Model(1, "model01")
        a

    [<HttpGet("{idVal}")>]
    member __.GetMyWork01(idVal: int): MyWork01Model =
        MyWork01Model(idVal, "model02")

      