CREATE TABLE [Behaviour].[IncidentTypesForIncidentGeneralClassification]
(
	[typeId] [int] NOT NULL,
	[generalClassificationID] [int] NOT NULL,
    CONSTRAINT [PK_IncidentTypesForIncidentGeneralClassification] PRIMARY KEY CLUSTERED ([typeId] ,[generalClassificationID] ),
    CONSTRAINT [FK_IncidentTypesForIncidentGeneralClassification_IncidentGeneralClassification] FOREIGN KEY([generalClassificationID])
REFERENCES [Behaviour].[IncidentGeneralClassification] ([generalClassificationID]),
    CONSTRAINT [FK_IncidentTypesForIncidentGeneralClassification_IncidentTypes] FOREIGN KEY([typeId])
REFERENCES [Behaviour].[IncidentTypes] ([typeId])
)
