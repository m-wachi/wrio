#from wrio.db_system import (myfunc03, myfunc02)
import wrio.db_system as sysdb 
from wrio.model import (Dimension, DtSet)
#from wrio.db_user_pg import (usrPgMyfunc01)
import wrio.db_user_pg as usrdb

def mylogic01():
    (pvt, dtSrc) = myfunc03()
    return {"pvt": str(pvt), "dtSrc": str(dtSrc)}

def mylogic02():
    (pvt, dtSrc) = myfunc03()
    return dtSrc

def mylogic03():
    return usrPgMyfunc01()

def mylogic04():
    sysConn = sysdb.getConn()

    pvt = sysdb.getPivot(sysConn)

    sysConn.close()

    return pvt
    
    
def mylogic05():
    sysConn = sysdb.getConn()
    pvt = sysdb.getPivot(sysConn)
    sysConn.close()

    usrConn = usrdb.getConn()
    sql = usrdb.usrPgMyfunc02(pvt)
    
    usrConn.close()

    return sql
    
