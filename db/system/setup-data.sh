#!/bin/bash
source ./pgenv.sh
psql -f ins_m_dataset.sql
psql -f ins_m_ds_table01.sql
psql -f ins_m_pivot01.sql
psql -f ins_m_ds_join.sql 
