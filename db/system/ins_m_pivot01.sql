delete from m_pivot;


-- insert into m_pivot values(1, 'usr1', 1, '{"ds_id": 1234, "colhdr": [], "rowhdr": ["d01.item_name"], "rowodr": ["d01.item_name"]}', CURRENT_TIMESTAMP);
insert into m_pivot values(1, 'usr1', 1, '{"datasetId": 1234, "colHdr": ["f01.sales_date"], "rowHdr": ["d01.item_name"], "rowOdr": ["d01.item_name"]}', CURRENT_TIMESTAMP);
