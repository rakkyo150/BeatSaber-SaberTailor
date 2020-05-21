using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Parser;
using SaberTailor.HarmonyPatches;
using SaberTailor.Settings.Classes;
using SaberTailor.Settings.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace SaberTailor.Settings.UI
{
    public class MainSettings : PersistentSingleton<MainSettings>
    {
        [UIParams]
        private BSMLParserParams parserParams;

        #region Precision
        [UIValue("saber-pos-unit-options")]
        public List<object> SaberPosUnitValues = Enum.GetNames(typeof(PositionUnit)).ToList<object>();

        [UIValue("saber-pos-display-unit-options")]
        public List<object> SaberPosDisplayUnitValues = Enum.GetNames(typeof(PositionDisplayUnit)).ToList<object>();

        [UIValue("saber-pos-unit-value")]
        public string SaberPosIncUnit
        {
            get => Configuration.Menu.SaberPosIncUnit.ToString();
            set
            {
                Configuration.Menu.SaberPosIncUnit = Enum.TryParse(value, out PositionUnit positionUnit) ? positionUnit : PositionUnit.cm;
                UpdateSaberPosIncrement(Configuration.Menu.SaberPosIncUnit);
                RefreshPositionSettings();
            }
        }

        [UIValue("saber-pos-increment-value")]
        public int SaberPosIncValue
        {
            get => Configuration.Menu.SaberPosIncValue;
            set
            {
                Configuration.Menu.SaberPosIncValue = value;
                UpdateSaberPosIncrement(Configuration.Menu.SaberPosIncUnit);
                RefreshPositionSettings();
            }
        }

        [UIValue("saber-rot-increment-value")]
        public int SaberRotIncrement
        {
            get => Configuration.Menu.SaberRotIncrement;
            set => Configuration.Menu.SaberRotIncrement = value;
        }

        [UIValue("saber-pos-display-unit-value")]
        public string SaberPosDisplayUnit
        {
            get => Configuration.Menu.SaberPosDisplayUnit.ToString();
            set
            {
                Configuration.Menu.SaberPosDisplayUnit = Enum.TryParse(value, out PositionDisplayUnit unit) ? unit : PositionDisplayUnit.cm;
                RefreshPositionSettings();
            }
        }
        #endregion

        #region Saber Grip
        [UIValue("saber-grip-tweak-enabled")]
        public bool GripTweakEnabled
        {
            get => Configuration.Grip.IsGripModEnabled;
            set
            {
                if (value)
                {
                    SaberTailorPatches.ApplyHarmonyPatches();
                }
                else
                {
                    SaberTailorPatches.RemoveHarmonyPatches();
                }
                Configuration.Grip.IsGripModEnabled = value;
            }
        }
        #endregion

        #region Saber Grip MenuHilt
        [UIValue("menuhiltadjust-enabled")]
        public bool GripModifyMenuHiltGrip
        {
            get => Configuration.Grip.ModifyMenuHiltGrip;
            set => Configuration.Grip.ModifyMenuHiltGrip = value;
        }
        #endregion

        #region Saber Grip Left
        [UIValue("saber-left-position-x")]
        public int GripLeftPositionX
        {
            get => Configuration.GripCfg.PosLeft.x;
            set
            {
                int newVal = Increment(Configuration.GripCfg.PosLeft.x, Configuration.Menu.SaberPosIncrement, value);
                Configuration.GripCfg.PosLeft.x = Mathf.Clamp(newVal, SaberPosMin, SaberPosMax);
                RefreshPositionSettings();
            }
        }

        [UIValue("saber-left-position-y")]
        public int GripLeftPositionY
        {
            get => Configuration.GripCfg.PosLeft.y;
            set
            {
                int newVal = Increment(Configuration.GripCfg.PosLeft.y, Configuration.Menu.SaberPosIncrement, value);
                Configuration.GripCfg.PosLeft.y = Mathf.Clamp(newVal, SaberPosMin, SaberPosMax);
                RefreshPositionSettings();
            }
        }

        [UIValue("saber-left-position-z")]
        public int GripLeftPositionZ
        {
            get => Configuration.GripCfg.PosLeft.z;
            set
            {
                int newVal = Increment(Configuration.GripCfg.PosLeft.z, Configuration.Menu.SaberPosIncrement, value);
                Configuration.GripCfg.PosLeft.z = Mathf.Clamp(newVal, SaberPosMin, SaberPosMax);
                RefreshPositionSettings();
            }
        }

        [UIValue("saber-left-rotation-x")]
        public int GripLeftRotationX
        {
            get => Configuration.GripCfg.RotLeft.x;
            set
            {
                int newVal = Increment(Configuration.GripCfg.RotLeft.x, SaberRotIncrement, value);
                Configuration.GripCfg.RotLeft.x = Mathf.Clamp(newVal, SaberRotMin, SaberRotMax);
                RefreshRotationSettings();
            }
        }

        [UIValue("saber-left-rotation-y")]
        public int GripLeftRotationY
        {
            get => Configuration.GripCfg.RotLeft.y;
            set
            {
                int newVal = Increment(Configuration.GripCfg.RotLeft.y, SaberRotIncrement, value);
                Configuration.GripCfg.RotLeft.y = Mathf.Clamp(newVal, SaberRotMin, SaberRotMax);
                RefreshRotationSettings();
            }
        }

        [UIValue("saber-left-rotation-z")]
        public int GripLeftRotationZ
        {
            get => Configuration.GripCfg.RotLeft.z;
            set
            {
                int newVal = Increment(Configuration.GripCfg.RotLeft.z, SaberRotIncrement, value);
                Configuration.GripCfg.RotLeft.z = Mathf.Clamp(newVal, SaberRotMin, SaberRotMax);
                RefreshRotationSettings();
            }
        }
        #endregion

        #region Saber Grip Right
        [UIValue("saber-right-position-x")]
        public int GripRightPositionX
        {
            get => Configuration.GripCfg.PosRight.x;
            set
            {
                int newVal = Increment(Configuration.GripCfg.PosRight.x, Configuration.Menu.SaberPosIncrement, value);
                Configuration.GripCfg.PosRight.x = Mathf.Clamp(newVal, SaberPosMin, SaberPosMax);
                RefreshPositionSettings();
            }
        }

        [UIValue("saber-right-position-y")]
        public int GripRightPositionY
        {
            get => Configuration.GripCfg.PosRight.y;
            set
            {
                int newVal = Increment(Configuration.GripCfg.PosRight.y, Configuration.Menu.SaberPosIncrement, value);
                Configuration.GripCfg.PosRight.y = Mathf.Clamp(newVal, SaberPosMin, SaberPosMax);
                RefreshPositionSettings();
            }
        }

        [UIValue("saber-right-position-z")]
        public int GripRightPositionZ
        {
            get => Configuration.GripCfg.PosRight.z;
            set
            {
                int newVal = Increment(Configuration.GripCfg.PosRight.z, Configuration.Menu.SaberPosIncrement, value);
                Configuration.GripCfg.PosRight.z = Mathf.Clamp(newVal, SaberPosMin, SaberPosMax);
                RefreshPositionSettings();
            }
        }

        [UIValue("saber-right-rotation-x")]
        public int GripRightRotationX
        {
            get => Configuration.GripCfg.RotRight.x;
            set
            {
                int newVal = Increment(Configuration.GripCfg.RotRight.x, SaberRotIncrement, value);
                Configuration.GripCfg.RotRight.x = Mathf.Clamp(newVal, SaberRotMin, SaberRotMax);
                RefreshRotationSettings();
            }
        }

        [UIValue("saber-right-rotation-y")]
        public int GripRightRotationY
        {
            get => Configuration.GripCfg.RotRight.y;
            set
            {
                int newVal = Increment(Configuration.GripCfg.RotRight.y, SaberRotIncrement, value);
                Configuration.GripCfg.RotRight.y = Mathf.Clamp(newVal, SaberRotMin, SaberRotMax);
                RefreshRotationSettings();
            }
        }

        [UIValue("saber-right-rotation-z")]
        public int GripRightRotationZ
        {
            get => Configuration.GripCfg.RotRight.z;
            set
            {
                int newVal = Increment(Configuration.GripCfg.RotRight.z, SaberRotIncrement, value);
                Configuration.GripCfg.RotRight.z = Mathf.Clamp(newVal, SaberRotMin, SaberRotMax);
                RefreshRotationSettings();
            }
        }
        #endregion

        #region Saber Scale
        [UIValue("saber-scale-tweak-enabled")]
        public bool ScaleTweakEnabled
        {
            get => Configuration.Scale.TweakEnabled;
            set => Configuration.Scale.TweakEnabled = value;
        }

        [UIValue("saber-scale-hitbox-enabled")]
        public bool ScaleHitboxEnabled
        {
            get => Configuration.Scale.ScaleHitBox;
            set => Configuration.Scale.ScaleHitBox = value;
        }

        [UIValue("saber-scale-length")]
        public int ScaleLength
        {
            get => Configuration.ScaleCfg.Length;
            set => Configuration.ScaleCfg.Length = value;
        }

        [UIValue("saber-scale-girth")]
        public int ScaleGirth
        {
            get => Configuration.ScaleCfg.Girth;
            set => Configuration.ScaleCfg.Girth = value;
        }
        #endregion

        #region Saber Trail
        [UIValue("saber-trail-tweak-enabled")]
        public bool TrailTweakEnabled
        {
            get => Configuration.Trail.TweakEnabled;
            set => Configuration.Trail.TweakEnabled = value;
        }

        [UIValue("saber-trail-enabled")]
        public bool TrailEnabled
        {
            get => Configuration.Trail.TrailEnabled;
            set => Configuration.Trail.TrailEnabled = value;
        }

        [UIValue("saber-trail-length")]
        public int TrailLength
        {
            get => Configuration.Trail.Length;
            set => Configuration.Trail.Length = value;
        }
        #endregion

        #region Transfer Grip
        [UIComponent("transfer-txt")]
        private TextMeshProUGUI TransferText;
        #endregion

        #region Limits
        [UIValue("saber-pos-inc-max")]
        public int SaberPosIncMax => 200;

        [UIValue("saber-pos-inc-min")]
        public int SaberPosIncMin => 1;

        [UIValue("saber-pos-max")]
        public int SaberPosMax => 500;

        [UIValue("saber-pos-min")]
        public int SaberPosMin => -500;

        [UIValue("saber-rot-max")]
        public int SaberRotMax => 360;

        [UIValue("saber-rot-min")]
        public int SaberRotMin => -360;
        #endregion

        #region Formatters
        [UIAction("position-inc-formatter")]
        public string PositionIncString(int value)
        {
            switch (Configuration.Menu.SaberPosIncUnit)
            {
                case PositionUnit.cm:
                    return $"{value} cm";
                //case PositionUnit.inches:
                //    return string.Format("{0}/8 inches", value);
                default:
                    return $"{value} mm";
            }
        }

        [UIAction("position-formatter")]
        public string PositionString(int value)
        {
            switch (Configuration.Menu.SaberPosDisplayUnit)
            {
                case PositionDisplayUnit.inches:
                    return string.Format("{0:0.000} inches", value / 25.4f);
                case PositionDisplayUnit.nauticalmiles:
                    return string.Format("{0:0.000000} nmi", value / 1852000f);
                case PositionDisplayUnit.miles:
                    return string.Format("{0:0.000000} miles", value / 1609344f);
                default:
                    return string.Format("{0:0.0} cm", value / 10f);
            }
        }

        [UIAction("rotation-formatter")]
        public string RotationString(int value)
        {
            return $"{value} deg";
        }

        [UIAction("multiplier-formatter")]
        public string MultiplierString(int value)
        {
            return $"{value}%";
        }

        [UIAction("bool-formatter")]
        public string BoolString(bool value)
        {
            if (value)
            {
                return "<color=#57d657>On</color>";
            }
            else
            {
                return "<color=#fb484e>Off</color>";
            }
        }

        [UIAction("bool-offcolor-formatter")]
        public string BoolOffColorString(bool value)
        {
            if (value)
            {
                return "<color=#fb484e>On</color>";
            }
            else
            {
                return "<color=#57d657>Off</color>";
            }
        }
        #endregion

        [UIAction("update-saber-rotation")]
        public async void OnUpdateSaberRotation(float _)
        {
            // Delay this UI event to allow UI value to be set first
            await Task.Delay(20);
            Configuration.UpdateSaberRotation();
        }

        [UIAction("update-saber-position")]
        public async void OnUpdateSaberPosition(float _)
        {
            // Delay this UI event to allow UI value to be set first
            await Task.Delay(20);
            Configuration.UpdateSaberPosition();
        }

        [UIAction("#apply")]
        public void OnApply() => StoreConfiguration();

        [UIAction("#ok")]
        public void OnOk() => StoreConfiguration();

        [UIAction("#reset-saber-config")]
        public void OnResetSaberConfig() => ReloadConfiguration();

        [UIAction("#reset-saber-config-grip-left")]
        public void OnResetSaberConfigGripLeft() => ReloadConfiguration(ConfigSection.GripLeft);

        [UIAction("#reset-saber-config-grip-right")]
        public void OnResetSaberConfigGripRight() => ReloadConfiguration(ConfigSection.GripRight);

        [UIAction("#reset-saber-config-scale")]
        public void OnResetSaberConfigScale() => ReloadConfiguration(ConfigSection.Scale);

        [UIAction("#reset-saber-config-trail")]
        public void OnResetSaberConfigTrail() => ReloadConfiguration(ConfigSection.Trail);

        [UIAction("#cancel")]
        public void OnCancel() => ReloadConfiguration();

        [UIAction("#saber-grip-export")]
        public void OnGripExport() => ExportGripToGameSettings();

        [UIAction("#saber-grip-import")]
        public void OnGripImport() => ImportGripFromGameSettings();

        /// <summary>
        /// Save and update configuration
        /// </summary>
        private void StoreConfiguration()
        {
            Configuration.Save();
            Configuration.UpdateModVariables();
        }

        /// <summary>
        /// Reload configuration and refresh UI
        /// </summary>
        private void ReloadConfiguration(ConfigSection cfgSection = ConfigSection.All)
        {
            Configuration.Reload(cfgSection);
            RefreshModSettingsUI();
        }

        /// <summary>
        /// Refresh the entire UI
        /// </summary>
        private void RefreshModSettingsUI()
        {
            RefreshRotationSettings();
            RefreshPositionSettings();
            parserParams.EmitEvent("refresh-sabertailor-values");
        }

        /// <summary>
        /// Refresh position settings UI
        /// </summary>
        private void RefreshPositionSettings()
        {
            parserParams.EmitEvent("refresh-sabertailor-position-values");
        }

        /// <summary>
        /// Refresh rotation settings UI
        /// </summary>
        private void RefreshRotationSettings()
        {
            parserParams.EmitEvent("refresh-sabertailor-rotation-values");
        }

        private void ImportGripFromGameSettings()
        {
            bool importSuccessful = GameSettingsTransfer.ImportFromGame();
            if (importSuccessful)
            {
                TransferText.text = "Import successul. Please remember to enable saber grip adjustments in SaberTailor for enabling SaberTailor.";
            }
            else
            {
                TransferText.text = "<color=#fb484e>Unable to import from base game: Unknown error.</color>";
            }
        }

        private void ExportGripToGameSettings()
        {
            // FIXME!
            bool isExportable = GameSettingsTransfer.CheckGripCompatibility(Vector3.zero, Vector3.zero, out string compMsg);

            if (isExportable)
            {
                GameSettingsTransfer.ExportToGame(out string statusMsg);
                TransferText.text = statusMsg;
            }
            else
            {
                TransferText.text = compMsg;
            }
        }

        private void UpdateSaberPosIncrement(PositionUnit unit)
        {
            switch (unit)
            {
                case PositionUnit.cm:
                    Configuration.Menu.SaberPosIncrement = Configuration.Menu.SaberPosIncValue * 10;
                    break;
                //case PositionUnit.inches:
                //    Configuration.Menu.SaberPosIncrement = Configuration.Menu.SaberPosIncValue / 25.4f;
                //    break;
                default:
                    Configuration.Menu.SaberPosIncrement = Configuration.Menu.SaberPosIncValue;
                    break;
            }
        }

        /// <summary>
        /// Returns a value incremented by the magic number
        /// </summary>
        /// <param name="currentValue">Current value</param>
        /// <param name="incrementBy">Magic increment number</param>
        /// <param name="value">Real increment number</param>
        /// <returns></returns>
        private int Increment(int currentValue, int incrementBy, int value)
        {
            int result = currentValue;

            if (currentValue < value)
            {
                result += incrementBy;
            }
            else if (currentValue > value)
            {
                result -= incrementBy;
            }

            return result;
        }
    }
}
