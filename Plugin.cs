using IllusionPlugin;
using Harmony;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SaberTailor
{
    [UsedImplicitly]
    public class Plugin : IPlugin
    {
        public const string Name = "SaberTailor";
        public const string Version = "1.5.0";

        string IPlugin.Name => Name;
        string IPlugin.Version => Version;

        internal static bool harmonyPatchesLoaded = false;
        internal static HarmonyInstance harmonyInstance = HarmonyInstance.Create("com.shadnix.BeatSaber.SaberTailor");

        readonly List<Tweaks.ITweak> _tweaks = new List<Tweaks.ITweak>
        {
            new Tweaks.SaberLength(),
            new Tweaks.SaberGrip(),
            new Tweaks.SaberTrail()
        };

        public void OnApplicationStart()
        {
            _tweaks.ForEach(tweak =>
            {
                try
                {
                    tweak.Load();
                    Log("Loaded tweak: {0}", tweak.Name);
                }
                catch (Exception ex)
                {
                    Log("Failed to load tweak: {0}. Exception: {1}", tweak.Name, ex);
                }
            });

            SceneManager.activeSceneChanged += SceneManagerOnActiveSceneChanged;
            SceneManager.sceneLoaded += SceneManagerSceneLoaded;
        }
        public void OnApplicationQuit()
        {
            _tweaks.ForEach(tweak =>
            {
                try
                {
                    tweak.Cleanup();
                    Log("Unloaded tweak: {0}", tweak.Name);
                }
                catch (Exception ex)
                {
                    Log("Failed to unload tweak: {0}. Exception: {1}", tweak.Name, ex);
                }
            });

            SceneManager.activeSceneChanged -= SceneManagerOnActiveSceneChanged;
            SceneManager.sceneLoaded -= SceneManagerSceneLoaded;
        }


        void SceneManagerOnActiveSceneChanged(Scene previousScene, Scene currentScene)
        {
            Preferences.Load();

            // Check if Harmony patches are already loaded
            if (harmonyPatchesLoaded) { return; }

            // Load Harmony patches if we are changing into main menu
            if (currentScene.name == "HealthWarning")
            {
                try
                {
                    Log("Loading Harmony patches...");
                    harmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
                    Log("Loaded Harmony patches. Successfully modified saber grip!");
                }
                catch (Exception e)
                {
                    Log("Loading Harmony patches failed. Please check if you have Harmony installed.");
                    Log(e.ToString());
                }
                harmonyPatchesLoaded = true;
            }
        }

        void SceneManagerSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "MenuCore")
            {
                UI.ModUI.CreateSettingsOptionsUI();
            }
        }


        public static void Log(string format, params object[] args)
        {
            Console.WriteLine($"[{Name}] " + format, args);
        }
        public static void Log(string message)
        {
            Log(message, new object[] { });
        }

        #region Unused IPlugin Members

        void IPlugin.OnUpdate() { }
        void IPlugin.OnFixedUpdate() { }
        void IPlugin.OnLevelWasLoaded(int level) { }
        void IPlugin.OnLevelWasInitialized(int level) { }

        #endregion
    }
}
