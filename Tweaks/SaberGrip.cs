using System;
using LogLevel = IPA.Logging.Logger.Level;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SaberTailor.Tweaks
{
    public class SaberGrip : ITweak
    {
        public string Name => "SaberGrip";
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

            // Apply Harmony patches
            // -> removed, loading of Harmony patches moved to plugin.cs
            // -> This file will get removed during the partial rewrite of this mod for BSIPA

            // Superseded by harmony patch
            //ApplyGameCoreModifications(loadedScene.GetRootGameObjects().First());
        }

        void ApplyGameCoreModifications(GameObject gameCore)
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
                // Modification code used to be called here
            }
            catch (NullReferenceException)
            {
                this.Log("Couldn't modify sabers, likely that the game structure has changed.", LogLevel.Error);
                return;
            }
            catch (Exception e)
            {
                this.Log(e, "{0} Exception caught.", LogLevel.Error);
            }

            this.Log("Successfully modified saber grip!");
        }

        // Now unused helper function to rotate a Vector point in space around a given pivot point
        Vector3 RotateAroundPivot(Vector3 point, Vector3 pivot, Quaternion rotation)
        {
            Vector3 direction = point - pivot;
            direction = rotation * direction;
            return (direction + pivot);
        }
    }
}
