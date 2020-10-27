#!/bin/bash
sudo -u postgres createuser wrio_test > /tmp/wrio_pg02.log 2>&1
sudo -u postgres createdb -O wrio_test wrio_test >> /tmp/wrio_pg02.log 2>&1
sudo -u postgres createuser user02_test >> /tmp/wrio_pg02.log 2>&1
sudo -u postgres createdb -O user02_test user02_test >> /tmp/wrio_pg01.log 2>&1

sudo -u postgres psql <<EOF
alter user wrio_test password 'wrio_test';
alter user user02_test password 'user02_test';
EOF
