using AntiHarassmentLite.Core;
using AntiHarassmentLite.Core.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace AntiHarassmentLite.Sql
{
    public class SuspensionRepository : ISuspensionRepository
    {
        private readonly ISqlAccess sql;
        private readonly ILogger<SuspensionRepository> logger;

        public SuspensionRepository(string sqlConnection, ILogger<SuspensionRepository> logger)
        {
            sql = SqlAccessBase.Create(sqlConnection);
            this.logger = logger;
        }

        public async Task<List<Suspension>> GetSuspensions(string username)
        {
            var result = new List<Suspension>();

            try
            {
                using (var command = sql.CreateStoredProcedure("[Core].[GetSuspensionsForUsername]"))
                {
                    command.WithParameter("username", username);
                    using (var reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                    {
                        while (await reader.ReadAsync().ConfigureAwait(false))
                        {
                            result.Add(JsonSerializer.Deserialize<Suspension>(reader.GetString("data")));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unable to fetch suspension for {username}", username);
                throw;
            }

            return result;
        }

        public async Task SaveSuspension(Suspension suspension)
        {
            try
            {
                using (var command = sql.CreateStoredProcedure("[Core].[SaveSuspension]"))
                {
                    command.WithParameter("channel", suspension.Channel)
                        .WithParameter("username", suspension.Username)
                        .WithParameter("suspensionType", suspension.SuspensionType)
                        .WithParameter("data", JsonSerializer.Serialize(suspension));

                    await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unable to save suspension");
                throw;
            }
        }
    }
}
