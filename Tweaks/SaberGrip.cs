using System;
using System.Linq;
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
            if (loadedScene.name != "GameCore") return;

            this.Log("Tweaking GameCore...");
            Preferences.Load();
            ApplyGameCoreModifications(loadedScene.GetRootGameObjects().First());
        }

        void ApplyGameCoreModifications(GameObject gameCore)
        {
            var handControllers = gameCore.transform
                .Find("Origin")
                ?.Find("VRGameCore")
                ?.Find("HandControllers");

            if (handControllers == null)
            {
                this.Log("Couldn't find HandControllers, bailing!");
                return;
            }

            try
            {
                // ToDo: Fix compatibility with CustomSabers (meshes are getting loaded at the original location)
                ModifySaber2(handControllers.Find("LeftSaber"), Preferences.GripLeftPosition, Preferences.GripLeftRotation);
                ModifySaber2(handControllers.Find("RightSaber"), Preferences.GripRightPosition, Preferences.GripRightRotation);
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

        void ModifySaber2(Transform saber, Vector3 position, Quaternion rotation)
        {
            // get positions
            var saberTop = saber.Find("Top");
            var saberBottom = saber.Find("Bottom");
            var saberHandle = saber.Find("Saber");
            var trailTop = saber.Find("TrailTop");
            var trailBottom = saber.Find("TrailBottom");

            this.Log("=======================================================================================");
            this.Log("Printing saber data before modification:");
            this.Log("Printing saber locations: Top: " + saberTop.position.ToString("F4") + " - Bottom: " + saberBottom.position.ToString("F4") + " - Handle: " + saberHandle.position.ToString("F4"));
            this.Log("Saber length over all: " + (saberTop.position - saberHandle.position).magnitude.ToString("F4"));
            this.Log("Saber length bottom to top: " + (saberTop.position - saberBottom.position).magnitude.ToString("F4"));
            this.Log("Saber length bottom to handle: " + (saberBottom.position - saberHandle.position).magnitude.ToString("F4"));
            this.Log("Printing saber rotation: Top: " + saberTop.rotation.ToString("F4") + " - Bottom: " + saberBottom.rotation.ToString("F4") + " - Handle: " + saberHandle.rotation.ToString("F4"));
            this.Log("=======================================================================================");

            // apply rotation
            saberTop.position = RotateAroundPivot(saberTop.position, saberHandle.position, rotation);
            saberBottom.position = RotateAroundPivot(saberBottom.position, saberHandle.position, rotation);
            saberHandle.localRotation = rotation;
            trailTop.position = RotateAroundPivot(trailTop.position, saberHandle.position, rotation);
            trailBottom.position = RotateAroundPivot(trailBottom.position, saberHandle.position, rotation);

            // apply offset
            saberTop.position += position;
            saberBottom.position += position;
            saberHandle.position += position;
            trailTop.position += position;
            trailBottom.position += position;

            this.Log("=======================================================================================");
            this.Log("Printing saber data after modification:");
            this.Log("Printing saber locations: Top: " + saberTop.position.ToString("F4") + " - Bottom: " + saberBottom.position.ToString("F4") + " - Handle: " + saberHandle.position.ToString("F4"));
            this.Log("Saber length over all: " + (saberTop.position - saberHandle.position).magnitude.ToString("F4"));
            this.Log("Saber length bottom to top: " + (saberTop.position - saberBottom.position).magnitude.ToString("F4"));
            this.Log("Saber length bottom to handle: " + (saberBottom.position - saberHandle.position).magnitude.ToString("F4"));
            this.Log("Printing saber rotation: Top: " + saberTop.rotation.ToString("F4") + " - Bottom: " + saberBottom.rotation.ToString("F4") + " - Handle: " + saberHandle.rotation.ToString("F4"));
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
