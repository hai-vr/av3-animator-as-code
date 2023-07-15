# AacFlSettingCurve
*Namespace: AnimatorAsCode.Framework*

## Methods
| Method | Return Type | Description |
|-|-|-|
| `WithOneFrame(float desiredValue)` | `void ` | Define the curve to be exactly one frame by defining two constant keyframes, usually lasting 1/60th of a second, with the desired value. |
| `WithFixedSeconds(float seconds, float desiredValue)` | `void ` | Define the curve to last a specific amount of seconds by defining two constant keyframes, with the desired value. |
| `WithSecondsUnit(Action<AacFlSettingKeyframes> action)` | `void ` | Start defining the keyframes with a lambda expression, expressing the unit to be in seconds. |
| `WithFrameCountUnit(Action<AacFlSettingKeyframes> action)` | `void ` | Start defining the keyframes with a lambda expression, expressing the unit in frames. |
| `WithUnit(AacFlUnit unit, Action<AacFlSettingKeyframes> action)` | `void ` | Start defining the keyframes with a lambda expression, expressing the unit. |
