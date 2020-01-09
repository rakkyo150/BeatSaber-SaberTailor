using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Parser;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace SaberTailor.Settings.UI
{
    public class MainSettings : PersistentSingleton<MainSettings>
    {
        [UIParams]
        private BSMLParserParams parserParams;

        #region Precision
        public int SaberPosIncrement = 10;      // Always in mm
        public int SaberPosIncValue = 1;
        public string SaberPosIncUnit = "cm";
        public string SaberPosDisplayUnit = "cm";

        [UIValue("saber-pos-unit-options")]
        public List<object> SaberPosUnitValues = new List<object>()
        {
            "cm",
            "mm",
            "inches"
        };

        [UIValue("saber-pos-display-unit-options")]
        public List<object> SaberPosDisplayUnitValues = new List<object>()
        {
            "cm",
            "inches",
            "miles"
        };

        [UIValue("saber-pos-unit-value")]
        public string _SaberPosIncUnit {
            get => SaberPosIncUnit;
            set
            {
                SaberPosIncUnit = value;
                switch (SaberPosIncUnit)
                {
                    case "cm":
                        SaberPosIncrement = SaberPosIncValue * 10;
                        SaberPosDisplayUnit = "cm";
                        break;
                    case "inches":
                        SaberPosIncrement = (int)Math.Round(SaberPosIncValue * 3.175f);
                        SaberPosDisplayUnit = "inches";
                        break;
                    default:
                        SaberPosIncrement = SaberPosIncValue;
                        SaberPosDisplayUnit = "cm";
                        break;
                }
                RefreshPositionSettings();
            }
        }

        [UIValue("saber-pos-increment-value")]
        public int _SaberPosIncValue {
            get
            {
                return SaberPosIncValue;
            }
            set
            {
                SaberPosIncValue = value;
                switch (SaberPosIncUnit)
                {
                    case "cm":
                        SaberPosIncrement = SaberPosIncValue * 10;
                        break;
                    case "inches":
                        SaberPosIncrement = (int)Math.Round(SaberPosIncValue * 3.175f);     // Modified in 1/8 inches steps, rounded to mm
                        break;
                    default:
                        SaberPosIncrement = SaberPosIncValue;
                        break;
                }
                RefreshPositionSettings();
            }
        }

        [UIValue("saber-rot-increment-value")]
        public int SaberRotIncrement { get; set; } = 5;

        [UIValue("saber-pos-display-unit-value")]
        public string _SaberPosDisplayUnit
        {
            get => SaberPosDisplayUnit;
            set
            {
                SaberPosDisplayUnit = value;
                RefreshPositionSettings();
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
                int newVal = Increment(Configuration.GripCfg.PosLeft.x, SaberPosIncrement, value, SaberPosIncUnit);
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
                int newVal = Increment(Configuration.GripCfg.PosLeft.y, SaberPosIncrement, value, SaberPosIncUnit);
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
                int newVal = Increment(Configuration.GripCfg.PosLeft.z, SaberPosIncrement, value, SaberPosIncUnit);
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
                int newVal = Increment(Configuration.GripCfg.PosRight.x, SaberPosIncrement, value, SaberPosIncUnit);
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
                int newVal = Increment(Configuration.GripCfg.PosRight.y, SaberPosIncrement, value, SaberPosIncUnit);
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
                int newVal = Increment(Configuration.GripCfg.PosRight.z, SaberPosIncrement, value, SaberPosIncUnit);
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
            switch (SaberPosIncUnit)
            {
                case "cm":
                    return $"{value} cm";
                case "inches":
                    return String.Format("{0}/8 inches", value);
                default:
                    return $"{value} mm";
            }
        }

        [UIAction("position-formatter")]
        public string PositionString(int value)
        {
            switch (SaberPosDisplayUnit)
            {
                case "inches":
                    return String.Format("{0:0.000} inches", value / 25.4f);
                case "miles":
                    return String.Format("{0:0.000000} miles", value / 1609344f);
                default:
                    return String.Format("{0:0.0} cm", value / 10f);
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

        [UIAction("#cancel")]
        public void OnCancel() => ReloadConfiguration();

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
        private void ReloadConfiguration()
        {
            Configuration.Reload();
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

        /// <summary>
        /// Returns a value incremented by the magic number
        /// </summary>
        /// <param name="currentValue">Current value</param>
        /// <param name="incrementBy">Magic increment number</param>
        /// <param name="value">Real increment number</param>
        /// <returns></returns>
        private int Increment(int currentValue, int incrementBy, int value, string unit="")
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
            if (unit == "inches")
            {
                result = RoundToOneEightInch(result);
            }

            return result;
        }

        /// <summary>
        /// Rounds a mm value to the closest 1/8 inch
        /// </summary>
        /// <param name="value">mm value to be rounded</param>
        /// <returns></returns>
        private int RoundToOneEightInch(int value)
        {
            double result = value * 8 / 25.4d;
            result = Math.Round((Math.Round(result) / 8) * 25.4);
            return (int)result;
        }
    }
}
