using System;
using System.Collections;
using LogLevel = IPA.Logging.Logger.Level;
using UnityEngine;

namespace SaberTailor.Tweaks
{
    public class SaberLength : MonoBehaviour
    {
        public string Name => "SaberLength";
        public bool IsPreventingScoreSubmission => Math.Abs(Configuration.SaberLength - 1.0f) > 0.01f || Math.Abs(Configuration.SaberGirth - 1.0f) > 0.01f;

        private static bool scoreDisabled = false;
        private static SaberLength Instance;

        private void Start()
        {
            if (Instance == null)
            {
                Instance = this;
                Load();
            }
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        private void Load()
        {
            // Allow the user to run in any mode, but don't allow ScoreSubmission
            if (IsPreventingScoreSubmission && !scoreDisabled)
            {
                Utilities.ScoreUtility.DisableScoreSubmission(this.Name);
                scoreDisabled = true;
            }
            else if (!IsPreventingScoreSubmission && scoreDisabled)
            {
                Utilities.ScoreUtility.EnableScoreSubmission(this.Name);
                scoreDisabled = false;
            }

            Configuration.UpdateSaberRotation();
            StartCoroutine(ApplyGameCoreModifications());
        }

        public IEnumerator ApplyGameCoreModifications()
        {
            Saber defaultRightSaber = null;
            Saber defaultLeftsaber = null;
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
                    defaultRightSaber = saber;
                    LeftSaber = saber.gameObject;
                }
                else if (saber.saberType == typeForHands[1])
                {
                    defaultLeftsaber = saber;
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
                }
                else
                {
                    Logger.Log("Either the Default Sabers are selected or CustomSaber were too slow!", LogLevel.Debug);
                }
            }

            if (LeftSaber != null)
            {
                RescaleSaber(LeftSaber, Configuration.SaberLength, Configuration.SaberGirth);
            }

            if (RightSaber != null)
            {
                RescaleSaber(RightSaber, Configuration.SaberLength, Configuration.SaberGirth);
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
