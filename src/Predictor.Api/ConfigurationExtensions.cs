using System.Text;
using Microsoft.Extensions.Configuration;

namespace Predictor.Api
{
    public static class ConfigurationExtensions
    {
        public static string Dump(this IConfiguration configuration)
        {
            StringBuilder log = new StringBuilder();
            log.Append("Configuration");
            log.AppendLine();
            foreach (IConfigurationSection child in configuration.GetChildren())
                DumpSection(child, log, 0, true);
            return log.ToString();
        }
        
        private static void DumpSection(
            IConfigurationSection section,
            StringBuilder log,
            int depth,
            bool rootSection = false)
        {
            log.Append('\t');
            log.Append(' ', depth * 2);
            log.AppendFormat("{0}: {1}\n", section.Key, section.Value);
            foreach (IConfigurationSection child in section.GetChildren())
                DumpSection(child, log, depth + 1, false);
            if (!rootSection)
                return;
            log.AppendLine();
        }
    }
}