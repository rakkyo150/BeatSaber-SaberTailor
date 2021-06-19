namespace SaberTailor.Settings.Classes
{
    public enum PositionUnit { m,dm,cm,mm }

    public enum RotationUnit { hundred, ten, one, tenth, hundredth}
    public enum PositionDisplayUnit { cm, inches, miles, nauticalmiles }

    public class MenuConfig
    {
        public float SaberPosIncrement;
        public float SaberPosIncValue;
        public float SaberRotIncrement;
        public float SaberRotIncValue;

        public PositionUnit SaberPosIncUnit;
        public RotationUnit SaberRotIncUnit;
        public PositionDisplayUnit SaberPosDisplayUnit;
    }
}
