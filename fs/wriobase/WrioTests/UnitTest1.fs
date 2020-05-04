module WrioTests

open NUnit.Framework
open Npgsql

open Wrio.Models
open Wrio.Logic
open Wrio.Db

let connStrDbSys = "Host=localhost;Username=wrio_test;Password=wrio_test;Database=wrio_test"

let getTestDbSysConn () =
    new NpgsqlConnection(connStrDbSys)

let execSql (conn: NpgsqlConnection) (sql: string) : int =
    let cmd = new NpgsqlCommand(sql, conn)
    cmd.ExecuteNonQuery()


[<SetUp>]
let Setup () =
    let dbSysConn = getTestDbSysConn ()
    dbSysConn.Open()
    //let tx = conn.BeginTransaction()
    execSql dbSysConn "delete from m_ds_table" |> ignore
    execSql dbSysConn "delete from m_pivot" |> ignore
    execSql dbSysConn "delete from m_ds_join" |> ignore

    //tx.Commit()
    dbSysConn.Close()
    ()


let prepData01 (dbSysConn : NpgsqlConnection) =
    let sql1 = "insert into m_ds_table values(3, 'f03', 't_table03', 1, NULL, NULL, NULL, NULL, CURRENT_TIMESTAMP)"
    let sql2 = "insert into m_ds_table values(3, 'd01', 'm_item', 2, 'item_cd', 'f01', 'item_cd', 2, CURRENT_TIMESTAMP)"

    execSql dbSysConn sql1 |> ignore
    execSql dbSysConn sql2 |> ignore



let prepData02 (dbSysConn : NpgsqlConnection) =

    let sql1 = "insert into m_pivot values(1, 'usr1', 3, '{\"datasetId\": 1234, \"colHdr\": [], \"rowHdr\": [\"d01.item_name\"], \"rowOdr\": [\"d01.item_name\"]}', CURRENT_TIMESTAMP)"

    //let cmd = new NpgsqlCommand(sql1, conn)
    //cmd.ExecuteNonQuery()
    execSql dbSysConn sql1 |> ignore
    ()

let prepData03 (dbSysConn : NpgsqlConnection) =

    let sql1 = "insert into m_ds_join values(3, 1, 3, 'item_cd', 'f01', 'item_cd', 2, CURRENT_TIMESTAMP);"
    //let cmd = new NpgsqlCommand(sql1, conn)
    //cmd.ExecuteNonQuery()
    execSql dbSysConn sql1 |> ignore
    ()


[<Test>]
let Test1 () =
    Assert.Pass()

[<Test>]
let Test2 () = 
    Assert.Pass()
[<Test>]
let Test3 () =
    let sRet = DummyLogic.hello "John"
    Assert.AreEqual("Hello John", sRet)
    //prepData01 ()

[<Test>]
let DbSystemGetDtJoinTest01 () =
    let dbSysConn = getTestDbSysConn ()
    dbSysConn.Open()
    prepData03 (dbSysConn)
    dbSysConn.Close()

    Assert.Fail("now implementing..")

(*
[<Test>]
let Test04 () =

    let dbSysConn = getTestDbSysConn ()
    dbSysConn.Open()
    prepData01 (dbSysConn)

    dbSysConn.Close()

    let cfg = MyConfig()
    cfg.SysConnStr <- connStrDbSys

    //let (ds: DtSet)  = BsLogic01.getDtSetLogic connStrDbSys 3
    let (ds: DtSet)  = BsLogic01.getDtSetLogic 3 cfg
    Assert.AreEqual(3, ds.DatasetId)
    Assert.AreEqual("t_table03", ds.FactTable)
    Assert.AreEqual("f03", ds.FactAbbrev)

[<Test>]
let GetPivotLogicTest01 () =

    let dbSysConn = getTestDbSysConn ()
    dbSysConn.Open()
    prepData01 (dbSysConn)
    prepData02 (dbSysConn)
    dbSysConn.Close()

    let cfg = MyConfig()
    cfg.SysConnStr <- connStrDbSys

    //let (ds: DtSet)  = BsLogic01.getDtSetLogic connStrDbSys 3
    let (pvt: Pivot)  = BsLogic01.getPivotLogic 1 cfg
    Assert.AreEqual(1, pvt.PivotId)
    Assert.AreEqual(3, pvt.DatasetId)
    Assert.AreEqual("d01.item_name", pvt.Setting.RowHdr.[0])
*)

[<Test>]
let UserPgToSqlTest01 () =

    let dim1 = Dimension("t_tbl01", "t1", "col0101", "m", "col0001", 1)

    let dtSet1 = DtSet(4, "t_main", "m")
    dtSet1.Dimensions <- [dim1]

    let pvtSt1 = PivotSetting()
    pvtSt1.DatasetId <- 4
    pvtSt1.RowHdr <- [|"t1.rowHdr01"; "m.rowHdr02"|]
    pvtSt1.ColHdr <- [|"m.col1"; "m.col2"|]
    pvtSt1.RowOdr <- [|"t1.row1"|]
    
    let pvt1 : Pivot = {
        PivotId = 2
        DatasetId = 4
        Setting = pvtSt1
        DtSet = dtSet1
    }

    let sqlExp1 = 
        "SELECT t1.rowHdr01, m.rowHdr02, \nm.col1, m.col2\n" +
        " FROM t_main m\n" +
        "  INNER JOIN t_tbl01 t1 \n" +
        "    ON m.col0001 = t1.col0101\n"

    let sqlAct1 = DbUserPg.toSql pvt1

    Assert.AreEqual(sqlExp1, sqlAct1)


    
