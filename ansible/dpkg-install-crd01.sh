#!/bin/bash
dpkg --install /tmp/chrome-remote-desktop_current_amd64.deb
apt install --assume-yes --fix-broken
touch /tmp/crdinst.txt


