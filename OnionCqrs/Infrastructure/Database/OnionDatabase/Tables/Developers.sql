CREATE TABLE [Projects].[Developers]
(
	[Id] BIGINT NOT NULL IDENTITY(1,1),
	[FirstName] NVARCHAR(50) NOT NULL,
	[LastName] NVARCHAR(50) NOT NULL,
	[CodeLinesPerHour] INT NOT NULL,
	[HoursPerDay] INT NOT NULL,
	[ProjectId] BIGINT NULL, 
    CONSTRAINT PK_Developer_Id PRIMARY KEY CLUSTERED ([Id]),
	CONSTRAINT FK_Project_Developer FOREIGN KEY ([ProjectID]) REFERENCES [Projects].[Projects](Id)
)
