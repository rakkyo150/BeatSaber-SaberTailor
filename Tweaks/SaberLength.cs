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
        public bool IsPreventingScoreSubmission => Math.Abs(Configuration.SaberLength - 1.0f) > 0.01f || Math.Abs(Configuration.SaberGirth - 1.0f) > 0.01f;

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

            this.Log("God save the Queen! We're doing it...", LogLevel.Warning);

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
                //FIXME (Maybe?)!!! This will probably need a split into hitbox modification and base game model modification
                //FIXME (Totally)!!! Check compatibility with CustomSabers
                RescaleSabers(handControllers.Find("LeftSaber")?.GetComponent<Saber>(), Configuration.SaberLength, Configuration.SaberGirth);
                RescaleSabers(handControllers.Find("RightSaber")?.GetComponent<Saber>(), Configuration.SaberLength, Configuration.SaberGirth);
            }
            catch (NullReferenceException)
            {
                this.Log("Couldn't modify sabers, likely that the game structure has changed.", LogLevel.Error);
                return;
            }

            this.Log("Successfully modified sabers!");
        }

        private void RescaleSabers(Saber saber, float lengthMultiplier, float widthMultiplier)
        {
            saber.transform.localScale = new Vector3
            {
                x = saber.transform.localScale.x * widthMultiplier,
                y = saber.transform.localScale.y * widthMultiplier,
                z = saber.transform.localScale.z * lengthMultiplier
            };
        }
    }
}
