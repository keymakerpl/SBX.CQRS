CREATE TABLE [Projects].[Projects]
(
	[Id] BIGINT NOT NULL IDENTITY(1,1),
	[Name] NVARCHAR(150) NOT NULL,
	[DevelopersLimit] INT NOT NULL,
	[CodeLinesToWrite] INT NOT NULL,
	[Status] NVARCHAR(50) NOT NULL,
	[StartDate] DATE NOT NULL,
	[DeadLine] DATE NOT NULL,
	CONSTRAINT [PK_Projects_Id] PRIMARY KEY ([Id])
)
