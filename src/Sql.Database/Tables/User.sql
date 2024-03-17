CREATE TABLE [dbo].[User]
(
	[Id]			INT				IDENTITY (1,1)	NOT NULL,
	[FirstName]		VARCHAR(100)					NOT NULL,
	[LastName]		VARCHAR(100)					NOT NULL,
	[CurrencyId]	INT								NOT NULL,

	CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([Id] ASC),

	CONSTRAINT [FK_UserCurrency] 
		FOREIGN KEY ([CurrencyId])
		REFERENCES [dbo].[Currency]([Id])
		ON UPDATE CASCADE
		ON DELETE NO ACTION,
)
