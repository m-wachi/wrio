﻿namespace Aspnet2.Controllers

open System
open System.Collections.Generic
open System.Linq
open System.Threading.Tasks
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open Npgsql
open Aspnet2
open Aspnet2.Models
open Aspnet2.Db.DbSystem

[<ApiController>]
[<Route("api/[controller]")>]
type MyWork02Controller (logger : ILogger<MyWork02Controller>) =
    inherit ControllerBase()

    [<HttpGet>]
    member __.Get() : MyWork01Model =
        let a = MyWork01Model(1, "model01")
        a

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

    [<HttpGet("dim3")>]
    member __.GetMyWork03(): Dimension =
        let dbSysConn = getDbSysConn
        dbSysConn.Open()
        let dm = getDtSetOld dbSysConn 1

        dbSysConn.Close()

        dm

    [<HttpGet("dts1")>]
    member __.GetMyWork04(): DtSet =
        let dbSysConn = getDbSysConn
        dbSysConn.Open()
        let dtSet = getDtSet dbSysConn 1

        dbSysConn.Close()

        dtSet
