namespace Wrio.Controllers

open System
open System.Collections.Generic
open System.Linq
open System.Threading.Tasks
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open Microsoft.Extensions.Configuration
open Npgsql
open Wrio
open Wrio.Models
open Wrio.Db.DbSystem
open Wrio.Logic

[<ApiController>]
[<Route("[controller]")>]
type Sys01Controller (logger : ILogger<Sys01Controller>, config : IConfiguration) =
    inherit ControllerBase()

    let summaries = [| "Freezing"; "Bracing"; "Chilly"; "Cool"; "Mild"; "Warm"; "Balmy"; "Hot"; "Sweltering"; "Scorching" |]

    [<HttpGet>]
    member __.Get() : MyWork01Model =
        let a = MyWork01Model(1, "model01")
        a

    [<HttpGet("{idVal}")>]
    member __.GetMyWork01(idVal: int): MyWork01Model =
        MyWork01Model(idVal, "model02")

    //http://localhost:5000/sys01/pvt1
    [<HttpGet("pvt1")>]
    member __.GetPivot01(): Pivot option =
        let myCfg = MyConfig()

        let a = config.GetSection("AppConfiguration")

        myCfg.SysConnStr <- a.GetValue("SystemConnectionString")


        BsLogic01.getPivotLogic 1 myCfg 

