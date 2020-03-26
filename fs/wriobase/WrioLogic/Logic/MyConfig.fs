namespace Wrio.Logic

type IMyConfig = 
    abstract member GetSysConnStr : unit -> string


type MyConfig() =
    let mutable sysConnStr = "abc"

    interface IMyConfig with
        member this.GetSysConnStr() = sysConnStr
        
    member this.SysConnStr
        with get () = sysConnStr
        and set (value) = sysConnStr <- value

