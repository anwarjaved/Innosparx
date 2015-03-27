CREATE TABLE [dbo].[Roles]
(
    [ID] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(64) NOT NULL, 
    [Description] NVARCHAR(1000) NULL, 
    [CreateDate] DATETIME NOT NULL, 
    [LastUpdatedDate] DATETIME NULL, 
    [RowVersion] ROWVERSION NOT NULL 
)

GO

CREATE UNIQUE INDEX [IX_Roles_Name] ON [dbo].[Roles] ([Name])
