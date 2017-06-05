CREATE TABLE [Behaviour].[IncidentTypes]
(
	[typeId] INT NOT NULL IDENTITY (1, 1),
    [type] VARCHAR(100) NOT NULL, 
    [level] INT NOT NULL, 
    CONSTRAINT [PK_IncidentTypes] PRIMARY KEY ([typeId])
)
