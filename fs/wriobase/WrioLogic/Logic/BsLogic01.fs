namespace Wrio.Logic

open System.Text.Json
open Wrio.Models
open Wrio.Db

module BsLogic01 =
    let hello name =
        sprintf "Hello %s" name

    
    //let getDtSetLogic (connStrSys: string) (datasetId: int) = 
    let getDtSetLogic (datasetId: int) (cfg: IMyConfig) : DtSet option = 
        //let dbSysConn = DbSystem.getDbSysConn connStrSys
        let sysConnStr = cfg.GetSysConnStr()
        let dbSysConn = DbSystem.getDbSysConn sysConnStr
        dbSysConn.Open()
        let lstDsTable = DbSystem.getDsTable dbSysConn datasetId

        //
        // DsTable -> Fact
        //
        let dsTbl1s = 
            query {
                for x in lstDsTable do
                where (x.TableType = 1)
                select x
            }

        let dst1 = Seq.head dsTbl1s
        let dtSet = DtSet(datasetId, dst1.TableName, dst1.TableAbbrev)

        //
        // DsTable -> Dimensions
        //
        let sqDim = 
            query {
                for x in lstDsTable do
                where (x.TableType = 2)
                select (Dimension(x.TableName, x.TableAbbrev, "", "", "", 0))
            }

        dtSet.Dimensions <- Seq.toArray sqDim

        //dtSet.Dimensions.[0].

        let lstDsJoin = DbSystem.getDsJoin dbSysConn datasetId

        dbSysConn.Close()
        
        Some dtSet


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
