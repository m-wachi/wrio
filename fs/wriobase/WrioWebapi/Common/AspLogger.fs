namespace Wrio.Webapi.Common

open Microsoft.Extensions.Logging
open Wrio.Common

(*
type Log4jLogger(pLog4jLogger: ILog) =
    let logger = pLog4jLogger
    interface IWrioLogger with
        member this.LogDebug(s: string) =
            logger.Debug(s)
*)

type AspLogger(pLogger: ILogger) =
    let logger = pLogger
    interface IWrioLogger with
        member this.LogDebug(s: string) =
            logger.LogDebug(s)
        member this.LogInformation(s: string) = 
            logger.LogInformation(s)

            
       

