#!/bin/bash
source ./pgenv.sh
psql -f t_table01.sql
psql -f m_item.sql
psql -f m_customer.sql
psql -f m_kitchenware_item.sql
psql -f m_merc_grp.sql
psql -f t_kitchenware_sales01.sql
