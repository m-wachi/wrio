namespace Wrio.Common

//open System
open log4net
open Npgsql

type IWrioLogger = 
    abstract member LogDebug: s:string -> unit
    abstract member LogInformation: s:string -> unit
    //let logger = 

type ConsoleLogger() =
    interface IWrioLogger with
        member this.LogDebug(s: string) =
            printfn "%s" s
        member this.LogInformation(s: string) =
            printfn "%s" s

type Log4jLogger(pLog4jLogger: ILog) =
    let logger = pLog4jLogger
    interface IWrioLogger with
        member this.LogDebug(s: string) =
            logger.Debug(s)
        member this.LogInformation(s: string) =
            logger.Info(s)

type IWrioContext =
    abstract member LogDebug : s:string -> unit
    abstract member LogInformation : s:string -> unit
    abstract member ConnectDbSys: unit -> unit
    abstract member OpenDbSys: unit -> unit

type WrioContext(pConfig: IMyConfig) =
    let mutable logger: IWrioLogger = ConsoleLogger() :> IWrioLogger
    let mutable cfg: IMyConfig = pConfig
    let mutable connDbSys: NpgsqlConnection = null
    let mutable connDbUsr: NpgsqlConnection = null
    member this.LogDebug(s: string) =
        logger.LogDebug(s) |> ignore
    member this.LogInformation(s: string) =
        logger.LogInformation(s) |> ignore
    member this.SetLogger(pLogger: IWrioLogger) =
        logger <- pLogger
    member this.Config
        with get() : IMyConfig = cfg
        and set(v: IMyConfig ) = cfg <- v
    member this.ConnDbSys
        with get() : NpgsqlConnection = connDbSys
        and set(v: NpgsqlConnection) = connDbSys <- v
    member this.ConnDbUsr
        with get() : NpgsqlConnection = connDbUsr
        and set(v: NpgsqlConnection) = connDbUsr <- v


module WrioCommon =
    let logInformation (ctx: WrioContext) (s: string) =
        ctx.LogInformation(s) |> ignore
