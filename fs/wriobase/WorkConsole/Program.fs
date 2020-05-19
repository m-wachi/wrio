﻿// Learn more about F# at http://fsharp.org

open System
open System.Text.Json
open Npgsql

open Wrio.Util
open Wrio.Models
open Wrio.Logic
open Wrio.Db

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


[<EntryPoint>]
let main argv =
    printfn "Hello World from F#!"

    let sJson1 = JsonSerializer.Serialize("hey hey")

    printfn "%s" sJson1
(*
    let dsJoin21: DbSysDsJoin = {
        DsTableId = 1; SeqNo = 1; DatasetId = 1;
        JoinSrcCol = "col1"; DstAbbrev = "b"; JoinDstCol = "col1_1";
        JoinDiv = 1
    }

    let dsJoin22 = {
        DsTableId = 1
        SeqNo = 2
        DatasetId = 1
        JoinSrcCol = "col1"
        DstAbbrev = "b"
        JoinDstCol = "col1_1"
        JoinDiv = 1
    }

    let dsJoin23 = {
        DsTableId = 2
        SeqNo = 1
        DatasetId = 1
        JoinSrcCol = "col1"
        DstAbbrev = "b"
        JoinDstCol = "col1_1"
        JoinDiv = 1
    }

    let lstDsJoin2 = [dsJoin21; dsJoin22; dsJoin23]
*)
    let dsJoin21: DsJoin = {
        JoinSrcCol = "col1"; DstAbbrev = "b"; JoinDstCol = "col1_1"
    }

    let dsJoin22 = {
        JoinSrcCol = "col1"; DstAbbrev = "b"; JoinDstCol = "col1_1"
    }

    let dsJoin23 = {
        JoinSrcCol = "col1"; DstAbbrev = "b"; JoinDstCol = "col1_1"
    }

    let lstTupleDsJoin2 = [(1, 1, dsJoin21); (1, 2, dsJoin22); (2, 1, dsJoin23)]

    (*
    let dsTable21 = {
        DsTableId = 1; TableAbbrev = "a"; TableName = "t_tbl01"; TableType = 2; JoinDiv = 1
    }

    let dsTable22 = {
        DsTableId = 2; TableAbbrev = "b"; TableName = "t_tbl02"; TableType = 2; JoinDiv = 1
    }

    let lstDsTable2 = [dsTable21; dsTable22]
    *)
    let dsJoin3 = {
        JoinSrcCol = "col1"
        DstAbbrev = "b"
        JoinDstCol = "col1_1"
    }

    let dm1 = Dimension("tbl1", "a1", 2, [|dsJoin3|])

    let sOpt = JsonSerializerOptions()
    sOpt.PropertyNamingPolicy <- JsonNamingPolicy.CamelCase

    let sJson3 = JsonSerializer.Serialize(dm1, sOpt)
    printfn "sJson3=%s" sJson3
    
    let dm1a : Dimension = JsonSerializer.Deserialize<Dimension>(sJson3, sOpt)

    printfn "dm1a="
    printfn "%A" dm1a

    let lst1 = [1; 2; 3; 4]

    let pred1 x = x < 3

    let spanRet1 = MyUtil.span pred1 lst1
    printfn "spanRet1 = %A" spanRet1

    let connStringTest = "Host=localhost;Username=wrio_test;Password=wrio_test;Database=wrio_test"
 
    let conn1 = new NpgsqlConnection(connStringTest)
    conn1.Open()

    let (datasetId1, settingJsonStr1) = DbSystem.getPivotBase conn1 1

    printfn "datasetId1=%d" datasetId1
    printfn "settingJsonStr1"
    printfn "%A" settingJsonStr1
    conn1.Close()

    let conn2 = new NpgsqlConnection(connStringTest)
    conn2.Open()

    let lstDsJoin1 = DbSystem.getDsJoin conn2 1

    printfn "lstDsJoin1"
    printfn "%A" lstDsJoin1

    conn2.Close()

    printfn "BsLogic01.getDtSetLogic"
    let cfg = MyConfig()
    cfg.SysConnStr <- connStringTest
    
    //let ds1 = BsLogic01.getDtSetLogic connStringTest 1
    //let sysConnStr = cfg.GetSysconnStr()
    let ds1 = BsLogic01.getDtSetLogic 3 cfg

    printfn "%s" (JsonSerializer.Serialize(ds1, sOpt))

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

    (*
    let conn1 = new NpgsqlConnection(connStringTest)
    conn1.Open()
    let sql1 =  "insert into m_dataset values(2, 'usr1', 'ds02', CURRENT_TIMESTAMP)"
  
    let sqlExecCnt = execSql conn1 sql1

    printfn "%A" sqlExecCnt
    *)
    (*
    let pvt2 = BsLogic01.getPivotLogic 1 cfg

    printfn "pvt2"
    printfn "%A" pvt2

    *)
    let pvtSt1 = PivotSetting()
    pvtSt1.DatasetId <- 3
    pvtSt1.RowHdr <- [|"rowHdr01"; "rowHdr02"|]
    pvtSt1.ColHdr <- [|"col1"; "col2"|]
    pvtSt1.RowOdr <- [|"row1"|]

    printfn "pvtSt1"
    printfn "%A" pvtSt1

    let sPvtSt1 = JsonSerializer.Serialize(pvtSt1, sOpt)

    printfn "sPvtSt1=%s" sPvtSt1

    (*
    let pvtSt2 : PivotSetting = JsonSerializer.Deserialize<PivotSetting>(settingJsonStr1, sOpt)
    printfn "settingJsonStr1=%s" settingJsonStr1
    printfn "pvtSt2.RowHdr[0]=%s" (pvtSt2.RowHdr.[0])

    let pvt1Op = BsLogic01.getPivotLogic 1 cfg

    let dtSet1 = DtSet(-1, "", "")

    let dummyPivot1 : Pivot = {
        PivotId = -1
        DatasetId = -1
        Setting = pvtSt1
        DtSet = dtSet1
    }

    let pvt1 = match pvt1Op with
                | Some x -> x
                | None -> dummyPivot1

    printfn "pvt1"
    printfn "%A" pvt1

    let sql1 = DbUserPg.toSql pvt1

    printfn "toSql pvt1 = [%s]" sql1

    let connStringUsrTest = "Host=localhost;Username=user02_test;Password=user02_test;Database=user02_test"

    let conn2 = new NpgsqlConnection(connStringUsrTest)
    conn2.Open()

    let rt1 = DbUserPg.usrPgMyfunc01 conn2
    printfn "rt1"
    printfn "%A" rt1

    let rt2 = DbUserPg.usrPgMyfunc02 conn2 pvt1
    printfn "rt2"
    printfn "%s" rt2


    conn2.Close()
    *)



    0 // return an integer exit code
