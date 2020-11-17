drop table m_merc_grp;

create table m_merc_grp(
    merc_grp_cd numeric(2) NOT NULL, -- •ª—ÞCD
	merc_grp_name character varying(40) NOT NULL, -- •ª—Þ–¼
	constraint pk_m_merc_grp primary key (merc_grp_cd)
);

