CREATE TABLE [dbo].[MessageQueue]
(
    [RowVersion] ROWVERSION NOT NULL, 
    [Name] NVARCHAR(80) NOT NULL, 
    [VisibilityTimeout] INT NOT NULL, 
    [MessageRetentionPeriod] INT NOT NULL, 
    [Delay] INT NOT NULL, 
    [CreatedTimestamp] DATETIME NOT NULL, 
    PRIMARY KEY ([Name]) 
)
