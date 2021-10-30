using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Wpf
{
    /// <summary>
    /// 
    /// </summary>
    class ConfigManager
    {
        /// <summary>
        /// Gets configuration setting from app.config file
        /// </summary>
        /// <param name="appSettingKey">What setting to fetch</param>
        /// <returns>value of setting</returns>
        public static string GetAppConf(string appSettingKey)
        {
            string value = ConfigurationManager.AppSettings[appSettingKey];
            string def = "https://localhost:5001/games";
            if (string.IsNullOrEmpty(value))
            {
                var message = $"Cannot find value for appSetting key: '{appSettingKey}', using default https://localhost:5001/games instead.";
                Trace.Listeners.Add(new TextWriterTraceListener("log.log", "listener"));
                Trace.TraceInformation(message);
                Trace.Flush();
                MessageBox.Show(message);
                return def;
                //throw new ConfigurationErrorsException(message);
            }
            return value;
        }
    }
}
