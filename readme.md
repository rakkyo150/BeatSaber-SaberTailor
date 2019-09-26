# Beat Saber - Saber Tailor v2.0.0

Tweak things about your sabers, including grip position/angle, saber length and trail length! Original mod by Ruu.

## Settings

Adjust the different settings in-game from the "Settings" menu.

You can also adjust your settings manually in `\Beat Saber\UserData\SaberTailor.json` (The game will have to be restarted)
The default values are below:

```json
{
  "RegenerateConfig": false,
  "ConfigVersion": 1,
  "IsSaberScaleModEnabled": false,
  "SaberScaleHitbox": false,
  "SaberLength": 100,
  "SaberGirth": 100,
  "IsTrailModEnabled": false,
  "IsTrailEnabled": true,
  "TrailLength": 20,
  "GripLeftPosition": {
    "x": 0,
    "y": 0,
    "z": 0
  },
  "GripRightPosition": {
    "x": 0,
    "y": 0,
    "z": 0
  },
  "GripLeftRotation": {
    "x": 0,
    "y": 0,
    "z": 0
  },
  "GripRightRotation": {
    "x": 0,
    "y": 0,
    "z": 0
  },
  "ModifyMenuHiltGrip": false
}
```

*If the file / settings don't exist, run your game once after installing!*

### Saber Scale Mod Toggle

- **Setting**: `IsSaberScaleModEnabled`
- **Values**: `true` | `false`
- **Default**: `false`

Enables or disables saber scale modifications by SaberTailor. This option *does not* disable score submission.

### SaberScaleHitbox

- **Setting**: `Scale hit-box`
- **Values**: `true` | `false`
- **Default**: `false`

**Enables scaling of the saber hit-box. Having this option set to true *will* disable score submission**

### Saber Length

- **Setting**: `SaberLength`
- **Unit**: Percentage
- **Default**: `100` (100%)
- **Minimum**: `5` (5%)
- **Maximum**: `500` (500%)

Adjust the length of the sabers. This option *does not* disable score submission

### Saber Width

- **Setting**: `SaberGirth`
- **Unit**: Percentage
- **Default**: `100` (100%)
- **Minimum**: `5` (5%)
- **Maximum**: `500` (500%)

Adjust the width of the sabers. This option *does not* disable score submission

### Trail Mod Toggle

- **Setting**: `IsTrailModEnabled`
- **Values**: `true` | `false`
- **Default**: `false`

Enables or disables trail modifications by SaberTailor. This option *does not* disable score submission.

### Trail Toggle

- **Setting**: `IsTrailEnabled`
- **Values**: `true` | `false`
- **Default**: `true`

Allows you to disable the saber trail entirely, when set to `0`. This option *does not* disable score submission.

### Trail Length

- **Setting**: `TrailLength`
- **Unit**: Integer (Whole Number)
- **Default**: `20`
- **Minimum**: `5`
- **Maximum**: `100`

Adjusts the length of the trail on the saber. This option *does not* disable score submission.

### Grip Position (Left + Right)

- **Setting**: `GripLeftPosition`, `GripRightPosition`
- **Unit**: Millimeters
- **Default**: `"x": 0, "y": 0, "z": 0`
- **Maximum**: `500` on any axis

- `+x` moves the saber right, EG: `200` moves the saber 20 centimeters right.
- `+y` moves the saber up, EG: `100` moves the saber 10 centimeters up.
- `+z` moves the saber forward, EG: `300` moves the saber 30 centimeters forward.

Alters the position of the left/right saber, relative to the default location. You cannot move the saber more than 50 centimeters away on any axis! This option *does not* disable score submission.

### Grip Rotation (Left + Right)

- **Setting**: `GripLeftRotation`, `GripRightRotation`
- **Unit**: Degrees (-360 - 360)
- **Default**: `"x": 0, "y": 0, "z": 0`

- `+x` tilts the saber down, EG: `20` tilts the saber 20 degrees down.
- `+y` rotates the saber right, EG: `10` rotates the saber 10 degrees right.
- `+z` rolls the saber counter-clockwise around its own axis, EG: `30` rotates the saber 30 degrees counter-clockwise. (This is only useful if you have custom sabers that are not cylindrical shaped and you want to correct for a different grip (e.g. Vive B-Grip)

Alters the rotation of the sabers. The center of rotation is where the saber's hit-box starts, which is just after the glowing line on the handle. This option *does not* disable score submission.

### Menu hilt adjustments

- **Setting**: `ModifyMenuHiltGrip`
- **Values**: `1` | `0`
- **Default**: `0`

Alters the position and angle of the menu hilts the same way as the sabers, when set to `1`. This option *does not* disable score submission.

### Note!

**The way the configuration is stored has changed.**</br>
The new configuration file is now `\Beat Saber\UserData\SaberTailor.json`

The first time you run the game after version 2.0.0, SaberTailor will attempt to import the old settings.</br>
If for whatever reason something should go wrong, you can still find the old settings in `\Beat Saber\UserData\modprefs.ini`.

## Developers

### Contributing to SaberTailor
In order to build this project, please add a `SaberTailor.csproj.user` file in the project directory and specify where your game is located on your disk:

```xml
<?xml version="1.0" encoding="utf-8"?>
<Project xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <PropertyGroup>
    <!-- Change this path if necessary. Make sure it ends with a backslash. -->
    <GameDirPath>C:\Program Files\Steam\steamapps\common\Beat Saber\</GameDirPath>
  </PropertyGroup>
</Project>
```
