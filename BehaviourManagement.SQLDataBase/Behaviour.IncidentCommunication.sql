CREATE TABLE [Behaviour].[IncidentCommunication]
(
	[communicationId] INT NOT NULL IDENTITY (1, 1),
	[incidentId] INT NOT NULL, 
    [sent] SMALLDATETIME NULL DEFAULT GetDate(), 
    [from] VARCHAR(255) NULL, 
    [to] VARCHAR(255) NULL, 
    [cc] VARCHAR(MAX) NULL, 
    [bcc] VARCHAR(MAX) NULL, 
    [message] VARCHAR(MAX) NULL, 
    [subject] VARCHAR(100) NULL, 
    CONSTRAINT [PK_IncidentCommunication] PRIMARY KEY ([communicationId]), 
    CONSTRAINT [FK_IncidentCommunication_Incident] FOREIGN KEY ([incidentId]) REFERENCES [Behaviour].[Incident]([incidentId])
)
