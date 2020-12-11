using System.Collections.Generic;
using GameAnalyticsSDK;
using UnityEditor;
using UnityEngine;

namespace HomaGames.HomaBelly
{
    public class GameAnalyticsPostprocessor
    {
        private static int androidPlatformIndex;
        private static int iosPlatformIndex;

        [InitializeOnLoadMethod]
        static void Configure()
        {
            HomaBellyEditorLog.Debug($"Configuring {HomaBellyGameAnalyticsConstants.ID}");
            PluginManifest pluginManifest = PluginManifest.LoadFromLocalFile();

            if (pluginManifest != null)
            {
                PackageComponent packageComponent = pluginManifest.Packages
                    .GetPackageComponent(HomaBellyGameAnalyticsConstants.ID, HomaBellyGameAnalyticsConstants.TYPE);
                if (packageComponent != null)
                {
                    Dictionary<string, string> configurationData = packageComponent.Data;

                    // Setup Game Analytics Settings
                    GameAnalytics.SettingsGA.UsePlayerSettingsBuildNumber = true;
                    GatherPlatformIndexes();

                    try
                    {
                        // Update keys
                        if (configurationData.ContainsKey("s_android_game_key"))
                        {
                            GameAnalytics.SettingsGA.UpdateGameKey(androidPlatformIndex, configurationData["s_android_game_key"]);
                            GameAnalytics.SettingsGA.UpdateSecretKey(androidPlatformIndex, configurationData["s_android_secret_key"]);
                        }

                        if (configurationData.ContainsKey("s_ios_game_key"))
                        {
                            GameAnalytics.SettingsGA.UpdateGameKey(iosPlatformIndex, configurationData["s_ios_game_key"]);
                            GameAnalytics.SettingsGA.UpdateSecretKey(iosPlatformIndex, configurationData["s_ios_secret_key"]);
                        }

                        // Determine if GA must submit FPS events or not
                        if (configurationData.ContainsKey("b_submit_fps"))
                        {
                            bool submitFps = true;
                            bool.TryParse(configurationData["b_submit_fps"], out submitFps);
                            GameAnalytics.SettingsGA.SubmitFpsAverage = submitFps;
                            GameAnalytics.SettingsGA.SubmitFpsCritical = submitFps;
                        }

                        // Mark Game Analytics to dirty to force save
                        EditorUtility.SetDirty(GameAnalytics.SettingsGA);
                    }
                    catch (System.Exception e)
                    {
                        HomaBellyEditorLog.Error($"Error configuring Game Analytics: {e.Message}");
                    }
                }
            }
        }

        /// <summary>
        /// Obtains ANDROID and IOS Game Analytics platform indexed. If no
        /// platforms found, this method creates them.
        /// </summary>
        private static void GatherPlatformIndexes()
        {
            bool androidPlatformFound = false;
            bool iosPlatformFound = false;

            // Platforms already available in settings. Gather their indexes
            if (GameAnalytics.SettingsGA.Platforms != null)
            {
                for (int i = 0; i < GameAnalytics.SettingsGA.Platforms.Count; i++)
                {
                    RuntimePlatform platform = GameAnalytics.SettingsGA.Platforms[i];
                    if (platform == RuntimePlatform.Android)
                    {
                        androidPlatformIndex = i;
                        androidPlatformFound = true;
                    }

                    if (platform == RuntimePlatform.IPhonePlayer)
                    {
                        iosPlatformIndex = i;
                        iosPlatformFound = true;
                    }
                }
            }

            // If no ANDROID found, create and update index
            if (!androidPlatformFound)
            {
                HomaBellyEditorLog.Debug($"Creating Game Analytics Android platform");
                GameAnalytics.SettingsGA.AddPlatform(UnityEngine.RuntimePlatform.Android);
                androidPlatformIndex = GameAnalytics.SettingsGA.Platforms.Count - 1;
            }

            // If no IOS found, create and update index
            if (!iosPlatformFound)
            {
                HomaBellyEditorLog.Debug($"Creating Game Analytics iOS platform");
                GameAnalytics.SettingsGA.AddPlatform(UnityEngine.RuntimePlatform.IPhonePlayer);
                iosPlatformIndex = GameAnalytics.SettingsGA.Platforms.Count - 1;
            }
        }
    }
}
