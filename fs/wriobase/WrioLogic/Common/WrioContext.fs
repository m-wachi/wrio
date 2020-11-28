namespace Wrio.Common

//open System
open log4net


type IWrioLogger = 
    abstract member LogDebug: s:string -> unit
    //let logger = 

type ConsoleLogger() =
    interface IWrioLogger with
        member this.LogDebug(s: string) =
            printfn "%s" s

type Log4jLogger(pLog4jLogger: ILog) =
    let logger = pLog4jLogger
    interface IWrioLogger with
        member this.LogDebug(s: string) =
            logger.Debug(s)

(*
type IWrioContext =
    abstract member LogDebug : s:string -> unit
*)

type WrioContext(pConfig: IMyConfig) =
    let mutable logger: IWrioLogger = ConsoleLogger() :> IWrioLogger
    let mutable cfg: IMyConfig = pConfig
    member this.LogDebug(s: string) =
        logger.LogDebug(s) |> ignore
    member this.SetLogger(pLogger: IWrioLogger) =
        logger <- pLogger
    member this.Config
        with get() : IMyConfig = cfg
        and set(v: IMyConfig ) = cfg <- v
