CREATE TABLE [dbo].[gslog] (
    [id] [int] IDENTITY (1, 1) NOT NULL,
    [date] [datetime] NOT NULL,
    [thread] [varchar] (255) NOT NULL,
    [level] [varchar] (50) NOT NULL,
    [logger] [varchar] (255) NOT NULL,
    [message] [varchar] (4000) NOT NULL,
    [exception] [varchar] (2000) NULL
)
go