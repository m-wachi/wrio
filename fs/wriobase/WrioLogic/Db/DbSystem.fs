namespace Wrio.Db

open Npgsql

open Wrio.Common
open Wrio.Models
(*
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
*)

module DbSystem =
 
    //let private connString = "Host=localhost;Username=wrio_user;Password=wrio_user;Database=wrio01; Pooling=True; Maximum Pool Size=5;"
    (*
    let getDbSysConn (connString: string): NpgsqlConnection = 
        let conn = new NpgsqlConnection(connString)
        //conn.Open()
        conn
    *)

    let connectDbSys (ctx: WrioContext) : WrioContext = 

        ctx.ConnDbSys <- new NpgsqlConnection(ctx.Config.GetSysConnStr())
        ctx

    let openDbSys (ctx: WrioContext) : WrioContext = 
        ctx.ConnDbSys.Open() |> ignore
        ctx

    let closeDbSys (ctx: WrioContext) : WrioContext =
        ctx.ConnDbSys.Close()
        ctx

    let private getDsJoinFromRdr (rdr :NpgsqlDataReader) : (int * int * DtJoin) =
(*
        let dsJoin : DtJoin =
            {
                JoinSrcCol =  rdr.GetString(3)
                DstAbbrev =  rdr.GetString(4)
                JoinDstCol = rdr.GetString(5)
                //JoinDiv = rdr.GetInt32(6)
            }
*)
        let dsJoin = DtJoin(rdr.GetString(3), rdr.GetString(4), rdr.GetString(5))
        let dsTableId = rdr.GetInt32(0)
        let seqNo = rdr.GetInt32(1)
        (dsTableId, seqNo, dsJoin)

    let rec private getDsJoins acc (rdr :NpgsqlDataReader) =
        if rdr.Read() then
            let dst = getDsJoinFromRdr rdr
            getDsJoins (dst :: acc) rdr
        else
            acc

    //let getDsJoin (conn : NpgsqlConnection) (datasetId : int) : (int * int * DtJoin) list =
    let getDsJoin (ctx : WrioContext) (datasetId : int) : (int * int * DtJoin) list =
        let sql = 
            "SELECT " + 
            "    ds_table_id, seq, dataset_id, " +
            "    join_src_col, dst_abbrev, join_dst_col " +
            "FROM m_ds_join " +
            "WHERE " +
            "    dataset_id = @dataset_id " +
            "ORDER BY ds_table_id, seq "

        let cmd = new NpgsqlCommand(sql, ctx.ConnDbSys)
        cmd.Parameters.AddWithValue("dataset_id", datasetId) |> ignore
        let rdr = cmd.ExecuteReader()

        let lstDsJoin = getDsJoins [] rdr
        rdr.Close()
        lstDsJoin


    let private getDsTableFromRdr (rdr :NpgsqlDataReader) : DsTable =
        let dsTableId = rdr.GetInt32(0)
        let abbrev = rdr.GetString(1)
        let tableName = rdr.GetString(2)
        let tableType = rdr.GetInt32(3)
        let joinDiv = 
            if rdr.IsDBNull(4) then 
                -1
            else
                rdr.GetInt32(4)
        DsTable(dsTableId, tableName, abbrev, tableType, joinDiv, [||])

    let rec private getDsTables acc (rdr :NpgsqlDataReader) =
        if rdr.Read() then
            let dst = getDsTableFromRdr rdr
            getDsTables (dst :: acc) rdr
        else
            acc

    //let getDsTable (conn : NpgsqlConnection) (datasetId : int) : DsTable list =
    let getDsTable (ctx : WrioContext) (datasetId : int) : DsTable list =
        let sql = 
            "select " + 
            "    ds_table_id, table_abbrev, table_name, table_type, join_div " +
            "from m_ds_table " +
            "where " +
            "    dataset_id = @dataset_id " +
            "order by table_type"

        let cmd = new NpgsqlCommand(sql, ctx.ConnDbSys)
        cmd.Parameters.AddWithValue("dataset_id", datasetId) |> ignore
        let rdr = cmd.ExecuteReader()

        
        let lstDsTbl = getDsTables [] rdr
        rdr.Close()
        List.rev lstDsTbl   //再帰で逆順になっているので戻す


    
    //let getPivotBase (conn : NpgsqlConnection) (pivotId : int)  = 
    let getPivotBase (ctx : WrioContext) (pivotId : int)  = 
        let sql = 
            "select dataset_id, setting_json from m_pivot " +
            "where " +
            "    pivot_id = @pivot_id "

        let cmd = new NpgsqlCommand(sql, ctx.ConnDbSys)
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

