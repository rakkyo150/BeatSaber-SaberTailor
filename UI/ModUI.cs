using CustomUI.Settings;
using System;
using System.Reflection;
using UnityEngine;

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
            SubMenu trailMenu = settingsMenu.AddSubMenu("Trail Settings", "Adjust trail settings for default trails.", true);

            // add menu hilt option to main menu
            BoolViewController menuHiltSettingsCtrl = settingsMenu.AddBool("Menu hilt adjustments", "Enable to reposition the menu hilts the same way as the sabers.");
            menuHiltSettingsCtrl.GetValue += delegate { return Preferences.ModifyMenuHiltGrip; };
            menuHiltSettingsCtrl.SetValue += delegate (bool value) { Preferences.ModifyMenuHiltGrip = value; Preferences.Save(); };


            // Add options for left saber adjustments
            IntViewController lPosxCtrl = leftSaberMenu.AddInt("Pos X (Left/Right)", "Moves the saber left/right relative to the controller.", -50, 50, 1);
            lPosxCtrl.GetValue += delegate { return (int)(Preferences.GripLeftPosition.x * 100); };
            lPosxCtrl.SetValue += delegate (int value) { Preferences.GripLeftPosition.x = value / 100f; Preferences.Save(); };

            IntViewController lPosyCtrl = leftSaberMenu.AddInt("Pos Y (Down/Up)", "Moves the saber down/up relative to the controller.", -50, 50, 1);
            lPosyCtrl.GetValue += delegate { return (int)(Preferences.GripLeftPosition.y * 100); };
            lPosyCtrl.SetValue += delegate (int value) { Preferences.GripLeftPosition.y = value / 100f; Preferences.Save(); };

            IntViewController lPoszCtrl = leftSaberMenu.AddInt("Pos Z (Backwards/Forwards)", "Moves the saber backward/forward relative to the controller.", -50, 50, 1);
            lPoszCtrl.GetValue += delegate { return (int)(Preferences.GripLeftPosition.z * 100); };
            lPoszCtrl.SetValue += delegate (int value) { Preferences.GripLeftPosition.z = value / 100f; Preferences.Save(); };

            IntViewController lRotxCtrl = leftSaberMenu.AddInt("Rot X (Up/Down)", "Tilts the saber up/down relative to the controller.", -360, 360, 5);
            lRotxCtrl.GetValue += delegate { return (int)Preferences.GripLeftRotationRaw.x; };
            lRotxCtrl.SetValue += delegate (int value) { Preferences.GripLeftRotationRaw.x = value; Preferences.UpdateSaberRotation(); Preferences.Save(); };

            IntViewController lRotyCtrl = leftSaberMenu.AddInt("Rot Y (Left/Right)", "Rotates the saber left/right relative to the controller.", -360, 360, 5);
            lRotyCtrl.GetValue += delegate { return (int)Preferences.GripLeftRotationRaw.y; };
            lRotyCtrl.SetValue += delegate (int value) { Preferences.GripLeftRotationRaw.y = value; Preferences.UpdateSaberRotation(); Preferences.Save(); };

            IntViewController lRotzCtrl = leftSaberMenu.AddInt("Rot Z (Saber axis)", "Rotates the saber around its own axis.", -360, 360, 5);
            lRotzCtrl.GetValue += delegate { return (int)Preferences.GripLeftRotationRaw.z; };
            lRotzCtrl.SetValue += delegate (int value) { Preferences.GripLeftRotationRaw.z = value; Preferences.UpdateSaberRotation(); Preferences.Save(); };


            // Add options for right saber adjustments
            IntViewController rPosxCtrl = rightSaberMenu.AddInt("Pos X (Left/Right)", "Moves the saber left/right relative to the controller.", -50, 50, 1);
            rPosxCtrl.GetValue += delegate { return (int)(Preferences.GripRightPosition.x * 100); };
            rPosxCtrl.SetValue += delegate (int value) { Preferences.GripRightPosition.x = value / 100f; Preferences.Save(); };

            IntViewController rPosyCtrl = rightSaberMenu.AddInt("Pos Y (Down/Up)", "Moves the saber down/up relative to the controller.", -50, 50, 1);
            rPosyCtrl.GetValue += delegate { return (int)(Preferences.GripRightPosition.y * 100); };
            rPosyCtrl.SetValue += delegate (int value) { Preferences.GripRightPosition.y = value / 100f; Preferences.Save(); };

            IntViewController rPoszCtrl = rightSaberMenu.AddInt("Pos Z (Backwards/Forwards)", "Moves the saber backward/forward relative to the controller.", -50, 50, 1);
            rPoszCtrl.GetValue += delegate { return (int)(Preferences.GripRightPosition.z * 100); };
            rPoszCtrl.SetValue += delegate (int value) { Preferences.GripRightPosition.z = value / 100f; Preferences.Save(); };

            IntViewController rRotxCtrl = leftSaberMenu.AddInt("Rot X (Up/Down)", "Tilts the saber up/down relative to the controller.", -360, 360, 5);
            rRotxCtrl.GetValue += delegate { return (int)Preferences.GripRightRotationRaw.x; };
            rRotxCtrl.SetValue += delegate (int value) { Preferences.GripRightRotationRaw.x = value; Preferences.UpdateSaberRotation(); Preferences.Save(); };

            IntViewController rRotyCtrl = leftSaberMenu.AddInt("Rot Y (Left/Right)", "Rotates the saber left/right relative to the controller.", -360, 360, 5);
            rRotxCtrl.GetValue += delegate { return (int)Preferences.GripRightRotationRaw.y; };
            rRotxCtrl.SetValue += delegate (int value) { Preferences.GripRightRotationRaw.y = value; Preferences.UpdateSaberRotation(); Preferences.Save(); };

            IntViewController rRotzCtrl = leftSaberMenu.AddInt("Rot Z (Saber axis)", "Rotates the saber around its own axis.", -360, 360, 5);
            rRotxCtrl.GetValue += delegate { return (int)Preferences.GripRightRotationRaw.z; };
            rRotxCtrl.SetValue += delegate (int value) { Preferences.GripRightRotationRaw.z = value; Preferences.UpdateSaberRotation(); Preferences.Save(); };


            // Add options for trail adjustments
            BoolViewController trailEnableCtrl = trailMenu.AddBool("Enable saber trails", "Currently only works with sabers using default trail.");
            trailEnableCtrl.GetValue += delegate { return Preferences.IsTrailEnabled; };
            trailEnableCtrl.SetValue += delegate (bool value) { Preferences.IsTrailEnabled = value; Preferences.Save(); };

            IntViewController trailLengthCtrl = trailMenu.AddInt("Trail length", "Adjusts trail length", 5, 100, 5);
            trailLengthCtrl.GetValue += delegate { return Preferences.TrailLength; };
            trailLengthCtrl.SetValue += delegate (int value) { Preferences.TrailLength = value; Preferences.Save(); };
        }
    }
}
