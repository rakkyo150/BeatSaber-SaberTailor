using System;
using System.Linq;
using LogLevel = IPA.Logging.Logger.Level;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SaberTailor.Tweaks
{
    public class SaberLength : ITweak
    {
        public string Name => "SaberLength";
        public bool IsPreventingScoreSubmission => Math.Abs(Configuration.Length - 1.0f) > 0.01f;

        private static bool scoreDisabled = false;

        public void Load()
        {
            SceneManager.sceneLoaded += SceneManagerOnSceneLoaded;
        }

        public void Cleanup()
        {
            SceneManager.sceneLoaded -= SceneManagerOnSceneLoaded;

            if (scoreDisabled)
            {
                Utilities.ScoreUtility.EnableScoreSubmission(this.Name);
                scoreDisabled = false;
            }
        }

        public void SceneManagerOnSceneLoaded(Scene loadedScene, LoadSceneMode loadSceneMode)
        {
            if (loadedScene.name != "GameCore")
            {
                return;
            }

            // FIXME!!! Remove the next two lines when reimplementing SaberLength adjustments
            this.Log("SaberLength adjustments not implemented for current version of Beat Saber", LogLevel.Warning);
            return;

            // FIXME!!! Should probably implement an in-game menu for changing this on the fly without fiddling with config files
            //          -> Actually, this should be done regardless
            
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

            this.Log("Tweaking GameCore...", LogLevel.Debug);
            Configuration.UpdateSaberRotation();
            ApplyGameCoreModifications(loadedScene.GetRootGameObjects().First());
        }

        public void ApplyGameCoreModifications(GameObject gameCore)
        {
            Transform handControllers = gameCore.transform
                .Find("Origin")
                ?.Find("VRGameCore");

            if (handControllers == null)
            {
                this.Log("Couldn't find HandControllers, bailing!", LogLevel.Debug);
                return;
            }

            try
            {
                //FIXME!!! This will probably need a split into hitbox modification and base game model modification
                //FIXME!!! Check compatibility with CustomSabers
                ModifySaber(handControllers.Find("LeftSaber")?.GetComponent<Saber>());
                ModifySaber(handControllers.Find("RightSaber")?.GetComponent<Saber>());
            }
            catch (NullReferenceException)
            {
                this.Log("Couldn't modify sabers, likely that the game structure has changed.", LogLevel.Error);
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
