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
                transform.Translate(Configuration.GripLeftPosition);
                transform.Rotate(Configuration.GripLeftRotation);
                return;
            }
            else if (transform.gameObject.name == "RightSaber")
            {
                transform.Translate(Configuration.GripRightPosition);
                transform.Rotate(Configuration.GripRightRotation);
                return;
            }

            // Check settings if modifications should also apply to menu hilts
            if (Configuration.ModifyMenuHiltGrip != false)
            {
                if (transform.gameObject.name == "ControllerLeft")
                {
                    transform.Translate(Configuration.GripLeftPosition);
                    transform.Rotate(Configuration.GripLeftRotation);
                    return;
                }
                else if (transform.gameObject.name == "ControllerRight")
                {
                    transform.Translate(Configuration.GripRightPosition);
                    transform.Rotate(Configuration.GripRightRotation);
                    return;
                }
            }
        }
    }
}
