import click
import psycopg2

#from flask import current_app, g
#from flask.cli import with_appcontext


def get_db():
#    if 'db' not in g:
#        g.db = sqlite3.connect(
#            current_app.config['DATABASE'],
#            detect_types=sqlite3.PARSE_DECLTYPES
#        )
#        g.db.row_factory = sqlite3.Row

    conn = psycopg2.connect("postgresql://user02:user02@localhost:5432/user02db")

#    return g.db
    return conn

#def close_db(e=None):
#    db = g.pop('db', None)

#    if db is not None:
#        db.close()

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
    sql = "select * from t_table01"
    retVal = None
    with conn.cursor() as cur:
        cur.execute(sql)
        retVal = cur.fetchall()

    conn.close()
    return retVal


