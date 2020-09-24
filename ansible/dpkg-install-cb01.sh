#!/bin/bash
dpkg --install /tmp/google-chrome-stable_current_amd64.deb
apt install --assume-yes --fix-broken
touch /tmp/cbinst.txt


