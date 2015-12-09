create table [dbo].[product] (
	id int primary key not null identity(100000, 1),
	product_uid nchar(32) not null,
	pos_id int not null,
	pos_user_uid nchar(32) not null,
	pay_system_uid nchar(32) not null,
	
	product_name nvarchar(512) not null,
	product_description nvarchar(1024),
	product_price smallmoney not null,
	currency_code nchar(3) not null,
	
	customer_name nvarchar(128),
	
	pos_name nvarchar(256) not null,
	pos_url nchar(256),
	pos_city nvarchar(128),
	pos_address nvarchar(256),
	
	phone_reservation nchar(512),
	email_reservation nchar(512),
	
	valid_from datetime2,
	valid_till datetime2
)
go

alter table [dbo].[product] add customer_phone nchar(128) null;
go

alter table [dbo].[product] add customer_email nchar(128) null;
go

alter table [dbo].[product] add remarks nvarchar(1024) null;
go
