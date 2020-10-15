using BS_Utils.Gameplay;
using IPA.Utilities;
using SaberTailor.Settings;
using SaberTailor.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SaberTailor.Tweaks
{
    public class SaberLength : MonoBehaviour
    {
        public static string Name => "SaberLength";
        private static bool EnableHitboxScaling = Configuration.Scale.ScaleHitBox;
        private static float LengthMultiplier = Configuration.Scale.Length;

#pragma warning disable IDE0051 // Used by MonoBehaviour
        private void Start() => Load();
#pragma warning restore IDE0051 // Used by MonoBehaviour

        private void Load()
        {
            // Disable scaling in multiplayer completly for now
            if (BS_Utils.Plugin.LevelData.IsSet)
            {
                if (BS_Utils.Plugin.LevelData.Mode == Mode.Multiplayer)
                {
                    Logger.log.Info("Multiplayer mode detected, skipping saber scaling.");
                    return;
                }
            }

            // Force disable hitbox scaling if in multiplayer and mission mode
            if (BS_Utils.Plugin.LevelData.IsSet)
            {
                if (BS_Utils.Plugin.LevelData.Mode == Mode.Multiplayer || BS_Utils.Plugin.LevelData.Mode == Mode.Mission)
                {
                    EnableHitboxScaling = false;
                }
            }

            // Allow the user to run in other mode, but don't allow ScoreSubmission
            if (EnableHitboxScaling)
            {
                ScoreSubmission.DisableSubmission(Plugin.PluginName);
                Logger.log.Info("ScoreSubmission has been disabled.");
            }

            StartCoroutine(ApplyGameCoreModifications());
        }

        private IEnumerator ApplyGameCoreModifications()
        {
            bool usingCustomModels = false;
            Saber defaultLeftSaber = null;
            Saber defaultRightSaber = null;
            GameObject LeftSaber = null;
            GameObject RightSaber = null;

            // Find and set the default sabers first
            IEnumerable<Saber> sabers = Resources.FindObjectsOfTypeAll<Saber>();
            foreach (Saber saber in sabers)
            {
                if (saber.saberType == SaberType.SaberB)
                {
                    defaultLeftSaber = saber;
                    LeftSaber = saber.gameObject;
                }
                else if (saber.saberType == SaberType.SaberA)
                {
                    defaultRightSaber = saber;
                    RightSaber = saber.gameObject;
                }
            }

            if (Utilities.Utils.IsPluginEnabled("Custom Sabers"))
            {
                // Wait a moment for CustomSaber to catch up
                yield return new WaitForSeconds(0.1f);
                GameObject customSaberClone = GameObject.Find("_CustomSaber(Clone)");

                // If customSaberClone is null, CustomSaber is most likely not replacing the default sabers.
                if (customSaberClone != null)
                {
                    LeftSaber = GameObject.Find("LeftSaber");
                    RightSaber = GameObject.Find("RightSaber");
                    usingCustomModels = true;
                }
                else
                {
                    Logger.log.Debug("Either the Default Sabers are selected or CustomSaber were too slow!");
                }
            }

            // Scaling sabers will affect its hitbox, so save the default hitbox positions first before scaling
            HitboxRevertWorkaround hitboxVariables = null;
            if (!EnableHitboxScaling)
            {
                hitboxVariables = new HitboxRevertWorkaround(defaultLeftSaber, defaultRightSaber);
            }

            // Rescale visible sabers (either default or custom)
            RescaleSaber(LeftSaber, LengthMultiplier, Configuration.Scale.Girth);
            RescaleSaber(RightSaber, LengthMultiplier, Configuration.Scale.Girth);

            // Revert hitbox changes to sabers, if hitbox scaling is disabled
            if (hitboxVariables != null)
            {
                hitboxVariables.RestoreHitbox();
            }


            // The new SaberTrail in 1.12.1 is kinda hardcoded to current blade top/bottom positions. So disabling trail scaling for now.
            /*
            IEnumerable<SaberModelController> saberModelControllers = Resources.FindObjectsOfTypeAll<SaberModelController>();
            foreach (SaberModelController saberModelController in saberModelControllers)
            {
                SaberTrail saberTrail = saberModelController.GetField<SaberTrail, SaberModelController>("_saberTrail");
                Logger.log.Debug("SaberTrailName is '" + saberTrail.name + "'.");

                if (!usingCustomModels || saberTrail.name != "BasicSaberModel")
                {
                    //RescaleWeaponTrail(saberTrail, Configuration.Scale.Length, usingCustomModels);
                }
            }
            */

            yield return null;
        }

        private void RescaleSaber(GameObject saber, float lengthMultiplier, float widthMultiplier)
        {
            if (saber != null)
            {
                saber.transform.localScale = Vector3Extensions.Rescale(saber.transform.localScale, widthMultiplier, widthMultiplier, lengthMultiplier);
            }
        }

        private void RescaleSaberHitBox(Saber saber, float lengthMultiplier)
        {
            if (saber != null)
            {
                Transform topPos = saber.GetField<Transform, Saber>("_saberBladeTopTransform");
                Transform bottomPos = saber.GetField<Transform, Saber>("_saberBladeBottomTransform");

                topPos.localPosition = Vector3Extensions.Rescale(topPos.localPosition, 1.0f, 1.0f, lengthMultiplier);
                bottomPos.localPosition = Vector3Extensions.Rescale(bottomPos.localPosition, 1.0f, 1.0f, lengthMultiplier);
            }
        }

        private void RescaleWeaponTrail(SaberTrail trail, float lengthMultiplier, bool usingCustomModels)
        {
            SaberTrailRenderer trailRenderer = trail.GetField<SaberTrailRenderer, SaberTrail>("_trailRenderer");

            float trailWidth = trailRenderer.GetField<float, SaberTrailRenderer>("_trailWidth");
            trailRenderer.SetField("_trailWidth", trailWidth * lengthMultiplier);

            // Fix the local z position for the default trail on custom sabers
            if (usingCustomModels)
            {
                Transform pointEnd = trail.GetField<Transform, SaberTrail>("_pointEnd");
                pointEnd.localPosition = Vector3Extensions.Rescale(pointEnd.localPosition, 1.0f, 1.0f, pointEnd.localPosition.z * lengthMultiplier);
            }
        }

        /// <summary>
        /// Work-Around for reverting Saber Hit-box scaling
        /// </summary>
        private class HitboxRevertWorkaround
        {
            private readonly Transform leftSaberTop;
            private readonly Transform leftSaberBot;
            private readonly Transform rightSaberTop;
            private readonly Transform rightSaberBot;

            private Vector3 leftDefaultHitboxTopPos;
            private Vector3 leftDefaultHitboxBotPos;
            private Vector3 rightDefaultHitboxTopPos;
            private Vector3 rightDefaultHitboxBotPos;

            public HitboxRevertWorkaround(Saber defaultLeftSaber, Saber defaultRightSaber)
            {
                // Scaling sabers will affect their hitboxes, so save the default hitbox positions first before scaling
                GetHitboxDefaultTransforms(defaultLeftSaber, out leftSaberTop, out leftSaberBot);
                leftDefaultHitboxTopPos = leftSaberTop.position.Clone();
                leftDefaultHitboxBotPos = leftSaberBot.position.Clone();

                GetHitboxDefaultTransforms(defaultRightSaber, out rightSaberTop, out rightSaberBot);
                rightDefaultHitboxTopPos = rightSaberTop.position.Clone();
                rightDefaultHitboxBotPos = rightSaberBot.position.Clone();
            }

            /// <summary>
            /// Restores the sabers original Hit-box scale
            /// </summary>
            public void RestoreHitbox()
            {
                leftSaberTop.position = leftDefaultHitboxTopPos;
                leftSaberBot.position = leftDefaultHitboxBotPos;
                rightSaberTop.position = rightDefaultHitboxTopPos;
                rightSaberBot.position = rightDefaultHitboxBotPos;
            }

            private void GetHitboxDefaultTransforms(Saber saber, out Transform saberTop, out Transform saberBot)
            {
                saberTop = saber.GetField<Transform, Saber>("_saberBladeTopTransform");
                saberBot = saber.GetField<Transform, Saber>("_saberBladeBottomTransform");
            }
        }
    }
}
