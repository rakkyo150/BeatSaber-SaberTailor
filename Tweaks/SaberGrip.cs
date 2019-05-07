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
            //ApplyGameCoreModifications(loadedScene.GetRootGameObjects().First());
            //Apply Harmony Patches
            this.Log("Loading harmony patches...");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            this.Log("Loaded harmony patches...");
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
                // ToDo: Fix compatibility with CustomSabers (meshes are getting loaded at the original location)
                //ModifySaber(handControllers.Find("LeftSaber"), Preferences.GripLeftPosition, Preferences.GripLeftRotation);
                //ModifySaber(handControllers.Find("RightSaber"), Preferences.GripRightPosition, Preferences.GripRightRotation);
                //ModifySaberHitbox(handControllers.Find("LeftSaber")?.GetComponent<Saber>(), Preferences.GripLeftPosition, Preferences.GripLeftRotation);
                //ModifySaberHitbox(handControllers.Find("RightSaber")?.GetComponent<Saber>(), Preferences.GripRightPosition, Preferences.GripRightRotation);
                // This below works - but we testing harmony now
                //ModifySaberModel(handControllers.Find("LeftSaber")?.GetComponent<SaberModelContainer>(), Preferences.GripLeftPosition, Preferences.GripLeftRotation);
                //ModifySaberModel(handControllers.Find("RightSaber")?.GetComponent<SaberModelContainer>(), Preferences.GripRightPosition, Preferences.GripRightRotation);
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

        // this moves hitbox, but not model
        // Probably need to patch VRController instance in LeftSaber
        // Or possibly VRPlatformHelper -> AdjustPlatformSpecificControllerTransform
        void ModifySaberHitbox(Saber saber, Vector3 position, Quaternion rotation)
        {
            var saberTop = ReflectionUtil.GetPrivateField<Transform>(saber, "_topPos");
            var saberBottom = ReflectionUtil.GetPrivateField<Transform>(saber, "_bottomPos");
            var saberHandle = ReflectionUtil.GetPrivateField<Transform>(saber, "_handlePos");

            // apply rotation
            saberTop.position = RotateAroundPivot(saberTop.position, saberHandle.position, rotation);
            saberBottom.position = RotateAroundPivot(saberBottom.position, saberHandle.position, rotation);
            saberHandle.localRotation = rotation;

            // apply offset
            saberTop.position += position;
            saberBottom.position += position;
            saberHandle.position += position;
        }

        void ModifySaberModel(SaberModelContainer sabermodel, Vector3 position, Quaternion rotation)
        {
            this.Log("SaberModelContainer: Position: " + sabermodel.transform.position.ToString("F4") + " - Rotation: " + sabermodel.transform.rotation.ToString("F4"));
            sabermodel.transform.position += position;
            sabermodel.transform.rotation *= rotation;
        }

        void ModifySaber(Transform saber, Vector3 position, Quaternion rotation)
        {
            // get positions
            //saber.localPosition = position;
            //saber.localRotation = rotation;
            var saberTop = saber.Find("Top");
            var saberBottom = saber.Find("Bottom");
            
            //var saberHandle = saber.Find("Saber");
            //var trailTop = saber.Find("TrailTop");
            //var trailBottom = saber.Find("TrailBottom");

            this.Log("=======================================================================================");
            this.Log("Printing saber data before modification:");
            this.Log("Printing saber locations: Top: " + saberTop.position.ToString("F4") + " - Bottom: " + saberBottom.position.ToString("F4"));
            //this.Log("Saber length over all: " + (saberTop.position - saberHandle.position).magnitude.ToString("F4"));
            this.Log("Saber length bottom to top: " + (saberTop.position - saberBottom.position).magnitude.ToString("F4"));
            //this.Log("Saber length bottom to handle: " + (saberBottom.position - saberHandle.position).magnitude.ToString("F4"));
            this.Log("Printing saber rotation: Top: " + saberTop.rotation.ToString("F4") + " - Bottom: " + saberBottom.rotation.ToString("F4"));
            this.Log("=======================================================================================");

            // apply rotation
            //saberTop.position = RotateAroundPivot(saberTop.position, saberHandle.position, rotation);
            //saberBottom.position = RotateAroundPivot(saberBottom.position, saberHandle.position, rotation);
            //saberHandle.localRotation = rotation;
            //trailTop.position = RotateAroundPivot(trailTop.position, saberHandle.position, rotation);
            //trailBottom.position = RotateAroundPivot(trailBottom.position, saberHandle.position, rotation);

            // saberTop.localPosition += RotateAroundPivot(saberTop.position, saberBottom.position, rotation);
            // saberBottom.localRotation = rotation;

            // apply offset

            saberTop.localPosition += position;
            saberBottom.localPosition += position;

            //saberHandle.position += position;
            //trailTop.position += position;
            //trailBottom.position += position;

            this.Log("=======================================================================================");
            this.Log("Printing saber data after modification:");
            this.Log("Printing saber locations: Top: " + saberTop.position.ToString("F4") + " - Bottom: " + saberBottom.position.ToString("F4"));
            //this.Log("Saber length over all: " + (saberTop.position - saberHandle.position).magnitude.ToString("F4"));
            this.Log("Saber length bottom to top: " + (saberTop.position - saberBottom.position).magnitude.ToString("F4"));
            //this.Log("Saber length bottom to handle: " + (saberBottom.position - saberHandle.position).magnitude.ToString("F4"));
            this.Log("Printing saber rotation: Top: " + saberTop.rotation.ToString("F4") + " - Bottom: " + saberBottom.rotation.ToString("F4"));
            this.Log("=======================================================================================");
        }

        Vector3 RotateAroundPivot(Vector3 point, Vector3 pivot, Quaternion rotation)
        {
            Vector3 direction = point - pivot;
            direction = rotation * direction;
            return (direction + pivot);
        }
    }
}
