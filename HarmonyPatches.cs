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
            transform.Translate(Preferences.GripRightPosition);
            transform.Rotate(Preferences.GripRightRotation.eulerAngles);
            return;
        }
    }
}
