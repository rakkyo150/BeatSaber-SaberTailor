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

        // Config vars for representing player settings before it gets mangled into something that works with the game,
        // but changes representation of these settings in the process - also avoiding floating points
        public static ConfigUtilities.StoreableIntVector3 GripLeftPositionCfg;
        public static ConfigUtilities.StoreableIntVector3 GripRightPositionCfg;
        public static ConfigUtilities.StoreableIntVector3 GripLeftRotationCfg;
        public static ConfigUtilities.StoreableIntVector3 GripRightRotationCfg;


        public static void Save()
        {
            Plugin.config.Value.SaberLength = SaberLength;
            Plugin.config.Value.SaberGirth = SaberGirth;

            Plugin.config.Value.IsTrailEnabled = IsTrailEnabled;
            Plugin.config.Value.TrailLength = TrailLength;

            // Even though the field says GripLeftPosition/GripRightPosition, it is actually the Cfg values that are stored!
            Plugin.config.Value.GripLeftPosition = GripLeftPositionCfg;
            Plugin.config.Value.GripRightPosition = GripRightPositionCfg;

            // Even though the field says GripLeftRotation/GripRightRotation, it is actually the Cfg values that are stored!
            Plugin.config.Value.GripLeftRotation = GripLeftRotationCfg;
            Plugin.config.Value.GripRightRotation = GripRightRotationCfg;

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

            // Update variables used by mod logic
            UpdateSaberPosition();
            UpdateSaberRotation();
        }

        public static void UpdateSaberPosition()
        {
            GripLeftPosition = FormattedVector3_To_Vector3(GripLeftPositionCfg) / 1000f;
            GripRightPosition = FormattedVector3_To_Vector3(GripRightPositionCfg) / 1000f;
        }

        public static void UpdateSaberRotation()
        {
            GripLeftRotation = Quaternion.Euler(FormattedVector3_To_Vector3(GripLeftRotationCfg)).eulerAngles;
            GripRightRotation = Quaternion.Euler(FormattedVector3_To_Vector3(GripRightRotationCfg)).eulerAngles;
        }

        private static void LoadConfig()
        {
            Plugin.configProvider.Load();
            if (Plugin.config.Value.SaberLength < 0.01f || Plugin.config.Value.SaberLength > 5f)
            {
                SaberLength = 1.0f;
            }
            else
            {
                SaberLength = Plugin.config.Value.SaberLength;
            }
            if (Plugin.config.Value.SaberGirth < 0.01f || Plugin.config.Value.SaberGirth > 5f)
            {
                SaberGirth = 1.0f;
            }
            else
            {
                SaberGirth = Plugin.config.Value.SaberGirth;
            }

            IsTrailEnabled = Plugin.config.Value.IsTrailEnabled;
            TrailLength = Math.Max(5, Math.Min(100, Plugin.config.Value.TrailLength));

            GripLeftPositionCfg = Plugin.config.Value.GripLeftPosition;
            GripLeftPositionCfg = new ConfigUtilities.StoreableIntVector3()
            {
                x = Mathf.Clamp(GripLeftPositionCfg.x, -500, 500),
                y = Mathf.Clamp(GripLeftPositionCfg.y, -500, 500),
                z = Mathf.Clamp(GripLeftPositionCfg.z, -500, 500)
            };
            //GripRightPosition = FormattedVector3_To_Vector3(Plugin.config.Value.GripRightPosition) / 100f;
            GripRightPositionCfg = Plugin.config.Value.GripRightPosition;
            GripRightPositionCfg = new ConfigUtilities.StoreableIntVector3()
            {
                x = Mathf.Clamp(GripRightPositionCfg.x, -500, 500),
                y = Mathf.Clamp(GripRightPositionCfg.y, -500, 500),
                z = Mathf.Clamp(GripRightPositionCfg.z, -500, 500)
            };

            // Even though the field says GripLeftRotation/GripRightRotation, it is actually the Cfg values that are stored!
            GripLeftRotationCfg = Plugin.config.Value.GripLeftRotation;
            GripRightRotationCfg = Plugin.config.Value.GripRightRotation;

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
