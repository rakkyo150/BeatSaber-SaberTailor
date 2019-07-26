using System;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;
using IPA;
using IPA.Config;
using IPA.Loader;
using IPA.Utilities;
using IPALogger = IPA.Logging.Logger;
using LogLevel = IPA.Logging.Logger.Level;
using Harmony;
using UnityEngine.SceneManagement;

namespace SaberTailor
{
    [UsedImplicitly]
    public class Plugin : IBeatSaberPlugin
    {
        public static string PluginName => "SaberTailor";
        public static string PluginVersion { get; private set; } = "0"; // Default. Actual version is retrieved from the manifest

        internal static Ref<ConfigUtilities.PluginConfig> config;
        internal static IConfigProvider configProvider;

        private static bool harmonyPatchesLoaded = false;
        internal static HarmonyInstance harmonyInstance;

        readonly List<Tweaks.ITweak> _tweaks = new List<Tweaks.ITweak>
        {
            new Tweaks.SaberLength(),
            new Tweaks.SaberGrip(),
            new Tweaks.SaberTrail()
        };

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

        public void OnApplicationStart()
        {
            Configuration.Load();

            _tweaks.ForEach(tweak =>
            {
                try
                {
                    tweak.Load();
                    Logger.Log($"Loaded tweak: {tweak.Name}", LogLevel.Debug);
                }
                catch (Exception ex)
                {
                    Logger.Log($"Failed to load tweak: {tweak.Name}", LogLevel.Error);
                    Logger.Log(ex, LogLevel.Error);
                }
            });

            ApplyHarmonyPatches();
            Logger.Log($"{PluginName} v.{PluginVersion} has started", LogLevel.Notice);
        }

        public void OnApplicationQuit()
        {
            _tweaks.ForEach(tweak =>
            {
                try
                {
                    tweak.Cleanup();
                    Logger.Log($"Unloaded tweak: {tweak.Name}", LogLevel.Debug);
                }
                catch (Exception ex)
                {
                    Logger.Log($"Failed to unload tweak: {tweak.Name}", LogLevel.Error);
                    Logger.Log(ex, LogLevel.Error);
                }
            });

            Configuration.Save();
            RemoveHarmonyPatches();
        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
        {
            if (scene.name == "MenuCore")
            {
                UI.ModUI.CreateSettingsOptionsUI();
            }
        }

        public void OnUpdate() { }
        public void OnFixedUpdate() { }
        public void OnSceneUnloaded(Scene scene) { }
        public void OnActiveSceneChanged(Scene prevScene, Scene nextScene) { }

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
                Logger.Log("Loaded Harmony patches. Successfully modified saber grip!", LogLevel.Debug);
                harmonyPatchesLoaded = true;
            }
            catch (Exception ex)
            {
                Logger.Log("Loading Harmony patches failed. Please check if you have Harmony installed.", LogLevel.Error);
                Logger.Log(ex, LogLevel.Error);
            }
        }

        private void RemoveHarmonyPatches()
        {
            if (harmonyInstance != null && harmonyPatchesLoaded)
            {
                try
                {
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
