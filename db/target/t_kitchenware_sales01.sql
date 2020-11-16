drop table t_kitchenware_sales01;

create table t_kitchenware_sales01(
    slip_no numeric(8) NOT NULL, -- “`•[”Ô†
    merc_cd character varying(9) NOT NULL, -- ¤•iCD
	sales_date date NOT NULL, -- ”„ã“ú
	customer_cd character varying(5) NOT NULL, -- “¾ˆÓæCD
	nof_sales numeric(5) NOT NULL, -- ”„ã”
	sales_amount numeric(10) NOT NULL, -- ”„ã‹àŠz
    gross_profit numeric(10) NOT NULL -- ‘e—˜‰v
	constraint t_kitchenware_sales01 primary key (slip_no) -- ?
);

