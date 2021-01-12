CREATE TABLE [Core].[Suspensions]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [Channel] NVARCHAR(200) NOT NULL,
    [Username] NVARCHAR(200) NOT NULL, 
    [SuspensionType] INT NOT NULL, 
    [Data] NVARCHAR(MAX) NOT NULL
)
GO

CREATE INDEX SUSPENSION_USERNAME ON [Core].[Suspensions]([Username])
GO

CREATE INDEX SUSPENSION_CHANNEL ON [Core].[Suspensions]([Channel])
GO