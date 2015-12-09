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
	[is_payment_processed] [bit] NOT NULL,
	[created_at] [datetime2] NOT NULL,
	[pay_system_response_at] [datetime2] NULL
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[transaction] ADD  CONSTRAINT [DF_transaction_pay_system_id]  DEFAULT ((0)) FOR [pay_system_id]
GO

ALTER TABLE [dbo].[transaction] ADD  CONSTRAINT [DF_transaction_is_payment_processed]  DEFAULT ((0)) FOR [is_payment_processed]
GO

