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

        let lstDsTable = DbSystem.getDsTable ctx datasetId

        sprintf "lstDsTable.Length=%d" lstDsTable.Length |> WrioCommon.logInformation ctx |> ignore

        let lstTupleDsJoin = DbSystem.getDsJoin ctx datasetId
     
        sprintf "lstTupleDsJoin.Length=%d" lstTupleDsJoin.Length |> WrioCommon.logInformation ctx |> ignore

        let fact = lstDsTable.Head

        fact.Columns <- DbUserPg.getColumns ctx fact.Table

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

        DbSystem.connectDbSys ctx |> ignore
        DbSystem.openDbSys ctx |> ignore

        let (datasetId, sSettingJson) = DbSystem.getPivotBase ctx pivotId

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


    let getPivotDataLogic (ctx: WrioContext) (pvt: Pivot)  : PivotData =

        DbUserPg.connectDbUsr ctx |> DbUserPg.openDbUsr |> ignore

        DbUserPg.getPivotData ctx pvt 

        
    //let getColumnsLogic (ctx: WrioContext) (dtSet: DtSet) =
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



