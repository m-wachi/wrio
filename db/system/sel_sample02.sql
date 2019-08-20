select dtb.table_abbrev, dtb.table_name, dtb.table_type,
       dtb.join_src_col, dtb.dst_abbrev, dtb.join_dst_col,
       dtb.join_div
from m_ds_table dtb
where dtb.dataset_id = 1
order by dtb.table_type;
