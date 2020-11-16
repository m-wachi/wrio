drop table t_kitchenware_sales01;

create table t_kitchenware_sales01(
    slip_no numeric(8) NOT NULL, -- `[Τ
    merc_cd character varying(9) NOT NULL, -- €iCD
	sales_date date NOT NULL, -- γϊ
	customer_cd character varying(5) NOT NULL, -- ΎΣζCD
	nof_sales numeric(5) NOT NULL, -- γ
	sales_amount numeric(10) NOT NULL, -- γΰz
    gross_profit numeric(10) NOT NULL -- ev
	constraint t_kitchenware_sales01 primary key (slip_no) -- ?
);

