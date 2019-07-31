drop table t_dataset;

create table t_dataset(
    user_id character varying(20) NOT NULL,
    dataset_name character varying(40) NOT NULL,
    table_name character varying(40) NOT NULL,
    table_abbrev character varying(4) NOT NULL,
    table_type int NOT NULL,
    join_src_col character varying(40) NOT NULL,
    dst_abbrev character varying(4),
    join_dst_col character varying(40),
    join_div int,
    upd_time timestamptz NOT NULL,
    constraint pk_t_dataset primary key (user_id, dataset_name, table_name)
);

comment on table t_dataset IS 'data-set';
comment on column t_dataset.user_id IS 'user id';
comment on column t_dataset.dataset_name IS 'data-set name';
comment on column t_dataset.table_name IS 'table name';
comment on column t_dataset.table_abbrev IS 'table略称';
comment on column t_dataset.table_type IS 'table type 1:FACT, 2:DIMENSION';
comment on column t_dataset.join_src_col IS 'JOINカラム(元)';
comment on column t_dataset.dst_abbrev IS '接続先table略称';
comment on column t_dataset.join_dst_col IS 'JOINカラム(接続先)';
comment on column t_dataset.join_div IS 'JOIN区分 1:INNER, 2:LEFT, 3:RIGHT';
comment on column t_dataset.upd_time IS '更新日時';

