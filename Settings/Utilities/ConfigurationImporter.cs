﻿using BS_Utils.Utilities;
using SaberTailor.Settings.Classes;
using System;
using System.Globalization;
using UnityEngine;

namespace SaberTailor.Settings.Utilities
{
    /// <summary>
    /// Imports the old configuration from ModPrefs. Just YEET this once ModPrefs support is dropped!
    /// </summary>
    internal class ConfigurationImporter
    {
        private static bool IsTrailEnabled;

        private static Vector3 GripLeftPosition;
        private static Vector3 GripRightPosition;

        private static Vector3 GripLeftRotation;
        private static Vector3 GripRightRotation;

        private static bool ModifyMenuHiltGrip;

        /// <summary>
        /// ONLY USED FOR A SPECIFIC PURPOSE IN "Configuration.cs". DO NOT USE ELSEWHERE!
        /// </summary>
        internal static PluginConfig ImportSettingsFromModPrefs(Config oldConfig)
        {
            PluginConfig importedSettings = new PluginConfig(); // Initialize a new default configuration

            try
            {
                // Import trail configuration
                IsTrailEnabled = oldConfig.GetBool(Plugin.PluginName, nameof(IsTrailEnabled), true, true);
                importedSettings.IsTrailEnabled = IsTrailEnabled;

                // Import grip position settings, convert old centimeter values to millimeter
                GripLeftPosition = ParseVector3(oldConfig.GetString(Plugin.PluginName, nameof(GripLeftPosition), "0,0,0", true));
                importedSettings.GripLeftPosition = new Float3()
                {
                    x = (int)Math.Round(Mathf.Clamp(GripLeftPosition.x, -50f, 50f) * 10),
                    y = (int)Math.Round(Mathf.Clamp(GripLeftPosition.y, -50f, 50f) * 10),
                    z = (int)Math.Round(Mathf.Clamp(GripLeftPosition.z, -50f, 50f) * 10)
                };

                GripLeftRotation = ParseVector3(oldConfig.GetString(Plugin.PluginName, nameof(GripLeftRotation), "0,0,0", true));
                importedSettings.GripLeftRotation = new Float3()
                {
                    x = (int)Math.Round(GripLeftRotation.x),
                    y = (int)Math.Round(GripLeftRotation.y),
                    z = (int)Math.Round(GripLeftRotation.z)
                };

                GripRightPosition = ParseVector3(oldConfig.GetString(Plugin.PluginName, nameof(GripRightPosition), "0,0,0", true));
                importedSettings.GripRightPosition = new Float3()
                {
                    x = (int)Math.Round(Mathf.Clamp(GripRightPosition.x, -50f, 50f) * 10),
                    y = (int)Math.Round(Mathf.Clamp(GripRightPosition.y, -50f, 50f) * 10),
                    z = (int)Math.Round(Mathf.Clamp(GripRightPosition.z, -50f, 50f) * 10)
                };

                GripRightRotation = ParseVector3(oldConfig.GetString(Plugin.PluginName, nameof(GripRightRotation), "0,0,0", true));
                importedSettings.GripRightRotation = new Float3()
                {
                    x = (int)Math.Round(GripRightRotation.x),
                    y = (int)Math.Round(GripRightRotation.y),
                    z = (int)Math.Round(GripRightRotation.z)
                };

                ModifyMenuHiltGrip = oldConfig.GetBool(Plugin.PluginName, nameof(ModifyMenuHiltGrip), true, true);
                importedSettings.ModifyMenuHiltGrip = ModifyMenuHiltGrip;

                // Mark imported config as config version 1 so it will run through all steps of the config updater
                importedSettings.ConfigVersion = 1;

                MarkAsExported(oldConfig);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return importedSettings;
        }

        private static void MarkAsExported(Config oldConfig)
        {
            oldConfig.SetBool(Plugin.PluginName, "IsExportedToNewConfig", true);
        }

        private static Vector3 ParseVector3(string originalString)
        {
            string[] components = originalString.Trim().Split(',');
            Vector3 parsedVector = Vector3.zero;

            if (components.Length == 3)
            {
                TryParseInvariantFloat(components[0], out parsedVector.x);
                TryParseInvariantFloat(components[1], out parsedVector.y);
                TryParseInvariantFloat(components[2], out parsedVector.z);
            }

            return parsedVector;
        }

        /// <summary>
        /// Tries to parse a float using invariant culture.
        /// </summary>
        /// <param name="number">The string containing the float to parse.</param>
        /// <param name="result">The parsed float, if successful.</param>
        /// <returns>True on success, false on failure.</returns>
        private static bool TryParseInvariantFloat(string number, out float result)
        {
            return float.TryParse(
                number,
                NumberStyles.Float,
                CultureInfo.InvariantCulture,
                out result
            );
        }

    }
}
