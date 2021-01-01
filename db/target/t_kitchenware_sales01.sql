drop table t_kitchenware_sales01;

create table t_kitchenware_sales01(
    slip_no numeric(8) NOT NULL, -- 伝票番号
    merc_cd character varying(9) NOT NULL, -- 商品CD
	sales_date date NOT NULL, -- 売上日
	customer_cd character varying(5) NOT NULL, -- 得意先CD
	nof_sales numeric(5) NOT NULL, -- 売上数
	sales_amount numeric(10) NOT NULL, -- 売上金額
    gross_profit numeric(10) NOT NULL, -- 粗利益
	constraint pk_t_kitchenware_sales01 primary key (slip_no) -- ?
);

