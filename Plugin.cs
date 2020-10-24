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
                    new GameObject(PluginName).AddComponent<SaberTrailTweak>();
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

            //FIXME: Debug, remove later!
            if ((prevScene != null) && (nextScene != null))
            {
                Logger.log.Info("Scene Changed - previous Scene: " + prevScene.name + " / Next Scene: " + nextScene.name);
                Logger.log.Debug("Printing scene information...");
                for (int i = 0; i < SceneManager.sceneCount; i++)
                {
                    Logger.log.Debug("Scene Index: " + i.ToString() + " : Scene.name: " + SceneManager.GetSceneAt(i).name);
                }
            }
            // Debug End
        }

        private void Load()
        {
            Configuration.Load();
            Settings.Utilities.ProfileManager.LoadProfiles();

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
