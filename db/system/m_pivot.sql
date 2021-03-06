drop table m_pivot;

create table m_pivot(
    pivot_id int not NULL,
    user_id character varying(20) NOT NULL,
    dataset_id int NOT NULL,
    setting_json character varying(400) NOT NULL,
    upd_time timestamptz NOT NULL,
    constraint pk_m_pivot primary key (pivot_id)
);

comment on table m_pivot IS 'pivot setting';
comment on column m_pivot.pivot_id IS 'pivot id';
comment on column m_pivot.user_id IS 'user id';
comment on column m_pivot.dataset_id IS 'dataset id';
comment on column m_pivot.setting_json IS 'setting情報(json形式)';
comment on column m_pivot.upd_time IS '更新日時';

