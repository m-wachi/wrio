drop table m_customer;

create table m_customer(
	customer_cd character varying(5) NOT NULL, -- 得意先CD
	customer_name character varying(40) NOT NULL, -- 得意先名
	constraint pk_m_customer primary key (customer_cd)
);

