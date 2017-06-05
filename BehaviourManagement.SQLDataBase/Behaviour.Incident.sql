CREATE TABLE [Behaviour].[Incident]
(
	[incidentId] INT NOT NULL IDENTITY (1, 1),
	[incidentDate] SMALLDATETIME NULL, 
    [incidentLocation] VARCHAR(100) NULL, 
	[incidentType] [varchar](100) NULL,
	[incidentLevel] [int] NULL,
	[studentID] VARCHAR(50) NULL,
	[staffID] VARCHAR(255) NULL, 
    [businessKey] INT NULL, 
    [concequence] VARCHAR(100) NULL, 
    [concequenceDateStart] DATE NULL, 
    [concequenceDateEnd] DATE NULL, 
    [processingRequired] BIT NULL, 
    [processedDate] SMALLDATETIME NULL, 
    [modifiedDate] SMALLDATETIME NULL, 
    [modifiedBy] VARCHAR(255) NULL, 
    CONSTRAINT [PK_Incident] PRIMARY KEY ([incidentId]), 
    CONSTRAINT [FK_Incident_To_DC_Staff] FOREIGN KEY (staffID) REFERENCES dbo.DC_Staff([employeeID]),
	CONSTRAINT [FK_Incident_To_DC_Student] FOREIGN KEY (studentID) REFERENCES dbo.DC_Student([studentID]) 
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Primary key for behaviour record in LOB application. i.e. eWorkspace ewBehaviour table Id value for this item',
    @level0type = N'SCHEMA',
    @level0name = N'Behaviour',
    @level1type = N'TABLE',
    @level1name = N'Incident',
    @level2type = N'COLUMN',
    @level2name = N'businessKey'