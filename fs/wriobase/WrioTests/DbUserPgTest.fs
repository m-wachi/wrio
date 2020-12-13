namespace Wrio.Tests

open NUnit.Framework
open Npgsql

open Wrio.Common
open Wrio.Models
open Wrio.Logic
open Wrio.Db

module DbUserPgTest =
    let connStrDbUsr = "Host=localhost;Username=user02_test;Password=user02_test;Database=user02_test"

    let getTestDbUsrConn() =
        new NpgsqlConnection(connStrDbUsr)

    let getWrioContext () : WrioContext =
        let cfg = MyConfig()
        cfg.UsrConnStr <- connStrDbUsr
        let ctx = WrioContext(cfg)
        ctx

    let getDsJoinData01() = 
        let dtJoin1: DtJoin = {JoinSrcCol = "col1"; DstAbbrev = "b"; JoinDstCol = "col1_1"}
        dtJoin1

    let getTestPivot01() =
        let dtJoin1: DtJoin = {JoinSrcCol = "col0101"; DstAbbrev = "m"; JoinDstCol = "col0001"}
        let dim1: DsTable = DsTable(2, "t_tbl01", "t1", 1, 1, [|dtJoin1|])
        let fact: DsTable = DsTable(1, "t_main", "m", 1, 1, [||])

        let dtSet1 = DtSet(4, fact, [|dim1|])

        let pvtSt1 = PivotSetting()
        pvtSt1.DatasetId <- 4
        pvtSt1.RowHdr <- [|"t1.rowHdr01"; "m.rowHdr02"|]
        pvtSt1.ColHdr <- [|"m.col1"; "m.col2"|]
        pvtSt1.RowOdr <- [|"t1.row1"|]
        pvtSt1.CellVal <- [|CellVal("col3", "m", 0); CellVal("col4", "m", 1)|]
        let pvt1 = Pivot(2, 4, pvtSt1, dtSet1)
        pvt1


    let getTestPivot02() =
        let dtJoin1: DtJoin = {JoinSrcCol = "item_cd"; DstAbbrev = "f"; JoinDstCol = "item_cd"}
        let dim1: DsTable = DsTable(2, "m_item", "d1", 1, 1, [|dtJoin1|])
        let fact: DsTable = DsTable(1, "t_table01", "f", 1, 1, [||])

        let dtSet1 = DtSet(4, fact, [|dim1|])

        let pvtSt1 = PivotSetting()
        pvtSt1.DatasetId <- 4
        pvtSt1.RowHdr <- [|"f.sales_date"|]
        pvtSt1.ColHdr <- [|"d1.item_name"|]
        pvtSt1.CellVal <- [|CellVal("nof_sales", "f", 1)|]
        pvtSt1.RowOdr <- [|"f.sales_date"|]
        
        let pvt1 = Pivot(2, 4, pvtSt1, dtSet1)
        pvt1

    let execSql (conn: NpgsqlConnection) (sql: string) : int =
        let cmd = new NpgsqlCommand(sql, conn)
        cmd.ExecuteNonQuery()

    let prepUsrTTable01 (dbUsrConn : NpgsqlConnection) =

        let sql1 = "insert into t_table01 values('2019-07-01', '001', 'A0001', 10, 5250)"
        let sql2 = "insert into t_table01 values('2019-07-01', '001', 'A0002', 15, 1500)"
        let sql3 = "insert into t_table01 values('2019-07-02', '001', 'A0001', 8, 4200)"
        let sql4 = "insert into t_table01 values('2019-07-02', '001', 'A0002', 12, 1200)"

        execSql dbUsrConn sql1 |> ignore
        execSql dbUsrConn sql2 |> ignore
        execSql dbUsrConn sql3 |> ignore
        execSql dbUsrConn sql4 |> ignore
        ()

    let prepUsrMItem01 (dbUsrConn : NpgsqlConnection) =

        let sql1 = "insert into m_item values('A0001', '001', 'アイテム０１')"
        let sql2 = "insert into m_item values('A0002', '001', 'アイテム０２')"

        execSql dbUsrConn sql1 |> ignore
        execSql dbUsrConn sql2 |> ignore
        ()


    [<SetUp>]
    let Setup () =
        let dbUsrConn = getTestDbUsrConn ()
        dbUsrConn.Open()
        //let tx = conn.BeginTransaction()
        execSql dbUsrConn "delete from t_table01" |> ignore
        execSql dbUsrConn "delete from m_item" |> ignore

        //tx.Commit()
        dbUsrConn.Close()
        ()


    [<Test>]
    let GetDsJoinTest01 () =
        let dsJoin1 = getDsJoinData01()

        let sRes = DbUserPg.joinCondSql "a" dsJoin1

        Assert.AreEqual("b.col1_1 = a.col1", sRes)
 
    [<Test>]
    let UserPgToSqlTest01 () =

        let pvt1 = getTestPivot01()

        let sqlExp1 = 
            "SELECT t1.rowHdr01, m.rowHdr02, \nm.col1, m.col2, \n" +
            "SUM(m.col3) col3, SUM(m.col4) col4\n" +
            " FROM t_main m\n" +
            "  INNER JOIN t_tbl01 t1 \n" +
            "    ON m.col0001 = t1.col0101\n" +
            " GROUP BY t1.rowHdr01, m.rowHdr02, \nm.col1, m.col2\n" +
            " ORDER BY t1.rowHdr01, m.rowHdr02, \nm.col1, m.col2 "

        let sqlAct1 = DbUserPg.toSql pvt1

        Assert.AreEqual(sqlExp1, sqlAct1)

    [<Test>]
    let UserPgToSqlTest02 () =

        let pvt2 = getTestPivot02()

        let sqlExp2 = 
            "SELECT f.sales_date, \nd1.item_name, \nSUM(f.nof_sales) nof_sales\n" +
            " FROM t_table01 f\n" +
            "  INNER JOIN m_item d1 \n" + 
            "    ON f.item_cd = d1.item_cd\n " +
            "GROUP BY f.sales_date, \nd1.item_name\n " +
            "ORDER BY f.sales_date, \nd1.item_name "

        let sqlAct2 = DbUserPg.toSql pvt2

        Assert.AreEqual(sqlExp2, sqlAct2)


    [<Test>]
    let UserPgGetPivotDataTest01 () =
        // let dbUsrConn = getTestDbUsrConn()
        // dbUsrConn.Open()

        let ctx = getWrioContext()
        DbUserPg.connectDbUsr ctx |> DbUserPg.openDbUsr |> ignore        

        prepUsrMItem01(ctx.ConnDbUsr)
        prepUsrTTable01(ctx.ConnDbUsr)

        let pvt1 = getTestPivot02()

        //let pvtData = DbUserPg.getPivotData dbUsrConn pvt1
        let pvtData = DbUserPg.getPivotData ctx pvt1

        //dbUsrConn.Close()
        DbUserPg.closeDbUsr ctx |> ignore

        //printfn "pvtData=%A" pvtData

        Assert.AreEqual(3, pvtData.ColNames.Length)
        Assert.AreEqual("sales_date", pvtData.ColNames.[0])
        Assert.AreEqual("item_name", pvtData.ColNames.[1])
        Assert.AreEqual("nof_sales", pvtData.ColNames.[2])
        
        let mutable i = 0
        Assert.AreEqual(4, pvtData.Rows.Length)
        Assert.AreEqual("2019/07/01", pvtData.Rows.[i].[0].ToString().Substring(0, 10))
        Assert.AreEqual("アイテム０１", pvtData.Rows.[i].[1])
        Assert.AreEqual(10, pvtData.Rows.[i].[2])

        i <- 1
        Assert.AreEqual("2019/07/01", pvtData.Rows.[i].[0].ToString().Substring(0, 10))
        Assert.AreEqual("アイテム０２", pvtData.Rows.[i].[1])
        Assert.AreEqual(15, pvtData.Rows.[i].[2])

        i <- 2
        Assert.AreEqual("2019/07/02", pvtData.Rows.[i].[0].ToString().Substring(0, 10))
        Assert.AreEqual("アイテム０１", pvtData.Rows.[i].[1])
        Assert.AreEqual(8, pvtData.Rows.[i].[2])

        i <- 3
        Assert.AreEqual("2019/07/02", pvtData.Rows.[i].[0].ToString().Substring(0, 10))
        Assert.AreEqual("アイテム０２", pvtData.Rows.[i].[1])
        Assert.AreEqual(12, pvtData.Rows.[i].[2])



   
