namespace Wrio.Controllers

type IMyConfig = 
    abstract member GetSysConnStr : unit -> string


type MyConfig() =
    let mutable sysConnStr = ""

    interface IMyConfig with
        member this.GetSysConnStr() = sysConnStr
        
    member this.SysConnStr
        with get () = sysConnStr
        and set (value) = sysConnStr <- value

