using IPA.Utilities;
using SaberTailor.Settings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SaberTailor.Tweaks
{
    public class SaberTrailTweak : MonoBehaviour
    {
        public static string Name => "SaberTrail";
        public static bool IsPreventingScoreSubmission => false;

#pragma warning disable IDE0051 // Used by MonoBehaviour
        private void Awake() => Load();
#pragma warning restore IDE0051 // Used by MonoBehaviour

        private void Load()
        {
            StartCoroutine(ApplyGameCoreModifications());
        }

        private IEnumerator ApplyGameCoreModifications()
        {
            IEnumerable<SaberTrailRenderer> saberTrailRenderers = Resources.FindObjectsOfTypeAll<SaberTrailRenderer>();
            foreach (SaberTrailRenderer saberTrailRenderer in saberTrailRenderers)
            {
                ModifyTrail(saberTrailRenderer, Configuration.Trail.Length);
                Logger.log.Info("Successfully modified trails!");
            }

            yield return null;
        }

        private void ModifyTrail(SaberTrailRenderer trail, int length)
        {
            if (Configuration.Trail.TrailEnabled)
            {
                trail.enabled = true;
                trail.SetField("_trailDuration", length/90f);
                trail.SetField("_granularity", length);
            }
            else
            {
                trail.enabled = false;
            }
        }
    }
}
