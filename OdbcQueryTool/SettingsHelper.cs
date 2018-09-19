using System;
using System.Configuration;

namespace OdbcQueryTool
{
    public class SettingsHelper
    {
        public string ConnectionString => ConfigurationManager.ConnectionStrings["OdbcDB"].ConnectionString;

        public TimeSpan Timeout
        {
            get
            {
                if (TimeSpan.TryParse(ConfigurationManager.AppSettings["CommmandTimeout"], out var result))
                {
                    return result;
                }
                return TimeSpan.FromSeconds(30);
            }
        }
    }
}
