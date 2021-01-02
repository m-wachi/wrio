namespace Wrio.Webapi.Controllers

open System
open System.Collections.Generic
open System.Linq
open System.Threading.Tasks
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open Microsoft.Extensions.Configuration
open Npgsql
open Wrio
open Wrio.Common
open Wrio.Util
open Wrio.Models
open Wrio.Db.DbSystem
open Wrio.Logic
open Wrio.Webapi.Common

[<ApiController>]
[<Route("[controller]")>]
type Sys01Controller (logger : ILogger<Sys01Controller>, config : IConfiguration) =
    inherit ControllerBase()

    //let summaries = [| "Freezing"; "Bracing"; "Chilly"; "Cool"; "Mild"; "Warm"; "Balmy"; "Hot"; "Sweltering"; "Scorching" |]

    [<HttpGet>]
    member __.Get() : MyWork01Model =
        let a = MyWork01Model(1, "model01")
        a

    [<HttpGet("{idVal}")>]
    member __.GetMyWork01(idVal: int): MyWork01Model =
        MyWork01Model(idVal, "model02")

    //http://localhost:5000/sys01/pvt/{pivotId}
    [<HttpGet("pvt/{pivotId}")>]
    member this.GetPivot01(pivotId: int): Pivot option =

        logger.LogInformation("Sys01Controller.GetPivot01 called. pivotId=%d", pivotId)

        this.HttpContext.Session.Set("testkey", System.Text.Encoding.ASCII.GetBytes("abc"))
        //.Set("testkey",     ) |> ignore

        this.HttpContext.Response.Cookies.Append("test2key", "test2val") |> ignore

        let myCfg = MyConfig()

        let a = config.GetSection("AppConfiguration")

        myCfg.SysConnStr <- a.GetValue("SystemConnectionString")

        let ctx = WrioContext(myCfg)

        BsLogic01.getPivotLogic ctx pivotId


    // return PivotData option
    // PUT: http://localhost:5000/sys01/pvt/{pivotId}
    [<HttpPut("pvt/{pivotId}")>]
    member this.PutPivot01(pivotId: int, pvt: Pivot): PivotData option =

        let myCfg = MyConfig()

        let a = config.GetSection("AppConfiguration")

        myCfg.SysConnStr <- a.GetValue("SystemConnectionString")
        myCfg.UsrConnStr <- a.GetValue("UserConnectionString")

        let ctx = WrioContext(myCfg)

        let aspLogger = AspLogger(logger)

        ctx.SetLogger(aspLogger)

        logger.LogInformation("logger.LogDebug on PutPivot01.")

        ctx.LogInformation("PutPivot01 start.")
        sprintf "pivotId=%d, pvt.DatasetId=%d" pivotId pvt.DatasetId |> WrioCommon.logInformation ctx |> ignore

        (*
        let optPvt1 = BsLogic01.getPivotLogic 1 myCfg 
        
        match optPvt1 with
            | Some pvt1 -> 
                let pvtData =  BsLogic01.getPivotDataLogic pvt1 myCfg
                Some pvtData
            | None -> None
        *)

        //let dtSet = 
        //let optDtSet = BsLogic01.getDtSetLogic pvt.DatasetId myCfg
        let optDtSet = BsLogic01.getDtSetLogic ctx pvt.DatasetId
        match optDtSet with
            | Some dtSet -> 
                pvt.DataSet <- dtSet
                //Some (BsLogic01.getPivotDataLogic pvt myCfg)
                Some (BsLogic01.getPivotDataLogic ctx pvt)
            | None -> None


    [<HttpGet("pvtdt1")>]
    member this.GetPivotData01(): PivotData option =

        let myCfg = MyConfig()

        let a = config.GetSection("AppConfiguration")

        myCfg.SysConnStr <- a.GetValue("SystemConnectionString")
        myCfg.UsrConnStr <- a.GetValue("UserConnectionString")

        let ctx = WrioContext(myCfg)

        let optPvt1 = BsLogic01.getPivotLogic ctx 1
        
        match optPvt1 with
            | Some pvt1 -> 
                //let pvtData =  BsLogic01.getPivotDataLogic pvt1 myCfg
                let pvtData =  BsLogic01.getPivotDataLogic ctx pvt1
                Some pvtData
            | None -> None



