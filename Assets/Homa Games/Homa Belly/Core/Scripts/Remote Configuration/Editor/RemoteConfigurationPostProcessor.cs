using System.Collections.Generic;
using System.IO;
using UnityEditor;
using HomaGames.HomaBelly.Utilities;
using UnityEngine;

namespace HomaGames.HomaBelly
{
    public class RemoteConfigurationPostProcessor
    {
        [InitializeOnLoadMethod]
        static void Configure()
        {
            // Track data for Remote Configuration
            PluginManifest pluginManifest = PluginManifest.LoadFromLocalFile();
            if (pluginManifest != null)
            {
                TrackConfiguration(pluginManifest.AppToken, pluginManifest.Packages.GetDependenciesAsTrackingString());
            }
        }

        /// <summary>
        /// Persists the Damysus configuration obtained from the installation
        /// process. This data will be sent to Damysus API on runtime
        /// </summary>
        /// <param name="appToken">Damysus App Token configured in the project</param>
        /// <param name="dependenciesAsTrackingString">Dependencies as a tracking string. ie: 'homabelly_core:1.0.3,gameanalytics:6.3.0'</param>
        private static void TrackConfiguration(string appToken, string dependenciesAsTrackingString)
        {
            WriteTrackingData(new Dictionary<string, object>()
            {
                { "ti", appToken },
                { "dp", dependenciesAsTrackingString }
            });
        }

        /// <summary>
        /// Writes the tracking data to the Streaming Assets config file
        /// </summary>
        /// <param name="trackingData"></param>
        private static void WriteTrackingData(Dictionary<string, object> trackingData)
        {
            // Create directory if does not exist
            string parentPath = Directory.GetParent(RemoteConfigurationConstants.TRACKING_FILE).ToString();
            if (!string.IsNullOrEmpty(parentPath) && !Directory.Exists(parentPath))
            {
                Directory.CreateDirectory(parentPath);
            }

            File.WriteAllText(RemoteConfigurationConstants.TRACKING_FILE, Json.Serialize(trackingData));
        }
    }
}
