drop table m_item;

create table m_item(
	item_cd character varying(6) NOT NULL,
	item_grp_cd char(3) NOT NULL,
	item_name character varying(40) NOT NULL,
	constraint pk_m_item primary key (item_cd)
);

