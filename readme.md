# Beat Saber - Saber Tailor

Tweak things about your sabers, including grip position/angle, saber length and trail length! Original mod by Ruu. Current features include:

- Change the position and angle of your saber relative to your controller (independently for left/right saber)
- Change the position of your saber relative to the world (independently for left/right saber)
- Import/Export your current game settings into/from SaberTailor
- Scale the length and width of your saber
  -  optional including the hitbox (enabling this option will disable score submission)
- Enable/Disable saber trail and trail whitestep
- Change trail length
- Profile support (save to/load from different profiles files)

## Note on updates from SaberTailor older than v2.0.0!

**The way the configuration is stored has changed.**</br>
The new configuration file is now `\Beat Saber\UserData\SaberTailor.json`

The first time you run the game after version 2.0.0, SaberTailor will attempt to import the old settings.</br>
If for whatever reason something should go wrong, you can still find the old settings in `\Beat Saber\UserData\modprefs.ini`.</br>
If you want to import old settings again, remove the line `IsExportedToNewConfig=1` from the `[SaberTailor]` section in `\Beat Saber\UserData\modprefs.ini` file.

Please note that grip position adjustments are now saved in millimeters. These were saved in centimeters in the old configuration file. As such, the saved values in `SaberTailor.json` are an order of magnitude higher than before.

## Settings

Adjust the different settings in-game from the "Mod Settings" menu.

You can also adjust your settings manually in `\Beat Saber\UserData\SaberTailor.json` (The game will have to be restarted)
The default values are below:

```json
{
  "ConfigVersion": 5,
  "IsSaberScaleModEnabled": false,
  "SaberScaleHitbox": false,
  "SaberLength": 100,
  "SaberGirth": 100,
  "IsTrailModEnabled": false,
  "IsTrailEnabled": true,
  "TrailDuration": 400,
  "TrailGranularity": 60,
  "TrailWhiteSectionDuration": 100,
  "IsGripModEnabled": true,  
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
  "GripLeftOffset": {
    "x": 0,
    "y": 0,
    "z": 0
  },
  "GripRightOffset": {
    "x": 0,
    "y": 0,
    "z": 0
  },
  "ModifyMenuHiltGrip": true,
  "UseBaseGameAdjustmentMode": false,
  "SaberPosIncrement": 10,
  "SaberPosIncValue": 1,
  "SaberRotIncrement": 5,
  "SaberPosIncUnit": "cm",
  "SaberPosDisplayUnit": "cm"
}
```

*If the file / settings don't exist, run your game once after installing!*

### Saber Scale Mod Toggle

- **Setting**: `IsSaberScaleModEnabled`
- **Values**: `true` | `false`
- **Default**: `false`

Enables or disables saber scale modifications by SaberTailor.

### SaberScaleHitbox

- **Setting**: `Scale hit-box`
- **Values**: `true` | `false`
- **Default**: `false`

Enables scaling of the saber hit-box. **Having this option set to true *will* disable score submission**

### Saber Length

- **Setting**: `SaberLength`
- **Unit**: Percentage
- **Default**: `100` (100%)
- **Minimum**: `5` (5%)
- **Maximum**: `500` (500%)

Adjust the length of the sabers.

### Saber Width

- **Setting**: `SaberGirth`
- **Unit**: Percentage
- **Default**: `100` (100%)
- **Minimum**: `5` (5%)
- **Maximum**: `500` (500%)

Adjust the width of the sabers.

### Trail Mod Toggle

- **Setting**: `IsTrailModEnabled`
- **Values**: `true` | `false`
- **Default**: `false`

Enables or disables trail modifications by SaberTailor.

### Trail Toggle

- **Setting**: `IsTrailEnabled`
- **Values**: `true` | `false`
- **Default**: `true`

Allows you to disable the saber trail entirely, when set to `false`.

### Trail Duration

- **Setting**: `TrailDuration`
- **Unit**: Milliseconds
- **Default**: `400` (0.4s)
- **Minimum**: `100` (0.1s)
- **Maximum**: `5000` (5s)

Adjusts the duration of the colored part of the trail on the saber in Milliseconds.

### Trail Granularity

- **Setting**: `TrailGranularity`
- **Unit**: Integer (Whole Number)
- **Default**: `60`
- **Minimum**: `10`
- **Maximum**: `200`

Adjusts the amount of segments of the trail.

### Trail White Section Duration

- **Setting**: `TrailWhiteSectionDuration`
- **Unit**: Milliseconds
- **Default**: `100` (0.1s)
- **Minimum**: `0` (0.0s)
- **Maximum**: `2000` (2s)

Adjusts the duration of the bright white part of the trail on the saber in Milliseconds.

### Grip Modification

- **Setting**: `IsGripModEnabled`
- **Values**: `true` | `false`
- **Default**: `false`

Controls if the base game grip adjustments or SaberTailor grip adjustments are active. Setting this to `true` enables SablerTailor, which overrides base game settings.

### Grip Position (Left + Right)

- **Setting**: `GripLeftPosition`, `GripRightPosition`
- **Unit**: Millimeters
- **Default**: `"x": 0, "y": 0, "z": 0`
- **Maximum**: `500` on any axis

- `+x` moves the saber right, EG: `200` moves the saber 20 centimeters right.
- `+y` moves the saber up, EG: `100` moves the saber 10 centimeters up.
- `+z` moves the saber forward, EG: `300` moves the saber 30 centimeters forward.

Alters the position of the left/right saber, relative to the default location. You cannot move the saber more than 50 centimeters away on any axis!

### Grip Rotation (Left + Right)

- **Setting**: `GripLeftRotation`, `GripRightRotation`
- **Unit**: Degrees (-360 - 360)
- **Default**: `"x": 0, "y": 0, "z": 0`

- `+x` tilts the saber down, EG: `20` tilts the saber 20 degrees down.
- `+y` rotates the saber right, EG: `10` rotates the saber 10 degrees right.
- `+z` rolls the saber counter-clockwise around its own axis, EG: `30` rotates the saber 30 degrees counter-clockwise. (This is only useful if you have custom sabers that are not cylindrical shaped and you want to correct for a different grip (e.g. Vive B-Grip)

Alters the rotation of the sabers. The center of rotation is where the saber's hit-box starts, which is just after the glowing line on the handle.

### Grip Offset (Left + Right)

- **Setting**: `GripLeftOffset`, `GripRightOffset`
- **Unit**: Millimeters
- **Default**: `"x": 0, "y": 0, "z": 0`
- **Maximum**: `500` on any axis

- `+x` moves the controller right, EG: `200` moves the controller 20 centimeters right.
- `+y` moves the controller up, EG: `100` moves the controller 10 centimeters up.
- `+z` moves the controller forward, EG: `300` moves the controller 30 centimeters forward.

Will simulate moving your physical controller location in case of drifts in the default position. You cannot move the controller more than 50 centimeters away on any axis!

### Menu hilt adjustments

- **Setting**: `ModifyMenuHiltGrip`
- **Values**: `true` | `false`
- **Default**: `true`

Alters the position and angle of the menu hilts the same way as the sabers, when set to `true`.

### Grip Adjustment Mode

- **Setting**: `UseBaseGameAdjustmentMode`
- **Values**: `true` | `false`
- **Default**: `true`

Controls how the saber position/rotation is being altered. When this is set to `true`, SaberTailor will mimic the adjustment mode of the base game. Setting this to `false` will use the old 'classic' SaberTailor adjustment mode, which differs a bit. 

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
