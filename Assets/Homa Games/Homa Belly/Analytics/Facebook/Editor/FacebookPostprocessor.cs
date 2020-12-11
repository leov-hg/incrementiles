﻿using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using UnityEditor;
using UnityEngine;

namespace HomaGames.HomaBelly
{
    public class FacebookPostprocessor
    {
        private static string EDITOR_PREFS_KEY_MANIFEST_REQUESTED = "homagames.homabelly.facebook.manifest_requested";

        [InitializeOnLoadMethod]
        static void Configure()
        {
            HomaBellyEditorLog.Debug($"Configuring {HomaBellyFacebookConstants.ID}");
            PluginManifest pluginManifest = PluginManifest.LoadFromLocalFile();

            if (pluginManifest != null)
            {
                PackageComponent packageComponent = pluginManifest.Packages
                    .GetPackageComponent(HomaBellyFacebookConstants.ID, HomaBellyFacebookConstants.TYPE);
                if (packageComponent != null)
                {
                    Dictionary<string, string> configurationData = packageComponent.Data;

                    if (configurationData != null)
                    {

                        try
                        {

                            if (!File.Exists(Path.Combine(Application.dataPath, "/FacebookSDK/SDK/Resources/FacebookSettings.asset")))
                            {
                                // Create Facebook settings
                                EditorApplication.ExecuteMenuItem(HomaBellyFacebookConstants.CREATE_FACEBOOK_SETTINGS_MENU);
                            }
                        }
                        catch (System.Exception)
                        {
                            // Ignore
                        }

                        // Configure app ID
                        Facebook.Unity.Settings.FacebookSettings.AppIds[0] = configurationData["s_app_id"];
                        Facebook.Unity.Settings.FacebookSettings.AppLabels[0] = Application.productName;
                        
                        // Determine if FB should send base events or no
                        if (configurationData.ContainsKey("b_auto_log_app_events_enabled"))
                        {
                            bool autoLogAppEventsEnabled = true;
                            bool.TryParse(configurationData["b_auto_log_app_events_enabled"], out autoLogAppEventsEnabled);
                            Facebook.Unity.Settings.FacebookSettings.AutoLogAppEventsEnabled = autoLogAppEventsEnabled;
                        }

#if UNITY_2019_3_OR_NEWER && UNITY_ANDROID
                        // If Facebook says Android SDK is not set, try to bypass the warning by setting `AndroidSdkRoot` to the actual path in Unity Editor
                        if (!Facebook.Unity.Editor.FacebookAndroidUtil.HasAndroidSDK())
                        {
                            EditorPrefs.SetString("AndroidSdkRoot", UnityEditor.Android.AndroidExternalToolsSettings.sdkRootPath);
                        }
#endif

                        // If an AndroidManifest.xml file is not found in Facebook.Unity.Editor.ManifestMod.AndroidManifestPath, generate it
                        if (!File.Exists(Path.Combine(Application.dataPath, Facebook.Unity.Editor.ManifestMod.AndroidManifestPath)))
                        {
                            // If not requested to add the AndroidManifest.xml before, ask dev to add it
                            if (!EditorPrefs.GetBool(EDITOR_PREFS_KEY_MANIFEST_REQUESTED, false))
                            {
                                EditorPrefs.SetBool(EDITOR_PREFS_KEY_MANIFEST_REQUESTED, true);
                                bool accepted = EditorUtility.DisplayDialog("Facebook Android Manifest", "Facebook requires a base AndroidManifest.xml file.\nWould you like to create it under Assets/Plugins/Android?", "Create (recommended)", "No, thanks");
                                if (accepted)
                                {
                                    Facebook.Unity.Editor.ManifestMod.GenerateManifest();
                                }
                            }
                        }

                        // Always try to remove `android:debuggable` attribute
                        RemoveDebuggableAttribute(Path.Combine(Application.dataPath, Facebook.Unity.Editor.ManifestMod.AndroidManifestPath));
                    }
                }
            }
        }

        /// <summary>
        /// Removes the `android:debuggable` attribute from `application` in AndroidManifest
        /// </summary>
        /// <param name="manifestFilePath">The path to the AndroidManifest file</param>
        private static void RemoveDebuggableAttribute(string manifestFilePath)
        {
            XDocument manifest = null;
            XNamespace androidNamespace = null;
            if (File.Exists(manifestFilePath))
            {
                try
                {
                    manifest = XDocument.Load(manifestFilePath);
                    androidNamespace = manifest.Root.GetNamespaceOfPrefix("android");
                }
                catch (IOException exception)
                {
                    HomaBellyEditorLog.Error($"Could not load manifest file: {exception.Message}");
                }
            }

            if (manifest != null)
            {
                // Sanity check: manifest
                var elementManifest = manifest.Element("manifest");
                if (elementManifest == null)
                {
                    HomaBellyEditorLog.Error($"Manifest does not have `manifest` element");
                    return;
                }

                // Sanity check: application
                var elementApplication = elementManifest.Element("application");
                if (elementApplication == null)
                {
                    HomaBellyEditorLog.Error($"Manifest does not have `application` element");
                    return;
                }

                // Get android:debuggable attribute (if any) and remove it
                XAttribute debuggableAttribute = elementApplication.Attribute(androidNamespace + "debuggable");
                if (debuggableAttribute != null)
                {
                    debuggableAttribute.Remove();
                }

                manifest.Save(manifestFilePath);
            }
        }
    }
}
