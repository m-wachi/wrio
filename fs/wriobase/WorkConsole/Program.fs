// Learn more about F# at http://fsharp.org

open System
open System.Data.Common
open System.Text.Json
open Npgsql
open log4net
open log4net.Config

open Wrio.Common
open Wrio.Util
open Wrio.Models
open Wrio.Logic
open Wrio.Db

(*
[<assembly: log4net.Config.XmlConfigurator(ConfigFile="log4net.config",Watch=true)>]
do()
*)

let execSql (conn: NpgsqlConnection) (sql: string) : int =
    let cmd = new NpgsqlCommand(sql, conn)
    cmd.ExecuteNonQuery()

(*
let rec func04 (lstDsTable : DbSysDsTable list) (lstDsJoin : DbSysDsJoin list) =
    if lstDsTable.IsEmpty then
        []
    else
        let dsTable1 = lstDsTable.Head
        let (lstDsJoin1, lstDsJoin2) = func02 dsTable1.DsTableId lstDsJoin
        (dsTable1, lstDsJoin1) :: func04 lstDsTable.Tail lstDsJoin2
*)
(*
let pairToDim (pairDsTblJoin : (DbSysDsTable, DbSysDsJoin)) =
    let (dsTbl, lstDsJoin) = pairDsTblJoin
    //Dimension(dsTbl.TableName, dsTbl.TableAbbrev, lstDsJoin)
*)

(*
let func01 acc (lstDsJoin1: DbSysDsJoin list) (lstDsJoin2 : DbSysDsJoin list) =
    if List.isEmpty lstDsJoin1 then
        acc
    else
        let dsJoin1 = lstDsJoin1.Head
        let retfunc03 dsJoin1 lstDsJoin2

        (dsJoin1, lst1, lst2)
*)

let getDsColumn(dtSet: DtSet) (colName1: string) : DsColumn option =
    let sepColName1 = colName1.Split(".")
    let abbrev1 = sepColName1.[0]
    let rawColName = sepColName1.[1]

    let fun1 (dsTable1: DsTable): DsColumn option = 
        let dsColumns1 = Array.filter (fun (x: DsColumn) -> x.ColName = rawColName)  dsTable1.Columns
        if dsColumns1.Length > 0 
            then Some dsColumns1.[0] 
            else None

    if dtSet.Fact.Abbrev = abbrev1 
        then  
            //let dsColumns1 = Array.filter (fun (x: DsColumn) -> x.ColName = rawColName)  dtSet.Fact.Columns
            //if dsColumns1.Length > 0 then Some dsColumns1.[0] 
            //                else None
            fun1 dtSet.Fact
        else
            let b = Array.filter (fun (x: DsTable) -> x.Abbrev = abbrev1) dtSet.Dimensions
            if b.Length > 0 
                then fun1 b.[0]
                else None

//TODO 
// move to correct .fs, write test
let getPivotColumns2 (pvtSetting: PivotSetting) (dtSet: DtSet) : DsColumn array =
    let pvtCols1: string array = Array.append pvtSetting.RowHdr pvtSetting.ColHdr

    let colName1: string = pvtCols1.[0]
    
    let pvtCols2: string array = 
        Array.append pvtCols1 
                     (Array.map (fun (x: CellVal) -> x.Abbrev + "." + x.ColName) 
                                pvtSetting.CellVal)
   
    let consOpt (ary: DsColumn list) (x: DsColumn option) =
        match x with
        | Some y -> y :: ary
        | None -> ary

    let lstOptDsColumn = List.map (getDsColumn dtSet) (Array.toList pvtCols2)

    let lstDsColumnRev = List.fold consOpt [] lstOptDsColumn
    
    List.rev lstDsColumnRev |> List.toArray

   
type MyLogger(pDummy: string) =
    let mutable logger : ILog = LogManager.GetLogger(typeof<MyLogger>)
    //let mutable dataSet : DtSet = pDataSet
    (*
    let mutable pivotId : int
    let mutable datasetId : int = pDatasetId
    let mutable settingJson : string
    *)
    (*
    member this.PivotId: int  = pivotId
    member this.DatasetId : int = datasetId
    member this.Setting : PivotSetting = setting
    member this.DataSet 
        with get() : DtSet = dataSet
        and set(v : DtSet) = dataSet <- v
        *)
    new() = MyLogger("")
    member this.LogInfo(s : string) = 
        logger.Info(s)



[<EntryPoint>]
let main argv =
    printfn "Hello World from F#!"

    let logger: ILog = LogManager.GetLogger("main")

    BasicConfigurator.Configure() |> ignore

    let sJson1 = JsonSerializer.Serialize("hey hey2")

    printfn "%s" sJson1

    logger.Info("logging: " + sJson1)

    let connStringTest = "Host=localhost;Username=wrio_test;Password=wrio_test;Database=wrio_test"
    let connString = "Host=localhost;Username=wrio_user;Password=wrio_user;Database=wrio01"
    let connStringUsr = "Host=localhost;Username=user02;Password=user02;Database=user02db"
    let connStringUsrTest = "Host=localhost;Username=user02_test;Password=user02_test;Database=user02_test"

    let cfg = MyConfig()
    cfg.SysConnStr <- connString
    cfg.UsrConnStr <- connStringUsr

    let ctx = WrioContext(cfg)

    ctx.SetLogger(Log4jLogger(logger)) |> ignore

    ctx.LogDebug("hello ctx.logging")


    let lg2 = MyLogger()
    lg2.LogInfo("aabbcc")

    let cfgTest = MyConfig()
    cfgTest.SysConnStr <- connStringTest
    cfgTest.UsrConnStr <- connStringUsrTest

    let ctxTest = WrioContext(cfgTest)

    let dsJoin21 = DsJoin("col1", "b", "col1_1")
    let dsJoin22 = DsJoin("col1", "b", "col1_1")
    let dsJoin23 = DsJoin("col1", "b", "col1_1")

    let lstTupleDsJoin2 = [(1, 1, dsJoin21); (1, 2, dsJoin22); (2, 1, dsJoin23)]

    let dsTable21 = DsTable(1, "tbl01", "a", 1, 0, [||], [||])
    let dsTable22 = DsTable(2, "tbl02", "b", 2, 1, [||], [||])

    let lstDsTable2 = [dsTable21; dsTable22]

    let dsJoin3 = DsJoin("col1", "b", "col1_1")

    let sOpt = JsonSerializerOptions()
    sOpt.PropertyNamingPolicy <- JsonNamingPolicy.CamelCase

(*
    let dm1 = Dimension("tbl1", "a1", 2, [|dsJoin3|])

    let sJson3 = JsonSerializer.Serialize(dm1, sOpt)
    printfn "sJson3=%s" sJson3
    
    let dm1a : Dimension = JsonSerializer.Deserialize<Dimension>(sJson3, sOpt)

    printfn "dm1a="
    printfn "%A" dm1a
*)

 


    //let conn1 = new NpgsqlConnection(connString)
    //conn1.Open()
    DbSystem.connectDbSys ctx |> DbSystem.openDbSys |> ignore

    let (datasetId1, settingJsonStr1) = DbSystem.getPivotBase ctx 1

    printfn "datasetId1=%d" datasetId1
    printfn "settingJsonStr1"
    printfn "%A" settingJsonStr1

    let pvtSt3 : PivotSetting = JsonSerializer.Deserialize<PivotSetting>(settingJsonStr1, sOpt)
    printfn "pvtSt3=%A" pvtSt3
    
    
    let lstDsTbl = DbSystem.getDsTable ctx 2
    printfn "lstDsTbl=%A" lstDsTbl

    let iResult1 = DbSystem.updatePivot 3 (3, settingJsonStr1) ctx
    printfn "updatePivot iResult1=%A" iResult1

    DbSystem.closeDbSys ctx |> ignore

    //let conn2 = new NpgsqlConnection(connStringTest)
    //conn2.Open()
(*
    DbSystem.connectDbSys ctxTest |> DbSystem.openDbSys |> ignore

    let lstDsJoin1 = DbSystem.getDsJoin ctxTest 1

    printfn "lstDsJoin1"
    printfn "%A" lstDsJoin1

    //conn2.Close()
    DbSystem.closeDbSys ctxTest |> ignore

    printfn "BsLogic01.getDtSetLogic"

    //let ds1 = BsLogic01.getDtSetLogic connStringTest 1
    //let sysConnStr = cfg.GetSysconnStr()
    //let ds1 = BsLogic01.getDtSetLogic 1 cfg
    let ds1 = BsLogic01.getDtSetLogic ctx 1 

    printfn "%s" (JsonSerializer.Serialize(ds1, sOpt))
    *)

    (*
    let func02Ret = func02 1 lstDsJoin2
    printfn "func02Ret"
    printfn "%A" func02Ret
    
    let func04Ret = func04 lstDsTable2 lstDsJoin2
    printfn "func04Ret"
    printfn "%A" func04Ret
    *)

    let sbdtRet1 = BsLogic01.spanByDsTableId 1 lstTupleDsJoin2

    printfn "sbdtRet1=%A" sbdtRet1

    let pvtSt1 = PivotSetting()
    pvtSt1.DatasetId <- 4
    pvtSt1.RowHdr <- [|"f.sales_date"|]
    pvtSt1.ColHdr <- [|"d1.item_name"|]
    pvtSt1.CellVal <- [|CellVal("nof_sales", "f", 1)|]
    pvtSt1.RowOdr <- [|"f.sales_date"|]


    printfn "pvtSt1"
    printfn "%A" pvtSt1

    let sPvtSt1 = JsonSerializer.Serialize(pvtSt1, sOpt)

    printfn "sPvtSt1=%s" sPvtSt1

    let pvtSt2 : PivotSetting = JsonSerializer.Deserialize<PivotSetting>(sPvtSt1, sOpt)
    
    printfn "pvtSt2.RowHdr[0]=%s" (pvtSt2.RowHdr.[0])

    //let optPvt1 = BsLogic01.getPivotLogic 1 cfg
    let optPvt1 = BsLogic01.getPivotLogic ctx 1
    printfn "optPvt1=%A" optPvt1

    (*
    let dummyPivot1 : Pivot = {
        PivotId = -1
        DatasetId = -1
        Setting = pvtSt1
        DtSet = DtSet()
    }
    *)
    let dummyPivot1 = Pivot(-1, -1, pvtSt1, DtSet())

    let pvt1 = match optPvt1 with
                | Some x -> x
                | None -> dummyPivot1

    printfn "pvt1"
    printfn "%A" pvt1

    let sql1 = DbUserPg.toSql pvt1

    printfn "toSql pvt1 = [%s]" sql1


    // let usrConn2 = new NpgsqlConnection(connStringUsrTest)
    // usrConn2.Open()

    DbUserPg.connectDbUsr ctxTest |> DbUserPg.openDbUsr |> ignore


    (*
    let rt1 = DbUserPg.usrPgMyfunc01 usrConn2
    printfn "rt1"
    printfn "%A" rt1

    let rt2 = DbUserPg.usrPgMyfunc02 usrConn2 pvt1
    printfn "rt2"
    printfn "%s" rt2
    *)

    //let pvtData = DbUserPg.getPivotData usrConn2 pvt1
    (*
    let pvtData = DbUserPg.getPivotData ctxTest pvt1
    printfn "pvtData=%A" pvtData

    //usrConn2.Close()
    DbUserPg.connectDbUsr ctxTest |> ignore
    

    //let pvtData2 = BsLogic01.getPivotDataLogic pvt1 cfg
    let pvtData2 = BsLogic01.getPivotDataLogic ctxTest pvt1 
    printfn "pvtData2=%A" pvtData2


    let pvtSt3 = PivotSetting()
    pvtSt3.DatasetId <- 2
    pvtSt3.RowHdr <- [|"f02.sales_date"|]
    pvtSt3.ColHdr <- [|"d02.merc_name"|]
    pvtSt3.CellVal <- [|CellVal("nof_sales", "f02", 1)|]
    pvtSt3.RowOdr <- [|"f02.sales_date"|]
    *)

    (*
    printfn "pvtSt3"
    printfn "%A" pvtSt3

    let sPvtSt3 = JsonSerializer.Serialize(pvtSt3, sOpt)

    printfn "sPvtSt3=%s" sPvtSt3
    *)

    let optPvt2 = BsLogic01.getPivotLogic ctx 2 
    printfn "optPvt2=%A" optPvt2

    let pvt2 = match optPvt2 with
                | Some x -> x
                | None -> dummyPivot1

    let sPvt2 = JsonSerializer.Serialize(pvt2, sOpt)


    printfn "pvt2=%A" sPvt2

    let dsCol1 = getDsColumn pvt2.DataSet "d02.merc_name"

    printfn "dsCol1=%A" dsCol1

    let dsColumns2 = getPivotColumns2 pvt2.Setting pvt2.DataSet

    printfn "dsColumns2=%A" dsColumns2

    //let pvtData3 = BsLogic01.getPivotDataLogic ctx pvt2
    //printfn "pvtData3=%A" pvtData3

    0 // return an integer exit code
