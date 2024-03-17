CREATE TABLE [dbo].[Currency]
(
	[Id]		INT			IDENTITY (1,1)		NOT NULL,
	[Code]		CHAR(3)							NOT NULL,
    [Name]		NVARCHAR(255)					NOT NULL,
    [Symbol]	NVARCHAR(10)					NOT NULL,

	CONSTRAINT [PK_Currency] PRIMARY KEY CLUSTERED ([Id] ASC),
);


