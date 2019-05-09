using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SaberTailor.Tweaks
{
    public class SaberLength : ITweak
    {
        public string Name => "SaberLength";
        public bool IsPreventingScoreSubmission => Math.Abs(Preferences.Length - 1.0f) > 0.01f;
        //private SoloFreePlayFlowCoordinator _soloFreePlayFlowCoordinator;
        //private PracticeViewController _practiceViewController;

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

            // FIXME!!! Remove the next two lines when reimplementing SaberLength adjustments
            this.Log("SaberLength adjustments not implemented for current version of Beat Saber");
            return;

            // FIXME!!! Use BS_Utils for disabling score submission instead of limiting this to modes without score submission
            // FIXME!!! Also check to reimplement generic disabling of score submission in Plugin.cs (or otherwise) depending on tweak parameter, instead of here
            // FIXME!!! Should probably implement an ingame menu as well for changing this on the fly without fiddling with config files
            //          -> Actually, this should be done regardless
            /*
            if (IsPreventingScoreSubmission)
            {
                // Check if practice mode is active
                // This part should probably be moved so it can be a shared function for multiple tweaks
                _soloFreePlayFlowCoordinator = Resources.FindObjectsOfTypeAll<SoloFreePlayFlowCoordinator>().FirstOrDefault();
                if (_soloFreePlayFlowCoordinator == null)
                {
                    this.Log("Couldn't find SoloFreePlayFlowCoordinator, bailing!");
                    return;
                }
                _practiceViewController = ReflectionUtil.GetPrivateField<PracticeViewController>(_soloFreePlayFlowCoordinator, "_practiceViewController");
                if (_practiceViewController == null)
                {
                    this.Log("Couldn't find PracticeViewController, bailing!");
                    return;
                }
                if (!_practiceViewController.isInViewControllerHierarchy)
                {
                    this.Log("Practice mode is not active. Not modifying sabers.");
                    return;
                }
            }

            this.Log("Tweaking GameCore...");
            Preferences.Load();
            ApplyGameCoreModifications(loadedScene.GetRootGameObjects().First());
            */
        }

        void ApplyGameCoreModifications(GameObject gameCore)
        {
            var handControllers = gameCore.transform
                .Find("Origin")
                ?.Find("VRGameCore");

            if (handControllers == null)
            {
                this.Log("Couldn't find HandControllers, bailing!");
                return;
            }

            try
            {
                //FIXME!!! This will probably need a split into hitbox modification and base game model modification
                //FIXME!!! Check compatibility with CustomSabers
                //ModifySaber(handControllers.Find("LeftSaber")?.GetComponent<Saber>());
                //ModifySaber(handControllers.Find("RightSaber")?.GetComponent<Saber>());
            }
            catch (NullReferenceException)
            {
                this.Log("Couldn't modify sabers, likely that the game structure has changed.");
                return;
            }

            this.Log("Successfully modified sabers!");
        }
        void ModifySaber(Saber saber)
        {
            //FIXME!!! Actual Implementation
            return;
        }
    }
}
