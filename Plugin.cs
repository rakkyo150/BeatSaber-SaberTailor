using System;
using System.Reflection;
using JetBrains.Annotations;
using IPA;
using IPA.Config;
using IPA.Loader;
using IPA.Utilities;
using IPALogger = IPA.Logging.Logger;
using LogLevel = IPA.Logging.Logger.Level;
using Harmony;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SaberTailor
{
    [UsedImplicitly]
    public class Plugin : IBeatSaberPlugin, IDisablablePlugin
    {
        public static string PluginName => "SaberTailor";
        public static string PluginVersion { get; private set; } = "0"; // Default. Actual version is retrieved from the manifest

        internal static Ref<ConfigUtilities.PluginConfig> config;
        internal static IConfigProvider configProvider;

        private static bool harmonyPatchesLoaded = false;
        internal static HarmonyInstance harmonyInstance;

        public void Init(IPALogger logger, [Config.PreferAttribute("json")] IConfigProvider cfgProvider, PluginLoader.PluginMetadata metadata)
        {
            if (logger != null)
            {
                Logger.log = logger;
                Logger.Log("Logger prepared", LogLevel.Debug);
            }

            configProvider = cfgProvider;
            config = cfgProvider.MakeLink<ConfigUtilities.PluginConfig>((p, v) =>
            {
                if (v.Value == null || v.Value.RegenerateConfig || v.Value == null && v.Value.RegenerateConfig)
                {
                    p.Store(v.Value = new ConfigUtilities.PluginConfig() { RegenerateConfig = false });
                }
                config = v;
            });
            Logger.Log("Configuration loaded", LogLevel.Debug);

            if (metadata != null)
            {
                PluginVersion = metadata.Version.ToString();
            }
        }

        public void OnApplicationStart() => Load();
        public void OnApplicationQuit() => Unload();
        public void OnEnable() => Load();
        public void OnDisable() => Unload();

        public void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
        {
            if (scene.name == "MenuCore")
            {
                UI.ModUI.CreateSettingsOptionsUI();
            }
        }

        public void OnActiveSceneChanged(Scene prevScene, Scene nextScene)
        {
            if (nextScene.name == "GameCore")
            {
                Configuration.UpdateModVariables();

                new GameObject(PluginName).AddComponent<Tweaks.SaberTrail>();
                new GameObject(PluginName).AddComponent<Tweaks.SaberLength>();
            }
        }

        public void OnUpdate() { }
        public void OnFixedUpdate() { }
        public void OnSceneUnloaded(Scene scene) { }

        private void Load()
        {
            Configuration.Load();
            Configuration.UpdateConfig();
            ApplyHarmonyPatches();
            Logger.Log($"{PluginName} v.{PluginVersion} has started", LogLevel.Notice);
        }

        private void Unload()
        {
            RemoveHarmonyPatches();
            Utilities.ScoreUtility.Cleanup();
            Configuration.Save();
        }

        private void ApplyHarmonyPatches()
        {
            if (harmonyPatchesLoaded)
            {
                return;
            }

            if (harmonyInstance == null)
            {
                harmonyInstance = HarmonyInstance.Create("com.shadnix.BeatSaber.SaberTailor");
            }

            try
            {
                Logger.Log("Loading Harmony patches...", LogLevel.Debug);
                harmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
                harmonyPatchesLoaded = true;
            }
            catch (Exception ex)
            {
                Logger.Log(ex, LogLevel.Error);
            }
        }

        private void RemoveHarmonyPatches()
        {
            if (harmonyInstance != null && harmonyPatchesLoaded)
            {
                try
                {
                    Logger.Log("Unloading Harmony patches...", LogLevel.Debug);
                    harmonyInstance.UnpatchAll("com.shadnix.BeatSaber.SaberTailor");
                    harmonyPatchesLoaded = false;
                }
                catch (Exception ex)
                {
                    Logger.Log(ex, LogLevel.Error);
                }
            }
        }
    }
}
