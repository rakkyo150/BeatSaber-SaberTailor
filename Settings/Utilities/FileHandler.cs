using IPA.Utilities;
using Newtonsoft.Json;
using System;
using System.IO;

namespace SaberTailor.Settings.Utilities
{
    class FileHandler
    {
        internal static readonly string configPath = Path.Combine(UnityGame.UserDataPath, "SaberTailor.json");

        internal static PluginConfig LoadConfig()
        {
            PluginConfig config;
            if (File.Exists(configPath))
            {   
                try
                {
                    config = JsonConvert.DeserializeObject<PluginConfig>(File.ReadAllText(configPath));
                }
                catch (Exception ex)
                {
                    Logger.log.Warn(ex);
                    Logger.log.Warn("Unable to load config from file. Creating new default configuration.");
                    config = new PluginConfig();
                }
            }
            else
            {
                Logger.log.Debug("Configuration file not found. Creating new default configuration.");
                config = new PluginConfig();
            }
            return config;
        }

        internal static bool SaveConfig(PluginConfig config)
        {
            bool saveSuccessful = true;
            try
            {
                File.WriteAllText(configPath, JsonConvert.SerializeObject(config, Formatting.Indented));
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex);
                Logger.log.Error("Unable to save configuration ");
                saveSuccessful = false;
            }

            return saveSuccessful;
        }
    }
}
