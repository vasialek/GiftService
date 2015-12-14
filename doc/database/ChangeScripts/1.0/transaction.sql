/****** Object:  Table [dbo].[transaction]    Script Date: 2015-12-06 00:34:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[transaction](
	[id] [int] IDENTITY(100000,1) PRIMARY KEY NOT NULL,
	[pos_id] [int] NOT NULL,
	[product_uid] [nchar](32) NOT NULL,
	[product_id] [int] NOT NULL,
	[pos_user_uid] [nchar](32) NOT NULL,
	[pay_system_uid] [nchar](32) NULL,
	
	[pay_system_id] [int] NOT NULL,
	
	-- Used by Paysera
	[project_id] [nchar](32) not null,
	
	[is_payment_processed] [bit] NOT NULL,
	
	[payment_status_id] [int] not null default(0),
	[is_test_payment] [bit] not null default(1),
	
	[paid_through] [nchar](64) null,
	
	[requested_amount] [smallmoney] not null,
	[requested_currency_code] [nchar](3),
	[paid_amount] [smallmoney] not null default(0),
	[paid_currency_code] [nchar](3) null,
	[remarks] [nvarchar](256) null,
	
	-- Information about payer
	[p_name] [nvarchar](64) null,
	[p_lastname] [nvarchar](128) null,
	[p_email] [nchar](128) null,
	[p_phone] [nchar](128) null,
	
	[response_from_payment] [nvarchar](2048) null,

	[created_at] [datetime2] NOT NULL,
	[pay_system_response_at] [datetime2] NULL
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[transaction] ADD  CONSTRAINT [DF_transaction_pay_system_id]  DEFAULT ((0)) FOR [pay_system_id]
GO

ALTER TABLE [dbo].[transaction] ADD  CONSTRAINT [DF_transaction_is_payment_processed]  DEFAULT ((0)) FOR [is_payment_processed]
GO

