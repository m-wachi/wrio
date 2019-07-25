import functools

from flask import (
    Blueprint, flash, g, redirect, render_template, request, session, url_for, jsonify
)
from werkzeug.security import check_password_hash, generate_password_hash

#from flaskr.db import get_db
from wrio.db import (myfunc01, myfunc02)
from wrio.model import MainTbl

bp = Blueprint('mdl01', __name__, url_prefix='/mdl01')


@bp.route('/path1', methods=('GET', 'POST'))
def path1():
    ret = myfunc01()
    #ret = {"v1": "hello mdl01",}
    return jsonify(ret)

@bp.route('/path2', methods=('GET', 'POST'))
def path2():
    ret = myfunc02()
    row = ret[0]
    #b = row[3]
    return jsonify(ret)

@bp.route('/path3', methods=('GET', 'POST'))
def path3():
    mtbl = MainTbl()
    obj = {"id": mtbl.id, "name": mtbl.name}
    return jsonify(obj)
