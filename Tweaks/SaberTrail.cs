using SaberTailor.Settings;
using SaberTailor.Utilities;
using System.Collections;
using UnityEngine;
using Xft;

namespace SaberTailor.Tweaks
{
    public class SaberTrail : MonoBehaviour//, ITweak
    {
        public static string Name => "SaberTrail";

#pragma warning disable IDE0051 // Used by MonoBehaviour
        private void Awake() => Load();
#pragma warning restore IDE0051 // Used by MonoBehaviour

        private void Load()
        {
            StartCoroutine(ApplyGameCoreModifications());
        }

        private IEnumerator ApplyGameCoreModifications()
        {
            BasicSaberModelController[] basicSaberModelControllers = Resources.FindObjectsOfTypeAll<BasicSaberModelController>();
            foreach (BasicSaberModelController basicSaberModelController in basicSaberModelControllers)
            {
                SaberWeaponTrail saberTrail = ReflectionUtil.GetPrivateField<SaberWeaponTrail>(basicSaberModelController, "_saberWeaponTrail");
                if (saberTrail.name == "BasicSaberModel")
                {
                    ModifyTrail(saberTrail, Configuration.Trail.Length);
                    Logger.Log("Successfully modified trails!");
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
