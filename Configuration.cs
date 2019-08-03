using System;
using IPA.Config;
using LogLevel = IPA.Logging.Logger.Level;
using UnityEngine;

namespace SaberTailor
{
    public static class Configuration
    {
        public static float SaberLength;
        public static float SaberGirth;

        public static bool IsTrailEnabled;
        public static int TrailLength;

        public static Vector3 GripLeftPosition;
        public static Vector3 GripRightPosition;
        public static Vector3 GripLeftRotation;
        public static Vector3 GripRightRotation;

        public static bool ModifyMenuHiltGrip;

        // ..raw vars for representing player settings before it gets mangled into something that works with the game,
        // but changes representation of these settings in the process - also avoiding floating points
        public static ConfigUtilities.StoreableIntVector3 GripLeftPositionRaw;
        public static ConfigUtilities.StoreableIntVector3 GripRightPositionRaw;
        public static ConfigUtilities.StoreableIntVector3 GripLeftRotationRaw;
        public static ConfigUtilities.StoreableIntVector3 GripRightRotationRaw;


        public static void Save()
        {
            Plugin.config.Value.SaberLength = SaberLength;
            Plugin.config.Value.SaberGirth = SaberGirth;

            Plugin.config.Value.IsTrailEnabled = IsTrailEnabled;
            Plugin.config.Value.TrailLength = TrailLength;

            // Even though the field says GripLeftPosition/GripRightPosition, it is actually the Raw values that are stored!
            Plugin.config.Value.GripLeftPosition = GripLeftPositionRaw;
            Plugin.config.Value.GripRightPosition = GripRightPositionRaw;

            // Even though the field says GripLeftRotation/GripRightRotation, it is actually the Raw values that are stored!
            Plugin.config.Value.GripLeftRotation = GripLeftRotationRaw;
            Plugin.config.Value.GripRightRotation = GripRightRotationRaw;

            Plugin.config.Value.ModifyMenuHiltGrip = ModifyMenuHiltGrip;

            // Store configuration to file
            Plugin.configProvider.Store(Plugin.config.Value);
        }

        public static void Load()
        {
            // Plan for this ModPrefs part is just to yeet it once BSIPA-Support for ModPrefs has been removed. Or replace by BSUtils INI implementation.
            #pragma warning disable CS0618 // ModPrefs is obsolete
            if (ModPrefs.HasKey(Plugin.PluginName, "GripLeftPosition") && !ModPrefs.GetBool(Plugin.PluginName, "IsExportedToNewConfig", false))
            #pragma warning restore CS0618 // ModPrefs is obsolete
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
            UpdateSaberPosition();
            UpdateSaberRotation();
        }

        public static void UpdateSaberPosition()
        {
            GripLeftPosition = FormattedVector3_To_Vector3(GripLeftPositionRaw) / 1000f;
            GripRightPosition = FormattedVector3_To_Vector3(GripRightPositionRaw) / 1000f;
        }

        public static void UpdateSaberRotation()
        {
            GripLeftRotation = Quaternion.Euler(FormattedVector3_To_Vector3(GripLeftRotationRaw)).eulerAngles;
            GripRightRotation = Quaternion.Euler(FormattedVector3_To_Vector3(GripRightRotationRaw)).eulerAngles;
        }

        private static void LoadConfig()
        {
            SaberLength = Math.Max(0.01f, Math.Min(5f, Plugin.config.Value.SaberLength));
            SaberGirth = Math.Max(0.01f, Math.Min(5f, Plugin.config.Value.SaberGirth));

            IsTrailEnabled = Plugin.config.Value.IsTrailEnabled;
            TrailLength = Math.Max(5, Math.Min(100, Plugin.config.Value.TrailLength));

            GripLeftPositionRaw = Plugin.config.Value.GripLeftPosition;
            GripLeftPositionRaw = new ConfigUtilities.StoreableIntVector3()
            {
                x = Mathf.Clamp(GripLeftPositionRaw.x, -500, 500),
                y = Mathf.Clamp(GripLeftPositionRaw.y, -500, 500),
                z = Mathf.Clamp(GripLeftPositionRaw.z, -500, 500)
            };
            //GripRightPosition = FormattedVector3_To_Vector3(Plugin.config.Value.GripRightPosition) / 100f;
            GripRightPositionRaw = Plugin.config.Value.GripRightPosition;
            GripRightPositionRaw = new ConfigUtilities.StoreableIntVector3()
            {
                x = Mathf.Clamp(GripRightPositionRaw.x, -500, 500),
                y = Mathf.Clamp(GripRightPositionRaw.y, -500, 500),
                z = Mathf.Clamp(GripRightPositionRaw.z, -500, 500)
            };

            // Even though the field says GripLeftRotation/GripRightRotation, it is actually the Raw values that are stored!
            GripLeftRotationRaw = Plugin.config.Value.GripLeftRotation;
            GripRightRotationRaw = Plugin.config.Value.GripRightRotation;

            ModifyMenuHiltGrip = Plugin.config.Value.ModifyMenuHiltGrip;
        }

        /// <summary>
        /// Converts the PluginConfig.StoreableIntVector3 to a UnityEngine.Vector3 format
        /// </summary>
        private static Vector3 FormattedVector3_To_Vector3(ConfigUtilities.StoreableIntVector3 vector3) => new Vector3()
        {
            x = vector3.x,
            y = vector3.y,
            z = vector3.z
        };
    }
}
