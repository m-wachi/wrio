#!/bin/bash
source ./pgenv.sh
psql -f m_dataset.sql
psql -f m_ds_table.sql
psql -f m_pivot.sql
psql -f m_ds_join.sql 
