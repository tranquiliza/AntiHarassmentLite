CREATE PROCEDURE [Core].[SaveJoinedChannel]
	@channelName NVARCHAR(200)
AS
BEGIN
	IF NOT EXISTS(SELECT * FROM [Core].[Channels] WHERE [ChannelName] = @channelName)
			INSERT INTO [Core].[Channels]([ChannelName]) 
			VALUES (@channelName)
END