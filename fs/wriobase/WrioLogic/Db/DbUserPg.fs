namespace Wrio.Db

open Npgsql
open Wrio.Models
open System

module DbUserPg =
 (*
def usrPgMyfunc01():
    sRet = ""

    sql = "select \n"
    sql += "   b.item_name, sum(a.nof_sales) nof_sales \n"
    sql += "from t_table01 a \n"
    sql += "inner join m_item b on a.item_cd = b.item_cd \n"
    sql += "group by b.item_name \n"

    conn = getConn()
    with conn.cursor() as cur:
        cur.execute(sql)
        row = cur.fetchone()
        sRet = str(row)

    conn.close()
    return sRet
*)
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

    let usrPgMyfunc02 (conn : NpgsqlConnection) (pvt: Pivot) =
        let dtSet = pvt.DtSet
        let dim1 = dtSet.Dimensions.[0]

        //let sSelectClause = "SELECT " + pvt.SettingJson["rowhdr"][0]
        let sSelectClause = "SELECT " + pvt.Setting.RowHdr.[0]
        let sFromClause = String.Format(" FROM {0} {1}", dtSet.Fact.Table, dtSet.Fact.Abbrev)

        //let sJoin1 = String.Format("  INNER JOIN {0} {1} \n    ON {2}", dim1.Table, dim1.Abbrev, dim1.JoinCond)
        let sJoin1 = String.Format("  INNER JOIN {0} {1} \n    ON {2}", dim1.Table, dim1.Abbrev, "")
        
        let sql = sSelectClause + "\n" + sFromClause + "\n"
        let sql2 = sql + sJoin1 + "\n"

        sql2
        (*
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
        *)

    let joinCondSql (srcAbbrev: string) (dsJoin: DsJoin) : string =
        //sprintf "%s.%s=%s.%s" dsJoin.DstAbbrev dsJoin.JoinDstCol srcAbbrev dsJoin.JoinSrcCol
        String.Format("{0}.{1} = {2}.{3}", dsJoin.DstAbbrev, dsJoin.JoinDstCol, srcAbbrev, dsJoin.JoinSrcCol)

    let toSql (pvt: Pivot) : string =
        let dtSet = pvt.DtSet
        let dim1 = dtSet.Dimensions.[0]

        //let sSelectClause = "SELECT " + pvt.SettingJson["rowhdr"][0]
        //let sSelectClause = "SELECT " + pvt.Setting.RowHdr.[0]
        let sSelectClause = "SELECT " + 
                            String.Join(", ", pvt.Setting.RowHdr) + ", \n" + 
                            String.Join(", ", pvt.Setting.ColHdr)

        let sFromClause = String.Format(" FROM {0} {1}", dtSet.Fact.Table, dtSet.Fact.Abbrev)

        let sJoinCond = joinCondSql dim1.Abbrev dim1.DsJoins.[0]

        let sJoin1 = String.Format("  INNER JOIN {0} {1} \n    ON {2}", dim1.Table, dim1.Abbrev, sJoinCond)
    
        let sql = sSelectClause + "\n" + sFromClause + "\n"
        let sql2 = sql + sJoin1 + "\n"

        sql2
