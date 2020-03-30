module WrioTests

open NUnit.Framework
open Npgsql

open Wrio.Models
open Wrio.Logic

let connStrDbSys = "Host=localhost;Username=wrio_test;Password=wrio_test;Database=wrio_test"

let getTestDbSysConn () =
    new NpgsqlConnection(connStrDbSys)

let execSql (conn: NpgsqlConnection) (sql: string) : int =
    let cmd = new NpgsqlCommand(sql, conn)
    cmd.ExecuteNonQuery()


[<SetUp>]
let Setup () =
    let conn = getTestDbSysConn ()
    conn.Open()
    //let tx = conn.BeginTransaction()
    execSql conn "delete from m_ds_table" |> ignore
    //tx.Commit()
    conn.Close()
    ()


let prepData01 () =
    let conn = new NpgsqlConnection(connStrDbSys)
    conn.Open()
    //let sql1 =  "insert into m_dataset values(1, 'usr1', 'ds01', CURRENT_TIMESTAMP)"
    let sql1 = "insert into m_ds_table values(3, 'f03', 't_table03', 1, NULL, NULL, NULL, NULL, CURRENT_TIMESTAMP)"
    let sql2 = "insert into m_ds_table values(3, 'd01', 'm_item', 2, 'item_cd', 'f01', 'item_cd', 2, CURRENT_TIMESTAMP)"

    //let cmd = new NpgsqlCommand(sql1, conn)
    //cmd.ExecuteNonQuery()
    execSql conn sql1 |> ignore
    execSql conn sql2 |> ignore

    conn.Close()


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
let Test04 () =
    prepData01 ()

    let cfg = new MyConfig()


    //let (ds: DtSet)  = BsLogic01.getDtSetLogic connStrDbSys 3
    let (ds: DtSet)  = BsLogic01.getDtSetLogic 3 cfg
    Assert.AreEqual(3, ds.DatasetId)
    Assert.AreEqual("t_table03", ds.FactTable)
    Assert.AreEqual("f03", ds.FactAbbrev)
