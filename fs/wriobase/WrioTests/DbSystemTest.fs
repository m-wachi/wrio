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

        let sql1 = "insert into m_ds_join values(3, 1, 3, 'item_cd', 'f01', 'item_cd', 2, CURRENT_TIMESTAMP);"
        //let cmd = new NpgsqlCommand(sql1, conn)
        //cmd.ExecuteNonQuery()
        execSql dbSysConn sql1 |> ignore
        ()


    let prepMDsTable01 (dbSysConn : NpgsqlConnection) =
        let sql1 = "insert into m_ds_table values(1, 3, 'f03', 't_table03', 1, CURRENT_TIMESTAMP)"
        let sql2 = "insert into m_ds_table values(2, 3, 'd01', 'm_item', 2, CURRENT_TIMESTAMP)"

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

        let lstDsJoin1 = DbSystem.getDsJoin dbSysConn 3

        dbSysConn.Close()

        Assert.AreEqual(1, lstDsJoin1.Length)

        let dsJoin1 = lstDsJoin1.Head

        Assert.AreEqual(3, dsJoin1.DsTableId)
        Assert.AreEqual(1, dsJoin1.SeqNo)
        Assert.AreEqual(3, dsJoin1.DatasetId)
        Assert.AreEqual("item_cd", dsJoin1.JoinSrcCol)
        Assert.AreEqual("f01", dsJoin1.DstAbbrev)
        Assert.AreEqual("item_cd", dsJoin1.JoinDstCol)
        Assert.AreEqual(2, dsJoin1.JoinDiv)

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
        Assert.AreEqual("f03", dsTable1.TableAbbrev)
        Assert.AreEqual("t_table03", dsTable1.TableName)
        Assert.AreEqual(1, dsTable1.TableType)

        let dsTable2 = lstDsTable1.Tail.Head
        Assert.AreEqual(2, dsTable2.DsTableId)
        Assert.AreEqual("d01", dsTable2.TableAbbrev)
        Assert.AreEqual("m_item", dsTable2.TableName)
        Assert.AreEqual(2, dsTable2.TableType)

