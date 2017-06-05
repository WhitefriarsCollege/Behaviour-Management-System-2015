CREATE TABLE [Behaviour].[ConcequenceCommunicationTemplateDataFields]
(
	[fieldId] INT NOT NULL IDENTITY (1, 1),
	[communicationTemplateId] INT NULL, 
    [fieldName] VARCHAR(50) NULL, 
    CONSTRAINT [FK_ConcequenceCommunicationTemplateDataFileds_To_ConcequenceCommunicationTemplate] FOREIGN KEY (communicationTemplateId) REFERENCES [Behaviour].[ConcequenceCommunicationTemplate]([communicationTemplateId]), 
    CONSTRAINT [PK_ConcequenceCommunicationTemplateDataFields] PRIMARY KEY ([fieldId]) 
)
