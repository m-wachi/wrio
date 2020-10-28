#!/bin/bash
cd /home/dev01
source .nvm/nvm.sh
nvm install --lts
npm install -g @angular/cli@10.2.0 <<EOF
N
EOF

touch nvm-work/install-ndjs-ng.txt

