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


(*
def usrPgMyfunc02(pvt):

    dtSet = pvt.dataset
    #mainTblName = dtSet.factTable
    #mainTblAbbrev = dtSet.factAbbrev

    dim1 = dtSet.dimensions[0]

    sSelectClause = "SELECT " + pvt.settingJson["rowhdr"][0]
    sFromClause = "FROM {0} {1}".format(dtSet.factTable, dtSet.factAbbrev)

    sJoin1 = "  INNER JOIN {0} {1} \n    ON {2}".format(
        dim1.table, dim1.abbrev, dim1.joinCond)
    
    sql = sSelectClause + "\n" + sFromClause + "\n"
    sql += sJoin1 + "\n"
    return sql
*)

    let usrPgMyfunc02 (conn : NpgsqlConnection) (pvt: Pivot) =
        let dtSet = pvt.DtSet
        let dim1 = dtSet.Dimenstions.Head

        //let sSelectClause = "SELECT " + pvt.SettingJson["rowhdr"][0]
        let sSelectClause = "SELECT " + pvt.Setting.RowHdr.[0]
        let sFromClause = String.Format("FROM {0} {1}", dtSet.FactTable, dtSet.FactAbbrev)

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
