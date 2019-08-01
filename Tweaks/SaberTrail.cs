using System;
using System.Linq;
using LogLevel = IPA.Logging.Logger.Level;
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
            if (loadedScene.name != "GameCore")
            {
                return;
            }

            this.Log("Tweaking GameCore...", LogLevel.Debug);
            Configuration.UpdateSaberRotation();
            ApplyGameCoreModifications(loadedScene.GetRootGameObjects().First());
        }

        void ApplyGameCoreModifications(GameObject gameCore)
        {
            Transform sceneContext = gameCore.transform.Find("SceneContext");

            if (sceneContext == null)
            {
                this.Log("Couldn't find SceneContext, bailing!", LogLevel.Debug);
                return;
            }

            try
            {
                // XWeaponTrail now in BasicSaberModelController as proteced _saberWeaponTrail, which is available through GameCoreInstaller
                GameCoreInstaller gci = sceneContext.GetComponent<GameCoreInstaller>();
                BasicSaberModelController bsmc = Utilities.ReflectionUtil.GetPrivateField<BasicSaberModelController>(gci, "_basicSaberModelControllerPrefab");
                SaberWeaponTrail saberTrail = Utilities.ReflectionUtil.GetPrivateField<SaberWeaponTrail>(bsmc, "_saberWeaponTrail");

                ModifyTrail(saberTrail);
            }
            catch (NullReferenceException)
            {
                this.Log("Couldn't modify trails, likely that the game structure has changed.", LogLevel.Error);
                return;
            }

            this.Log("Successfully modified trails!");
        }

        void ModifyTrail(XWeaponTrail trail)
        {
            int length = Configuration.TrailLength;

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
