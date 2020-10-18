using IPA.Utilities;
using SaberTailor.Settings.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SaberTailor.Settings.Utilities
{
    class ProfileManager
    {
        internal static bool profilesLoaded = false;
        internal static bool profilesPresent = false;
        internal static List<string> profileNames;

        internal static void LoadProfiles()
        {
            profileNames = new List<string>();

            string[] fileNames = Directory.GetFiles(UnityGame.UserDataPath, @"SaberTailor.*.json");
            foreach (string fileName in fileNames)
            {
                Regex r = new Regex(@"^SaberTailor\.([a-zA-Z0-9_-]+)\.json$");
                Match m = r.Match(Path.GetFileName(fileName));
                if (m.Success)
                {
                    profileNames.Add(m.Groups[1].Value);
                }
            }

            if (profileNames.Count > 0)
            {
                profilesPresent = true;
            }

            profilesLoaded = true;
        }

        internal static bool LoadProfile(string profileName, out string statusMsg)
        {
            string fileName = @"SaberTailor." + profileName + @".json";
            bool loadSuccessful = FileHandler.LoadConfig(out PluginConfig config, fileName);
            if (loadSuccessful)
            {
                PluginConfig.Instance = config;
                Configuration.Load();
                statusMsg = "Profile successfully loaded.";
                return true;
            }
            else
            {
                statusMsg = "Profile loading failed. Please check logs files.";
                return false;
            }
        }

        internal static void PrintProfileNames()
        {
            Logger.log.Debug("Printing list of Profile names:");
            if (!profilesPresent)
            {
                Logger.log.Debug("No profiles present.");
                return;
            }
            Logger.log.Debug(String.Join("; ", profileNames));
        }
    }
}
