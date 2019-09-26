using IPA.Config;
using SaberTailor.ConfigUtilities;
using System;
using UnityEngine;
using LogLevel = IPA.Logging.Logger.Level;

namespace SaberTailor
{
    public static class Configuration
    {
        public static int ConfigVersion;

        public static bool IsSaberScaleModEnabled;
        public static float SaberLength;
        public static float SaberGirth;

        public static bool IsTrailModEnabled;
        public static bool IsTrailEnabled;
        public static int TrailLength;

        public static Vector3 GripLeftPosition;
        public static Vector3 GripRightPosition;
        public static Vector3 GripLeftRotation;
        public static Vector3 GripRightRotation;

        public static bool ModifyMenuHiltGrip;

        // Config vars for representing player settings before it gets mangled into something that works with the game,
        // but changes representation of these settings in the process - also avoiding floating points
        public static int SaberLengthCfg;
        public static int SaberGirthCfg;
        public static StoreableIntVector3 GripLeftPositionCfg;
        public static StoreableIntVector3 GripRightPositionCfg;
        public static StoreableIntVector3 GripLeftRotationCfg;
        public static StoreableIntVector3 GripRightRotationCfg;


        public static void Save()
        {
            Plugin.config.Value.ConfigVersion = ConfigVersion;

            Plugin.config.Value.IsSaberScaleModEnabled = IsSaberScaleModEnabled;
            Plugin.config.Value.SaberLength = SaberLengthCfg;
            Plugin.config.Value.SaberGirth = SaberGirthCfg;

            Plugin.config.Value.IsTrailModEnabled = IsTrailModEnabled;
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
                    ConfigurationImporter.ImportSettingsFromModPrefs();
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

            UpdateConfig();
            Logger.Log("Configuration has been set", LogLevel.Debug);

            // Update variables used by mod logic
            UpdateModVariables();
        }

        public static void UpdateModVariables()
        {
            UpdateSaberLength();
            UpdateSaberPosition();
            UpdateSaberRotation();
        }

        public static void UpdateSaberLength()
        {
            SaberLength = SaberLengthCfg / 100f;
            SaberGirth = SaberGirthCfg / 100f;
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

            ConfigVersion = Plugin.config.Value.ConfigVersion;

            IsSaberScaleModEnabled = Plugin.config.Value.IsSaberScaleModEnabled;
            if (Plugin.config.Value.SaberLength < 5 || Plugin.config.Value.SaberLength > 500)
            {
                SaberLengthCfg = 100;
            }
            else
            {
                SaberLengthCfg = Plugin.config.Value.SaberLength;
            }

            if (Plugin.config.Value.SaberGirth < 5 || Plugin.config.Value.SaberGirth > 500)
            {
                SaberGirthCfg = 100;
            }
            else
            {
                SaberGirthCfg = Plugin.config.Value.SaberGirth;
            }

            IsTrailModEnabled = Plugin.config.Value.IsTrailModEnabled;
            IsTrailEnabled = Plugin.config.Value.IsTrailEnabled;
            TrailLength = Math.Max(5, Math.Min(100, Plugin.config.Value.TrailLength));

            // Even though the field says GripLeftPosition/GripRightPosition, it is actually the Cfg values that are loaded!
            GripLeftPositionCfg = Plugin.config.Value.GripLeftPosition;
            GripLeftPositionCfg = new StoreableIntVector3()
            {
                x = Mathf.Clamp(GripLeftPositionCfg.x, -500, 500),
                y = Mathf.Clamp(GripLeftPositionCfg.y, -500, 500),
                z = Mathf.Clamp(GripLeftPositionCfg.z, -500, 500)
            };
            GripRightPositionCfg = Plugin.config.Value.GripRightPosition;
            GripRightPositionCfg = new StoreableIntVector3()
            {
                x = Mathf.Clamp(GripRightPositionCfg.x, -500, 500),
                y = Mathf.Clamp(GripRightPositionCfg.y, -500, 500),
                z = Mathf.Clamp(GripRightPositionCfg.z, -500, 500)
            };

            // Even though the field says GripLeftRotation/GripRightRotation, it is actually the Cfg values that are loaded!
            GripLeftRotationCfg = Plugin.config.Value.GripLeftRotation;
            GripRightRotationCfg = Plugin.config.Value.GripRightRotation;

            ModifyMenuHiltGrip = Plugin.config.Value.ModifyMenuHiltGrip;
        }

        // Handle updates and additions to configuration
        private static void UpdateConfig()
        {
            // v1 -> v2: Added enable/disable options for trail and scale modifications
            if (ConfigVersion == 1)
            {
                // Disable trail modifications if settings are default
                if (IsTrailEnabled && TrailLength == 20)
                {
                    IsTrailModEnabled = false;
                }
                else
                {
                    IsTrailModEnabled = true;
                }
                ConfigVersion = 2;
            }
        }

        /// <summary>
        /// Converts the PluginConfig.StoreableIntVector3 to a UnityEngine.Vector3 format
        /// </summary>
        private static Vector3 FormattedVector3_To_Vector3(StoreableIntVector3 vector3) => new Vector3()
        {
            x = vector3.x,
            y = vector3.y,
            z = vector3.z
        };
    }
}
