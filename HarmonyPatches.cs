using Harmony;
using System;
using UnityEngine;

namespace SaberTailor.HarmonyPatches
{
    [HarmonyPatch(typeof(VRPlatformHelper))]
    [HarmonyPatch("AdjustPlatformSpecificControllerTransform")]
    [HarmonyPatch(new Type[] { typeof(Transform) })]
    class AdjustPlatformSpecificControllerTransformPatch
    {
        static void Prefix(Transform transform)
        {
            if (transform.gameObject.name == "LeftSaber")
            {
                transform.Translate(Preferences.GripLeftPosition);
                transform.Rotate(Preferences.GripLeftRotation);
                return;
            }
            else if (transform.gameObject.name == "RightSaber")
            {
                transform.Translate(Preferences.GripRightPosition);
                transform.Rotate(Preferences.GripRightRotation);
                return;
            }
            return;
        }
    }
}
