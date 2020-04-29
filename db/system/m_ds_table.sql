drop table m_ds_table;

create table m_ds_table(
    ds_table_id int NOT NULL,
    dataset_id int NOT NULL,
    table_abbrev character varying(4) NOT NULL,
    table_name character varying(40) NOT NULL,
    table_type int NOT NULL,
    -- join_src_col character varying(40),
    -- dst_abbrev character varying(4),
    -- join_dst_col character varying(40),
    -- join_div int,
    upd_time timestamptz NOT NULL,
    constraint pk_m_ds_table primary key (ds_table_id)
    -- constraint pk_m_ds_table primary key (dataset_id, table_abbrev)
);

comment on table m_ds_table IS 'data-set table setting';
comment on column m_ds_table.ds_table_id IS 'ds_table id';
comment on column m_ds_table.dataset_id IS 'dataset id';
comment on column m_ds_table.table_abbrev IS 'table略称';
comment on column m_ds_table.table_name IS 'table name';
comment on column m_ds_table.table_type IS 'table type 1:FACT, 2:DIMENSION';
-- comment on column m_ds_table.join_src_col IS 'JOINカラム(元)';
-- comment on column m_ds_table.dst_abbrev IS '接続先table略称';
-- comment on column m_ds_table.join_dst_col IS 'JOINカラム(接続先)';
-- comment on column m_ds_table.join_div IS 'JOIN区分 1:INNER, 2:LEFT, 3:RIGHT';
comment on column m_ds_table.upd_time IS '更新日時';

