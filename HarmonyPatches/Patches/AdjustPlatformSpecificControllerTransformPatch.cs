using HarmonyLib;
using SaberTailor.Settings;
using UnityEngine;
using UnityEngine.XR;

namespace SaberTailor.HarmonyPatches
{
    [HarmonyPatch(typeof(DevicelessVRHelper))]
    [HarmonyPatch("AdjustControllerTransform")]
    internal class DevicelessVRHelperAdjustControllerTransform
    {
        private static void Prefix(XRNode node, Transform transform, ref Vector3 position, ref Vector3 rotation)
        {
            position = Vector3.zero;
            rotation = Vector3.zero;

            // Always check for sabers first and modify and exit out immediately if found
            if (transform.gameObject.name == "LeftSaber" || transform.gameObject.name.Contains("Saber A"))
            {
                transform.Translate(Configuration.Grip.PosLeft);
                transform.Rotate(Configuration.Grip.RotLeft);
                return;
            }
            else if (transform.gameObject.name == "RightSaber" || transform.gameObject.name.Contains("Saber B"))
            {
                transform.Translate(Configuration.Grip.PosRight);
                transform.Rotate(Configuration.Grip.RotRight);
                return;
            }

            // Check settings if modifications should also apply to menu hilts
            if (Configuration.Grip.ModifyMenuHiltGrip != false)
            {
                if (transform.gameObject.name == "ControllerLeft")
                {
                    transform.Translate(Configuration.Grip.PosLeft);
                    transform.Rotate(Configuration.Grip.RotLeft);
                    return;
                }
                else if (transform.gameObject.name == "ControllerRight")
                {
                    transform.Translate(Configuration.Grip.PosRight);
                    transform.Rotate(Configuration.Grip.RotRight);
                    return;
                }
            }
        }
    }

    [HarmonyPatch(typeof(OculusVRHelper))]
    [HarmonyPatch("AdjustControllerTransform")]
    internal class OculusVRHelperAdjustControllerTransform
    {
        private static void Prefix(XRNode node, Transform transform, ref Vector3 position, ref Vector3 rotation)
        {
            position = Vector3.zero;
            rotation = Vector3.zero;

            // Always check for sabers first and modify and exit out immediately if found
            if (transform.gameObject.name == "LeftSaber" || transform.gameObject.name.Contains("Saber A"))
            {
                transform.Translate(Configuration.Grip.PosLeft);
                transform.Rotate(Configuration.Grip.RotLeft);
                return;
            }
            else if (transform.gameObject.name == "RightSaber" || transform.gameObject.name.Contains("Saber B"))
            {
                transform.Translate(Configuration.Grip.PosRight);
                transform.Rotate(Configuration.Grip.RotRight);
                return;
            }

            // Check settings if modifications should also apply to menu hilts
            if (Configuration.Grip.ModifyMenuHiltGrip != false)
            {
                if (transform.gameObject.name == "ControllerLeft")
                {
                    transform.Translate(Configuration.Grip.PosLeft);
                    transform.Rotate(Configuration.Grip.RotLeft);
                    return;
                }
                else if (transform.gameObject.name == "ControllerRight")
                {
                    transform.Translate(Configuration.Grip.PosRight);
                    transform.Rotate(Configuration.Grip.RotRight);
                    return;
                }
            }
        }
    }

    [HarmonyPatch(typeof(OpenVRHelper))]
    [HarmonyPatch("AdjustControllerTransform")]
    internal class OpenVRHelperAdjustControllerTransform
    {
        private static void Prefix(XRNode node, Transform transform, ref Vector3 position, ref Vector3 rotation)
        {
            position = Vector3.zero;
            rotation = Vector3.zero;

            // Always check for sabers first and modify and exit out immediately if found
            if (transform.gameObject.name == "LeftSaber" || transform.gameObject.name.Contains("Saber A"))
            {
                transform.Translate(Configuration.Grip.PosLeft);
                transform.Rotate(Configuration.Grip.RotLeft);
                return;
            }
            else if (transform.gameObject.name == "RightSaber" || transform.gameObject.name.Contains("Saber B"))
            {
                transform.Translate(Configuration.Grip.PosRight);
                transform.Rotate(Configuration.Grip.RotRight);
                return;
            }

            // Check settings if modifications should also apply to menu hilts
            if (Configuration.Grip.ModifyMenuHiltGrip != false)
            {
                if (transform.gameObject.name == "ControllerLeft")
                {
                    transform.Translate(Configuration.Grip.PosLeft);
                    transform.Rotate(Configuration.Grip.RotLeft);
                    return;
                }
                else if (transform.gameObject.name == "ControllerRight")
                {
                    transform.Translate(Configuration.Grip.PosRight);
                    transform.Rotate(Configuration.Grip.RotRight);
                    return;
                }
            }
        }
    }
}