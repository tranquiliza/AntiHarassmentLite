using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.Configuration
{
    public static class ConfigurationExtensions
    {
        public static T GetRequiredValue<T>(this IConfiguration configuration, string key)
        {
            var result = configuration.GetValue<T>(key);
            if (EqualityComparer<T>.Default.Equals(result, default))
                throw new Exception($"Value with name {key} has not been set");

            if (result is string && string.IsNullOrEmpty(result as string))
                throw new Exception($"Value with name {key} has not been set");

            return result;
        }
    }
}
