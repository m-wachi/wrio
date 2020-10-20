#!/bin/bash
source ./pgenv_test.sh
psql -f t_table01.sql
psql -f m_item.sql
