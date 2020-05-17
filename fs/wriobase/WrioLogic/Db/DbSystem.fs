namespace Wrio.Db

open Npgsql
open Wrio.Models

type DbSysDsTable = {
    DsTableId: int
    TableAbbrev: string
    TableName: string
    TableType: int
}

type DbSysDsJoin = {
    DsTableId: int
    SeqNo: int
    DatasetId: int
    JoinSrcCol: string
    DstAbbrev: string
    JoinDstCol: string
    JoinDiv: int
}

module DbSystem =
 
    //let private connString = "Host=localhost;Username=wrio_user;Password=wrio_user;Database=wrio01; Pooling=True; Maximum Pool Size=5;"
    
    let getDbSysConn (connString: string): NpgsqlConnection = 
        let conn = new NpgsqlConnection(connString)
        //conn.Open()
        conn
(*    
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
*)
(*
    let private getDsJoinFromRdr (rdr :NpgsqlDataReader) : DbSysDsJoin =
        { 
            DsTableId = rdr.GetInt32(0)
            SeqNo = rdr.GetInt32(1)
            DatasetId = rdr.GetInt32(2)
            JoinSrcCol =  rdr.GetString(3)
            DstAbbrev =  rdr.GetString(4)
            JoinDstCol = rdr.GetString(5)
            JoinDiv = rdr.GetInt32(6)
        }
*)
    let private getDsJoinFromRdr (rdr :NpgsqlDataReader) : (int * int * DsJoin) =
        let dsJoin : DsJoin =
            {
                JoinSrcCol =  rdr.GetString(3)
                DstAbbrev =  rdr.GetString(4)
                JoinDstCol = rdr.GetString(5)
                JoinDiv = rdr.GetInt32(6)
             }
        let dsTableId = rdr.GetInt32(0)
        let seqNo = rdr.GetInt32(1)
        (dsTableId, seqNo, dsJoin)

    let rec private getDsJoins acc (rdr :NpgsqlDataReader) =
        if rdr.Read() then
            let dst = getDsJoinFromRdr rdr
            getDsJoins (dst :: acc) rdr
        else
            acc


    let getDsJoin (conn : NpgsqlConnection) (datasetId : int) : (int * int * DsJoin) list =
        let sql = 
            "SELECT " + 
            "    ds_table_id, seq, dataset_id, " +
            "    join_src_col, dst_abbrev, join_dst_col, join_div " +
            "FROM m_ds_join " +
            "WHERE " +
            "    dataset_id = @dataset_id " +
            "ORDER BY ds_table_id, seq "

        let cmd = new NpgsqlCommand(sql, conn)
        cmd.Parameters.AddWithValue("dataset_id", datasetId) |> ignore
        let rdr = cmd.ExecuteReader()

        let lstDsJoin = getDsJoins [] rdr
        rdr.Close()
        lstDsJoin



    // let getDtSet (conn : NpgsqlConnection) (datasetId : int) : DtSet =
    //     let sql = 
    //         "select " + 
    //         "    table_abbrev, table_name, table_type, " +
    //         "    join_src_col, dst_abbrev, join_dst_col, join_div " +
    //         "from m_ds_table " +
    //         "where " +
    //         "    dataset_id = @dataset_id " +
    //         "order by table_type"

    //     let cmd = new NpgsqlCommand(sql, conn)
    //     cmd.Parameters.AddWithValue("dataset_id", datasetId) |> ignore
    //     let rdr = cmd.ExecuteReader()

    //     let lstDst = getDsTables [] rdr

    //     let dst1s = 
    //         query {
    //             for x in lstDst do
    //             where (x.TableType = 1)
    //             select x
    //         }

    //     let dst1 = Seq.head dst1s

    //     let dtSet = DtSet(datasetId, dst1.TableName, dst1.TableAbbrev)

    //     let dst2s = 
    //         query {
    //             for x in lstDst do
    //             where (x.TableType = 2)
    //             select (Dimension(x.TableName, x.TableAbbrev, x.JoinSrcCol, x.DstAbbrev, x.JoinDstCol, x.JoinDiv))
    //         }
    //     dtSet.Dimensions <- Seq.toList dst2s

    //     dtSet


    let private getDsTableFromRdr (rdr :NpgsqlDataReader) : DbSysDsTable =
        { 
            DsTableId = rdr.GetInt32(0)
            TableAbbrev = rdr.GetString(1)
            TableName = rdr.GetString(2)
            TableType = rdr.GetInt32(3)
        }

    let rec private getDsTables acc (rdr :NpgsqlDataReader) =
        if rdr.Read() then
            let dst = getDsTableFromRdr rdr
            getDsTables (dst :: acc) rdr
        else
            acc


    let getDsTableOld (conn : NpgsqlConnection) (datasetId : int) : DtSet =
        let sql = 
            "select " + 
            "    table_abbrev, table_name, table_type " +
            "from m_ds_table " +
            "where " +
            "    dataset_id = @dataset_id " +
            "order by table_type"

        let cmd = new NpgsqlCommand(sql, conn)
        cmd.Parameters.AddWithValue("dataset_id", datasetId) |> ignore
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

        // let dst2s = 
        //     query {
        //         for x in lstDst do
        //         where (x.TableType = 2)
        //         select (Dimension(x.TableName, x.TableAbbrev, x.JoinSrcCol, x.DstAbbrev, x.JoinDstCol, x.JoinDiv))
        //     }
        // dtSet.Dimensions <- Seq.toList dst2s

        dtSet

    let getDsTable (conn : NpgsqlConnection) (datasetId : int) : DbSysDsTable list =
        let sql = 
            "select " + 
            "    ds_table_id, table_abbrev, table_name, table_type " +
            "from m_ds_table " +
            "where " +
            "    dataset_id = @dataset_id " +
            "order by table_type"

        let cmd = new NpgsqlCommand(sql, conn)
        cmd.Parameters.AddWithValue("dataset_id", datasetId) |> ignore
        let rdr = cmd.ExecuteReader()

        //再帰で逆順になっているので戻す
        let lstDsTbl = getDsTables [] rdr
        rdr.Close()
        List.rev lstDsTbl 

    let getPivotBase (conn : NpgsqlConnection) (pivotId : int)  = 
        let sql = 
            "select dataset_id, setting_json from m_pivot " +
            "where " +
            "    pivot_id = @pivot_id "

        let cmd = new NpgsqlCommand(sql, conn)
        cmd.Parameters.AddWithValue("pivot_id", pivotId) |> ignore
        use rdr = cmd.ExecuteReader()   // use = c# using

        if rdr.Read() then
            let datasetId = rdr.GetInt32(0)
            let settingJson = rdr.GetString(1)
            //rdr.Close()
            (datasetId, settingJson)
        else
            //rdr.Close()
            (-1, "")

(*
    let getPivotBase (conn : NpgsqlConnection) (pivotId : int)  = 
        let sql = 
            "select dataset_id, setting_json from m_pivot" +
            "where " +
            "    pivot_id = @pivot_id "

        let cmd = new NpgsqlCommand(sql, conn)
        cmd.Parameters.AddWithValue("pivot_id", pivotId) |> ignore
        let rdr = cmd.ExecuteReader()

        if rdr.Read() then
            let datasetId = rdr.GetInt32(0)
            let settingJson = rdr.GetString(1)

            let dtSet = getDtSet conn datasetId

            let pvt : Pivot = {
                DatasetId = datasetId
                SettingJson = settingJson
                DtSet = dtSet
            }
            pvt
        else
            let pvt : Pivot = {
                DatasetId = -1
                SettingJson = ""
                DtSet = null
            }
            pvt
*)
            //let dst = getDsTable rdr
            //getDsTables (dst :: acc) rdr
(*
            TableName = rdr.GetString(1)
            TableType = rdr.GetInt32(2)
            JoinSrcCol = if rdr.IsDBNull(3) then "" else rdr.GetString(3)
            DstAbbrev = if rdr.IsDBNull(4) then "" else rdr.GetString(4)
*)