#!/bin/bash
source ./pgenv.sh
psql -f insert_item01.sql
psql -f insert_table01.sql
