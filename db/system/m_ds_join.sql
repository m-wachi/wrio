drop table m_ds_join;

create table m_ds_join(
	ds_table_id int NOT NULL,
    seq int NOT NULL,
    join_src_col character varying(40),
    dst_abbrev character varying(4),
    join_dst_col character varying(40),
    join_div int,
    upd_time timestamptz NOT NULL,
    constraint pk_m_ds_join primary key (ds_table_id, seq)
);

comment on table m_ds_join IS 'data-set join setting';
comment on column m_ds_join.ds_table_id IS 'ds_table id';
comment on column m_ds_join.seq IS '連番';
comment on column m_ds_join.join_src_col IS 'JOINカラム(元)';
comment on column m_ds_join.dst_abbrev IS '接続先table略称';
comment on column m_ds_join.join_dst_col IS 'JOINカラム(接続先)';
comment on column m_ds_join.join_div IS 'JOIN区分 1:INNER, 2:LEFT, 3:RIGHT';
comment on column m_ds_join.upd_time IS '更新日時';

