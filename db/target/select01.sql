select a.sales_date, a.item_cd, a.nof_sales
, b.item_name
from t_table01 a
inner join m_item b on a.item_cd = b.item_cd;
