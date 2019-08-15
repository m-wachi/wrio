/*
drop table m_ds_table;

create table m_ds_table(
    dataset_id int NOT NULL,
    table_abbrev character varying(4) NOT NULL,
    table_name character varying(40) NOT NULL,
    table_type int NOT NULL,
    join_src_col character varying(40) NOT NULL,
    dst_abbrev character varying(4),
    join_dst_col character varying(40),
    join_div int,
    upd_time timestamptz NOT NULL,
    constraint pk_m_ds_table primary key (dataset_id, table_abbrev)
);

comment on table m_ds_table IS 'data-set table setting';
comment on column m_ds_table.dataset_id IS 'dataset id';
comment on column m_ds_table.table_abbrev IS 'table略称';
comment on column m_ds_table.table_name IS 'table name';
comment on column m_ds_table.table_type IS 'table type 1:FACT, 2:DIMENSION';
comment on column m_ds_table.join_src_col IS 'JOINカラム(元)';
comment on column m_ds_table.dst_abbrev IS '接続先table略称';
comment on column m_ds_table.join_dst_col IS 'JOINカラム(接続先)';
comment on column m_ds_table.join_div IS 'JOIN区分 1:INNER, 2:LEFT, 3:RIGHT';
comment on column m_ds_table.upd_time IS '更新日時';

*/

delete from m_ds_table;
insert into m_ds_table values(1, 'f01', 't_table01', 1, NULL, NULL, NULL, NULL, CURRENT_TIMESTAMP);
insert into m_ds_table values(1, 'd01', 'm_item', 2, 'item_cd', 'f01', 'item_cd', 2, CURRENT_TIMESTAMP);
