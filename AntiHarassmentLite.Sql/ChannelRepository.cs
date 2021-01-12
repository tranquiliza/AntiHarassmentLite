using AntiHarassmentLite.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AntiHarassmentLite.Sql
{
    public class ChannelRepository : IChannelRepository
    {
        private readonly ISqlAccess sql;
        private readonly ILogger<ChannelRepository> logger;

        public ChannelRepository(string connectionString, ILogger<ChannelRepository> logger)
        {
            sql = SqlAccessBase.Create(connectionString);
            this.logger = logger;
        }

        public async Task<List<string>> GetChannelsToJoin()
        {
            var result = new List<string>();

            try
            {
                using (var command = sql.CreateStoredProcedure("[Core].[GetChannels]"))
                using (var reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    while (await reader.ReadAsync().ConfigureAwait(false))
                    {
                        result.Add(reader.GetString("channelName"));
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unable to fetch channels to join");
                throw;
            }
            return result;
        }

        public async Task JoinChannel(string channelName)
        {
            try
            {
                using (var command = sql.CreateStoredProcedure("[Core].[SaveJoinedChannel]"))
                {
                    command.WithParameter("channelName", channelName);

                    await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unable to save channel");
                throw;
            }
        }
    }
}
