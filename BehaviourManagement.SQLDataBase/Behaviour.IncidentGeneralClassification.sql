CREATE TABLE [Behaviour].[IncidentGeneralClassification]
(
	generalClassificationID int NOT NULL IDENTITY (1, 1),
	locationID int,
	[classification] varchar(100) NOT NULL, 
	[displayOrder] int NOT NULL CONSTRAINT DF_IncidentGeneralClassification_displayOrder DEFAULT 0, 
    CONSTRAINT [PK_IncidentGeneralClassification] PRIMARY KEY (generalClassificationID), 
    CONSTRAINT [FK_IncidentGeneralClassification_IncidentLocation] FOREIGN KEY (locationID) REFERENCES [Behaviour].[IncidentLocations](locationID) 
)