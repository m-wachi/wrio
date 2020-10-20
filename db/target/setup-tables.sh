#!/bin/bash
source ./pgenv.sh
psql -f t_table01.sql
psql -f m_item.sql
