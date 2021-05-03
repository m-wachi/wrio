namespace Wrio.Tests

open NUnit.Framework
open Npgsql

open Wrio.Common
open Wrio.Models
open Wrio.Logic
open Wrio.Db

module WrioModel01Test =
    let connStrDbSys = "Host=localhost;Username=wrio_test;Password=wrio_test;Database=wrio_test"
    let connStrDbUsr = "Host=localhost;Username=user02_test;Password=user02_test;Database=user02_test"

    let getTestDbSysConn () =
        new NpgsqlConnection(connStrDbSys)

    let getWrioContext () : WrioContext =
        let cfg = MyConfig()
        cfg.SysConnStr <- connStrDbSys
        cfg.UsrConnStr <- connStrDbUsr
        let ctx = WrioContext(cfg)
        ctx

    let execSql (conn: NpgsqlConnection) (sql: string) : int =
        let cmd = new NpgsqlCommand(sql, conn)
        cmd.ExecuteNonQuery()

    let prepSysMDsJoin01 (dbSysConn : NpgsqlConnection) =
        let sql1 = "insert into m_ds_join values(2, 1, 3, 'item_cd', 'f01', 'item_cd', CURRENT_TIMESTAMP);"
        execSql dbSysConn sql1 |> ignore
        ()

    let prepMDsTable01 (dbSysConn : NpgsqlConnection) =
        let sql1 = "insert into m_ds_table values(1, 3, 'f03', 't_table01', 1, NULL, CURRENT_TIMESTAMP)"
        let sql2 = "insert into m_ds_table values(2, 3, 'd01', 'm_item', 2, 1, CURRENT_TIMESTAMP)"

        execSql dbSysConn sql1 |> ignore
        execSql dbSysConn sql2 |> ignore    

(*
    let prepLstTplData01 =
        let dsJoin21: DtJoin = {
            JoinSrcCol = "col1"; DstAbbrev = "b"; JoinDstCol = "col1_1"
        }
        let dsJoin22 = {
            JoinSrcCol = "col2"; DstAbbrev = "b"; JoinDstCol = "col1_1"
        }
        let dsJoin23 = {
            JoinSrcCol = "col3"; DstAbbrev = "c"; JoinDstCol = "col1_1"
        }
        [(1, 1, dsJoin21); (1, 2, dsJoin22); (2, 1, dsJoin23)]
*)
    let prepLstTplData01 =
        let dsJoin21 = DsJoin("col1", "b", "col1_1")
        let dsJoin22 = DsJoin("col2", "b", "col1_1")
        let dsJoin23 = DsJoin("col3", "c", "col1_1")
        
        [(1, 1, dsJoin21); (1, 2, dsJoin22); (2, 1, dsJoin23)]

    let prepMPivot01 (dbSysConn : NpgsqlConnection) =
        let sql1 = """insert into m_pivot values(4, 'usr1', 3, '{"datasetId":3,"colHdr":["d01.item_name"],"rowHdr":["f01.sales_date"],"cellVal":[{"colName":"nof_sales","abbrev":"f01","aggFuncDiv":1}],"rowOdr":["f01.sales_date"],"colOdr":[]}', CURRENT_TIMESTAMP)"""

        execSql dbSysConn sql1 |> ignore

(*
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
*)

    [<Test>]
    let getDsColumnTest01 () =

        let colfct1 = DsColumn("item_grp_cd", WrioValueType.STRING)
        let colfct2 = DsColumn("item_cd", WrioValueType.STRING)
        let colfct3 = DsColumn("nof_sales", WrioValueType.NUMBER)
        let dstblFact = DsTable(1, "t_table01", "f01", 1, -1, [||], [|colfct1; colfct2; colfct3|])

        let coldim1 = DsColumn("item_grp_cd", WrioValueType.STRING)
        let coldim2 = DsColumn("item_cd", WrioValueType.STRING)
        let coldim3 = DsColumn("item_name", WrioValueType.STRING)

        let dstblDim1 = DsTable(2, "m_item", "d01", 2, -1, [||], [|coldim1; coldim2; coldim3|])
        
        let dtSet = DtSet(1, dstblFact, [|dstblDim1|] )
        let optDsCol1 = MdlFunc.getDsColumn dtSet "d01.item_name"

        match optDsCol1 with
        | Some dsCol1 ->
            Assert.AreEqual(dsCol1.ColName, "item_name")
            Assert.AreEqual(dsCol1.ColType, WrioValueType.STRING)
        | None -> Assert.Fail("must be Some xxx")

        let optDsCol2 = MdlFunc.getDsColumn dtSet "f01.item_cd"

        match optDsCol2 with
        | Some dsCol2 ->
            Assert.AreEqual(dsCol2.ColName, "item_cd")
            Assert.AreEqual(dsCol2.ColType, WrioValueType.STRING)
        | None -> Assert.Fail("must be Some yyy")

        let optDsCol3 = MdlFunc.getDsColumn dtSet "f02.item_cd"
        match optDsCol3 with
        | Some _ -> Assert.Fail("must be None")
        | None -> Assert.Pass()



        