CREATE TABLE [Behaviour].[IncidentLocations]
(
	locationID int NOT NULL IDENTITY (1, 1),
	location varchar(100) NOT NULL, 
	[displayOrder] int NOT NULL CONSTRAINT DF_IncidentLocations_displayOrder DEFAULT 0, 
    CONSTRAINT [PK_IncidentLocations] PRIMARY KEY ([locationID]) 
)
