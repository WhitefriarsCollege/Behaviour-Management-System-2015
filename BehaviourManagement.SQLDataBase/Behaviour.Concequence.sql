CREATE TABLE [Behaviour].[Concequence]
(
	[concequenceId] INT NOT NULL IDENTITY (1, 1),
    [concequence] VARCHAR(100) NOT NULL, 
    [multiplesPerDay] BIT NOT NULL DEFAULT 1, 
    [length] INT NOT NULL DEFAULT 1,
	[processorRoleClassification] VARCHAR(50) NULL, 
    CONSTRAINT [PK_Concequence] PRIMARY KEY ([concequenceId]) 
)
