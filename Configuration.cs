using System;
using IPA.Config;
using LogLevel = IPA.Logging.Logger.Level;
using UnityEngine;

namespace SaberTailor
{
    public static class Configuration
    {
        public static float Length;
        public static bool IsTrailEnabled;
        public static int TrailLength;

        public static Vector3 GripLeftPosition;
        public static Vector3 GripRightPosition;
        public static Vector3 GripLeftRotation;
        public static Vector3 GripRightRotation;

        public static bool ModifyMenuHiltGrip;

        // ..raw vars for representing player settings before Unity mangled it into something that works with the game 
        // but also changed representation of these settings
        public static Vector3 GripLeftRotationRaw;
        public static Vector3 GripRightRotationRaw;


        public static void Save()
        {
            Plugin.config.Value.Length = Length;
            Plugin.config.Value.IsTrailEnabled = IsTrailEnabled;
            Plugin.config.Value.TrailLength = TrailLength;

            Plugin.config.Value.GripLeftPosition = Vector3_To_FormattedVector3(GripLeftPosition * 100f);
            Plugin.config.Value.GripRightPosition = Vector3_To_FormattedVector3(GripRightPosition * 100f);

            // Even though the field says GripLeftRotation/GripRightRotation, it is actually the Raw values that are stored!
            Plugin.config.Value.GripLeftRotation = Vector3_To_FormattedVector3(GripLeftRotationRaw);
            Plugin.config.Value.GripRightRotation = Vector3_To_FormattedVector3(GripRightRotationRaw);

            Plugin.config.Value.ModifyMenuHiltGrip = ModifyMenuHiltGrip;

            // Store configuration to file
            Plugin.configProvider.Store(Plugin.config.Value);
        }

        public static void Load()
        {
            if (ModPrefs.HasKey(Plugin.PluginName, "GripLeftPosition") && !ModPrefs.GetBool(Plugin.PluginName, "IsExportedToNewConfig", false))
            {
                // Import SaberTailor's settings from the old configuration (ModPrefs)
                try
                {
                    ConfigUtilities.ConfigurationImporter.ImportSettingsFromModPrefs();
                    Logger.Log("Configuration loaded from ModPrefs", LogLevel.Notice);
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                    Logger.Log("Failed to import ModPrefs configuration. Loading default BSIPA configuration instead.", LogLevel.Notice);

                    LoadConfig();
                }
            }
            else
            {
                LoadConfig();
            }

            Logger.Log("Configuration has been set", LogLevel.Debug);
            UpdateSaberRotation();
        }

        public static void UpdateSaberRotation()
        {
            GripLeftRotation = Quaternion.Euler(GripLeftRotationRaw).eulerAngles;
            GripRightRotation = Quaternion.Euler(GripRightRotationRaw).eulerAngles;
        }

        private static void LoadConfig()
        {
            Length = Math.Max(0.01f, Math.Min(2f, Plugin.config.Value.Length));
            IsTrailEnabled = Plugin.config.Value.IsTrailEnabled;
            TrailLength = Math.Max(5, Math.Min(100, Plugin.config.Value.TrailLength));

            GripLeftPosition = FormattedVector3_To_Vector3(Plugin.config.Value.GripLeftPosition) / 100f;
            GripLeftPosition = new Vector3
            {
                x = Mathf.Clamp(GripLeftPosition.x, -0.5f, 0.5f),
                y = Mathf.Clamp(GripLeftPosition.y, -0.5f, 0.5f),
                z = Mathf.Clamp(GripLeftPosition.z, -0.5f, 0.5f)
            };
            GripRightPosition = FormattedVector3_To_Vector3(Plugin.config.Value.GripRightPosition) / 100f;
            GripRightPosition = new Vector3
            {
                x = Mathf.Clamp(GripRightPosition.x, -0.5f, 0.5f),
                y = Mathf.Clamp(GripRightPosition.y, -0.5f, 0.5f),
                z = Mathf.Clamp(GripRightPosition.z, -0.5f, 0.5f)
            };

            // Even though the field says GripLeftRotation/GripRightRotation, it is actually the Raw values that are stored!
            GripLeftRotationRaw = FormattedVector3_To_Vector3(Plugin.config.Value.GripLeftRotation);
            GripRightRotationRaw = FormattedVector3_To_Vector3(Plugin.config.Value.GripRightRotation);

            ModifyMenuHiltGrip = Plugin.config.Value.ModifyMenuHiltGrip;
        }

        /// <summary>
        /// Converts the UnityEngine.Vector3 format to the PluginConfig.StoreableVector3 format for storage
        /// </summary>
        private static ConfigUtilities.StoreableVector3 Vector3_To_FormattedVector3(Vector3 vector3) => new ConfigUtilities.StoreableVector3()
        {
            x = vector3.x,
            y = vector3.y,
            z = vector3.z
        };

        /// <summary>
        /// Converts the PluginConfig.StoreableVector3 to a UnityEngine.Vector3 format
        /// </summary>
        private static Vector3 FormattedVector3_To_Vector3(ConfigUtilities.StoreableVector3 vector3) => new Vector3()
        {
            x = vector3.x,
            y = vector3.y,
            z = vector3.z
        };
    }
}
