using System;
using System.Globalization;
using IPA.Config;
using UnityEngine;

namespace SaberTailor.ConfigUtilities
{
    /// <summary>
    /// Imports the old configuration from ModPrefs
    /// </summary>
    internal class ConfigurationImporter
    {
        private static float Length;
        private static bool IsTrailEnabled;
        private static int TrailLength;

        private static Vector3 GripLeftPosition;
        private static Vector3 GripRightPosition;

        private static Vector3 GripLeftRotation;
        private static Vector3 GripRightRotation;

        private static bool ModifyMenuHiltGrip;

        /// <summary>
        /// ONLY USED FOR A SPECIFIC PURPOSE IN "Configuration.cs". DO NOT USE ELSEWHERE!
        /// </summary>
        internal static void ImportSettingsFromModPrefs()
        {
            try
            {
#pragma warning disable CS0618 // ModPrefs is obsolete
                Length = ModPrefs.GetFloat(Plugin.PluginName, nameof(Length), 1f, true);
                Configuration.SaberLength = Math.Max(0.01f, Math.Min(2f, Length));

                Configuration.IsTrailEnabled = ModPrefs.GetBool(Plugin.PluginName, nameof(IsTrailEnabled), true, true);

                TrailLength = ModPrefs.GetInt(Plugin.PluginName, nameof(TrailLength), 20, true);
                Configuration.TrailLength = Math.Max(5, Math.Min(100, TrailLength));

                GripLeftPosition = ParseVector3(ModPrefs.GetString(Plugin.PluginName, nameof(GripLeftPosition), "0,0,0", true)) / 100f;
                Configuration.GripLeftPosition = new Vector3
                {
                    x = Mathf.Clamp(GripLeftPosition.x, -0.5f, 0.5f),
                    y = Mathf.Clamp(GripLeftPosition.y, -0.5f, 0.5f),
                    z = Mathf.Clamp(GripLeftPosition.z, -0.5f, 0.5f)
                };
                Configuration.GripLeftRotationRaw = ParseVector3(ModPrefs.GetString(Plugin.PluginName, nameof(GripLeftRotation), "0,0,0", true));

                GripRightPosition = ParseVector3(ModPrefs.GetString(Plugin.PluginName, nameof(GripRightPosition), "0,0,0", true)) / 100f;
                Configuration.GripRightPosition = new Vector3
                {
                    x = Mathf.Clamp(GripRightPosition.x, -0.5f, 0.5f),
                    y = Mathf.Clamp(GripRightPosition.y, -0.5f, 0.5f),
                    z = Mathf.Clamp(GripRightPosition.z, -0.5f, 0.5f)
                };
                Configuration.GripRightRotationRaw = ParseVector3(ModPrefs.GetString(Plugin.PluginName, nameof(GripRightRotation), "0,0,0", true));

                Configuration.ModifyMenuHiltGrip = ModPrefs.GetBool(Plugin.PluginName, nameof(ModifyMenuHiltGrip), false, true);
#pragma warning restore CS0618 // ModPrefs is obsolete

                // Save configuration in the new format
                Configuration.Save();
                MarkAsExported();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void MarkAsExported()
        {
#pragma warning disable CS0618 // ModPrefs is obsolete
            ModPrefs.SetBool(Plugin.PluginName, "IsExportedToNewConfig", true);
#pragma warning restore CS0618 // ModPrefs is obsolete
        }

        private static Vector3 ParseVector3(string originalString)
        {
            string[] components = originalString.Trim().Split(',');
            Vector3 parsedVector = Vector3.zero;

            if (components.Length != 3) return parsedVector;

            TryParseInvariantFloat(components[0], out parsedVector.x);
            TryParseInvariantFloat(components[1], out parsedVector.y);
            TryParseInvariantFloat(components[2], out parsedVector.z);

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
