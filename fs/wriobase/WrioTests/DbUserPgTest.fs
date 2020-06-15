namespace Wrio.Tests

open NUnit.Framework
open Npgsql

open Wrio.Models
open Wrio.Logic
open Wrio.Db

module DbUserPgTest =
    let connStrDbUsr = "Host=localhost;Username=user02_test;Password=user02_test;Database=user02_test"

    let getTestDbUsrConn() =
        new NpgsqlConnection(connStrDbUsr)


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
        let pvt1 : Pivot = {
            PivotId = 2
            DatasetId = 4
            Setting = pvtSt1
            DtSet = dtSet1
        }
        pvt1


    let getTestPivot02() =
        let dtJoin1: DtJoin = {JoinSrcCol = "item_cd"; DstAbbrev = "f"; JoinDstCol = "item_cd"}
        let dim1: DsTable = DsTable(2, "m_item", "d1", 1, 1, [|dtJoin1|])
        let fact: DsTable = DsTable(1, "t_table01", "f", 1, 1, [||])

        let dtSet1 = DtSet(4, fact, [|dim1|])

        let pvtSt1 = PivotSetting()
        pvtSt1.DatasetId <- 4
        pvtSt1.RowHdr <- [|"f.sales_date"|]
        pvtSt1.ColHdr <- [|"itm.item_name"|]
        pvtSt1.CellVal <- [|CellVal("nof_sales", "f", 1)|]
        pvtSt1.RowOdr <- [|"f.sales_date"|]
        
        let pvt1 : Pivot = {
            PivotId = 2
            DatasetId = 4
            Setting = pvtSt1
            DtSet = dtSet1
        }
        pvt1


    [<Test>]
    let GetDsJoinTest01 () =
        let dsJoin1 = getDsJoinData01()

        let sRes = DbUserPg.joinCondSql "a" dsJoin1

        Assert.AreEqual("b.col1_1 = a.col1", sRes)
 
    [<Test>]
    let UserPgToSqlTest01 () =
        (*
        let dtJoin1: DtJoin = {JoinSrcCol = "col0101"; DstAbbrev = "m"; JoinDstCol = "col0001"}
        let dim1: DsTable = DsTable(2, "t_tbl01", "t1", 1, 1, [|dtJoin1|])
        let fact: DsTable = DsTable(1, "t_main", "m", 1, 1, [||])
        //let dim1 = Dimension("t_tbl01", "t1", 1, [||])

        let dtSet1 = DtSet(4, fact, [|dim1|])

        //dtSet1.Dimensions <- [|dim1|]

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
        *)

        let pvt1 = getTestPivot01()

        let sqlExp1 = 
            "SELECT t1.rowHdr01, m.rowHdr02, \nm.col1, m.col2, \n" +
            "SUM(m.col3) col3, SUM(m.col4) col4\n" +
            " FROM t_main m\n" +
            "  INNER JOIN t_tbl01 t1 \n" +
            "    ON m.col0001 = t1.col0101\n"

        let sqlAct1 = DbUserPg.toSql pvt1

        Assert.AreEqual(sqlExp1, sqlAct1)

    [<Test>]
    let UserPgToSqlTest02 () =

        let pvt2 = getTestPivot02()

        let sqlExp2 = 
            "SELECT f.sales_date, \nitm.item_name, \nSUM(f.nof_sales) nof_sales\n" +
            " FROM t_table01 f\n" +
            "  INNER JOIN m_item itm " + 
            "    ON f.item_cd = itm.item_cd " +
            "GROUP BY f.sales_date, itm.item_name "

        let sqlAct2 = DbUserPg.toSql pvt2

        Assert.AreEqual(sqlExp2, sqlAct2)


    [<Test>]
    let UserPgGetPivotDataTest01 () =
        let dbUsrConn = getTestDbUsrConn()
        dbUsrConn.Open()
        let pvt1 = getTestPivot01()

        let pvtData = DbUserPg.getPivotData dbUsrConn pvt1

        dbUsrConn.Close()

        printfn "pvtData=%A" pvtData


        Assert.Fail("not implemented yet.")


   
