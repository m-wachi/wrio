namespace Wrio.Logic

open System.Text.Json
open Npgsql

open Wrio.Common
open Wrio.Util
open Wrio.Models
open Wrio.Db

module BsLogic01 =
    let hello name =
        sprintf "Hello %s" name


    let runLogic (ctx: WrioContext) funcLogic =
        try
            DbSystem.connectDbSys ctx |> DbSystem.openDbSys |> ignore
            DbUserPg.connectDbUsr ctx |> DbUserPg.openDbUsr |> ignore

            let retVal = funcLogic ctx
            retVal
        finally
            DbUserPg.closeDbUsr ctx |> ignore
            DbSystem.closeDbSys ctx |> ignore


    let spanByDsTableId dsTableId (lstTupleDtJoin: (int * int * DsJoin) list) =
        let pred (x : (int * int * DsJoin)) = 
            let (dsTableIdTuple, seqNo, dsJoin) = x
            dsTableId = dsTableIdTuple
        MyUtil.span pred lstTupleDtJoin

    let rec zipDsTblJoin (lstDsTable : DsTable list) (lstTplDsJoin : (int * int * DsJoin) list) : DsTable list =
        let getDsJoinFromTuple lstTplDtJoin3 =
            let (_, _, lstDsJoin3)= lstTplDtJoin3
            lstDsJoin3

        if lstDsTable.IsEmpty then
            []
        else
            let dsTable1 = lstDsTable.Head
            let (lstTplDsJoin1, lstTplDsJoin2) = spanByDsTableId dsTable1.DsTableId lstTplDsJoin
            dsTable1.DsJoins <- List.toArray (List.map getDsJoinFromTuple lstTplDsJoin1)
            dsTable1 :: zipDsTblJoin lstDsTable.Tail lstTplDsJoin2

(*    
    let pairToDim (pairDsTblJoin : (DbSysDsTable, DbSysDsJoin)) =
        let (dsTbl, lstDsJoin) = pairDsTblJoin
        //Dimension(dsTbl.TableName, dsTbl.TableAbbrev, "", "", "", 0)
*)

    let getDtSetBase (ctx: WrioContext) (datasetId: int) : DtSet option  =
        ctx.LogInformation("BsLogic01.getDtSetBase start")

        sprintf "datasetId=%d" datasetId |> WrioCommon.logInformation ctx |> ignore

        let lstDsTable0 = DbSystem.getDsTable ctx datasetId

        sprintf "lstDsTable0.Length=%d" lstDsTable0.Length |> WrioCommon.logInformation ctx |> ignore

        let setColumns (x: DsTable) = 
            //WrioCommon.logInformation ctx ("setColumns: table=" + x.Table)
            x.Columns <- DbUserPg.getColumns ctx x.Table
            x

        let lstDsTable : DsTable list = List.map setColumns lstDsTable0 

        let lstTupleDsJoin = DbSystem.getDsJoin ctx datasetId
     
        sprintf "lstTupleDsJoin.Length=%d" lstTupleDsJoin.Length |> WrioCommon.logInformation ctx |> ignore

        let fact = lstDsTable.Head

        // fact.Columns <- DbUserPg.getColumns ctx fact.Table

        let dimensions = List.toArray <| zipDsTblJoin lstDsTable.Tail lstTupleDsJoin

        let dtSet = DtSet(datasetId, fact, dimensions)

        Some dtSet


    let getDtSetLogic (ctx: WrioContext) (datasetId: int) : DtSet option = 
        ctx.LogInformation("BsLogic01.getDtSetLogic start")
        sprintf "datasetId=%d" datasetId |> WrioCommon.logInformation ctx |> ignore

        DbSystem.connectDbSys ctx |> ignore
        DbSystem.openDbSys ctx |> ignore

        DbUserPg.connectDbUsr ctx |> DbUserPg.openDbUsr |> ignore

        let optDtSet = getDtSetBase ctx datasetId

        DbSystem.closeDbSys ctx |> ignore
        DbUserPg.closeDbUsr ctx |> ignore

        optDtSet


    let getPivotLogic (ctx: WrioContext) (pivotId: int)  : Pivot option =

        DbSystem.connectDbSys ctx |> DbSystem.openDbSys |> ignore

        DbUserPg.connectDbUsr ctx |> DbUserPg.openDbUsr |> ignore

        let (datasetId, sSettingJson) = DbSystem.getPivotBase ctx pivotId

        let optDtset = getDtSetBase ctx datasetId

        let sOpt = JsonSerializerOptions()
        sOpt.PropertyNamingPolicy <- JsonNamingPolicy.CamelCase

        let pvtSetting : PivotSetting = JsonSerializer.Deserialize<PivotSetting>(sSettingJson, sOpt)

        let optPvt = 
            match optDtset with
                | Some dtSet -> 
                    let pvt = Pivot(pivotId, datasetId, pvtSetting, dtSet)
                    Some pvt
                | None -> None

        DbSystem.closeDbSys ctx |> ignore
        DbUserPg.closeDbUsr ctx |> ignore

        optPvt

    let savePivot (pvt: Pivot) (ctx: WrioContext) =
        let sSettingJson = MyUtil.toJson<PivotSetting> pvt.Setting
        let iUpdResult = DbSystem.updatePivot pvt.PivotId (pvt.DatasetId, sSettingJson) ctx

        if 0 = iUpdResult then
            0   //TODO do insert pivot
        else 
            iUpdResult


    let putPivotLogic (pvt: Pivot) (ctx: WrioContext)  : PivotData option =
        savePivot pvt ctx |> ignore

        let optDtSet = getDtSetBase ctx pvt.DatasetId
        match optDtSet with
            | Some dtSet ->  
                pvt.DataSet <- dtSet
                DbUserPg.getPivotData ctx pvt 

            | None -> None

    let getPivotDataLogic (ctx: WrioContext) (pvt: Pivot)  : PivotData option =

        DbUserPg.connectDbUsr ctx |> DbUserPg.openDbUsr |> ignore

        let optPvtData = DbUserPg.getPivotData ctx pvt 

        DbUserPg.closeDbUsr ctx |> ignore

        optPvtData
    (*
    let getColumnsLogic (ctx: WrioContext) (datasetId: int) =

        WrioCommon.logInformation ctx "getColumnsLogic called."

        DbSystem.connectDbSys ctx |> DbSystem.openDbSys |> ignore

        let optDtset = getDtSetBase ctx datasetId

        DbSystem.closeDbSys ctx |> ignore

        let retVal = 
            match optDtset with
                | Some dtSet -> 
                    DbUserPg.connectDbUsr ctx |> DbUserPg.openDbUsr |> ignore
                    let aryColNames = DbUserPg.getColumns ctx dtSet.Fact.Table
                    DbSystem.closeDbSys ctx |> ignore
                    aryColNames
                | None -> [||]

        retVal
    *)


