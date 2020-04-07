namespace Wrio.Logic

open Wrio.Db

module BsLogic01 =
    let hello name =
        sprintf "Hello %s" name

    //let getDtSetLogic (connStrSys: string) (datasetId: int) = 
    let getDtSetLogic (datasetId: int) (cfg: IMyConfig) = 
        //let dbSysConn = DbSystem.getDbSysConn connStrSys
        let sysConnStr = cfg.GetSysConnStr()
        let dbSysConn = DbSystem.getDbSysConn sysConnStr
        dbSysConn.Open()
        let dtSet = DbSystem.getDtSet dbSysConn datasetId

        dbSysConn.Close()
        
        dtSet

    let getPivotLogic (pivotId: int) (cfg: IMyConfig) =
        let sysConnStr = cfg.GetSysConnStr()
        use dbSysConn = DbSystem.getDbSysConn sysConnStr
        dbSysConn.Open()
        DbSystem.getPivot dbSysConn pivotId
