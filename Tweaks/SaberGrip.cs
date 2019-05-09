using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Harmony;
using System.Reflection;

namespace SaberTailor.Tweaks
{
    public class SaberGrip : ITweak
    {
        public string Name => "SaberGrip";
        public bool IsPreventingScoreSubmission => false;
        HarmonyInstance harmony = HarmonyInstance.Create("SaberTailorHarmonyInstance");

        public void Load()
        {
            SceneManager.sceneLoaded += SceneManagerOnSceneLoaded;
            SceneManager.sceneUnloaded += SceneManagerOnSceneUnloaded;
        }
        public void Cleanup()
        {
            SceneManager.sceneLoaded -= SceneManagerOnSceneLoaded;
            SceneManager.sceneUnloaded -= SceneManagerOnSceneUnloaded;
        }

        void SceneManagerOnSceneLoaded(Scene loadedScene, LoadSceneMode loadSceneMode)
        {
            if (loadedScene.name != "GameCore") return;

            this.Log("Tweaking GameCore...");
            Preferences.Load();
            
            // Apply Harmony Patches
            this.Log("Loading harmony patches...");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            this.Log("Loaded harmony patches...");

            // Superseeded by harmony patch
            //ApplyGameCoreModifications(loadedScene.GetRootGameObjects().First());
        }

        void SceneManagerOnSceneUnloaded(Scene unloadedScene)
        {
            if (unloadedScene.name != "GameCore") return;

            this.Log("Running Cleanup after unloading GameCore...");
            this.Log("Unloading harmony patches...");
            harmony.UnpatchAll("SaberTailorHarmonyInstance");
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
                // Modification code used be called here
            }
            catch (NullReferenceException)
            {
                this.Log("Couldn't modify sabers, likely that the game structure has changed.");
                return;
            }
            catch (Exception e)
            {
                this.Log("{0} Exception caught.", e);
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
