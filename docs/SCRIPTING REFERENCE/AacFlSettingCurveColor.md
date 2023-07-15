# AacFlSettingCurveColor
*Namespace: AnimatorAsCode.Framework*

## Methods
| Method | Return Type | Description |
|-|-|-|
| `WithOneFrame(Color desiredValue)` | `void ` | Define the curve to be exactly one frame by defining two constant keyframes, usually lasting 1/60th of a second, with the desired color value. |
| `WithKeyframes(AacFlUnit unit, Action<AacFlSettingKeyframesColor> action)` | `void ` | Start defining the keyframes with a lambda expression, expressing the unit. |
