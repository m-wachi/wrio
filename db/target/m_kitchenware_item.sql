drop table m_kitchenware_item;

create table m_kitchenware_item(
	merc_cd character varying(9) NOT NULL, -- ���iCD
	merc_name character varying(40) NOT NULL, -- ���i��
    merc_grp_cd numeric(2) NOT NULL, -- ����CD
	constraint pk_m_kitchenware_item primary key (merc_cd)
);

