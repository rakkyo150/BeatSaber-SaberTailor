using UnityEngine;

namespace SaberTailor.Settings.Classes
{
    public class Float3
    {
        public float x;
        public float y;
        public float z;

        public static Float3 zero => new Float3(0, 0, 0);

        public Float3() { }

        public Float3(Float3 float3) : this(float3.x, float3.y, float3.z) { }

        public Float3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /// <summary>
        /// Converts the PluginConfig.StoreableIntVector3 to a UnityEngine.Vector3 format
        /// </summary>
        public static Vector3 ToVector3(Float3 float3) => new Vector3()
        {
            x = float3.x,
            y = float3.y,
            z = float3.z
        };

        public override bool Equals(object obj) => base.Equals(obj);
        public override int GetHashCode() => base.GetHashCode();

        public static bool operator ==(Float3 left, Float3 right)
        {
            return left.x == right.x
                && left.y == right.y
                && left.z == right.z;
        }

        public static bool operator !=(Float3 left, Float3 right)
        {
            return left.x != right.x
                || left.y != right.y
                || left.z != right.z;
        }
    }
}
