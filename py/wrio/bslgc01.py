from wrio.db_system import (myfunc03, myfunc02)
from wrio.model import (Dimension, DtSet)
from wrio.db_user_pg import (usrPgMyfunc01)

def mylogic01():
    (pvt, dtSrc) = myfunc03()
    return {"pvt": str(pvt), "dtSrc": str(dtSrc)}

def mylogic02():
    (pvt, dtSrc) = myfunc03()
    return dtSrc

def mylogic03():
    return usrPgMyfunc01()
