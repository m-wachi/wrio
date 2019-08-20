import functools

from flask import (
    Blueprint, flash, g, redirect, render_template, request, session, url_for, jsonify
)
from werkzeug.security import check_password_hash, generate_password_hash

#from flaskr.db import get_db
#from wrio.db import (myfunc01, myfunc02)

from wrio.model import MainTbl
from wrio.db_system import (myfunc01, myfunc02)
from wrio.bslgc01 import mylogic01

bp = Blueprint('mdl01', __name__, url_prefix='/mdl01')


@bp.route('/path1', methods=('GET', 'POST'))
def path1():
    ret = myfunc01()
    #ret = {"v1": "hello mdl01",}
    return jsonify(ret)

@bp.route('/path2', methods=('GET', 'POST'))
def path2():
    ret = myfunc02()
    #row = ret[0]
    row = ret
    #b = row[3]
    return jsonify(ret)

@bp.route('/path3', methods=('GET', 'POST'))
def path3():
    mtbl = MainTbl()
    #obj = {"id": mtbl.id, "name": mtbl.name}
    #return jsonify(obj)
    return jsonify(mtbl.getJSONObj())

@bp.route('/path4', methods=('GET', 'POST'))
def path4():
    ret = mylogic01()
    return jsonify(ret)
