using SaberTailor.Settings;
using SaberTailor.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xft;

namespace SaberTailor.Tweaks
{
    public class SaberTrail : MonoBehaviour
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
            IEnumerable<BasicSaberModelController> basicSaberModelControllers = Resources.FindObjectsOfTypeAll<BasicSaberModelController>();
            foreach (BasicSaberModelController basicSaberModelController in basicSaberModelControllers)
            {
                SaberWeaponTrail saberTrail = ReflectionUtil.GetPrivateField<SaberWeaponTrail>(basicSaberModelController, "_saberWeaponTrail");
                if (saberTrail.name == "BasicSaberModel")
                {
                    ModifyTrail(saberTrail, Configuration.Trail.Length);
                    Logger.log.Info("Successfully modified trails!");
                }
            }

            yield return null;
        }

        private void ModifyTrail(XWeaponTrail trail, int length)
        {
            if (Configuration.Trail.TrailEnabled)
            {
                trail.enabled = true;
                ReflectionUtil.SetPrivateField(trail, "_maxFrame", length);
                ReflectionUtil.SetPrivateField(trail, "_granularity", length * 3);
            }
            else
            {
                trail.enabled = false;
            }
        }
    }
}
