CREATE TABLE [dbo].[SystemLogs]
(
    [ID] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    [RowVersion] ROWVERSION NOT NULL, 
    [Timestamp] DATETIME NOT NULL, 
    [MachineName] NVARCHAR(250) NULL, 
    [Message] NVARCHAR(MAX) NOT NULL, 
    [User] NVARCHAR(250) NULL, 
    [Type] TINYINT NOT NULL, 
    [ExceptionType] NVARCHAR(250) NULL, 
    [ApplicationName] NVARCHAR(250) NULL, 
    [SourceFile] NVARCHAR(250) NULL, 
    [LineNumber] INT NULL, 
    [MethodName] NVARCHAR(250) NULL, 
)

GO

CREATE INDEX [IX_SystemLog_MachineName] ON [dbo].[SystemLogs] ([MachineName])

GO

CREATE INDEX [IX_SystemLog_User] ON [dbo].[SystemLogs] ([User])

GO

CREATE INDEX [IX_SystemLog_ApplicationName] ON [dbo].[SystemLogs] ([ApplicationName])

GO

CREATE INDEX [IX_SystemLog_Timestamp] ON [dbo].[SystemLogs] ([Timestamp])

GO

CREATE INDEX [IX_SystemLog_Type] ON [dbo].[SystemLogs] ([Type])
