using SaberTailor.Settings.Classes;

namespace SaberTailor.Settings.Utilities
{
    public class PluginConfig
    {
        public bool RegenerateConfig = true;
        public int ConfigVersion = 3;

        public bool IsSaberScaleModEnabled = false;
        public bool SaberScaleHitbox = false;
        public int SaberLength = 100;
        public int SaberGirth = 100;

        public bool IsTrailModEnabled = false;
        public bool IsTrailEnabled = true;
        public int TrailLength = 20;                    // Length in frames

        public Int3 GripLeftPosition = new Int3();      // Position in mm
        public Int3 GripRightPosition = new Int3();

        public Int3 GripLeftRotation = new Int3();      // Rotation in °
        public Int3 GripRightRotation = new Int3();

        public bool ModifyMenuHiltGrip = true;
    }
}
