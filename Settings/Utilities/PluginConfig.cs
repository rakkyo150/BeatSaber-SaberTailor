using SaberTailor.Settings.Classes;

namespace SaberTailor.Settings.Utilities
{
    public class PluginConfig
    {
        public static PluginConfig Instance;

        public int ConfigVersion = 5;

        public bool IsSaberScaleModEnabled = false;
        public bool SaberScaleHitbox = false;
        public int SaberLength = 100;
        public int SaberGirth = 100;

        public bool IsTrailModEnabled = false;
        public bool IsTrailEnabled = true;
        public int TrailDuration = 400;                 // Age of trail - in ms?
        public int TrailGranularity = 60;               // Segments count in trail
        public int TrailWhiteSectionDuration = 100;     // Duration of gradient from white

        public bool IsGripModEnabled = false;

        public Float3 GripLeftPosition = new Float3();      // Position in mm
        public Float3 GripRightPosition = new Float3();

        public Float3 GripLeftRotation = new Float3();      // Rotation in °
        public Float3 GripRightRotation = new Float3();

        public Float3 GripLeftOffset = new Float3();        // Offset in mm
        public Float3 GripRightOffset = new Float3();

        public bool ModifyMenuHiltGrip = true;
        public bool UseBaseGameAdjustmentMode = true;

        public float SaberPosIncrement = 10;
        public float SaberPosIncValue = 1;
        public float SaberRotIncrement = 1;
        public string SaberPosIncUnit = "cm";
        public string SaberPosDisplayUnit = "cm";

        /// <summary>
        /// Call this to save to disk
        /// </summary>
        public virtual void Changed() { }
    }
}
