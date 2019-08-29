using System;
using CustomUI.Settings;

namespace SaberTailor.UI
{
    class ModUI
    {
        public static void CreateSettingsOptionsUI()
        {
            // setup main menu
            SubMenu settingsMenu = SettingsUI.CreateSubMenu("SaberTailor");

            // setup sub menus
            SubMenu leftSaberMenu = settingsMenu.AddSubMenu("Left Saber Settings", "Adjust position and rotation of the left saber.", true);
            SubMenu rightSaberMenu = settingsMenu.AddSubMenu("Right Saber Settings", "Adjust position and rotation of the right saber.", true);
            SubMenu saberScale = settingsMenu.AddSubMenu("Saber Scaling", "ScoreSubmission will be disabled if scales are not default!\nAdjust length and width of the sabers.", true);
            SubMenu trailMenu = settingsMenu.AddSubMenu("Trail Settings", "Adjust trail settings for default trails.", true);

            // add menu hilt option to main menu
            BoolViewController menuHiltSettingsCtrl = settingsMenu.AddBool("Menu hilt adjustments", "Enable to reposition the menu hilts the same way as the sabers.");
            menuHiltSettingsCtrl.GetValue += delegate
            {
                return Configuration.ModifyMenuHiltGrip;
            };
            menuHiltSettingsCtrl.SetValue += delegate (bool value)
            {
                Configuration.ModifyMenuHiltGrip = value;
            };


            // Add options for left saber adjustments
            IntViewController lPosxCtrl = leftSaberMenu.AddInt("Pos X (Left/Right)", "Moves the saber left/right relative to the controller.", -50, 50, 1);
            lPosxCtrl.GetValue += delegate
            {
                return Configuration.GripLeftPositionCfg.x / 10;
            };
            lPosxCtrl.SetValue += delegate (int value)
            {
                Configuration.GripLeftPositionCfg.x = value * 10;
            };

            IntViewController lPosyCtrl = leftSaberMenu.AddInt("Pos Y (Down/Up)", "Moves the saber down/up relative to the controller.", -50, 50, 1);
            lPosyCtrl.GetValue += delegate
            {
                return Configuration.GripLeftPositionCfg.y / 10;
            };
            lPosyCtrl.SetValue += delegate (int value)
            {
                Configuration.GripLeftPositionCfg.y = value * 10;
            };

            IntViewController lPoszCtrl = leftSaberMenu.AddInt("Pos Z (Backwards/Forwards)", "Moves the saber backwards/forwards relative to the controller.", -50, 50, 1);
            lPoszCtrl.GetValue += delegate
            {
                return Configuration.GripLeftPositionCfg.z / 10;
            };
            lPoszCtrl.SetValue += delegate (int value)
            {
                Configuration.GripLeftPositionCfg.z = value * 10;
            };

            IntViewController lRotxCtrl = leftSaberMenu.AddInt("Rot X (Up/Down)", "Tilts the saber up/down relative to the controller.", -360, 360, 5);
            lRotxCtrl.GetValue += delegate
            {
                return Configuration.GripLeftRotationCfg.x;
            };
            lRotxCtrl.SetValue += delegate (int value)
            {
                Configuration.GripLeftRotationCfg.x = value;
                Configuration.UpdateSaberRotation();
            };

            IntViewController lRotyCtrl = leftSaberMenu.AddInt("Rot Y (Left/Right)", "Rotates the saber left/right relative to the controller.", -360, 360, 5);
            lRotyCtrl.GetValue += delegate
            {
                return Configuration.GripLeftRotationCfg.y;
            };
            lRotyCtrl.SetValue += delegate (int value)
            {
                Configuration.GripLeftRotationCfg.y = value;
                Configuration.UpdateSaberRotation();
            };

            IntViewController lRotzCtrl = leftSaberMenu.AddInt("Rot Z (Saber axis)", "Rotates the saber around its own axis.", -360, 360, 5);
            lRotzCtrl.GetValue += delegate
            {
                return Configuration.GripLeftRotationCfg.z;
            };
            lRotzCtrl.SetValue += delegate (int value)
            {
                Configuration.GripLeftRotationCfg.z = value;
                Configuration.UpdateSaberRotation();
            };


            // Add options for right saber adjustments
            IntViewController rPosxCtrl = rightSaberMenu.AddInt("Pos X (Left/Right)", "Moves the saber left/right relative to the controller.", -50, 50, 1);
            rPosxCtrl.GetValue += delegate
            {
                return Configuration.GripRightPositionCfg.x / 10;
            };
            rPosxCtrl.SetValue += delegate (int value)
            {
                Configuration.GripRightPositionCfg.x = value * 10;
            };

            IntViewController rPosyCtrl = rightSaberMenu.AddInt("Pos Y (Down/Up)", "Moves the saber down/up relative to the controller.", -50, 50, 1);
            rPosyCtrl.GetValue += delegate
            {
                return Configuration.GripRightPositionCfg.y / 10;
            };
            rPosyCtrl.SetValue += delegate (int value)
            {
                Configuration.GripRightPositionCfg.y = value * 10;
            };

            IntViewController rPoszCtrl = rightSaberMenu.AddInt("Pos Z (Backwards/Forwards)", "Moves the saber backwards/forwards relative to the controller.", -50, 50, 1);
            rPoszCtrl.GetValue += delegate
            {
                return Configuration.GripRightPositionCfg.z / 10;
            };
            rPoszCtrl.SetValue += delegate (int value)
            {
                Configuration.GripRightPositionCfg.z = value * 10;
            };

            IntViewController rRotxCtrl = rightSaberMenu.AddInt("Rot X (Up/Down)", "Tilts the saber up/down relative to the controller.", -360, 360, 5);
            rRotxCtrl.GetValue += delegate
            {
                return Configuration.GripRightRotationCfg.x;
            };
            rRotxCtrl.SetValue += delegate (int value)
            {
                Configuration.GripRightRotationCfg.x = value;
                Configuration.UpdateSaberRotation();
            };

            IntViewController rRotyCtrl = rightSaberMenu.AddInt("Rot Y (Left/Right)", "Rotates the saber left/right relative to the controller.", -360, 360, 5);
            rRotyCtrl.GetValue += delegate
            {
                return Configuration.GripRightRotationCfg.y;
            };
            rRotyCtrl.SetValue += delegate (int value)
            {
                Configuration.GripRightRotationCfg.y = value;
                Configuration.UpdateSaberRotation();
            };

            IntViewController rRotzCtrl = rightSaberMenu.AddInt("Rot Z (Saber axis)", "Rotates the saber around its own axis.", -360, 360, 5);
            rRotzCtrl.GetValue += delegate
            {
                return Configuration.GripRightRotationCfg.z;
            };
            rRotzCtrl.SetValue += delegate (int value)
            {
                Configuration.GripRightRotationCfg.z = value;
                Configuration.UpdateSaberRotation();
            };


            // Add options for saber size adjustments
            IntViewController scaleLengthCtrl = saberScale.AddInt("Length (Default:100%)", "Scales the saber length.", 5, 500, 5);
            scaleLengthCtrl.GetValue += delegate
            {
                return Configuration.SaberLengthCfg;
            };
            scaleLengthCtrl.SetValue += delegate (int value)
            {
                Configuration.SaberLengthCfg = value;
            };

            IntViewController scaleGirthCtrl = saberScale.AddInt("Width (Default:100%)", "Scales the saber width.", 5, 500, 5);
            scaleGirthCtrl.GetValue += delegate
            {
                return Configuration.SaberGirthCfg;
            };
            scaleGirthCtrl.SetValue += delegate (int value)
            {
                Configuration.SaberGirthCfg = value;
            };


            // Add options for trail adjustments
            BoolViewController trailEnableCtrl = trailMenu.AddBool("Enable saber trails", "Currently only works with sabers using default trail.");
            trailEnableCtrl.GetValue += delegate
            {
                return Configuration.IsTrailEnabled;
            };
            trailEnableCtrl.SetValue += delegate (bool value)
            {
                Configuration.IsTrailEnabled = value;
            };

            IntViewController trailLengthCtrl = trailMenu.AddInt("Trail length", "Adjusts trail length. Currently only works with sabers using default trail.", 5, 100, 5);
            trailLengthCtrl.GetValue += delegate
            {
                return Configuration.TrailLength;
            };
            trailLengthCtrl.SetValue += delegate (int value)
            {
                Configuration.TrailLength = value;
            };
        }
    }
}
