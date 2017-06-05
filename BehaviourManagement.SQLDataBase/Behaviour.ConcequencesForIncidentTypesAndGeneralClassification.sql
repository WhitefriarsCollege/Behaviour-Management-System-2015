CREATE TABLE [Behaviour].[ConcequencesForIncidentTypesAndGeneralClassification]
(
	[typeId] INT NOT NULL,
	[concequenceId] INT NOT NULL, 
	[generalClassificationID] INT NOT NULL,
    CONSTRAINT [PK_ConcequencesForIncidentTypesAndGeneralClassification] PRIMARY KEY ([typeId], [concequenceId],[generalClassificationID]), 
    CONSTRAINT [FK_ConcequencesForIncidentTypesAndGeneralClassification_Concequence] FOREIGN KEY ([concequenceId]) REFERENCES [Behaviour].[Concequence]([concequenceId]), 
    CONSTRAINT [FK_ConcequencesForIncidentTypesAndGeneralClassification_IncidentTypes] FOREIGN KEY ([typeId]) REFERENCES [Behaviour].[IncidentTypes]([typeId]), 
	CONSTRAINT [FK_ConcequencesForIncidentTypesAndGeneralClassification_IncidentGeneralClassification] FOREIGN KEY ([generalClassificationID]) REFERENCES [Behaviour].[IncidentGeneralClassification]([generalClassificationID])
)
