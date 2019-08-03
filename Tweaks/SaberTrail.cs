using System.Collections;
using LogLevel = IPA.Logging.Logger.Level;
using UnityEngine;
using Xft;

namespace SaberTailor.Tweaks
{
    public class SaberTrail : MonoBehaviour, ITweak
    {
        public string Name => "SaberTrail";
        public bool IsPreventingScoreSubmission => false;

        private void Awake()
        {
            Load();
        }

        private void Load()
        {
            StartCoroutine(ApplyGameCoreModifications());
        }

        private IEnumerator ApplyGameCoreModifications()
        {
            BasicSaberModelController[] basicSaberModelControllers = Resources.FindObjectsOfTypeAll<BasicSaberModelController>();
            foreach (BasicSaberModelController basicSaberModelController in basicSaberModelControllers)
            {
                SaberWeaponTrail saberTrail = Utilities.ReflectionUtil.GetPrivateField<SaberWeaponTrail>(basicSaberModelController, "_saberWeaponTrail");
                if (saberTrail.name == "BasicSaberModel")
                {
                    ModifyTrail(saberTrail, Configuration.TrailLength);
                    this.Log("Successfully modified trails!");
                }
            }

            yield return null;
        }

        private void ModifyTrail(XWeaponTrail trail, int length)
        {
            if (Configuration.IsTrailEnabled)
            {
                trail.enabled = true;
                Utilities.ReflectionUtil.SetPrivateField(trail, "_maxFrame", length);
                Utilities.ReflectionUtil.SetPrivateField(trail, "_granularity", length * 3);
            }
            else
            {
                trail.enabled = false;
            }
        }
    }
}
