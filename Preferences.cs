using IllusionPlugin;
using System;
using System.Globalization;
using UnityEngine;

namespace SaberTailor
{
    public static class Preferences
    {
        public static float Length { get; private set; }

        public static bool IsTrailEnabled;
        public static int TrailLength;

        public static Vector3 GripLeftPosition;
        public static Vector3 GripRightPosition;

        public static Vector3 GripLeftRotation { get; private set; }
        public static Vector3 GripRightRotation { get; private set; }

        // ..raw vars for representing player settings before Unity mangled it into something that works with the game 
        // but also changed representation of these settings
        public static Vector3 GripLeftRotationRaw;
        public static Vector3 GripRightRotationRaw;

        public static bool ModifyMenuHiltGrip;

        static Preferences()
        {
            Load();
        }

        public static void Load()
        {
            Length = ModPrefs.GetFloat(Plugin.Name, nameof(Length), 1f, true);
            Length = Math.Max(0.01f, Math.Min(2f, Length));

            IsTrailEnabled = ModPrefs.GetBool(Plugin.Name, nameof(IsTrailEnabled), true, true);

            TrailLength = ModPrefs.GetInt(Plugin.Name, nameof(TrailLength), 20, true);
            TrailLength = Math.Max(5, Math.Min(100, TrailLength));

            GripLeftPosition = ParseVector3(ModPrefs.GetString(Plugin.Name, nameof(GripLeftPosition), "0,0,0", true)) / 100f;
            GripLeftPosition = new Vector3
            {
                x = Mathf.Clamp(GripLeftPosition.x, -0.5f, 0.5f),
                y = Mathf.Clamp(GripLeftPosition.y, -0.5f, 0.5f),
                z = Mathf.Clamp(GripLeftPosition.z, -0.5f, 0.5f)
            };
            GripLeftRotationRaw = ParseVector3(ModPrefs.GetString(Plugin.Name, nameof(GripLeftRotation), "0,0,0", true));

            GripRightPosition = ParseVector3(ModPrefs.GetString(Plugin.Name, nameof(GripRightPosition), "0,0,0", true)) / 100f;
            GripRightPosition = new Vector3
            {
                x = Mathf.Clamp(GripRightPosition.x, -0.5f, 0.5f),
                y = Mathf.Clamp(GripRightPosition.y, -0.5f, 0.5f),
                z = Mathf.Clamp(GripRightPosition.z, -0.5f, 0.5f)
            };
            GripRightRotationRaw = ParseVector3(ModPrefs.GetString(Plugin.Name, nameof(GripRightRotation), "0,0,0", true));

            ModifyMenuHiltGrip = ModPrefs.GetBool(Plugin.Name, nameof(ModifyMenuHiltGrip), false, true);

            UpdateSaberRotation();
        }

        public static void Save()
        {
            // Ignore length for now because it isn't implemented anyways

            ModPrefs.SetBool(Plugin.Name, nameof(IsTrailEnabled), IsTrailEnabled);
            ModPrefs.SetInt(Plugin.Name, nameof(TrailLength), TrailLength);

            ModPrefs.SetString(Plugin.Name, nameof(GripLeftPosition), (GripLeftPosition.x * 100).ToString("D") + "," + (GripLeftPosition.y * 100).ToString("D") + "," + (GripLeftPosition.z * 100).ToString("D"));
            ModPrefs.SetString(Plugin.Name, nameof(GripLeftRotation), GripLeftRotationRaw.ToString("D") + "," + GripLeftRotationRaw.y.ToString("D") + "," + GripLeftRotationRaw.z.ToString("D"));

            ModPrefs.SetString(Plugin.Name, nameof(GripRightPosition), (GripRightPosition.x * 100).ToString("D") + "," + (GripRightPosition.y * 100).ToString("D") + "," + (GripRightPosition.z * 100).ToString("D"));
            ModPrefs.SetString(Plugin.Name, nameof(GripRightRotation), GripRightRotationRaw.ToString("D") + "," + GripRightRotationRaw.y.ToString("D") + "," + GripRightRotationRaw.z.ToString("D"));

            ModPrefs.SetBool(Plugin.Name, nameof(ModifyMenuHiltGrip), ModifyMenuHiltGrip);
        }

        public static void UpdateSaberRotation()
        {
            GripLeftRotation = Quaternion.Euler(GripLeftRotationRaw).eulerAngles;
            GripRightRotation = Quaternion.Euler(GripRightRotationRaw).eulerAngles;
        }

        static Vector3 ParseVector3(string originalString)
        {
            var components = originalString.Trim().Split(',');
            var parsedVector = Vector3.zero;

            if (components.Length != 3) return parsedVector;

            TryParseInvariantFloat(components[0], out parsedVector.x);
            TryParseInvariantFloat(components[1], out parsedVector.y);
            TryParseInvariantFloat(components[2], out parsedVector.z);

            return parsedVector;
        }

        /// <summary>
        /// Tries to parse a float using invariant culture.
        /// </summary>
        /// <param name="number">The string containing the float to parse.</param>
        /// <param name="result">The parsed float, if successful.</param>
        /// <returns>True on success, false on failure.</returns>
        static bool TryParseInvariantFloat(string number, out float result)
        {
            return float.TryParse(
                number,
                NumberStyles.Float,
                CultureInfo.InvariantCulture,
                out result
            );
        }
    }
}
