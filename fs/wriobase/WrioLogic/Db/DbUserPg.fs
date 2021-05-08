namespace Wrio.Db

open System.Data.Common
open Npgsql

open Wrio.Common
open Wrio.Models
open System

module DbUserPg =

    // let getDbUsrConn (connString: string): NpgsqlConnection = 
    //     let conn = new NpgsqlConnection(connString)
    //     //conn.Open()
    //     conn

    let connectDbUsr (ctx: WrioContext) : WrioContext = 

        ctx.ConnDbUsr <- new NpgsqlConnection(ctx.Config.GetUsrConnStr())
        ctx

    let openDbUsr (ctx: WrioContext) : WrioContext = 
        ctx.ConnDbUsr.Open() |> ignore
        ctx

    let closeDbUsr (ctx: WrioContext) : WrioContext =
        ctx.ConnDbUsr.Close() |> ignore
        ctx

    let usrPgMyfunc01 (conn : NpgsqlConnection)  =
        let sql = 
            "select \n" +
            "   b.item_name, sum(a.nof_sales) nof_sales \n" +
            "from t_table01 a \n" +
            "    inner join m_item b on a.item_cd = b.item_cd \n" +
            "group by b.item_name \n"

        let cmd = new NpgsqlCommand(sql, conn)
            //cmd.Parameters.AddWithValue("pivot_id", pivotId) |> ignore
        use rdr = cmd.ExecuteReader()   // use = c# using

        if rdr.Read() then
            let itemName = rdr.GetString(0)
            let nofSales = rdr.GetInt32(1)
            (itemName, nofSales)
        else
            //rdr.Close()
            ("", 0)

    let joinCondSql (srcAbbrev: string) (dsJoin: DsJoin) : string =
        String.Format("{0}.{1} = {2}.{3}", dsJoin.DstAbbrev, dsJoin.JoinDstCol, srcAbbrev, dsJoin.JoinSrcCol)

    let toSql (pvt: Pivot) : string =
        let dtSet = pvt.DataSet
        let dim1 = dtSet.Dimensions.[0]

        let getAccumFuncExp (x: CellVal) = "SUM(" + x.Abbrev + "." + x.ColName + ") " + x.ColName
        //let sSelectClause = "SELECT " + pvt.SettingJson["rowhdr"][0]
        //let sSelectClause = "SELECT " + pvt.Setting.RowHdr.[0]

        let sDimensionColumns = String.Join(", ", pvt.Setting.RowHdr) + ", \n" + 
                                String.Join(", ", pvt.Setting.ColHdr) 
        let sSelectClause = "SELECT " + 
                            sDimensionColumns + ", \n" +
                            String.Join(", ", (Array.map getAccumFuncExp pvt.Setting.CellVal))

        let sFromClause = String.Format(" FROM {0} {1}", dtSet.Fact.Table, dtSet.Fact.Abbrev)

        let sJoinCond = joinCondSql dim1.Abbrev dim1.DsJoins.[0]

        let sJoin1 = String.Format("  INNER JOIN {0} {1} \n    ON {2}", dim1.Table, dim1.Abbrev, sJoinCond)
    
        let sql = sSelectClause + "\n" + sFromClause + "\n"
        let sql2 = sql + sJoin1 + "\n" + 
                    " GROUP BY " + sDimensionColumns + "\n" +
                    " ORDER BY " + sDimensionColumns + " "

        sql2

    let getPivotColumns (pvtSetting: PivotSetting) =
        let pvtCols1: string array = Array.append pvtSetting.RowHdr pvtSetting.ColHdr
        let pvtCols2: string array = 
            Array.append pvtCols1 
                         (Array.map (fun (x: CellVal) -> x.Abbrev + "." + x.ColName) 
                                    pvtSetting.CellVal)
        pvtCols2

    let getWrioValueType (dataType: string) : WrioValueType =
        match dataType with
            | "numeric" -> WrioValueType.NUMBER
            | "character varying" -> WrioValueType.STRING
            | "date" -> WrioValueType.DATE
            | _ -> WrioValueType.OTHER

    let getColumns (ctx: WrioContext) (sTable: string) : DsColumn array =

        let sql1 = "SELECT column_name, data_type " + "\n" +
                   " FROM information_schema.columns \n" +
                   " WHERE table_name='{0}'\n" +
                   " ORDER BY column_name "
                   
        let sql2 = String.Format(sql1, sTable)

        let cmd = new NpgsqlCommand(sql2, ctx.ConnDbUsr)

        let rdr = cmd.ExecuteReader() 

        let f (rdr2: NpgsqlDataReader) = 
            DsColumn((rdr2.GetString(0)), getWrioValueType (rdr2.GetString(1)), "")

        let ret = [| while rdr.Read() do yield (f rdr) |]

        rdr.Close() |> ignore

        ret


    let getPivotData (ctx : WrioContext) (pvt: Pivot) =

        let getRowVals (rdr: DbDataReader) = 
            Array.map (fun i -> rdr.GetValue(i)) [|0 .. rdr.FieldCount - 1|]

        let getRows (rdr: DbDataReader) = 
            [| while rdr.Read() do yield (getRowVals rdr) |]

        let sql = toSql pvt

        WrioCommon.logInformation ctx ("pvt.Setting=" + pvt.Setting.ToString())

        WrioCommon.logInformation ctx sql        

        let cmd = new NpgsqlCommand(sql, ctx.ConnDbUsr)
            //cmd.Parameters.AddWithValue("pivot_id", pivotId) |> ignore

        let mutable rows: (obj array) array = [||]

        //use rdr = cmd.ExecuteReader()   // use = c# using
        let rdr = cmd.ExecuteReader() 

        if rdr.Read() then
            //let colNames = Array.map (fun i -> rdr.GetName(i)) [|0 .. rdr.FieldCount - 1|]
            let colNames = getPivotColumns pvt.Setting
            //1st row values
            let rowVals1 = getRowVals rdr
            //2nd row and following rows
            let rows2 = getRows rdr

            let rows = Array.append [|rowVals1|] rows2
            Some (PivotData(colNames, rows))
        else
            printfn "rdr.Read() = false"
            //PivotData()
            None



