using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Xft;

namespace SaberTailor.Tweaks
{
    public class SaberTrail : ITweak
    {
        public string Name => "SaberTrail";
        public bool IsPreventingScoreSubmission => false;

        public void Load()
        {
            SceneManager.sceneLoaded += SceneManagerOnSceneLoaded;
        }
        public void Cleanup()
        {
            SceneManager.sceneLoaded -= SceneManagerOnSceneLoaded;
        }

        void SceneManagerOnSceneLoaded(Scene loadedScene, LoadSceneMode loadSceneMode)
        {
            if (loadedScene.name != "GameCore") return;

            this.Log("Tweaking GameCore...");
            Preferences.Load();
            ApplyGameCoreModifications(loadedScene.GetRootGameObjects().First());
        }

        void ApplyGameCoreModifications(GameObject gameCore)
        {
            var sceneContext = gameCore.transform.Find("SceneContext");

            if (sceneContext == null)
            {
                this.Log("Couldn't find SceneContext, bailing!");
                return;
            }

            try
            {
                // XWeaponTrail now in BasicSaberModelController as proteced _saberWeaponTrail, which is available through GameCoreInstaller
                GameCoreInstaller gci = sceneContext.GetComponent<GameCoreInstaller>();
                BasicSaberModelController bsmc = ReflectionUtil.GetPrivateField<BasicSaberModelController>(gci, "_basicSaberModelControllerPrefab");
                SaberWeaponTrail saberTrail = ReflectionUtil.GetPrivateField<SaberWeaponTrail>(bsmc, "_saberWeaponTrail");

                ModifyTrail(saberTrail);
            }
            catch (NullReferenceException)
            {
                this.Log("Couldn't modify trails, likely that the game structure has changed.");
                return;
            }

            this.Log("Successfully modified trails!");
        }
        void ModifyTrail(XWeaponTrail trail)
        {
            var length = Preferences.TrailLength;

            if (Preferences.IsTrailEnabled)
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
