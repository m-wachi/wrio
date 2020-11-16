drop table m_kitchenware_item;

create table m_kitchenware_item(
	merc_cd character varying(9) NOT NULL, -- è§ïiCD
	merc_name character varying(40) NOT NULL, -- è§ïiñº
    merc_grp_cd numeric(2) NOT NULL, -- ï™óﬁCD
	constraint pk_m_kitchenware_item primary key (merc_cd)
);

