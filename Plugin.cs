﻿using BeatSaberMarkupLanguage.Settings;
using IPA;
using IPA.Loader;
using SaberTailor.HarmonyPatches;
using SaberTailor.Settings;
using SaberTailor.Settings.UI;
using SaberTailor.Tweaks;
using UnityEngine;
using UnityEngine.SceneManagement;
using IPALogger = IPA.Logging.Logger;

namespace SaberTailor
{
    [Plugin(RuntimeOptions.DynamicInit)]
    public class Plugin
    {
        public static string PluginName => "SaberTailor";
        public static Hive.Versioning.Version PluginVersion { get; private set; } = new Hive.Versioning.Version("0.0.0"); // Default

        [Init]
        public void Init(IPALogger logger, PluginMetadata metadata)
        {
            Logger.log = logger;
            Configuration.Init();

            if (metadata?.HVersion != null)
            {
                PluginVersion = metadata.HVersion;
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
                if (Configuration.Scale.TweakEnabled)
                {
                    new GameObject(PluginName).AddComponent<SaberLength>();
                }
            }
        }

        private void Load()
        {
            Configuration.Load();
            Settings.Utilities.ProfileManager.LoadProfiles();

            AddEvents();

            BSMLSettings.instance.AddSettingsMenu("SaberTailor", "SaberTailor.Settings.UI.Views.mainsettings.bsml", MainSettings.instance);
            Logger.log.Info($"{PluginName} v.{PluginVersion} has started.");
        }

        private void Unload()
        {
            SaberTailorPatches.RemoveHarmonyPatches();
            Configuration.Save();
            RemoveEvents();

            BSMLSettings.instance.RemoveSettingsMenu(MainSettings.instance);
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
