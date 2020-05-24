namespace Wrio.Tests

open NUnit.Framework
open Npgsql

open Wrio.Models
open Wrio.Logic
open Wrio.Db

module DbUserPgTest =
    let connStrDbSys = "Host=localhost;Username=wrio_test;Password=wrio_test;Database=wrio_test"

    let getDsJoinData01() = 
        let dtJoin1: DtJoin = {JoinSrcCol = "col1"; DstAbbrev = "b"; JoinDstCol = "col1_1"}
        dtJoin1


    [<Test>]
    let GetDsJoinTest01 () =
        let dsJoin1 = getDsJoinData01()

        let sRes = DbUserPg.joinCondSql "a" dsJoin1

        Assert.AreEqual("b.col1_1 = a.col1", sRes)
 
    [<Test>]
    let UserPgToSqlTest01 () =
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

        let sqlExp1 = 
            "SELECT t1.rowHdr01, m.rowHdr02, \nm.col1, m.col2\n" +
            " FROM t_main m\n" +
            "  INNER JOIN t_tbl01 t1 \n" +
            "    ON m.col0001 = t1.col0101\n"

        let sqlAct1 = DbUserPg.toSql pvt1

        Assert.AreEqual(sqlExp1, sqlAct1)



   
