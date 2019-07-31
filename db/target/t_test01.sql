drop table t_table01;

create table t_table01(
	sales_date date NOT NULL,
	item_grp_cd char(3) NOT NULL,
	item_cd character varying(6) NOT NULL,
	nof_sales numeric(5) NOT NULL,
	sales_amount numeric(10) NOT NULL,
	constraint pk_t_table01 primary key (sales_date, item_grp_cd, item_cd)
);

