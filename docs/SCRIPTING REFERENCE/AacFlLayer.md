# AacFlLayer
*Namespace: AnimatorAsCode.Framework*

## Methods
| Method | Return Type | Description |
|-|-|-|
| `NewState(string name)` | `AacFlState ` | Create a new state, initially positioned below the last generated state of this layer. |
| `NewState(string name, int x, int y)` | `AacFlState ` | Create a new state at a specific position `x` and `y`, in grid units. The grid size is defined in the DefaultsProvider of the AacConfiguration of AAC. `x` positive goes right, `y` positive goes down. |
| `AnyTransitionsTo(AacFlState destination)` | `AacFlTransition ` | Create a transition from Any to the `destination` state. |
| `EntryTransitionsTo(AacFlState destination)` | `AacFlEntryTransition ` | Create a transition from the Entry to the `destination` state. |
| `BoolParameter(string parameterName)` | `AacFlBoolParameter ` | Create a Bool parameter in the animator. |
| `TriggerParameterAsBool(string parameterName)` | `AacFlBoolParameter ` | Create a Trigger parameter in the animator, but returns a Bool parameter for use in AAC. |
| `FloatParameter(string parameterName)` | `AacFlFloatParameter ` | Create a Float parameter in the animator. |
| `IntParameter(string parameterName)` | `AacFlIntParameter ` | Create an Int parameter in the animator. |
| `BoolParameters(params string[] parameterNames)` | `AacFlBoolParameterGroup ` | Create multiple Bool parameters in the animator, and returns a group of multiple Bools. |
| `TriggerParametersAsBools(params string[] parameterNames)` | `AacFlBoolParameterGroup ` | Create multiple Trigger parameters in the animator, but returns a group of multiple Bools for use in AAC. |
| `FloatParameters(params string[] parameterNames)` | `AacFlFloatParameterGroup ` | Create multiple Float parameters in the animator, and returns a group of multiple Bools. |
| `IntParameters(params string[] parameterNames)` | `AacFlIntParameterGroup ` | Create multiple Int parameters in the animator, and returns a group of multiple Bools. |
| `BoolParameters(params AacFlBoolParameter[] parameters)` | `AacFlBoolParameterGroup ` | Combine multiple Int parameters into a group. |
| `TriggerParametersAsBools(params AacFlBoolParameter[] parameters)` | `AacFlBoolParameterGroup ` | Combine multiple Trigger parameters into a group. FIXME: This is a pointless function because BoolParameters already exists. |
| `FloatParameters(params AacFlFloatParameter[] parameters)` | `AacFlFloatParameterGroup ` | Combine multiple Int parameters into a group. |
| `IntParameters(params AacFlIntParameter[] parameters)` | `AacFlIntParameterGroup ` | Combine multiple Int parameters into a group. |
| `Av3()` | `AacAv3 ` | Return an AacAv3 object, letting you select various standard Avatars 3.0 Animator Parameters. Subsequently invoking its functions will create invoked parameters in that layer. |
| `OverrideValue(AacFlBoolParameter toBeForced, bool value)` | `void ` | Set the Bool value of `toBeForced` parameter to `value` in the animator. |
| `OverrideValue(AacFlFloatParameter toBeForced, float value)` | `void ` | Set the Float value of `toBeForced` parameter to `value` in the animator. |
| `OverrideValue(AacFlIntParameter toBeForced, int value)` | `void ` | Set the Int value of `toBeForced` parameter to `value` in the animator. |
| `WithAvatarMask(AvatarMask avatarMask)` | `AacFlLayer ` | Set the Avatar Mask of the layer. |
| `WithAvatarMaskNoTransforms()` | `void ` | Set the Avatar Mask of the layer to be an Avatar Mask which denies all transforms. The asset is generated into the container. |
| `ResolveAvatarMask(Transform[] paths)` | `void ` | Set the Avatar Mask of the layer to be an Avatar Mask that allows the specified transforms. If `paths` is an empty array, all transforms are denied, which is effectively the same as calling `.WithAvatarMaskNoTransforms()`. The asset is generated into the container. |



