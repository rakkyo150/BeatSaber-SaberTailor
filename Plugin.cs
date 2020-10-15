using BeatSaberMarkupLanguage.Settings;
using IPA;
using IPA.Config;
using IPA.Loader;
using IPA.Utilities;
using SaberTailor.HarmonyPatches;
using SaberTailor.Settings;
using SaberTailor.Settings.UI;
using SaberTailor.Tweaks;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using IPALogger = IPA.Logging.Logger;

namespace SaberTailor
{
    [Plugin(RuntimeOptions.DynamicInit)]
    public class Plugin
    {
        public static string PluginName => "SaberTailor";
        public static SemVer.Version PluginVersion { get; private set; } = new SemVer.Version("0.0.0"); // Default

        [Init]
        public void Init(IPALogger logger, PluginMetadata metadata)
        {
            Logger.log = logger;
            Configuration.Init();

            if (metadata?.Version != null)
            {
                PluginVersion = metadata.Version;
            }
        }

        [OnEnable]
        public void OnEnable() => Load();
        [OnDisable]
        public void OnDisable() => Unload();

        public void OnActiveSceneChanged(Scene prevScene, Scene nextScene)
        {
            if (nextScene.name == "GameCore")
            {
                if (Configuration.Trail.TweakEnabled)
                {
                    //new GameObject(PluginName).AddComponent<SaberTrailTweak>();
                }

                if (Configuration.Scale.TweakEnabled)
                {
                    new GameObject(PluginName).AddComponent<SaberLength>();
                }
            }
            else if (nextScene.name == "MenuViewControllers" && prevScene.name == "EmptyTransition")
            {
                BSMLSettings.instance.AddSettingsMenu("SaberTailor", "SaberTailor.Settings.UI.Views.mainsettings.bsml", MainSettings.instance);
            }
            
            //Debugging/Testing
            if (nextScene.name == "MenuViewControllers")
            {
                OculusVRHelper[] oculusVRHelpers = Resources.FindObjectsOfTypeAll<OculusVRHelper>();
                bool activeHelper = false;
                if (oculusVRHelpers != null)
                {
                    Logger.log.Info("Found VRHelper of Type OculusVRHelper - there are " + oculusVRHelpers.Length + " instances.");
                    foreach (OculusVRHelper vrHelper in oculusVRHelpers) {
                        if (vrHelper.gameObject.activeInHierarchy)
                            activeHelper = true;
                    }
                    if (activeHelper)
                        Logger.log.Info("At least one OculusVRHelper is active!");
                }
                OpenVRHelper[] openVRHelpers = Resources.FindObjectsOfTypeAll<OpenVRHelper>();
                if (openVRHelpers != null)
                {
                    Logger.log.Info("Found VRHelper of Type OpenVRHelper - there are " + openVRHelpers.Length + " instances.");
                    foreach (OpenVRHelper vrHelper in openVRHelpers)
                    {
                        if (vrHelper.gameObject.activeInHierarchy)
                        {
                            activeHelper = true;
                            Logger.log.Info(vrHelper.GetField<OpenVRHelper.VRControllerManufacturerName, OpenVRHelper>("_vrControllerManufacturerName").ToString());
                        }
                    }
                    if (activeHelper)
                        Logger.log.Info("At least one OpenVRHelper is active!");
                }

                Logger.log.Info("Printing XRSettings.loadedDeviceName: " + XRSettings.loadedDeviceName);
            }
        }

        private void Load()
        {
            Configuration.Load();
            AddEvents();

            Logger.log.Info($"{PluginName} v.{PluginVersion} has started.");
        }

        private void Unload()
        {
            SaberTailorPatches.RemoveHarmonyPatches();
            Configuration.Save();
            RemoveEvents();
        }

        private void AddEvents()
        {
            RemoveEvents();
            SceneManager.activeSceneChanged += OnActiveSceneChanged;
        }

        private void RemoveEvents()
        {
            SceneManager.activeSceneChanged -= OnActiveSceneChanged;
        }
    }
}
