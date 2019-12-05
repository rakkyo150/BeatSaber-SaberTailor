using SaberTailor.Settings;
using SaberTailor.Utilities;
using System.Collections;
using UnityEngine;
using Xft;
using LogLevel = IPA.Logging.Logger.Level;

namespace SaberTailor.Tweaks
{
    public class SaberLength : MonoBehaviour
    {
        public static string Name => "SaberLength";
        public static bool IsPreventingScoreSubmission => Configuration.Scale.ScaleHitBox;

#pragma warning disable IDE0051 // Used by MonoBehaviour
        private void Start() => Load();
#pragma warning restore IDE0051 // Used by MonoBehaviour

        private void Load()
        {
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
            Saber[] sabers = Resources.FindObjectsOfTypeAll<Saber>();
            Saber.SaberType[] typeForHands = new Saber.SaberType[]
            {
                Saber.SaberType.SaberB,
                Saber.SaberType.SaberA
            };

            foreach (Saber saber in sabers)
            {
                if (saber.saberType == typeForHands[0])
                {
                    defaultLeftSaber = saber;
                    LeftSaber = saber.gameObject;
                }
                else if (saber.saberType == typeForHands[1])
                {
                    defaultRightSaber = saber;
                    RightSaber = saber.gameObject;
                }
            }

            if (Utils.IsPluginEnabled("Custom Sabers"))
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
                    Logger.Log("Either the Default Sabers are selected or CustomSaber were too slow!", LogLevel.Debug);
                }
            }

            // Scaling default saber will affect its hitbox, so save the default hitbox positions first before scaling
            HitboxRevertWorkaround hitboxVariables = null;
            // Core version comes without disabling score submission, so disable hitbox scaling
            //bool restoreHitbox = !usingCustomModels && !Configuration.Scale.ScaleHitBox;
            bool restoreHitbox = !usingCustomModels;
            if (restoreHitbox)
            {
                hitboxVariables = new HitboxRevertWorkaround(defaultLeftSaber, defaultRightSaber);
            }

            // Rescale visible sabers (either default or custom)
            RescaleSaber(LeftSaber, Configuration.Scale.Length, Configuration.Scale.Girth);
            RescaleSaber(RightSaber, Configuration.Scale.Length, Configuration.Scale.Girth);

            // Revert hitbox changes to default sabers, if hitbox scaling is disabled
            if (restoreHitbox)
            {
                hitboxVariables.RestoreHitbox();
            }

            BasicSaberModelController[] basicSaberModelControllers = Resources.FindObjectsOfTypeAll<BasicSaberModelController>();
            foreach (BasicSaberModelController basicSaberModelController in basicSaberModelControllers)
            {
                SaberWeaponTrail saberWeaponTrail = ReflectionUtil.GetPrivateField<SaberWeaponTrail>(basicSaberModelController, "_saberWeaponTrail");
                if (!usingCustomModels || saberWeaponTrail.name != "BasicSaberModel")
                {
                    RescaleWeaponTrail(saberWeaponTrail, Configuration.Scale.Length, usingCustomModels);
                }
            }

            yield return null;
        }

        private void RescaleSaber(GameObject saber, float lengthMultiplier, float widthMultiplier)
        {
            if (saber != null)
            {
                saber.transform.localScale = Vector3Extensions.Rescale(saber.transform.localScale, widthMultiplier, widthMultiplier, lengthMultiplier);
            }
        }

        private void RescaleWeaponTrail(XWeaponTrail trail, float lengthMultiplier, bool usingCustomModels)
        {
            float trailWidth = ReflectionUtil.GetPrivateField<float>(trail, "_trailWidth");
            ReflectionUtil.SetPrivateField(trail, "_trailWidth", trailWidth * lengthMultiplier);

            // Fix the local z position for the default trail on custom sabers
            if (usingCustomModels)
            {
                Transform pointEnd = ReflectionUtil.GetPrivateField<Transform>(trail, "_pointEnd");
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
                // Scaling default saber will affect its hitbox, so save the default hitbox positions first before scaling
                SetHitboxDefaultPosition(defaultLeftSaber, out leftSaberTop, out leftSaberBot);
                leftDefaultHitboxTopPos = leftSaberTop.position.Clone();
                leftDefaultHitboxBotPos = leftSaberBot.position.Clone();

                SetHitboxDefaultPosition(defaultRightSaber, out rightSaberTop, out rightSaberBot);
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

            private void SetHitboxDefaultPosition(Saber saber, out Transform saberTop, out Transform saberBot)
            {
                saberTop = ReflectionUtil.GetPrivateField<Transform>(saber, "_topPos");
                saberBot = ReflectionUtil.GetPrivateField<Transform>(saber, "_bottomPos");
            }
        }
    }
}
