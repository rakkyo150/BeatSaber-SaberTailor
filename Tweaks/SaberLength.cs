using SaberTailor.Settings;
using SaberTailor.Utilities;
using System.Collections;
using UnityEngine;
using Xft;
using LogLevel = IPA.Logging.Logger.Level;

namespace SaberTailor.Tweaks
{
    public class SaberLength : MonoBehaviour, ITweak
    {
        public string Name => "SaberLength";
        public bool IsPreventingScoreSubmission => Configuration.Scale.ScaleHitBox;

#pragma warning disable IDE0051 // Used by MonoBehaviour
        private void Start() => Load();
#pragma warning restore IDE0051 // Used by MonoBehaviour

        private void Load()
        {
            // Allow the user to run in any mode, but don't allow ScoreSubmission
            if (IsPreventingScoreSubmission)
            {
                ScoreUtility.DisableScoreSubmission(Name);
            }
            else if (ScoreUtility.ScoreIsBlocked)
            {
                ScoreUtility.EnableScoreSubmission(Name);
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
            Transform leftSaberTop;
            Transform leftSaberBot;
            Transform rightSaberTop;
            Transform rightSaberBot;
            Vector3 leftDefaultHitboxTopPos;
            Vector3 leftDefaultHitboxBotPos;
            Vector3 rightDefaultHitboxTopPos;
            Vector3 rightDefaultHitboxBotPos;

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
                // Wait for a moment for CustomSaber to catch up
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
                    this.Log("Either the Default Sabers are selected or CustomSaber were too slow!", LogLevel.Debug);
                }
            }

            // Scaling default saber will affect its hitbox, so save the default hitbox positions first before scaling
            leftSaberTop = ReflectionUtil.GetPrivateField<Transform>(defaultLeftSaber, "_topPos");
            leftSaberBot = ReflectionUtil.GetPrivateField<Transform>(defaultLeftSaber, "_bottomPos");
            rightSaberTop = ReflectionUtil.GetPrivateField<Transform>(defaultRightSaber, "_topPos");
            rightSaberBot = ReflectionUtil.GetPrivateField<Transform>(defaultRightSaber, "_bottomPos");

            leftDefaultHitboxTopPos = new Vector3(leftSaberTop.position.x, leftSaberTop.position.y, leftSaberTop.position.z);
            leftDefaultHitboxBotPos = new Vector3(leftSaberBot.position.x, leftSaberBot.position.y, leftSaberBot.position.z);
            rightDefaultHitboxTopPos = new Vector3(rightSaberTop.position.x, rightSaberTop.position.y, rightSaberTop.position.z);
            rightDefaultHitboxBotPos = new Vector3(rightSaberBot.position.x, rightSaberBot.position.y, rightSaberBot.position.z);

            // Rescale visible sabers (either default or custom)
            RescaleSaber(LeftSaber, Configuration.Scale.Length, Configuration.Scale.Girth);
            RescaleSaber(RightSaber, Configuration.Scale.Length, Configuration.Scale.Girth);

            // Scaling custom sabers will not change their hitbox, so a manual hitbox rescale is necessary, if the option is enabled
            if (usingCustomModels && Configuration.Scale.ScaleHitBox)
            {
                RescaleSaberHitBox(defaultLeftSaber, Configuration.Scale.Length);
                RescaleSaberHitBox(defaultRightSaber, Configuration.Scale.Length);
            }

            // Revert hitbox changes to default sabers, if hitbox scaling is disabled
            if (!usingCustomModels && !Configuration.Scale.ScaleHitBox)
            {
                leftSaberTop.position = leftDefaultHitboxTopPos;
                leftSaberBot.position = leftDefaultHitboxBotPos;
                rightSaberTop.position = rightDefaultHitboxTopPos;
                rightSaberBot.position = rightDefaultHitboxBotPos;
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
                saber.transform.localScale = RescaleVector3Transform(saber.transform.localScale, lengthMultiplier, widthMultiplier);
            }
        }

        private void RescaleSaberHitBox(Saber saber, float lengthMultiplier)
        {
            if (saber != null)
            {
                Transform topPos = ReflectionUtil.GetPrivateField<Transform>(saber, "_topPos");
                Transform bottomPos = ReflectionUtil.GetPrivateField<Transform>(saber, "_bottomPos");

                topPos.localPosition = RescaleVector3Transform(topPos.localPosition, lengthMultiplier);
                bottomPos.localPosition = RescaleVector3Transform(bottomPos.localPosition, lengthMultiplier);
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
                pointEnd.localPosition = RescaleVector3Transform(pointEnd.localPosition, pointEnd.localPosition.z * lengthMultiplier);
            }
        }

        private void UndoRescaleSaberHitBox(Saber saber, float lengthMultiplier)
        {
            if (saber != null)
            {
                RescaleSaberHitBox(saber, 1.0f / lengthMultiplier);
            }
        }

        private Vector3 RescaleVector3Transform(Vector3 baseVector, float lenght, float width = 1.0f)
        {
            Vector3 result = new Vector3()
            {
                x = baseVector.x * width,
                y = baseVector.y * width,
                z = baseVector.z * lenght
            };

            return result;
        }
    }
}
