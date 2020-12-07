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

    let spanByDsTableId dsTableId (lstTupleDtJoin: (int * int * DtJoin) list) =
        let pred (x : (int * int * DtJoin)) = 
            let (dsTableIdTuple, seqNo, dsJoin) = x
            dsTableId = dsTableIdTuple
        MyUtil.span pred lstTupleDtJoin

    let rec zipDsTblJoin (lstDsTable : DsTable list) (lstTplDsJoin : (int * int * DtJoin) list) : DsTable list =
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


    //let getDtSetBase (dbSysConn: NpgsqlConnection) (datasetId: int) : DtSet option  =
    let getDtSetBase (ctx: WrioContext) (datasetId: int) : DtSet option  =
        //let lstDsTable = DbSystem.getDsTable dbSysConn datasetId
        let lstDsTable = DbSystem.getDsTable ctx datasetId

        //let lstTupleDsJoin = DbSystem.getDsJoin dbSysConn datasetId
        let lstTupleDsJoin = DbSystem.getDsJoin ctx datasetId
     
        let fact = lstDsTable.Head

        let dimensions = List.toArray <| zipDsTblJoin lstDsTable.Tail lstTupleDsJoin

        let dtSet = DtSet(datasetId, fact, dimensions)

        Some dtSet


    //let getDtSetLogic (connStrSys: string) (datasetId: int) = 
    //let getDtSetLogic (datasetId: int) (cfg: IMyConfig) : DtSet option = 
    let getDtSetLogic (ctx: WrioContext) (datasetId: int) : DtSet option = 
        ctx.LogDebug("BsLogic01.getDtSetLogic start")

        //let sysConnStr = ctx.Config.GetSysConnStr()
        //let dbSysConn = DbSystem.getDbSysConn sysConnStr
        DbSystem.connectDbSys ctx |> ignore
        DbSystem.openDbSys ctx |> ignore

        ctx.LogDebug("getDtSetLogic start")
        (*
        let lstDsTable = DbSystem.getDsTable dbSysConn datasetId

        let lstTupleDsJoin = DbSystem.getDsJoin dbSysConn datasetId
     
        let fact = lstDsTable.Head

        let dimensions = List.toArray <| zipDsTblJoin lstDsTable.Tail lstTupleDsJoin

        let dtSet = DtSet(datasetId, fact, dimensions)
        *)
        //let optDtSet = getDtSetBase dbSysConn datasetId
        let optDtSet = getDtSetBase ctx datasetId

        DbSystem.closeDbSys ctx |> ignore

        optDtSet


    //let getPivotLogic (pivotId: int) (cfg: IMyConfig) : Pivot option =
    let getPivotLogic (ctx: WrioContext) (pivotId: int)  : Pivot option =
        //let sysConnStr = cfg.GetSysConnStr()
        //use dbSysConn = DbSystem.getDbSysConn sysConnStr
        //dbSysConn.Open()
        DbSystem.connectDbSys ctx |> ignore
        DbSystem.openDbSys ctx |> ignore

        //let (datasetId, sSettingJson) = DbSystem.getPivotBase dbSysConn pivotId
        let (datasetId, sSettingJson) = DbSystem.getPivotBase ctx pivotId

        //let optDtset = getDtSetBase dbSysConn datasetId
        let optDtset = getDtSetBase ctx datasetId

        let sOpt = JsonSerializerOptions()
        sOpt.PropertyNamingPolicy <- JsonNamingPolicy.CamelCase

        let pvtSetting : PivotSetting = JsonSerializer.Deserialize<PivotSetting>(sSettingJson, sOpt)

        (*
        let pvt : Pivot = {
            PivotId = pivotId
            DatasetId = datasetId
            Setting = pvtSetting
            DtSet = dtSet
        }
        pvt
        *)

        let optPvt = 
            match optDtset with
                | Some dtSet -> 
(*
                    let pvt : Pivot = {
                        PivotId = pivotId
                        DatasetId = datasetId
                        Setting = pvtSetting
                        DtSet = dtSet
                    }
*)
                    let pvt = Pivot(pivotId, datasetId, pvtSetting, dtSet)
                    Some pvt
                | None -> None

        optPvt

    //let getPivotDataLogic (pvt: Pivot) (cfg: IMyConfig) : PivotData =
    let getPivotDataLogic (ctx: WrioContext) (pvt: Pivot)  : PivotData =
        // let usrConnStr = cfg.GetUsrConnStr()
        // use dbUsrConn = DbUserPg.getDbUsrConn usrConnStr
        // dbUsrConn.Open()

        DbUserPg.connectDbUsr ctx |> DbUserPg.openDbUsr |> ignore

        //DbUserPg.getPivotData dbUsrConn pvt 
        DbUserPg.getPivotData ctx pvt 

        




