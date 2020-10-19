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
            yield return new WaitForSeconds(0.1f);

            IEnumerable<SaberTrailRenderer> saberTrailRenderers = Resources.FindObjectsOfTypeAll<SaberTrailRenderer>();
            foreach (SaberTrailRenderer saberTrailRenderer in saberTrailRenderers)
            {
                ModifyTrail(saberTrailRenderer, Configuration.Trail.Duration, Configuration.Trail.Granularity, Configuration.Trail.WhiteSectionDuration);
                Logger.log.Info("Successfully modified trails!");
            }

            yield return null;
        }

        private void ModifyTrail(SaberTrailRenderer trail, int duration, int granularity, int whiteSectionDuration)
        {
            if (!Configuration.Trail.TrailEnabled)
            {
                trail.enabled = false;
            }
        }
    }
}
