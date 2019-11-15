import psycopg2

from wrio.model import (Dimension, DtSet, Pivot)

def getConn():
    """
    get connection
    """
    
    conn = psycopg2.connect("postgresql://user02:user02@localhost:5432/user02db")

    return conn


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


def usrPgMyfunc02():
    pass



