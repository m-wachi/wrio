module WrioTests2

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



let prepData02 (dbSysConn : NpgsqlConnection) =

    let sql1 = "insert into m_pivot values(1, 'usr1', 3, '{\"datasetId\": 1234, \"colHdr\": [], \"rowHdr\": [\"d01.item_name\"], \"rowOdr\": [\"d01.item_name\"]}', CURRENT_TIMESTAMP)"

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
    
