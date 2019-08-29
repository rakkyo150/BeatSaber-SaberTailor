namespace SaberTailor.ConfigUtilities
{
    public class PluginConfig
    {
        public bool RegenerateConfig = true;
        public int ConfigVersion = 1;

        public float SaberLength = 1.0f;
        public float SaberGirth = 1.0f;

        public bool IsTrailEnabled = true;
        public int TrailLength = 20;                                                    // Length in frames (iirc)

        public StoreableIntVector3 GripLeftPosition = new StoreableIntVector3();        // Position in mm
        public StoreableIntVector3 GripRightPosition = new StoreableIntVector3();

        public StoreableIntVector3 GripLeftRotation = new StoreableIntVector3();        // Rotation in °
        public StoreableIntVector3 GripRightRotation = new StoreableIntVector3();

        public bool ModifyMenuHiltGrip = true;
    }

    /// <summary>
    /// Struct for serializing and saving settings vector data
    /// </summary>
    public struct StoreableIntVector3
    {
        public int x, y, z;
    }
}
