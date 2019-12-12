using BeatSaberMarkupLanguage.Settings;
using IPA;
using IPA.Config;
using IPA.Loader;
using IPA.Utilities;
using SaberTailor.HarmonyPatches;
using SaberTailor.Settings;
using SaberTailor.Settings.UI;
using SaberTailor.Settings.Utilities;
using SaberTailor.Tweaks;
using SaberTailor.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;
using IPALogger = IPA.Logging.Logger;
using LogLevel = IPA.Logging.Logger.Level;

namespace SaberTailor
{
    public class Plugin : IBeatSaberPlugin, IDisablablePlugin
    {
        public static string PluginName => "SaberTailor";
        public static SemVer.Version PluginVersion { get; private set; } = new SemVer.Version("0.0.0"); // Default

        internal static Ref<PluginConfig> config;
        internal static IConfigProvider configProvider;

        public void Init(IPALogger logger, [Config.Prefer("json")] IConfigProvider cfgProvider, PluginLoader.PluginMetadata metadata)
        {
            Logger.log = logger;

            configProvider = cfgProvider;
            config = cfgProvider.MakeLink<PluginConfig>((p, v) =>
            {
                if (v.Value == null || v.Value.RegenerateConfig || v.Value == null && v.Value.RegenerateConfig)
                {
                    p.Store(v.Value = new PluginConfig() { RegenerateConfig = false });
                }
                config = v;
            });

            if (metadata?.Version != null)
            {
                PluginVersion = metadata.Version;
            }
        }

        public void OnEnable() => Load();
        public void OnDisable() => Unload();
        public void OnApplicationQuit() => Unload();

        public void OnActiveSceneChanged(Scene prevScene, Scene nextScene)
        {
            if (nextScene.name == "GameCore")
            {
                if (Configuration.Trail.TweakEnabled)
                {
                    new GameObject(PluginName).AddComponent<SaberTrail>();
                }

                if (Configuration.Scale.TweakEnabled)
                {
                    new GameObject(PluginName).AddComponent<SaberLength>();
                }
                else if (ScoreUtility.ScoreIsBlocked)
                {
                    ScoreUtility.EnableScoreSubmission(SaberLength.Name);
                }
            }
            else if (nextScene.name == "MenuViewControllers")
            {
                BSMLSettings.instance.AddSettingsMenu("SaberTailor", "SaberTailor.Settings.UI.Views.mainsettings.bsml", MainSettings.instance);
            }
        }

        public void OnApplicationStart() { }
        public void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode) { }
        public void OnSceneUnloaded(Scene scene) { }
        public void OnUpdate() { }
        public void OnFixedUpdate() { }

        private void Load()
        {
            Configuration.Load();
            Patches.ApplyHarmonyPatches();
            Logger.Log($"{PluginName} v.{PluginVersion} has started.", LogLevel.Info);
        }

        private void Unload()
        {
            Patches.RemoveHarmonyPatches();
            ScoreUtility.Cleanup();
            Configuration.Save();
        }
    }
}
