drop table m_dataset;

create table m_dataset(
    dataset_id int NOT NULL,
    user_id character varying(20) NOT NULL,
    dataset_name character varying(40) NOT NULL,
    upd_time timestamptz NOT NULL,
    constraint pk_m_dataset primary key (dataset_id)
);

create unique index ak1_m_dataset on m_dataset(user_id, dataset_name);

comment on table m_dataset IS 'data-set';
comment on column m_dataset.user_id IS 'user id';
comment on column m_dataset.dataset_name IS 'data-set name';
comment on column t_dataset.upd_time IS '更新日時';

