using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Parser;
using System.Threading.Tasks;

namespace SaberTailor.Settings.UI
{
    public class MainSettings : PersistentSingleton<MainSettings>
    {
        private bool isFakeCancelEvent = false;

        [UIParams]
        private BSMLParserParams parserParams;

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
            get => Configuration.GripCfg.PosLeft.x / 10;
            set => Configuration.GripCfg.PosLeft.x = value * 10;
        }

        [UIValue("saber-left-position-y")]
        public int GripLeftPositionY
        {
            get => Configuration.GripCfg.PosLeft.y / 10;
            set => Configuration.GripCfg.PosLeft.y = value * 10;
        }

        [UIValue("saber-left-position-z")]
        public int GripLeftPositionZ
        {
            get => Configuration.GripCfg.PosLeft.z / 10;
            set => Configuration.GripCfg.PosLeft.z = value * 10;
        }

        [UIValue("saber-left-rotation-x")]
        public int GripLeftRotationX
        {
            get => Configuration.GripCfg.RotLeft.x;
            set => Configuration.GripCfg.RotLeft.x = value;
        }

        [UIValue("saber-left-rotation-y")]
        public int GripLeftRotationY
        {
            get => Configuration.GripCfg.RotLeft.y;
            set => Configuration.GripCfg.RotLeft.y = value;
        }

        [UIValue("saber-left-rotation-z")]
        public int GripLeftRotationZ
        {
            get => Configuration.GripCfg.RotLeft.z;
            set => Configuration.GripCfg.RotLeft.z = value;
        }
        #endregion

        #region Saber Grip Right
        [UIValue("saber-right-position-x")]
        public int GripRightPositionX
        {
            get => Configuration.GripCfg.PosRight.x / 10;
            set => Configuration.GripCfg.PosRight.x = value * 10;
        }

        [UIValue("saber-right-position-y")]
        public int GripRightPositionY
        {
            get => Configuration.GripCfg.PosRight.y / 10;
            set => Configuration.GripCfg.PosRight.y = value * 10;
        }

        [UIValue("saber-right-position-z")]
        public int GripRightPositionZ
        {
            get => Configuration.GripCfg.PosRight.z / 10;
            set => Configuration.GripCfg.PosRight.z = value * 10;
        }

        [UIValue("saber-right-rotation-x")]
        public int GripRightRotationX
        {
            get => Configuration.GripCfg.RotRight.x;
            set => Configuration.GripCfg.RotRight.x = value;
        }

        [UIValue("saber-right-rotation-y")]
        public int GripRightRotationY
        {
            get => Configuration.GripCfg.RotRight.y;
            set => Configuration.GripCfg.RotRight.y = value;
        }

        [UIValue("saber-right-rotation-z")]
        public int GripRightRotationZ
        {
            get => Configuration.GripCfg.RotRight.z;
            set => Configuration.GripCfg.RotRight.z = value;
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

        [UIAction("#reset-saber-config")]
        public void OnResetSaberConfig() => ReloadConfiguration();

        [UIAction("#apply")]
        public void OnApply() => StoreConfiguration();

        [UIAction("#ok")]
        public void OnOk() => StoreConfiguration();

        [UIAction("#cancel")]
        public void OnCancel()
        {
            // Only reload on an actual CancelAction
            if (!isFakeCancelEvent)
            {
                ReloadConfiguration();
            }
            else
            {
                isFakeCancelEvent = false;
            }
        }

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
        /// Reload configuration and refresh UI (Lazy workaround)
        /// </summary>
        private void RefreshModSettingsUI()
        {
            isFakeCancelEvent = true;
            parserParams.EmitEvent("cancel");
        }
    }
}
