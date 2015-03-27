CREATE TABLE [dbo].[QueueMessages]
(
    [ID] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [RowVersion] ROWVERSION NOT NULL, 
    [QueueName] NVARCHAR(80) NOT NULL, 
    [Body] NVARCHAR(256) NOT NULL, 
    [SentTimestamp] DATETIME NOT NULL, 
    [ApproximateReceiveCount] INT NOT NULL, 
    [ApproximateFirstReceiveTimestamp] DATETIME NOT NULL, 
    [LastAccessTimestamp] DATETIME NULL, 
    CONSTRAINT [FK_QueueMessages_ToMsgQueue] FOREIGN KEY ([QueueName]) REFERENCES [MessageQueue]([Name]) ON DELETE CASCADE
)
