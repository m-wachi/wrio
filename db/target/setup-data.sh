#!/bin/bash
source ./pgenv.sh
psql -f insert_item01.sql
psql -f insert_table01.sql
psql -f ins_m_customer.sql
psql -f ins_m_kitchenware_item.sql
psql -f ins_m_merc_grp.sql
psql -f ins_t_kitsales.sql
