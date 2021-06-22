## 日本語版READMEは[こちら](README_JP.md)

# (Improved) Saber Tailor

## How to install
1. Download SaberTailor.dll from [Releases](https://github.com/rakkyo150/BeatSaber-SaberTailor/releases)
2. Add SaberTailor.dll to the Plugins folder under the Beat Saber installation folder

For the Steam version of Beat Saber, the location of the Plugin folder is<br>
C:\Program Files (x86)\Steam\steamapps\common\Beat Saber\Plugins
is the default. <BR>
Just in case.

## Differences from the [original version](https://github.com/Shadnix-was-taken/BeatSaber-SaberTailor)

1. More fine-tuned settings for saber position, angle and world offset
2. Extended limits for saber position and world offset. 3.
3. Improved UI.
4. Fixed UI bug in world offset Y and Z.


## About 1
Saber position and angle and world offset can now be adjusted to the decimal point.

## Adjustment methods
The following methods support adjustment to the second decimal place. <br>
**For adjustments to the third decimal place and below, please go to Beat Saber/UserData/SaberTailor.json.** <br>
In addition to the changes made in this article, other parts are omitted from the explanation. <br> 
Please understand.


(1)<br>
Select Settings from the main menu.
![proccess1](Images/process1.png)

(2)<br>
Select MOD SETTINGS.
![process2](Images/process2.png)

(3) <br>
Scroll down and select Saber Grip Positions/Rotation...
![process3](Images/process3.png)

(4)<br>
Scroll down and select Precision and other settings...
![process4](Images/process4.png)

(5)<br>
The Saber Position Inc. Digit and Saber Rotation Inc. Digit are the parts where you can select the digit to increase or decrease. <br>
You can choose from 0.01/0.1/1/10/100. <br>
The units are cm and degree. <br>
The Saber Position Increment and Saber Rotation Increment are the parts that determine how much to increase or decrease each time. <br> 
Basically you not need to tweak them. (In the image, Saber Position Increment is tweaked.)<br>
For example, if the Saber Position Inc. Digit is set to 100 cm in the image, the Saber Position Increment will automatically be set to 200 cm.
![process5](Images/process5.png)

(6)<br>
To set up the left saber, select <back and then select Left Saber Settings. <br>
To configure the right saber settings, select Right Saber Settings.
![process6](Images/process6.png)

(7)<br>
The images have been increased or decreased once for each. <br>
The settings in (5) are reflected. <br> 
The formula is also shown here. <br>
Pos or World Offset=(Saber Position Inc. Digit x Number of clicks of Saber Position Increment) x Number of clicks of Saber Setting<br>
Rot=(Saber Rotation Inc. Digit x Saber Rotation Increment clicks) x Saber Setting clicks
![process7](Images/process7.png)

(8)<br>
For World Offset, Saber Position increment is used. <br>
The image shows the X, Y, and Z of the World Offset being incremented or decremented once each. <BR>
MIRROR TO RIGHT allows the right saber to reflect the settings of the left saber. <BR>
If you press REVERT, it will mean that there was no change in the settings of the left saber.  <br>
Press OK to complete the setting.
Note that pressing CANCEL, on the other hand, will mean that no changes were made to the settings of the entire SaberTailor.
![process8](Images/process8.png)


By the way, if you want to display and adjust the actual saber, install [Custom Saber](https://twitter.com/nalulululuna/status/1406288209093435398), which is currently developed by naluluna, and click Show Saber in Menu Always should be enabled.

## About 2
Saber position and world offset limits have been extended from 50cm to 500cm (5m).

## About 3
Added a heading to the submenu. <br>
The back button to the main menu has been placed at the top left of the submenu. <br>
The back button is also placed in the lower left corner of the submenus that have scrolling. <br>
Revert button and Mirror to Left/Right button are now easier to understand.

## About 4
There was a bug in the original that prevented the world offset Y and Z from being changed in the UI (as of 6/21/2021). <br>
This has been fixed.
