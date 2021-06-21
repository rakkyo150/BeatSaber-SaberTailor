namespace SaberTailor.Settings.Classes
{


    public class PositionDigit
    {
        public static string[] PositionDigitArray = new string[5] { "100 cm", "10 cm", "1 cm", "0.1 cm", "0.01 cm" };
    }

    public class RotationDigit
    {
        public static string[] RotationDigitArray = new string[5] { "100 deg", "10 deg", "1 deg", "0.1 deg", "0.01 deg" };
    }

    public enum PositionDisplayUnit { cm, inches, miles, nauticalmiles }

    public class MenuConfig
    {
        public float SaberPosIncrement;
        public float SaberPosIncValue;
        public float SaberRotIncrement;
        public float SaberRotIncValue;

        public string SaberPosIncDigit;
        public string SaberRotIncDigit;
        public PositionDisplayUnit SaberPosDisplayUnit;
    }
}
