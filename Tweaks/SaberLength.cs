using System;
using System.Collections;
using LogLevel = IPA.Logging.Logger.Level;
using UnityEngine;
using Xft;

namespace SaberTailor.Tweaks
{
    public class SaberLength : MonoBehaviour, ITweak
    {
        public string Name => "SaberLength";
        public bool IsPreventingScoreSubmission => Math.Abs(Configuration.SaberLength - 1.0f) > 0.01f || Math.Abs(Configuration.SaberGirth - 1.0f) > 0.01f;

        private void Start()
        {
            Load();
        }

        private void Load()
        {
            // Allow the user to run in any mode, but don't allow ScoreSubmission
            if (IsPreventingScoreSubmission)
            {
                Utilities.ScoreUtility.DisableScoreSubmission(this.Name);
            }
            else
            {
                Utilities.ScoreUtility.EnableScoreSubmission(this.Name);
            }

            StartCoroutine(ApplyGameCoreModifications());
        }

        private IEnumerator ApplyGameCoreModifications()
        {
            bool usingCustomModels = false;
            Saber defaultLeftsaber = null;
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
                    defaultLeftsaber = saber;
                    LeftSaber = saber.gameObject;
                }
                else if (saber.saberType == typeForHands[1])
                {
                    defaultRightSaber = saber;
                    RightSaber = saber.gameObject;
                }
            }

            if (Utilities.Utils.IsPluginEnabled("Custom Sabers"))
            {
                // Wait for half a second for CustomSaber to catch up
                yield return new WaitForSecondsRealtime(0.5f);
                GameObject customSaberClone = GameObject.Find("_CustomSaber(Clone)");

                // If customSaberClone is null, CustomSaber is most likely not replacing the default sabers.
                if (customSaberClone != null)
                {
                    if (defaultRightSaber != null)
                    {
                        RescaleSaberHitBox(defaultRightSaber, Configuration.SaberLength);
                    }

                    if (defaultLeftsaber != null)
                    {
                        RescaleSaberHitBox(defaultLeftsaber, Configuration.SaberLength);
                    }

                    LeftSaber = GameObject.Find("LeftSaber");
                    RightSaber = GameObject.Find("RightSaber");
                    usingCustomModels = true;
                }
                else
                {
                    this.Log("Either the Default Sabers are selected or CustomSaber were too slow!", LogLevel.Debug);
                }
            }

            if (LeftSaber != null)
            {
                RescaleSaber(LeftSaber, Configuration.SaberLength, Configuration.SaberGirth);
                this.Log("Successfully modified left saber length!");
            }

            if (RightSaber != null)
            {
                RescaleSaber(RightSaber, Configuration.SaberLength, Configuration.SaberGirth);
                this.Log("Successfully modified right saber length!");
            }

            BasicSaberModelController[] basicSaberModelControllers = Resources.FindObjectsOfTypeAll<BasicSaberModelController>();
            foreach (BasicSaberModelController basicSaberModelController in basicSaberModelControllers)
            {
                SaberWeaponTrail saberWeaponTrail = Utilities.ReflectionUtil.GetPrivateField<SaberWeaponTrail>(basicSaberModelController, "_saberWeaponTrail");
                if (!usingCustomModels || saberWeaponTrail.name != "BasicSaberModel")
                {
                    RescaleWeaponTrail(saberWeaponTrail, Configuration.SaberLength, usingCustomModels);
                }
            }

            yield return null;
        }

        private void RescaleSaber(GameObject saber, float lengthMultiplier, float widthMultiplier)
        {
            saber.transform.localScale = RescaleVector3Transform(saber.transform.localScale, lengthMultiplier, widthMultiplier);
        }

        private void RescaleSaberHitBox(Saber saber, float lengthMultiplier)
        {
            Transform topPos = Utilities.ReflectionUtil.GetPrivateField<Transform>(saber, "_topPos");
            Transform bottomPos = Utilities.ReflectionUtil.GetPrivateField<Transform>(saber, "_bottomPos");

            topPos.localPosition = RescaleVector3Transform(topPos.localPosition, lengthMultiplier);
            bottomPos.localPosition = RescaleVector3Transform(bottomPos.localPosition, lengthMultiplier);
        }

        private void RescaleWeaponTrail(XWeaponTrail trail, float lengthMultiplier, bool usingCustomModels)
        {
            float trailWidth = Utilities.ReflectionUtil.GetPrivateField<float>(trail, "_trailWidth");
            Utilities.ReflectionUtil.SetPrivateField(trail, "_trailWidth", trailWidth * lengthMultiplier);

            // Fix the local z position for the default trail on custom sabers
            if (usingCustomModels)
            {
                Transform pointEnd = Utilities.ReflectionUtil.GetPrivateField<Transform>(trail, "_pointEnd");
                pointEnd.localPosition = RescaleVector3Transform(pointEnd.localPosition, pointEnd.localPosition.z * lengthMultiplier);
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
