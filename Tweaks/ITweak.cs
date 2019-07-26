namespace SaberTailor.Tweaks
{
    public interface ITweak
    {
        string Name { get; }

        bool IsPreventingScoreSubmission { get; }

        void Load();
        void Cleanup();
    }

    public static class TweakExtensions
    {
        public static void Log(this ITweak tweak, string message, IPA.Logging.Logger.Level severity = IPA.Logging.Logger.Level.Info)
        {
            Logger.Log($"[{tweak.Name}] {message}", severity);
        }

        public static void Log(this ITweak tweak, System.Exception ex, IPA.Logging.Logger.Level severity = IPA.Logging.Logger.Level.Error)
        {
            Logger.Log(ex, severity);
        }

        public static void Log(this ITweak tweak, System.Exception ex, string message, IPA.Logging.Logger.Level severity = IPA.Logging.Logger.Level.Error)
        {
            Log(tweak, message, severity);
            Log(tweak, ex, severity);
        }
    }
}
