import click
import psycopg2

from wrio.model import (Dimension, DtSet, Pivot)
#from flask import current_app, g
#from flask.cli import with_appcontext

import json

def getConn():
    """
    get system database(wrio) connection
    """
    conn = psycopg2.connect("postgresql://wrio_user:wrio_user@localhost:5432/wrio01")
    return conn
  

def myfunc01():
    sRet = ""
    conn = get_db()
    with conn.cursor() as cur:
        cur.execute("SELECT 'PostgreSQL'")
        row = cur.fetchone()
        sRet = str(row)

    conn.close()
    return sRet

def myfunc02():
    conn = get_db()
    sql = "select dtb.table_abbrev, dtb.table_name, dtb.table_type, \n"
    sql += "   dtb.join_src_col, dtb.dst_abbrev, dtb.join_dst_col, \n"
    sql += "   dtb.join_div \n"
    sql += "from m_ds_table dtb \n"
    sql += "where dtb.dataset_id = 1 \n"
    sql += "order by dtb.table_type "

    retVal = None
    v1 = None
    d1 = ""
    f1 = ""
    with conn.cursor() as cur:
        cur.execute(sql)
        for row in cur:
            tableType = row[2]
            if tableType == 1:
                fct1 = DtSrc()
                fct1.factTable = row[1]
                fct1.factAbbrev = row[0]
                f1 = str(fct1)
            elif row[2] == 2:
                v1 = row
                dim1 = Dimension()
                dim1.table = row[1]
                dim1.abbrev = row[0]
                dim1.joinCond = "%s.%s = %s.%s" % (row[4], row[5], dim1.abbrev, row[3])
                d1 = str(dim1)
        #v1 = cur.fetchall()


    sql2 = "select dataset_id, setting_json from m_pivot"
    v2 = None
    jsonStr = ""
    jsonObj = None
    with conn.cursor() as cur:
        cur.execute(sql2)
        v2 = cur.fetchall()
        jsonStr = v2[0][1]
        jsonObj = json.loads(jsonStr)
        #jsonObj = json.loads('{"ds_id": 1234}')
        
    retVal = {"v1": v1, "v2": v2, "d1": d1, "f1": f1, "jsonStr": jsonStr, "json": jsonObj}

    conn.close()
    return retVal


def getPivot(conn):
    pvt = getPivotBase(conn)
    pvt.dataset = getDtSet(conn, pvt.datasetId)
    return pvt

def getPivotBase(conn):
    sql = "select dataset_id, setting_json from m_pivot"
    jsonStr = ""
    pvt = Pivot()
    with conn.cursor() as cur:
        cur.execute(sql)
        row = cur.fetchone()
        jsonStr = row[1]
        pvt.datasetId = row[0]
        pvt.settingJson = json.loads(jsonStr)
        pvt.jsonObj = json.loads(jsonStr)

    return pvt

def getDtSet(conn, datasetId):
    sql = "select dtb.table_abbrev, dtb.table_name, dtb.table_type, \n"
    sql += "   dtb.join_src_col, dtb.dst_abbrev, dtb.join_dst_col, \n"
    sql += "   dtb.join_div \n"
    sql += "from m_ds_table dtb \n"
    sql += "where dtb.dataset_id = 1 \n"
    sql += "order by dtb.table_type "

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
                dim1.table = row[1]
                dim1.abbrev = row[0]
                dim1.joinCond = "%s.%s = %s.%s" % (row[4], row[5], dim1.abbrev, row[3])
                dtSet.dimensions = [dim1]

    return dtSet


def myfunc03():
    conn = get_db()

    pvt = getPivot(conn)
    dtSet = getDtSet(conn, pvt.datasetId)
    
    conn.close()

    return (pvt, dtSet)

