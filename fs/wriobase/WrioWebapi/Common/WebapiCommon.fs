namespace Wrio.Webapi.Common

open Wrio.Common
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.Logging

module WebapiCommon =
    let setupContext<'T> (config: IConfiguration) (logger : ILogger<'T>) : WrioContext=

        let myCfg = MyConfig()

        let cfgSec = config.GetSection("AppConfiguration")

        myCfg.SysConnStr <- cfgSec.GetValue("SystemConnectionString")
        myCfg.UsrConnStr <- cfgSec.GetValue("UserConnectionString")

        let ctx = WrioContext(myCfg)

        let aspLogger = AspLogger(logger)

        ctx.SetLogger(aspLogger)

        ctx