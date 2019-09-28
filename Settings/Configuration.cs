using IPA.Config;
using SaberTailor.Settings.Classes;
using SaberTailor.Settings.Utilities;
using System;
using UnityEngine;
using LogLevel = IPA.Logging.Logger.Level;

namespace SaberTailor.Settings
{
    public static class Configuration
    {
        public static readonly int ConfigVersion = 3;   // Current configuration version
        public static bool ShowCallSource;              // Set this to true in configuration to enable call source in logs and terminal

        public static SaberGripConfiguration Grip = new SaberGripConfiguration();
        public static SaberScaleConfiguration Scale = new SaberScaleConfiguration();
        public static SaberTrailConfiguration Trail = new SaberTrailConfiguration();

        // Config vars for representing player settings before it gets mangled into something that works with the game,
        // but changes representation of these settings in the process - also avoiding floating points
        public static SaberGripRawConfiguration GripCfg = new SaberGripRawConfiguration();
        public static SaberScaleRawConfiguration ScaleCfg = new SaberScaleRawConfiguration();

        public static void Save()
        {
            #region Internal settings
            Plugin.config.Value.ConfigVersion = ConfigVersion;
            Plugin.config.Value.Logging["ShowCallSource"] = ShowCallSource;
            #endregion

            #region Saber scale
            Plugin.config.Value.IsSaberScaleModEnabled = Scale.TweakEnabled;
            Plugin.config.Value.SaberScaleHitbox = Scale.ScaleHitBox;
            Plugin.config.Value.SaberLength = ScaleCfg.Length;
            Plugin.config.Value.SaberGirth = ScaleCfg.Girth;
            #endregion

            #region Saber trail
            Plugin.config.Value.IsTrailModEnabled = Trail.TweakEnabled;
            Plugin.config.Value.IsTrailEnabled = Trail.TrailEnabled;
            Plugin.config.Value.TrailLength = Trail.Length;
            #endregion

            #region Saber grip
            // Even though the field says GripLeftPosition/GripRightPosition, it is actually the Cfg values that are stored!
            Plugin.config.Value.GripLeftPosition = GripCfg.PosLeft;
            Plugin.config.Value.GripRightPosition = GripCfg.PosRight;

            // Even though the field says GripLeftRotation/GripRightRotation, it is actually the Cfg values that are stored!
            Plugin.config.Value.GripLeftRotation = GripCfg.RotLeft;
            Plugin.config.Value.GripRightRotation = GripCfg.RotRight;

            Plugin.config.Value.ModifyMenuHiltGrip = Grip.ModifyMenuHiltGrip;
            #endregion

            // Store configuration to file
            Plugin.configProvider.Store(Plugin.config.Value);
        }

        public static void Load()
        {
            // Just YEET this once ModPrefs support is dropped!
#pragma warning disable CS0618 // ModPrefs is obsolete
            if (ModPrefs.HasKey(Plugin.PluginName, "GripLeftPosition") && !ModPrefs.GetBool(Plugin.PluginName, "IsExportedToNewConfig", false))
#pragma warning restore CS0618 // ModPrefs is obsolete
            {
                // Import SaberTailor's settings from the old configuration (ModPrefs)
                try
                {
                    PluginConfig importedConfig = ConfigurationImporter.ImportSettingsFromModPrefs();
                    Plugin.config.Value = importedConfig;

                    // Store configuration in the new format immediately
                    Plugin.configProvider.Store(Plugin.config.Value);

                    Logger.Log("Configuration loaded from ModPrefs", LogLevel.Notice);
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                    Logger.Log("Failed to import ModPrefs configuration. Loading default BSIPA configuration instead.", LogLevel.Notice);
                }
            }

            LoadConfig();
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
            Scale.Length = ScaleCfg.Length / 100f;
            Scale.Girth = ScaleCfg.Girth / 100f;
        }

        public static void UpdateSaberPosition()
        {
            Grip.PosLeft = FormattedVector3_To_Vector3(GripCfg.PosLeft) / 1000f;
            Grip.PosRight = FormattedVector3_To_Vector3(GripCfg.PosRight) / 1000f;
        }

        public static void UpdateSaberRotation()
        {
            Grip.RotLeft = Quaternion.Euler(FormattedVector3_To_Vector3(GripCfg.RotLeft)).eulerAngles;
            Grip.RotRight = Quaternion.Euler(FormattedVector3_To_Vector3(GripCfg.RotRight)).eulerAngles;
        }

        private static void LoadConfig()
        {
            Plugin.configProvider.Load();

            #region Internal settings
            if (Plugin.config.Value.Logging.TryGetValue("ShowCallSource", out object showCallSource) && showCallSource is bool loggerShowCallSource)
            {
                ShowCallSource = loggerShowCallSource;
            }
            #endregion

            #region Saber scale
            Scale.TweakEnabled = Plugin.config.Value.IsSaberScaleModEnabled;
            Scale.ScaleHitBox = Plugin.config.Value.SaberScaleHitbox;

            if (Plugin.config.Value.SaberLength < 5 || Plugin.config.Value.SaberLength > 500)
            {
                ScaleCfg.Length = 100;
            }
            else
            {
                ScaleCfg.Length = Plugin.config.Value.SaberLength;
            }

            if (Plugin.config.Value.SaberGirth < 5 || Plugin.config.Value.SaberGirth > 500)
            {
                ScaleCfg.Girth = 100;
            }
            else
            {
                ScaleCfg.Girth = Plugin.config.Value.SaberGirth;
            }
            #endregion

            #region Saber trail
            Trail.TweakEnabled = Plugin.config.Value.IsTrailModEnabled;
            Trail.TrailEnabled = Plugin.config.Value.IsTrailEnabled;
            Trail.Length = Math.Max(5, Math.Min(100, Plugin.config.Value.TrailLength));
            #endregion

            #region Saber grip
            // Even though the field says GripLeftPosition/GripRightPosition, it is actually the Cfg values that are loaded!
            StoreableIntVector3 gripLeftPosition = Plugin.config.Value.GripLeftPosition;
            GripCfg.PosLeft = new StoreableIntVector3()
            {
                x = Mathf.Clamp(gripLeftPosition.x, -500, 500),
                y = Mathf.Clamp(gripLeftPosition.y, -500, 500),
                z = Mathf.Clamp(gripLeftPosition.z, -500, 500)
            };

            StoreableIntVector3 gripRightPosition = Plugin.config.Value.GripRightPosition;
            GripCfg.PosRight = new StoreableIntVector3()
            {
                x = Mathf.Clamp(gripRightPosition.x, -500, 500),
                y = Mathf.Clamp(gripRightPosition.y, -500, 500),
                z = Mathf.Clamp(gripRightPosition.z, -500, 500)
            };

            // Even though the field says GripLeftRotation/GripRightRotation, it is actually the Cfg values that are loaded!
            GripCfg.RotLeft = Plugin.config.Value.GripLeftRotation;
            GripCfg.RotRight = Plugin.config.Value.GripRightRotation;

            Grip.ModifyMenuHiltGrip = Plugin.config.Value.ModifyMenuHiltGrip;
            #endregion
        }
        
        /// <summary>
        /// Handle updates and additions to configuration
        /// </summary>
        private static void UpdateConfig()
        {
            if (Plugin.config.Value.ConfigVersion != ConfigVersion)
            {
                // v1 -> v{latest}: Added enable/disable options for trail and scale modifications
                if (ConfigVersion == 1)
                {
                    // Disable trail modifications if settings are default
                    Trail.TweakEnabled = (!Trail.TrailEnabled || Trail.Length != 20);
                }
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
