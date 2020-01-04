namespace Aspnet2.Db

open Npgsql
open Aspnet2.Models

module DbSystem =
 
    let private connString = "Host=localhost;Username=wrio_user;Password=wrio_user;Database=wrio01; Pooling=True; Maximum Pool Size=5;"
    
    let getDbSysConn : NpgsqlConnection = 
        let conn = new NpgsqlConnection(connString)
        //conn.Open()
        conn
    
    let getDtSetOld (conn : NpgsqlConnection) (datasetId : int) : Dimension =
        let sql = 
            "select " + 
            "    table_abbrev, table_name, table_type, " +
            "    join_src_col, dst_abbrev, join_dst_col, join_div " +
            " from m_ds_table " +
            " where " +
            "    table_type = 2"

        let cmd = new NpgsqlCommand(sql, conn)
        let rdr = cmd.ExecuteReader()
        let dms = 
            if rdr.Read() then
                let tableAbbrev =  rdr.GetString(0)
                let tableName = rdr.GetString(1)
                let joinSrcCol = rdr.GetString(3)
                let dstAbbrev = rdr.GetString(4)
                let joinDstCol = rdr.GetString(5)
                let joinDiv = rdr.GetInt32(6)
                Dimension(tableName, tableAbbrev, joinSrcCol, dstAbbrev, joinDstCol, joinDiv)
            else
                Dimension("", "", "", "", "", 0)
                
        //rdr.Close()

        dms

    type private DsTable = {
        TableAbbrev: string
        TableName: string
        TableType: int
        JoinSrcCol: string
        DstAbbrev: string
        JoinDstCol: string
        JoinDiv: int
    }

    let private getDsTable (rdr :NpgsqlDataReader) : DsTable =
        { 
            TableAbbrev = rdr.GetString(0)
            TableName = rdr.GetString(1)
            TableType = rdr.GetInt32(2)
            JoinSrcCol = if rdr.IsDBNull(3) then "" else rdr.GetString(3)
            DstAbbrev = if rdr.IsDBNull(4) then "" else rdr.GetString(4)
            JoinDstCol = if rdr.IsDBNull(5) then "" else rdr.GetString(5)
            JoinDiv = if rdr.IsDBNull(6) then -1 else rdr.GetInt32(6)
        }

    let rec private getDsTables acc (rdr :NpgsqlDataReader) =
        if rdr.Read() then
            let dst = getDsTable rdr
            getDsTables (dst :: acc) rdr
        else
            acc


    let getDtSet (conn : NpgsqlConnection) (datasetId : int) : DtSet =
        let sql = 
            "select " + 
            "    table_abbrev, table_name, table_type, " +
            "    join_src_col, dst_abbrev, join_dst_col, join_div " +
            "from m_ds_table " +
            "where " +
            "    dataset_id = 1 " +
            "order by table_type"
        (*
            dtSet = DtSet()
            dtSet.datasetId = datasetId
            with conn.cursor() as cur:
                cur.execute(sql)
                for row in cur:
                    tableType = row[2]
                    if tableType == 1:
                        dtSet.factTable = row[1]
                        dtSet.factAbbrev = row[0]
                    elif row[2] == 2:
                        dim1 = Dimension()
        *)

        let cmd = new NpgsqlCommand(sql, conn)
        let rdr = cmd.ExecuteReader()

        let lstDst = getDsTables [] rdr

        let dst1s = 
            query {
                for x in lstDst do
                where (x.TableType = 1)
                select x
            }

        let dst1 = Seq.head dst1s

        let dtSet = DtSet(datasetId, dst1.TableName, dst1.TableAbbrev)

        let dst2s = 
            query {
                for x in lstDst do
                where (x.TableType = 2)
                select (Dimension(x.TableName, x.TableAbbrev, x.JoinSrcCol, x.DstAbbrev, x.JoinDstCol, x.JoinDiv))
            }
        dtSet.Dimenstions <- Seq.toList dst2s

        dtSet


