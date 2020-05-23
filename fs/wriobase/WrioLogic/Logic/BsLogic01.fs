namespace Wrio.Logic

open System.Text.Json

open Wrio.Util
open Wrio.Models
open Wrio.Db

module BsLogic01 =
    let hello name =
        sprintf "Hello %s" name

    let spanByDsTableId dsTableId (lstTupleDsJoin: (int * int * DsJoin) list) =
        let pred (x : (int * int * DsJoin)) = 
            let (dsTableIdTuple, seqNo, dsJoin) = x
            dsTableId = dsTableIdTuple
        MyUtil.span pred lstTupleDsJoin

    let rec zipDsTblJoin (lstDsTable : DsTable list) (lstTplDsJoin : (int * int * DsJoin) list) : DsTable list =
        let getDsJoinFromTuple lstTplDsJoin3 =
            let (_, _, lstDsJoin3)= lstTplDsJoin3
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

    //let getDtSetLogic (connStrSys: string) (datasetId: int) = 
    let getDtSetLogic (datasetId: int) (cfg: IMyConfig) : DtSet option = 
        //let dbSysConn = DbSystem.getDbSysConn connStrSys
        let sysConnStr = cfg.GetSysConnStr()
        let dbSysConn = DbSystem.getDbSysConn sysConnStr
        dbSysConn.Open()
        let lstDsTable = DbSystem.getDsTable dbSysConn datasetId

        let lstTupleDsJoin = DbSystem.getDsJoin dbSysConn datasetId

        // DsTable -> Fact
        // let dsTbl1s = 
        //     query {
        //         for x in lstDsTable do
        //         where (x.TableType = 1)
        //         select x
        //     }
        // let dst1 = Seq.head dsTbl1s
        let dst1 = lstDsTable.Head
        //let dtSet = DtSet(datasetId, dst1.TableName, dst1.TableAbbrev)

        //
        // DsTable -> Dimensions
        //
        // let sqDim = 
        //     query {
        //         for x in lstDsTable do
        //         where (x.TableType = 2)
        //         select (Dimension(x.TableName, x.TableAbbrev, "", "", "", 0))
        //     }
        let lstDsTbl2 = lstDsTable.Tail


        // dtSet.Dimensions <- Seq.toArray sqDim

        //dtSet.Dimensions.[0].

        //let lstDsJoin = DbSystem.getDsJoin dbSysConn datasetId

        //let lstPair = zipDsTblJoin lstDsTbl2 lstDsJoin

        // let sqDim = 
        //     query {
        //         for x in lstPair do
        //             let 
        //             select (Dimension(x.TableName, x.TableAbbrev, "", "", "", 0))
        //     }


        dbSysConn.Close()
        
        //Some dtSet
        None


    let getPivotLogic (pivotId: int) (cfg: IMyConfig) : Pivot option =
        let sysConnStr = cfg.GetSysConnStr()
        use dbSysConn = DbSystem.getDbSysConn sysConnStr
        dbSysConn.Open()


        let (datasetId, sSettingJson) = DbSystem.getPivotBase dbSysConn pivotId

        //let dtSet = DbSystem.getDtSet dbSysConn datasetId

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

        None
