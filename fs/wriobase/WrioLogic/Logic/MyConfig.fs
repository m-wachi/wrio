namespace Wrio.Logic

type IMyConfig = 
    abstract member GetSysConnStr : unit -> string
    abstract member GetUsrConnStr : unit -> string

type MyConfig() =
    let mutable sysConnStr : string = "abc"
    let mutable usrConnStr : string = ""

    interface IMyConfig with
        member this.GetSysConnStr() = sysConnStr
        member this.GetUsrConnStr() = usrConnStr    

    member this.SysConnStr
        with get () = sysConnStr
        and set (value) = sysConnStr <- value

    member this.UsrConnStr
        with get () = usrConnStr
        and set (value) = usrConnStr <- value
