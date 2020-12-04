namespace Wrio.Controllers

open System
open System.Collections.Generic
open System.Linq
open System.Threading.Tasks
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open Microsoft.Extensions.Configuration
open Npgsql
open Wrio
open Wrio.Common
open Wrio.Util
open Wrio.Models
open Wrio.Db.DbSystem
open Wrio.Logic

[<ApiController>]
[<Route("[controller]")>]
type MyWork02Controller (logger : ILogger<MyWork02Controller>, config : IConfiguration) =
    inherit ControllerBase()

    //http://localhost:5000/mywork02/
    [<HttpGet>]
    member __.Get() : MyWork01Model =
        let a = MyWork01Model(3, "model03")
        a
(*
    [<HttpGet("dim1")>]
    member __.GetMyWork01(): Dimension =
        Dimension("dummy1", "a1", "", "", "", 0)

    [<HttpGet("dim2")>]
     member __.GetMyWork02(): Dimension =
        let connString = "Host=localhost;Username=wrio_user;Password=wrio_user;Database=wrio01"
        let conn = new NpgsqlConnection(connString)
        conn.Open()

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

        rdr.Close()
        conn.Close()

        dms
*)
(*
    [<HttpGet("dim3")>]
    member __.GetMyWork03(): Dimension =
        let connString = "Host=localhost;Username=wrio_user;Password=wrio_user;Database=wrio01"

        let dbSysConn = getDbSysConn connString
        dbSysConn.Open()
        let dm = getDtSetOld dbSysConn 1

        dbSysConn.Close()

        dm

    [<HttpGet("dts1")>]
    member __.GetMyWork04(): DtSet =
        let connString = "Host=localhost;Username=wrio_user;Password=wrio_user;Database=wrio01"

        let dbSysConn = getDbSysConn connString
        dbSysConn.Open()
        //let dtSet = getDtSet dbSysConn 1
        let dtSet = DtSet()
        dbSysConn.Close()

        dtSet
*)

    [<HttpGet("dts2")>]
    member __.GetMyWork05(): DtSet =

        let myCfg = MyConfig()
        let a = config.GetSection("AppConfiguration")
        myCfg.SysConnStr <- a.GetValue("SystemConnectionString")

        let ctx = WrioContext(myCfg)

        let ret = BsLogic01.getDtSetLogic ctx 1
        
        match ret with
            | Some dtSet -> dtSet
            | _ -> DtSet()

    [<HttpGet("conf2")>]
    member __.GetMyWork07(): String =
        //let connString = "Host=localhost;Username=wrio_user;Password=wrio_user;Database=wrio01"

        let a = config.GetSection("AppConfiguration")
        a.GetValue("SystemConnectionString")

        //let dtSet = BsLogic01.getDtSetLogic connString 1

    [<HttpGet("work08")>]
    member __.GetMyWork08(): MyConfig =

        let myCfg = MyConfig()

        let a = config.GetSection("AppConfiguration")

        myCfg.SysConnStr <- a.GetValue("SystemConnectionString")

        myCfg


        //a
