CREATE PROCEDURE [Core].[GetChannels]
AS
BEGIN
	SELECT [ChannelName] FROM [Core].[Channels]
END