namespace Wrio.Logic

open Wrio.Db

module BsLogic01 =
    let hello name =
        sprintf "Hello %s" name

    let getDtSetLogic (connStrSys: string) (datasetId: int) = 
        let dbSysConn = DbSystem.getDbSysConn connStrSys
        dbSysConn.Open()
        let dtSet = DbSystem.getDtSet dbSysConn datasetId

        dbSysConn.Close()
        
        dtSet