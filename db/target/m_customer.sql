drop table m_customer;

create table m_customer(
	customer_cd character varying(5) NOT NULL, -- ���Ӑ�CD
	customer_name character varying(40) NOT NULL, -- ���Ӑ於
	constraint pk_m_customer primary key (customer_cd)
);

