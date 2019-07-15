import functools

from flask import (
    Blueprint, flash, g, redirect, render_template, request, session, url_for, jsonify
)
from werkzeug.security import check_password_hash, generate_password_hash

#from flaskr.db import get_db

bp = Blueprint('auth', __name__, url_prefix='/mdl01')


@bp.route('/register', methods=('GET', 'POST'))
def register():
    ret = {"v1": "hello mdl01",}
    return jsonify(ret)
