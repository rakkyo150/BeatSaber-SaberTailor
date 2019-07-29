# Beat Saber - Saber Tailor v1.6.0

Tweak things about your sabers, including grip position/angle and trail length! Original mod by Ruu.

## Settings

Adjust the different settings in-game from the "Settings" menu.

You can also adjust your settings manually in `\Beat Saber\UserData\SaberTailor.json` (The game will have to be restarted)
The default values are below:

```json
{
  "RegenerateConfig": false,
  "SaberLength": 1.0,
  "SaberGirth": 1.0,
  "IsTrailEnabled": true,
  "TrailLength": 20,
  "GripLeftPosition": {
    "x": 0.0,
    "y": 0.0,
    "z": 0.0
  },
  "GripRightPosition": {
    "x": 0.0,
    "y": 0.0,
    "z": 0.0
  },
  "GripLeftRotation": {
    "x": 0.0,
    "y": 0.0,
    "z": 0.0
  },
  "GripRightRotation": {
    "x": 0.0,
    "y": 0.0,
    "z": 0.0
  },
  "ModifyMenuHiltGrip": false
}
```

*If the file / settings don't exist, run your game once after installing!*

### Saber Length

- **Setting**: `SaberLength`
- **Unit**: Percentage
- **Default**: `1.0` (100%)
- **Minimum**: `0.0` (0%)
- **Maximum**: `5.0` (500%)

**Setting this to anything other than 1 will disable score submissions!**

### Saber Width

- **Setting**: `SaberGirth`
- **Unit**: Percentage
- **Default**: `1.0` (100%)
- **Minimum**: `0.0` (0%)
- **Maximum**: `5.0` (500%)

**Setting this to anything other than 1 will disable score submissions!**

### Trail Toggle

- **Setting**: `IsTrailEnabled`
- **Values**: `1` | `0`
- **Default**: `1`

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
- **Unit**: Centimeters
- **Default**: `"x": 0.0, "y": 0.0, "z": 0.0`
- **Maximum**: `50` on any axis

- `+x` moves the saber right, EG: `20,0,0` moves the saber 20 centimeters right.
- `+y` moves the saber up, EG: `0,10,0` moves the saber 10 centimeters up.
- `+z` moves the saber forward, EG: `0,0,30` moves the saber 30 centimeters forward.

Alters the position of the left/right saber, relative to the default location. You cannot move the saber more than 50 centimeters away on any axis! This option *does not* disable score submission.

### Grip Rotation (Left + Right)

- **Setting**: `GripLeftRotation`, `GripRightRotation`
- **Unit**: Degrees (0 - 360)
- **Default**: `"x": 0.0, "y": 0.0, "z": 0.0`

- `+x` tilts the saber down, EG: `20,0,0` tilts the saber 20 degrees down.
- `+y` rotates the saber right, EG: `0,10,0` rotates the saber 10 degrees right.
- `+z` rolls the saber counter-clockwise around its own axis, EG: `0,0,30` rotates the saber 30 degrees counter-clockwise. (This is only useful if you have custom sabers that are not cylindrical shaped and you want to correct for a different grip (e.g. Vive B-Grip)

Alters the rotation of the sabers. The center of rotation is where the saber's hit-box starts, which is just after the glowing line on the handle. This option *does not* disable score submission.

### Menu hilt adjustments

- **Setting**: `ModifyMenuHiltGrip`
- **Values**: `1` | `0`
- **Default**: `0`

Alters the position and angle of the menu hilts the same way as the sabers, when set to `1`. This option *does not* disable score submission.

### Note!

**The way the configuration is stored has changed.**</br>
The new configuration file is now `\Beat Saber\UserData\SaberTailor.json`

The first time you run the game after version 1.6.0, SaberTailor will attempt to import the old settings.</br>
If for whatever reason something should go wrong, you can still find the old settings in `\Beat Saber\UserData\modprefs.ini`.