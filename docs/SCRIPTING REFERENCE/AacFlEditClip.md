# AacFlEditClip
*Namespace: AnimatorAsCode.Framework*

## Parameters
| Parameter | Type | Description |
|-|-|-|
| `Clip` | `AnimationClip` | Exposes the underlying clip object. |

## Methods
| Method | Return Type | Description |
|-|-|-|
| `Animates(string path, Type type, string propertyName)` | `AacFlSettingCurve ` | Animates a path the traditional way. |
| `Animates(Transform transform, Type type, string propertyName)` | `AacFlSettingCurve ` | Animates an object in the hierarchy relative to the animator root, the traditional way. |
| `Animates(GameObject gameObject)` | `AacFlSettingCurve ` | Animates the active property of a GameObject, toggling it. |
| `Animates(Component anyComponent, string property)` | `AacFlSettingCurve ` | Animates the enabled property of a component, toggling it. The runtime type of the component will be used. |
| `Animates(Component[] anyComponents, string property)` | `AacFlSettingCurve ` | Animates the enabled property of several components, toggling them. The runtime type of the component will be used. // FIXME: Safety is not provived on nulls. It should probably be, for convenience. |
| `AnimatesAnimator(AacFlParameter floatParameter)` | `AacFlSettingCurve ` | Animates a Float parameter of the animator (may sometimes be referred to as an Animated Animator Parameter, or AAP). |
| `AnimatesColor(Component anyComponent, string property)` | `AacFlSettingCurveColor ` | Animates a color property of a component. The runtime type of the component will be used. |
| `AnimatesColor(Component[] anyComponents, string property)` | `AacFlSettingCurveColor ` | Animates a color property of several components. The runtime type of the component will be used. // FIXME: Safety is not provived on nulls. It should probably be, for convenience. |
| `BindingFromComponent(Component anyComponent, string propertyName)` | `EditorCurveBinding ` | Returns an EditorCurveBinding of a component, relative to the animator root. The runtime type of the component will be used. This is meant to be used in conjunction with traditional animation APIs. |
