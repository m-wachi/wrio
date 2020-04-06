﻿// Learn more about F# at http://fsharp.org

open System
open System.Text.Json
open Npgsql

open Wrio.Models
open Wrio.Logic
open Wrio.Db

let execSql (conn: NpgsqlConnection) (sql: string) : int =
    let cmd = new NpgsqlCommand(sql, conn)
    cmd.ExecuteNonQuery()


[<EntryPoint>]
let main argv =
    printfn "Hello World from F#!"

    let sJson1 = JsonSerializer.Serialize("hey hey")

    printfn "%s" sJson1

    let dm1 = Dimension("tbl1", "a1", "col1", "d1", "col2", 2)

    let sJson2 = JsonSerializer.Serialize(dm1)

    printfn "%s" sJson2

    let sOpt = JsonSerializerOptions()
    sOpt.PropertyNamingPolicy <- JsonNamingPolicy.CamelCase

    let sJson3 = JsonSerializer.Serialize(dm1, sOpt)
    printfn "%s" sJson3
    
    let connStringTest = "Host=localhost;Username=wrio_test;Password=wrio_test;Database=wrio_test"
 
    let cfg = MyConfig()
    cfg.SysConnStr <- connStringTest
    
    //let ds1 = BsLogic01.getDtSetLogic connStringTest 1
    //let sysConnStr = cfg.GetSysconnStr()
    let ds1 = BsLogic01.getDtSetLogic 1 cfg

    printfn "%s" (JsonSerializer.Serialize(ds1, sOpt))

    (*
    let conn1 = new NpgsqlConnection(connStringTest)
    conn1.Open()
    let sql1 =  "insert into m_dataset values(2, 'usr1', 'ds02', CURRENT_TIMESTAMP)"
  
    let sqlExecCnt = execSql conn1 sql1

    printfn "%A" sqlExecCnt
    *)
    let conn1 = new NpgsqlConnection(connStringTest)
    conn1.Open()

    let (datasetId1, settingJsonStr1) = DbSystem.getPivotBase conn1 1
    conn1.Close()

    printfn "datasetId1=%d" datasetId1
    printfn "settingJsonStr1"
    printfn "%A" settingJsonStr1

    0 // return an integer exit code
