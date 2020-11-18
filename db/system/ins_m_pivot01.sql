delete from m_pivot;

-- insert into m_pivot values(1, 'usr1', 1, '{"ds_id": 1234, "colhdr": [], "rowhdr": ["d01.item_name"], "rowodr": ["d01.item_name"]}', CURRENT_TIMESTAMP);
-- insert into m_pivot values(1, 'usr1', 1, '{"datasetId": 1234, "colHdr": ["f01.sales_date"], "rowHdr": ["d01.item_name"], "rowOdr": ["d01.item_name"]}', CURRENT_TIMESTAMP);
insert into m_pivot values(1, 'usr1', 1, '{"datasetId":4,"colHdr":["d01.item_name"],"rowHdr":["f01.sales_date"],"cellVal":[{"colName":"nof_sales","abbrev":"f01","aggFuncDiv":1}],"rowOdr":["f01.sales_date"],"colOdr":[]}', CURRENT_TIMESTAMP);
insert into m_pivot values(2, 'usr1', 2, '{"datasetId":2,"colHdr":["d02.merc_name"],"rowHdr":["f02.sales_date"],"cellVal":[{"colName":"nof_sales","abbrev":"f02","aggFuncDiv":1}],"rowOdr":["f02.sales_date"],"colOdr":[]}', CURRENT_TIMESTAMP);

