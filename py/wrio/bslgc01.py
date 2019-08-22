from wrio.db_system import (myfunc03, myfunc02)
from wrio.model import (Dimension, DtSrc)


def mylogic01():
    (pvt, dtSrc) = myfunc03()
    return {"pvt": str(pvt), "dtSrc": str(dtSrc)}
