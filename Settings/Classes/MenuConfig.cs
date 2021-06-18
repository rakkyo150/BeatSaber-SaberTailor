namespace SaberTailor.Settings.Classes
{
    public enum PositionUnit { cm, mm }
    public enum PositionDisplayUnit { cm, inches, miles, nauticalmiles }

    public class MenuConfig
    {
        public float SaberPosIncrement;
        public float SaberPosIncValue;
        public float SaberRotIncrement;

        public PositionUnit SaberPosIncUnit;
        public PositionDisplayUnit SaberPosDisplayUnit;
    }
}
