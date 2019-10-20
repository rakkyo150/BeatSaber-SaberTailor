using CustomUI.Settings;
using System;
using System.Reflection;
using UnityEngine;
using LogLevel = IPA.Logging.Logger.Level;

namespace SaberTailor.Settings.UI
{
    internal class ModUI
    {
        public static void CreateSettingsOptionsUI()
        {
            // setup main menu
            SubMenu settingsMenu = SettingsUI.CreateSubMenu("SaberTailor");

            // setup sub menus
            SubMenu leftSaberMenu = settingsMenu.AddSubMenu("Left Saber Settings", "Adjust position and rotation of the left saber.", true);
            SubMenu rightSaberMenu = settingsMenu.AddSubMenu("Right Saber Settings", "Adjust position and rotation of the right saber.", true);
            SubMenu saberScaleMenu = settingsMenu.AddSubMenu("Saber Scaling", "ScoreSubmission will be disabled if scales are not default!\nAdjust length and width of the sabers.", true);
            SubMenu trailMenu = settingsMenu.AddSubMenu("Trail Settings", "Adjust trail settings for default trails.", true);

            // add menu hilt option to main menu
            BoolViewController menuHiltSettingsCtrl = settingsMenu.AddBool("Menu hilt adjustments", "Enable to reposition the menu hilts the same way as the sabers.");
            menuHiltSettingsCtrl.GetValue += delegate
            {
                return Configuration.Grip.ModifyMenuHiltGrip;
            };
            menuHiltSettingsCtrl.SetValue += delegate (bool value)
            {
                Configuration.Grip.ModifyMenuHiltGrip = value;
            };


            // Add options for left saber adjustments
            IntViewController lPosxCtrl = leftSaberMenu.AddInt("Pos X (Left/Right) in cm", "Moves the saber left/right relative to the controller.", -50, 50, 1);
            lPosxCtrl.GetValue += delegate
            {
                return Configuration.GripCfg.PosLeft.x / 10;
            };
            lPosxCtrl.SetValue += delegate (int value)
            {
                Configuration.GripCfg.PosLeft.x = value * 10;
                Configuration.UpdateSaberPosition();
            };

            IntViewController lPosyCtrl = leftSaberMenu.AddInt("Pos Y (Down/Up) in cm", "Moves the saber down/up relative to the controller.", -50, 50, 1);
            lPosyCtrl.GetValue += delegate
            {
                return Configuration.GripCfg.PosLeft.y / 10;
            };
            lPosyCtrl.SetValue += delegate (int value)
            {
                Configuration.GripCfg.PosLeft.y = value * 10;
                Configuration.UpdateSaberPosition();
            };

            IntViewController lPoszCtrl = leftSaberMenu.AddInt("Pos Z (Bwd./Fwd.) in cm", "Moves the saber backwards/forwards relative to the controller.", -50, 50, 1);
            lPoszCtrl.GetValue += delegate
            {
                return Configuration.GripCfg.PosLeft.z / 10;
            };
            lPoszCtrl.SetValue += delegate (int value)
            {
                Configuration.GripCfg.PosLeft.z = value * 10;
                Configuration.UpdateSaberPosition();
            };

            IntViewController lRotxCtrl = leftSaberMenu.AddInt("Rot X (Up/Down) in degree", "Tilts the saber up/down relative to the controller.", -360, 360, 5);
            lRotxCtrl.GetValue += delegate
            {
                return Configuration.GripCfg.RotLeft.x;
            };
            lRotxCtrl.SetValue += delegate (int value)
            {
                Configuration.GripCfg.RotLeft.x = value;
                Configuration.UpdateSaberRotation();
            };

            IntViewController lRotyCtrl = leftSaberMenu.AddInt("Rot Y (Left/Right) in degree", "Rotates the saber left/right relative to the controller.", -360, 360, 5);
            lRotyCtrl.GetValue += delegate
            {
                return Configuration.GripCfg.RotLeft.y;
            };
            lRotyCtrl.SetValue += delegate (int value)
            {
                Configuration.GripCfg.RotLeft.y = value;
                Configuration.UpdateSaberRotation();
            };

            IntViewController lRotzCtrl = leftSaberMenu.AddInt("Rot Z (Saber axis) in degree", "Rotates the saber around its own axis.", -360, 360, 5);
            lRotzCtrl.GetValue += delegate
            {
                return Configuration.GripCfg.RotLeft.z;
            };
            lRotzCtrl.SetValue += delegate (int value)
            {
                Configuration.GripCfg.RotLeft.z = value;
                Configuration.UpdateSaberRotation();
            };


            // Add options for right saber adjustments
            IntViewController rPosxCtrl = rightSaberMenu.AddInt("Pos X (Left/Right) in cm", "Moves the saber left/right relative to the controller.", -50, 50, 1);
            rPosxCtrl.GetValue += delegate
            {
                return Configuration.GripCfg.PosRight.x / 10;
            };
            rPosxCtrl.SetValue += delegate (int value)
            {
                Configuration.GripCfg.PosRight.x = value * 10;
                Configuration.UpdateSaberPosition();
            };

            IntViewController rPosyCtrl = rightSaberMenu.AddInt("Pos Y (Down/Up) in cm", "Moves the saber down/up relative to the controller.", -50, 50, 1);
            rPosyCtrl.GetValue += delegate
            {
                return Configuration.GripCfg.PosRight.y / 10;
            };
            rPosyCtrl.SetValue += delegate (int value)
            {
                Configuration.GripCfg.PosRight.y = value * 10;
                Configuration.UpdateSaberPosition();
            };

            IntViewController rPoszCtrl = rightSaberMenu.AddInt("Pos Z (Bwd./Fwd.) in cm", "Moves the saber backwards/forwards relative to the controller.", -50, 50, 1);
            rPoszCtrl.GetValue += delegate
            {
                return Configuration.GripCfg.PosRight.z / 10;
            };
            rPoszCtrl.SetValue += delegate (int value)
            {
                Configuration.GripCfg.PosRight.z = value * 10;
                Configuration.UpdateSaberPosition();
            };

            IntViewController rRotxCtrl = rightSaberMenu.AddInt("Rot X (Up/Down) in degree", "Tilts the saber up/down relative to the controller.", -360, 360, 5);
            rRotxCtrl.GetValue += delegate
            {
                return Configuration.GripCfg.RotRight.x;
            };
            rRotxCtrl.SetValue += delegate (int value)
            {
                Configuration.GripCfg.RotRight.x = value;
                Configuration.UpdateSaberRotation();
            };

            IntViewController rRotyCtrl = rightSaberMenu.AddInt("Rot Y (Left/Right) in degree", "Rotates the saber left/right relative to the controller.", -360, 360, 5);
            rRotyCtrl.GetValue += delegate
            {
                return Configuration.GripCfg.RotRight.y;
            };
            rRotyCtrl.SetValue += delegate (int value)
            {
                Configuration.GripCfg.RotRight.y = value;
                Configuration.UpdateSaberRotation();
            };

            IntViewController rRotzCtrl = rightSaberMenu.AddInt("Rot Z (Saber axis) in degree", "Rotates the saber around its own axis.", -360, 360, 5);
            rRotzCtrl.GetValue += delegate
            {
                return Configuration.GripCfg.RotRight.z;
            };
            rRotzCtrl.SetValue += delegate (int value)
            {
                Configuration.GripCfg.RotRight.z = value;
                Configuration.UpdateSaberRotation();
            };


            // Add options for saber size adjustments
            BoolViewController scaleModEnableCtrl = saberScaleMenu.AddBool("Enable saber scale modification", "Enable/Disable any scale modifications.");
            scaleModEnableCtrl.GetValue += delegate
            {
                return Configuration.Scale.TweakEnabled;
            };
            scaleModEnableCtrl.SetValue += delegate (bool value)
            {
                Configuration.Scale.TweakEnabled = value;
            };

            BoolViewController hitboxScaleCtrl = saberScaleMenu.AddBool("Scale hit-box", "Enable/Disable saber hit-box scaling\n<color=\"red\">Score Submission will be disabled as long as this option is enabled!");
            hitboxScaleCtrl.GetValue += delegate
            {
                return Configuration.Scale.ScaleHitBox;
            };
            hitboxScaleCtrl.SetValue += delegate (bool value)
            {
                Configuration.Scale.ScaleHitBox = value;
            };

            BoolViewController hitboxScaleTxt = saberScaleMenu.AddBool("<color=\"red\">Enabling hit-box scaling will disable score submission.");
            hitboxScaleTxt.GetValue += delegate { return false; };
            hitboxScaleTxt.SetValue += delegate (bool value) { };
            // Hack to convert bool segment to text only (based on hack in BSTweaks mod)
            try
            {
                var hitboxScaleTxtButtonToDisable = hitboxScaleTxt.GetType().BaseType.BaseType.GetField("_decButton", BindingFlags.NonPublic | BindingFlags.Instance);
                var hitboxScaleTxtDecButton = (MonoBehaviour)hitboxScaleTxtButtonToDisable.GetValue(hitboxScaleTxt);
                hitboxScaleTxtButtonToDisable = hitboxScaleTxt.GetType().BaseType.BaseType.GetField("_incButton", BindingFlags.NonPublic | BindingFlags.Instance);
                var hitboxScaleTxtIncButton = (MonoBehaviour)hitboxScaleTxtButtonToDisable.GetValue(hitboxScaleTxt);

                hitboxScaleTxtDecButton.gameObject.SetActive(false);
                hitboxScaleTxtIncButton.gameObject.SetActive(false);

                var hitboxScaleTxtTextToDisable = hitboxScaleTxt.GetType().BaseType.BaseType.GetField("_text", BindingFlags.NonPublic | BindingFlags.Instance);
                var hitboxScaleTxtUselessText = (MonoBehaviour)hitboxScaleTxtTextToDisable.GetValue(hitboxScaleTxt);

                hitboxScaleTxtUselessText.gameObject.SetActive(false);
            }
            catch (Exception ex)
            {
                Logger.Log("Error trying to disable first comment in settings menu:" + ex.ToString(), LogLevel.Error);
            }

            IntViewController scaleLengthCtrl = saberScaleMenu.AddInt("Length (Default: 100%)", "Scales the saber length.", 5, 500, 5);
            scaleLengthCtrl.GetValue += delegate
            {
                return Configuration.ScaleCfg.Length;
            };
            scaleLengthCtrl.SetValue += delegate (int value)
            {
                Configuration.ScaleCfg.Length = value;
            };

            IntViewController scaleGirthCtrl = saberScaleMenu.AddInt("Width (Default: 100%)", "Scales the saber width.", 5, 500, 5);
            scaleGirthCtrl.GetValue += delegate
            {
                return Configuration.ScaleCfg.Girth;
            };
            scaleGirthCtrl.SetValue += delegate (int value)
            {
                Configuration.ScaleCfg.Girth = value;
            };


            // Add options for trail adjustments
            BoolViewController trailModEnableCtrl = trailMenu.AddBool("Enable saber trail modification", "Enable/Disable any trail modifications.");
            trailModEnableCtrl.GetValue += delegate
            {
                return Configuration.Trail.TweakEnabled;
            };
            trailModEnableCtrl.SetValue += delegate (bool value)
            {
                Configuration.Trail.TweakEnabled = value;
            };

            BoolViewController trailEnableCtrl = trailMenu.AddBool("Enable saber trails", "Currently only works with sabers using default trail.");
            trailEnableCtrl.GetValue += delegate
            {
                return Configuration.Trail.TrailEnabled;
            };
            trailEnableCtrl.SetValue += delegate (bool value)
            {
                Configuration.Trail.TrailEnabled = value;
            };

            IntViewController trailLengthCtrl = trailMenu.AddInt("Trail length", "Adjusts trail length. Currently only works with sabers using default trail.", 5, 100, 5);
            trailLengthCtrl.GetValue += delegate
            {
                return Configuration.Trail.Length;
            };
            trailLengthCtrl.SetValue += delegate (int value)
            {
                Configuration.Trail.Length = value;
            };
        }
    }
}
