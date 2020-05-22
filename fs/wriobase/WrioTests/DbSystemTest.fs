namespace Wrio.Tests

open NUnit.Framework
open Npgsql

open Wrio.Models
open Wrio.Logic
open Wrio.Db

module DbSystemTest =
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
    let GetDtJoinTest01 () =
        let dbSysConn = getTestDbSysConn ()
        dbSysConn.Open()
        prepSysMDsJoin01 (dbSysConn)

        let lstTplDsJoin1 = DbSystem.getDsJoin dbSysConn 3

        dbSysConn.Close()

        Assert.AreEqual(1, lstTplDsJoin1.Length)

        let (dsTableId, seqNo, dsJoin1) = lstTplDsJoin1.Head

        Assert.AreEqual(3, dsTableId)
        Assert.AreEqual(1, seqNo)
        Assert.AreEqual("item_cd", dsJoin1.JoinSrcCol)
        Assert.AreEqual("f01", dsJoin1.DstAbbrev)
        Assert.AreEqual("item_cd", dsJoin1.JoinDstCol)

    [<Test>]
    let GetDtTableTest01 () =
        let dbSysConn = getTestDbSysConn ()
        dbSysConn.Open()
        prepMDsTable01 (dbSysConn)

        let lstDsTable1 = DbSystem.getDsTable dbSysConn 3

        dbSysConn.Close()

        Assert.AreEqual(2, lstDsTable1.Length)

        let dsTable1 = lstDsTable1.Head
        Assert.AreEqual(1, dsTable1.DsTableId)
        Assert.AreEqual("f03", dsTable1.Abbrev)
        Assert.AreEqual("t_table03", dsTable1.Table)
        Assert.AreEqual(1, dsTable1.TableType)
        Assert.AreEqual(-1, dsTable1.JoinDiv)

        let dsTable2 = lstDsTable1.Tail.Head
        Assert.AreEqual(2, dsTable2.DsTableId)
        Assert.AreEqual("d01", dsTable2.Abbrev)
        Assert.AreEqual("m_item", dsTable2.Table)
        Assert.AreEqual(2, dsTable2.TableType)
        Assert.AreEqual(1, dsTable2.JoinDiv)

