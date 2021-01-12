CREATE PROCEDURE [Core].[GetSuspensionsForUsername]
	@username NVARCHAR(200)
AS
BEGIN
	SELECT [Data] FROM [Core].[Suspensions] WHERE [Username] = @username
END