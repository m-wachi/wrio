/*
create table m_item(
	item_cd character varying(6) NOT NULL,
	item_grp_cd char(3) NOT NULL,
	item_name character varying(40) NOT NULL,
	constraint pk_m_item primary key (item_cd)
);
*/


insert into m_item 
values('A0001', '001', 'アイテム０１');
insert into m_item 
values('A0002', '001', 'アイテム０２');
