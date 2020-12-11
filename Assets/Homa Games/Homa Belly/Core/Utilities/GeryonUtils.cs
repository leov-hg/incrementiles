using System;
using System.Linq;
using System.Reflection;

namespace HomaGames.HomaBelly.Utilities
{
    public static class GeryonUtils
    {
        /// <summary>
        /// Try to obtain Geryon NTesting ID with reflectionl. If not found,
        /// returns an empty string.
        /// </summary>
        /// <returns>The Geryon NTESTING_ID if found, or an empty string if not</returns>
        public static string GetGeryonTestingId()
		{
            string geryonNtestingId = "";
            try
            {
                HomaGamesLog.Debug($"Looking for Geryon NTESTING_ID");
                Type geryonConfig = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                                     from type in assembly.GetTypes()
                                     where type.Namespace == "HomaGames.Geryon" && type.Name == "Config"
                                     select type).FirstOrDefault();
                if (geryonConfig != null)
                {
                    System.Reflection.PropertyInfo ntestingIdPropertyInfo = geryonConfig.GetProperty("NTESTING_ID", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                    if (ntestingIdPropertyInfo != null)
                    {
                        var propertyValue = ntestingIdPropertyInfo.GetValue(null, null);
                        if (propertyValue != null && int.TryParse(propertyValue.ToString(), out int unused))
                        {
                            geryonNtestingId = propertyValue.ToString().PadLeft(9, '0');
                            HomaGamesLog.Debug($"Geryon NTESTING_ID found: {geryonNtestingId}");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                HomaGamesLog.Warning($"Geryon not found: {e.Message}");
            }

            return geryonNtestingId;
        }

        /// <summary>
        /// Try to obtain Geryon dynamic variable
        /// </summary>
        /// <param name="propertyName">The property name of the variable. All in caps and without type prefix: ie. IDFA_CONSENT_POPUP_DELAY_S</param>
        /// <returns></returns>
        public static string GetGeryonDynamicVariableValue(string propertyName)
        {
            string value = null;
            try
            {
                Type geryonDvrType = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                                      from type in assembly.GetTypes()
                                      where type.Name == "DVR"
                                      select type).FirstOrDefault();
                if (geryonDvrType != null)
                {
                    FieldInfo field = geryonDvrType.GetField(propertyName);
                    if (field != null)
                    {
                        value = field.GetValue(null).ToString();
                        UnityEngine.Debug.Log($"{propertyName} value from Geryon: {value}");
                    }
                }
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogWarning($"Could not obtain {propertyName} value from Geryon: {e.Message}");
            }

            return value;
        }
    }
}

