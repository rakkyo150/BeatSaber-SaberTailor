using HarmonyLib;
using JetBrains.Annotations;
using SaberTailor.Settings;
using SaberTailor.Settings.Utilities;
using System;
using UnityEngine;
using UnityEngine.XR;

namespace SaberTailor.HarmonyPatches
{
    // https://github.com/Reezonate/EasyOffset/blob/master/Source/0_Harmony/VRControllerPatches/VRControllerUpdatePatch.cs
    [HarmonyPatch(typeof(VRController), "Update")]
    internal class VRControllerUpdatePatch
    {
        private static readonly Vector3 DefaultLeftPosition = new Vector3(-0.2f, 0.05f, 0.0f);
        private static readonly Vector3 DefaultRightPosition = new Vector3(0.2f, 0.05f, 0.0f);

        [UsedImplicitly]
        private static bool Prefix(
        VRController __instance,
        IVRPlatformHelper ____vrPlatformHelper,
        ref Vector3 ____lastTrackedPosition,
        ref Quaternion ____lastTrackedRotation
    )
        {
            if (____vrPlatformHelper.GetNodePose(__instance.node, __instance.nodeIdx, out var pos, out var rot))
            {
                ____lastTrackedPosition = pos;
                ____lastTrackedRotation = rot;
            }
            else
            {
                pos = ____lastTrackedPosition != Vector3.zero ? ____lastTrackedPosition : (__instance.node == XRNode.LeftHand ? DefaultLeftPosition : DefaultRightPosition);
                rot = ____lastTrackedRotation != Quaternion.identity ? ____lastTrackedRotation : Quaternion.identity;
            }

            var transform = __instance.transform;
            transform.SetLocalPositionAndRotation(pos, rot);
            AdjustControllerTransform(__instance.node, transform, ref pos, ref rot);
            if (Configuration.Grip.UseBaseGameAdjustmentMode)
            {
                transform.SetLocalPositionAndRotation(pos, rot);
            }
            return false;
        }

        internal static void AdjustControllerTransform(
        XRNode node,
        Transform transform,
        ref Vector3 position,
        ref Quaternion rotation
    )
        {
            switch (node)
            {
                case XRNode.LeftHand:
                    Utilities.AdjustControllerTransform(node, transform, ref position, ref rotation);
                    break;
                case XRNode.RightHand:
                    Utilities.AdjustControllerTransform(node, transform, ref position, ref rotation);
                    break;
                case XRNode.LeftEye: return;
                case XRNode.RightEye: return;
                case XRNode.CenterEye: return;
                case XRNode.Head: return;
                case XRNode.GameController: return;
                case XRNode.TrackingReference: return;
                case XRNode.HardwareTracker: return;
                default: throw new ArgumentOutOfRangeException(nameof(node), node, null);
            }
        }
    }


    internal class Utilities
    {
        internal static void AdjustControllerTransform(XRNode node, Transform transform, ref Vector3 lastTrackedPosition, ref Quaternion lastTrackedRotation)
        {
            // Always check for sabers first and modify and exit out immediately if found
            if (transform.gameObject.name == "LeftHand" || transform.gameObject.name.Contains("Saber A"))
            {
                if (Configuration.Grip.UseBaseGameAdjustmentMode)
                {
                    lastTrackedPosition = lastTrackedPosition + Configuration.Grip.PosLeft;
                    var rot = Quaternion.Euler(Configuration.Grip.RotLeft.x, Configuration.Grip.RotLeft.y, Configuration.Grip.RotLeft.z);
                    lastTrackedRotation = lastTrackedRotation * rot;
                }
                else
                {
                    transform.Translate(Configuration.Grip.PosLeft);
                    transform.Rotate(Configuration.Grip.RotLeft);
                    transform.Translate(Configuration.Grip.OffsetLeft, Space.World);
                }
                return;
            }
            else if (transform.gameObject.name == "RightHand" || transform.gameObject.name.Contains("Saber B"))
            {
                if (Configuration.Grip.UseBaseGameAdjustmentMode)
                {
                    lastTrackedPosition = lastTrackedPosition + Configuration.Grip.PosRight;
                    var rot = Quaternion.Euler(Configuration.Grip.RotRight.x, Configuration.Grip.RotRight.y, Configuration.Grip.RotRight.z);
                    lastTrackedRotation = lastTrackedRotation * rot;
                }
                else
                {
                    transform.Translate(Configuration.Grip.PosRight);
                    transform.Rotate(Configuration.Grip.RotRight);
                    transform.Translate(Configuration.Grip.OffsetRight, Space.World);
                }
                return;
            }

            // Check settings if modifications should also apply to menu hilts
            if (Configuration.Grip.ModifyMenuHiltGrip != false)
            {
                if (transform.gameObject.name == "ControllerLeft")
                {
                    if (Configuration.Grip.UseBaseGameAdjustmentMode)
                    {
                        lastTrackedPosition = lastTrackedPosition + Configuration.Grip.PosLeft;
                        var rot = Quaternion.Euler(Configuration.Grip.RotLeft.x, Configuration.Grip.RotLeft.y, Configuration.Grip.RotLeft.z);
                        lastTrackedRotation = lastTrackedRotation * rot;
                    }
                    else
                    {
                        transform.Translate(Configuration.Grip.PosLeft);
                        transform.Rotate(Configuration.Grip.RotLeft);
                        transform.Translate(Configuration.Grip.OffsetLeft, Space.World);
                    }
                    return;
                }
                else if (transform.gameObject.name == "ControllerRight")
                {
                    if (Configuration.Grip.UseBaseGameAdjustmentMode)
                    {
                        lastTrackedPosition = lastTrackedPosition + Configuration.Grip.PosRight;
                        var rot = Quaternion.Euler(Configuration.Grip.RotRight.x, Configuration.Grip.RotRight.y, Configuration.Grip.RotRight.z);
                        lastTrackedRotation = lastTrackedRotation * rot;
                    }
                    else
                    {
                        transform.Translate(Configuration.Grip.PosRight);
                        transform.Rotate(Configuration.Grip.RotRight);
                        transform.Translate(Configuration.Grip.OffsetRight, Space.World);
                    }
                    return;
                }
            }
        }
    }
}