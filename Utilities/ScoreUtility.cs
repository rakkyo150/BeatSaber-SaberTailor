using BS_Utils.Gameplay;
using System.Collections.Generic;
using LogLevel = IPA.Logging.Logger.Level;

namespace SaberTailor.Utilities
{
    public static class ScoreUtility
    {
        private static List<string> ScoreBlockList = new List<string>();
        private static object acquireLock = new object();

        public static bool ScoreIsBlocked { get; private set; } = false;

        internal static void DisableScoreSubmission(string BlockedBy)
        {
            lock (acquireLock)
            {
                if (!ScoreBlockList.Contains(BlockedBy))
                {
                    ScoreBlockList.Add(BlockedBy);
                }

                if (!ScoreIsBlocked)
                {
                    Logger.Log("ScoreSubmission has been disabled.", LogLevel.Info);
                    ScoreSubmission.ProlongedDisableSubmission(Plugin.PluginName);
                    ScoreIsBlocked = true;
                }
            }
        }

        internal static void EnableScoreSubmission(string BlockedBy)
        {
            lock (acquireLock)
            {
                if (ScoreBlockList.Contains(BlockedBy))
                {
                    ScoreBlockList.Remove(BlockedBy);
                }

                if (ScoreIsBlocked && ScoreBlockList.Count == 0)
                {
                    Logger.Log("ScoreSubmission has been re-enabled.", LogLevel.Info);
                    ScoreSubmission.RemoveProlongedDisable(Plugin.PluginName);
                    ScoreIsBlocked = false;
                }
            }
        }

        /// <summary>
        /// Should only be called on exit!
        /// </summary>
        internal static void Cleanup()
        {
            lock (acquireLock)
            {
                if (ScoreIsBlocked)
                {
                    Logger.Log("Plugin is exiting, ScoreSubmission has been re-enabled.", LogLevel.Info);
                    ScoreSubmission.RemoveProlongedDisable(Plugin.PluginName);
                    ScoreIsBlocked = false;
                }

                if (ScoreBlockList.Count != 0)
                {
                    ScoreBlockList.Clear();
                }
            }
        }
    }
}
