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
        HarmonyInstance harmony = HarmonyInstance.Create("com.shadnix.BeatSaber.SaberTailor");

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
            try
            {
                this.Log("Loading Harmony patches...");
                harmony.PatchAll(Assembly.GetExecutingAssembly());
                this.Log("Loaded Harmony patches. Successfully modified saber grip!");
            }
            catch (Exception e)
            {
                this.Log("Loading Harmony patches failed. Please check if you have Harmony installed.");
                this.Log(e.ToString());
            }

            // Superseeded by harmony patch
            //ApplyGameCoreModifications(loadedScene.GetRootGameObjects().First());
        }

        void SceneManagerOnSceneUnloaded(Scene unloadedScene)
        {
            if (unloadedScene.name != "GameCore") return;

            this.Log("Running Cleanup after unloading GameCore...");
            this.Log("Unloading Harmony patches...");
            harmony.UnpatchAll("com.shadnix.BeatSaber.SaberTailor");
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
                // Modification code used to be called here
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
