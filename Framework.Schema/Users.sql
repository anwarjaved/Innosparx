CREATE TABLE [dbo].[Users]
(
    [ID] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
      [CreateDate] DATETIME NOT NULL, 
    [LastUpdatedDate] DATETIME NULL, 
    [RowVersion] ROWVERSION NOT NULL, 
    [Email] NVARCHAR(256) NOT NULL,
    [Password] [nvarchar](64) NOT NULL,
	[PasswordSalt] [nvarchar](64) NOT NULL,
	[FirstName] [nvarchar](256) NOT NULL,
	[LastName] [nvarchar](256) NOT NULL,
	[IsVerified] [bit] NOT NULL,
	[PasswordFailureSinceLastSuccess] [int] NOT NULL,
	[LastPasswordFailureDate] [datetime] NULL,
	[LastActivityDate] [datetime] NULL,
	[LastLockoutDate] [datetime] NULL,
	[LastLoginDate] [datetime] NULL,
	[IsLockedOut] [bit] NOT NULL,
	[IsSuspended] [bit] NOT NULL,
	[LastPasswordChangedDate] [datetime] NULL,
	[Address1] [nvarchar](256) NULL,
	[Address2] [nvarchar](256) NULL,
	[City] [nvarchar](256) NULL,
	[State] [nvarchar](256) NULL,
	[ZipCode] [nvarchar](32) NULL,
	[Country] [nvarchar](512) NULL,
	[Phone] [nvarchar](20) NULL, 
    [GeoAddress] [geography] NULL, 
    [CompanyName] NVARCHAR(250) NULL, 
    [ProfileImage] NVARCHAR(250) NULL, 
    [ProfileThumbImage] NVARCHAR(250) NULL,
)

GO

CREATE UNIQUE INDEX [IX_Users_Email] ON [dbo].[Users] ([Email])

GO

CREATE INDEX [IX_Users_FirstName] ON [dbo].[Users] ([FirstName])

GO

CREATE INDEX [IX_Users_LastName] ON [dbo].[Users] ([LastName])
