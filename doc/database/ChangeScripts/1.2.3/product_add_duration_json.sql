alter table [dbo].[product] add product_duration nvarchar(256) null;
go

-- Store custom options as JSON object
alter table [dbo].[product] add custom_json varchar(4096) null;
go

