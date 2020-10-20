#!/bin/bash
createuser wrio_user > /tmp/wrio_pg01.log 2>&1
createdb -O wrio_user wrio01 >> /tmp/wrio_pg01.log 2>&1
createuser user02 >> /tmp/wrio_pg01.log 2>&1
createdb -O user02 user02db >> /tmp/wrio_pg01.log 2>&1

psql <<EOF
alter user wrio_user password 'wrio_user';
alter user user02 password 'user02';
EOF
