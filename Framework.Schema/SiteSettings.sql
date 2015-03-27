CREATE TABLE [dbo].[SiteSettings]
(
    [Name] NVARCHAR(250) NOT NULL PRIMARY KEY, 
    [Value] NVARCHAR(MAX) NULL, 
    [RowVersion] ROWVERSION NOT NULL
)
