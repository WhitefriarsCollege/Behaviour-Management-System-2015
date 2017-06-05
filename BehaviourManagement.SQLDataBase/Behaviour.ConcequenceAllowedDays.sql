CREATE TABLE [Behaviour].[ConcequenceAllowedDays]
(
	[allowedDaysId] INT NOT NULL IDENTITY (1, 1),
	[concequenceId] INT, 
    [allowedOnDay] BIT NOT NULL DEFAULT 1, 
    [cycleDay] BIT NOT NULL DEFAULT 1, 
	[day] INT NOT NULL DEFAULT 1,
	[yearLevel] INT NULL, 
    CONSTRAINT [PK_ConcequenceAllowedDays] PRIMARY KEY ([allowedDaysId]), 
    CONSTRAINT [FK_ConcequenceAllowedDays_Concequence] FOREIGN KEY ([concequenceId]) REFERENCES [Behaviour].[Concequence]([concequenceId]) 
)
