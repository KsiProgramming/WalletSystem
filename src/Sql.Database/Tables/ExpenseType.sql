CREATE TABLE [dbo].[ExpenseType]
(
	[Id]		INT				IDENTITY (1,1)		NOT NULL,
	[Label]		VARCHAR(MAX)						NOT NULL,

	CONSTRAINT [PK_ExepenseType] PRIMARY KEY CLUSTERED ([Id] ASC)
)