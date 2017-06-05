CREATE TABLE [Behaviour].[ConcequenceCommunicationTemplate]
(
	[communicationTemplateId] INT NOT NULL IDENTITY (1, 1),
    [description] VARCHAR(MAX) NULL, 
	[subject] VARCHAR(100) NULL, 
    [message] VARCHAR(MAX) NULL, 
    [concequenceId] INT NULL, 
	sendToStudent bit NOT NULL CONSTRAINT DF_ConcequenceCommunicationTemplate_sendToStudent DEFAULT 0,
	sendToParent bit NOT NULL CONSTRAINT DF_ConcequenceCommunicationTemplate_sendToParent DEFAULT 0,
	sendToPCT bit NOT NULL CONSTRAINT DF_ConcequenceCommunicationTemplate_sendToPCT DEFAULT 1,
	sendToHOH bit NOT NULL CONSTRAINT DF_ConcequenceCommunicationTemplate_sendToHOH DEFAULT 0,
	sendToDP bit NOT NULL CONSTRAINT DF_ConcequenceCommunicationTemplate_sendToDP DEFAULT 0
    CONSTRAINT [FK_ConcequenceCommunicationTemplate_To_Concequence] FOREIGN KEY ([concequenceId]) REFERENCES [Behaviour].[Concequence]([concequenceId]), 
    CONSTRAINT [PK_ConcequenceCommunicationTemplate] PRIMARY KEY ([communicationTemplateId])
)
