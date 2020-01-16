using SaberTailor.Settings.Classes;
using System;
using System.Linq;
using UnityEngine;

namespace SaberTailor.Settings.Utilities
{
    internal class GameSettingsTransfer
    {
        internal static bool ImportFromGame()
        {
            // Get reference to MainSettingsModelSO
            MainSettingsModelSO mainSettings = Resources.FindObjectsOfTypeAll<MainSettingsModelSO>().FirstOrDefault();
            if (mainSettings == null)
            {
                Logger.log.Error("ImportFromGame: Unable to get a handle on MainSettingsModelSO. Exiting...");
                return false;
            }

            // Check for position/rotation settings
            if (mainSettings.controllerPosition == null)
            {
                Logger.log.Error("ImportFromGame: Settings for controller position not found. Exiting...");
                return false;
            }

            if (mainSettings.controllerRotation == null)
            {
                Logger.log.Error("ImportFromGame: Settings for controller rotation not found. Exiting...");
                return false;
            }

            Vector3SO ctrlPos = mainSettings.controllerPosition;
            Vector3SO ctrlRot = mainSettings.controllerRotation;

            Configuration.GripCfg.PosLeft = new Int3()
            {
                x = -(int)ctrlPos.value.x * 1000,
                y = (int)ctrlPos.value.y * 1000,
                z = (int)ctrlPos.value.z * 1000
            };

            Configuration.GripCfg.RotLeft = new Int3()
            {
                x = (int)ctrlRot.value.x,
                y = -(int)ctrlRot.value.y,
                z = (int)ctrlRot.value.z
            };

            Configuration.GripCfg.PosRight = new Int3()
            {
                x = -(int)ctrlPos.value.x * 1000,
                y = (int)ctrlPos.value.y * 1000,
                z = (int)ctrlPos.value.z * 1000
            };

            Configuration.GripCfg.RotRight = new Int3()
            {
                x = (int)ctrlRot.value.x,
                y = -(int)ctrlRot.value.y,
                z = (int)ctrlRot.value.z
            };

            return true;
        }

        internal static bool ExportToGame(out string statusMsg)
        {
            bool exportable = true;
            statusMsg = "";

            exportable = CheckGripCompatibility(out statusMsg);

            if (exportable)
            {
                try
                {
                    // Get reference to MainSettingsModelSO
                    MainSettingsModelSO mainSettings = Resources.FindObjectsOfTypeAll<MainSettingsModelSO>().FirstOrDefault();
                    if (mainSettings == null)
                    {
                        Logger.log.Error("ImportFromGame: Unable to get a handle on MainSettingsModelSO. Exiting...");
                        return false;
                    }

                    // Set new position vector
                    mainSettings.controllerPosition.value = new Vector3(
                        Configuration.GripCfg.PosRight.x / 1000f,
                        Configuration.GripCfg.PosRight.y / 1000f,
                        Configuration.GripCfg.PosRight.z / 1000f);

                    // Set new rotation vector. Base game settings are clamped to -180,+180 on load, hence the extra work
                    mainSettings.controllerRotation.value = new Vector3(
                        NormalizeAngle(Configuration.GripCfg.RotRight.x),
                        NormalizeAngle(Configuration.GripCfg.RotRight.y),
                        NormalizeAngle(Configuration.GripCfg.RotRight.z));

                    //mainSettings.Save();
                    //mainSettings.Load(true);

                    statusMsg = "Export successful.";
                }
                catch (Exception ex)
                {
                    Logger.log.Error("Error trying to export SaberTailor grip config to base game settings.");
                    Logger.log.Error(ex.ToString());
                    statusMsg = "<color=#fb484e>Unable to export to base game: Unknown error.</color>";
                }
            }
            return exportable;
        }

        internal static bool CheckGripCompatibility(out string statusMsg)
        {
            bool exportable = true;
            statusMsg = "";

            int baseGamePosLimit = 100;             // in mm
            int baseGameRotLimit = 225;             // in deg

            // Check to see if position adjustments are within base game limits (Check right only because left values needs mirrored to right anyway)
            if (Configuration.GripCfg.PosRight.x < -baseGamePosLimit)
            {
                statusMsg = AddErrorStatus(statusMsg, String.Format("Right position X is smaller than -{0:0} cm", baseGamePosLimit), ref exportable);
            }
            if (Configuration.GripCfg.PosRight.x > baseGamePosLimit)
            {
                statusMsg = AddErrorStatus(statusMsg, String.Format("Right position X is larger than {0:0} cm", baseGamePosLimit), ref exportable);
            }
            if (Configuration.GripCfg.PosRight.y < -baseGamePosLimit)
            {
                statusMsg = AddErrorStatus(statusMsg, String.Format("Right position Y is smaller than -{0:0} cm", baseGamePosLimit), ref exportable);
            }
            if (Configuration.GripCfg.PosRight.y > baseGamePosLimit)
            {
                statusMsg = AddErrorStatus(statusMsg, String.Format("Right position Y is larger than {0:0} cm", baseGamePosLimit), ref exportable);
            }
            if (Configuration.GripCfg.PosRight.z < -baseGamePosLimit)
            {
                statusMsg = AddErrorStatus(statusMsg, String.Format("Right position Z is smaller than -{0:0} cm", baseGamePosLimit), ref exportable);
            }
            if (Configuration.GripCfg.PosRight.z > baseGamePosLimit)
            {
                statusMsg = AddErrorStatus(statusMsg, String.Format("Right position Z is larger than {0:0} cm", baseGamePosLimit), ref exportable);
            }

            // Check to see if rotation adjustments are within base game limits (Check right only because left values needs mirrored to right anyway)
            if (Configuration.GripCfg.RotRight.x < -baseGameRotLimit)
            {
                statusMsg = AddErrorStatus(statusMsg, String.Format("Right rotation X is smaller than -{0:0} deg", baseGameRotLimit), ref exportable);
            }
            if (Configuration.GripCfg.RotRight.x > baseGameRotLimit)
            {
                statusMsg = AddErrorStatus(statusMsg, String.Format("Right rotation X is larger than {0:0} deg", baseGameRotLimit), ref exportable);
            }
            if (Configuration.GripCfg.RotRight.y < -baseGameRotLimit)
            {
                statusMsg = AddErrorStatus(statusMsg, String.Format("Right rotation Y is smaller than -{0:0} deg", baseGameRotLimit), ref exportable);
            }
            if (Configuration.GripCfg.RotRight.y > baseGameRotLimit)
            {
                statusMsg = AddErrorStatus(statusMsg, String.Format("Right rotation Y is larger than {0:0} deg", baseGameRotLimit), ref exportable);
            }
            if (Configuration.GripCfg.RotRight.z < -baseGameRotLimit)
            {
                statusMsg = AddErrorStatus(statusMsg, String.Format("Right rotation Z is smaller than -{0:0} deg", baseGameRotLimit), ref exportable);
            }
            if (Configuration.GripCfg.RotRight.z > baseGameRotLimit)
            {
                statusMsg = AddErrorStatus(statusMsg, String.Format("Right rotation Z is larger than {0:0} deg", baseGameRotLimit), ref exportable);
            }

            // Check to see if position adjustments are mirrored
            if (Configuration.GripCfg.PosRight.x != -Configuration.GripCfg.PosLeft.x)
            {
                statusMsg = AddErrorStatus(statusMsg, "Right position X is not equal to inverted left position X", ref exportable);
            }
            if (Configuration.GripCfg.PosRight.y != Configuration.GripCfg.PosLeft.y)
            {
                statusMsg = AddErrorStatus(statusMsg, "Right position Y is not equal to left position Y", ref exportable);
            }
            if (Configuration.GripCfg.PosRight.z != Configuration.GripCfg.PosLeft.z)
            {
                statusMsg = AddErrorStatus(statusMsg, "Right position Z is not equal to left position Z", ref exportable);
            }

            // Check to see if rotation adjustments are mirrored
            if (Configuration.GripCfg.RotRight.x != Configuration.GripCfg.RotLeft.x)
            {
                statusMsg = AddErrorStatus(statusMsg, "Right rotation X is not equal to left rotation X", ref exportable);
            }
            if (Configuration.GripCfg.RotRight.y != -Configuration.GripCfg.RotLeft.y)
            {
                statusMsg = AddErrorStatus(statusMsg, "Right rotation Y is not equal to inverted left rotation Y", ref exportable);
            }
            if (Configuration.GripCfg.RotRight.z != Configuration.GripCfg.RotLeft.z)
            {
                statusMsg = AddErrorStatus(statusMsg, "Right rotation Z is not equal to left rotation Z", ref exportable);
            }

            if (exportable)
            {
                statusMsg = "Export to game settings is possible.";
            }

            return exportable;
        }

        private static string AddErrorStatus(string status, string addStatus, ref bool firstMsg)
        {
            if (firstMsg)
            {
                firstMsg = false;
                return "<color=#fb484e>Unable to export to base game: " + addStatus;
            }
            else
            {
                return status + " | " + addStatus;
            }
        }

        private static int NormalizeAngle(int angle)
        {
            while (angle > 180)
            {
                angle = angle - 360;
            }

            while (angle < -180)
            {
                angle = angle + 360;
            }

            return angle;
        }
    }
}
