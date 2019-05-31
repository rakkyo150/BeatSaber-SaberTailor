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
            // Always check for sabers first and modify and exit out immediately if found
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

            // Check settings if modifications should also apply to menu hilts
            if (Preferences.ModifyMenuHiltGrip == false) { return; }
            
            if (transform.gameObject.name == "ControllerLeft")
            {
                transform.Translate(Preferences.GripLeftPosition);
                transform.Rotate(Preferences.GripLeftRotation);
                return;
            }
            else if (transform.gameObject.name == "ControllerRight")
            {
                transform.Translate(Preferences.GripRightPosition);
                transform.Rotate(Preferences.GripRightRotation);
                return;
            }

            return;
        }
    }
}
