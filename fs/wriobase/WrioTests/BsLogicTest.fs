namespace Wrio.Tests

open NUnit.Framework
open Npgsql

open Wrio.Models
open Wrio.Logic
open Wrio.Db

module BsLogicTest =
    let connStrDbSys = "Host=localhost;Username=wrio_test;Password=wrio_test;Database=wrio_test"

    let getTestDbSysConn () =
        new NpgsqlConnection(connStrDbSys)

    let execSql (conn: NpgsqlConnection) (sql: string) : int =
        let cmd = new NpgsqlCommand(sql, conn)
        cmd.ExecuteNonQuery()

    let prepSysMDsJoin01 (dbSysConn : NpgsqlConnection) =

        let sql1 = "insert into m_ds_join values(3, 1, 3, 'item_cd', 'f01', 'item_cd', CURRENT_TIMESTAMP);"
        //let cmd = new NpgsqlCommand(sql1, conn)
        //cmd.ExecuteNonQuery()
        execSql dbSysConn sql1 |> ignore
        ()


    let prepMDsTable01 (dbSysConn : NpgsqlConnection) =
        let sql1 = "insert into m_ds_table values(1, 3, 'f03', 't_table03', 1, NULL, CURRENT_TIMESTAMP)"
        let sql2 = "insert into m_ds_table values(2, 3, 'd01', 'm_item', 2, 1, CURRENT_TIMESTAMP)"

        execSql dbSysConn sql1 |> ignore
        execSql dbSysConn sql2 |> ignore    
    
    let prepLstTplData01 =
        let dsJoin21: DsJoin = {
            JoinSrcCol = "col1"; DstAbbrev = "b"; JoinDstCol = "col1_1"
        }
        let dsJoin22 = {
            JoinSrcCol = "col2"; DstAbbrev = "b"; JoinDstCol = "col1_1"
        }
        let dsJoin23 = {
            JoinSrcCol = "col3"; DstAbbrev = "c"; JoinDstCol = "col1_1"
        }
        [(1, 1, dsJoin21); (1, 2, dsJoin22); (2, 1, dsJoin23)]


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

    [<Test>]
    let spanByDsTableIdTest01 () =
        let lstTupleDsJoin2 = prepLstTplData01
 
        let (lst1, lst2)  = BsLogic01.spanByDsTableId 1 lstTupleDsJoin2
    
        Assert.AreEqual(2, lst1.Length)
        Assert.AreEqual(1, lst2.Length)

        let dsTableId1, seq1, dsJoin1 = lst1.Head
        Assert.AreEqual(1, dsTableId1)

        Assert.AreEqual("col1", dsJoin1.JoinSrcCol)

        let dsTableId2, seq2, dsJoin2 = lst1.Tail.Head

        Assert.AreEqual(1, dsTableId2)
        Assert.AreEqual("col2", dsJoin2.JoinSrcCol)
        
        let dsTableId3, seq3, dsJoin3 = lst2.Head

        Assert.AreEqual(2, dsTableId3)
        Assert.AreEqual("col3", dsJoin3.JoinSrcCol)

    [<Test>]
    let zipDsTblJoinTest01 () =
        let lstTupleDsJoin2 = prepLstTplData01
 
        let dsTable21 = DsTable(1, "tbl01", "a", 1, 0, [||])
        let dsTable22 = DsTable(2, "tbl02", "b", 2, 1, [||])
        let lstDsTable2 = [dsTable21; dsTable22]

        let zdtjRet1 = BsLogic01.zipDsTblJoin lstDsTable2 lstTupleDsJoin2

        let dsTable3 = zdtjRet1.Head

        let dsJoin1 = dsTable3

        Assert.AreEqual(1, dsTable3.DsTableId)
        Assert.AreEqual(2, dsTable3.DsJoins.Length)

        let dsTable4 = zdtjRet1.Tail.Head

        Assert.AreEqual(2, dsTable4.DsTableId)
        Assert.AreEqual(1, dsTable4.DsJoins.Length)

