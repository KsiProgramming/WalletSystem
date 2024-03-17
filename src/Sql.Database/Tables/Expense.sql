CREATE TABLE [dbo].[Expense]
(
	[Id]            INT          IDENTITY    (1,1)    NOT NULL,
	[DateTime]      DATETIME2                (7)      NOT NULL,
	[Description]   NVARCHAR                  (MAX)    NOT NULL,
	[TypeId]        INT                               NOT NULL,
	[UserId]        INT                               NOT NULL,
	[Amount]        DECIMAL                  (18,4)   NOT NULL,

	CONSTRAINT [PK_Expense] PRIMARY KEY CLUSTERED ([Id]),

	CONSTRAINT [FK_ExpenseType]
		FOREIGN KEY ([TypeId])
		REFERENCES [dbo].[ExpenseType]([Id])
		ON UPDATE CASCADE
		ON DELETE NO ACTION,

	CONSTRAINT [FK_ExpenseUser]
		FOREIGN KEY ([UserId])
		REFERENCES [dbo].[User]([Id])
		ON UPDATE CASCADE
		ON DELETE NO ACTION,

	INDEX [IX_Expense_Amount] NONCLUSTERED ([Amount] ASC),

	INDEX [IX_Expense_DateTime] NONCLUSTERED ([DateTime] ASC),

	INDEX [IX_Expense_UserId] NONCLUSTERED ([UserId] ASC)
)
