namespace SaberTailor.ConfigUtilities
{
    public class PluginConfig
    {
        public bool RegenerateConfig = true;

        public float SaberLength = 1.0f;
        public float SaberGirth = 1.0f;

        public bool IsTrailEnabled = true;
        public int TrailLength = 20;

        public StoreableVector3 GripLeftPosition = new StoreableVector3();
        public StoreableVector3 GripRightPosition = new StoreableVector3();

        public StoreableVector3 GripLeftRotation = new StoreableVector3();
        public StoreableVector3 GripRightRotation = new StoreableVector3();

        public bool ModifyMenuHiltGrip;
    }

    /// <summary>
    /// Struct for serializing UnityEngine.Vector3
    /// </summary>
    public struct StoreableVector3
    {
        public float x, y, z;
    }
}
