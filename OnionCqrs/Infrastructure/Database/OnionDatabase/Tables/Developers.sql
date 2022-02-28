CREATE TABLE [Persons].[Developers]
(
	[DeveloperId] INT NOT NULL IDENTITY(1,1),
	[FirstName] NVARCHAR(50) NOT NULL,
	[LastName] NVARCHAR(50) NOT NULL,
	[Email] NVARCHAR(50) NOT NULL,
	[CodeLinesPerHour] INT NOT NULL,
	[HoursPerDay] INT NOT NULL,
	CONSTRAINT PK_DeveloperId PRIMARY KEY CLUSTERED ([DeveloperId])
)
