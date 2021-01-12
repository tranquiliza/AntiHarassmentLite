CREATE PROCEDURE [Core].[SaveSuspension]
	@channel NVARCHAR(200),
	@username NVARCHAR(200),
	@suspensionType INT,
	@data NVARCHAR(MAX)
AS
BEGIN
	INSERT INTO [Core].[Suspensions]([Channel], [Username], [SuspensionType], [Data])
	VALUES(@channel, @username, @suspensionType, @data)
END