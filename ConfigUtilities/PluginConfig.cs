using System.Collections.Generic;

namespace SaberTailor.ConfigUtilities
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
        public int TrailLength = 20;                                                    // Length in frames

        public StoreableIntVector3 GripLeftPosition = new StoreableIntVector3();        // Position in mm
        public StoreableIntVector3 GripRightPosition = new StoreableIntVector3();

        public StoreableIntVector3 GripLeftRotation = new StoreableIntVector3();        // Rotation in °
        public StoreableIntVector3 GripRightRotation = new StoreableIntVector3();

        public bool ModifyMenuHiltGrip = true;

        public Dictionary<string, object> Logging = new Dictionary<string, object>()
        {
            { "ShowCallSource", false },
        };
    }

    /// <summary>
    /// Struct for serializing and saving settings vector data
    /// </summary>
    public struct StoreableIntVector3
    {
        public int x, y, z;
    }
}
