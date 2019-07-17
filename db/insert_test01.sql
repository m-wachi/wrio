/*
create table t_table01(
	sales_date date NOT NULL,
	item_grp_cd char(3) NOT NULL,
	item_cd character varying(6) NOT NULL,
	nof_sales numeric(5) NOT NULL,
	sales_amount numeric(10) NOT NULL,
	constraint pk_t_table01 primary key (sales_date, item_grp_cd, item_cd)
);
*/


insert into t_table01
values('2019-07-01', '001', 'A0001', 10, 5250);
insert into t_table01
values('2019-07-01', '001', 'A0002', 15, 1500);
insert into t_table01
values('2019-07-02', '001', 'A0001', 8, 4200);
insert into t_table01
values('2019-07-02', '001', 'A0002', 12, 1200);


