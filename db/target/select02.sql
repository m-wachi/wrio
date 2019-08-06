select 
 b.item_name, sum(a.nof_sales) nof_sales
from t_table01 a
inner join m_item b on a.item_cd = b.item_cd
group by b.item_name
