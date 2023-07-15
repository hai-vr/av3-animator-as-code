# AacFlState
*Namespace: AnimatorAsCode.Framework*

## Parameters
| Parameter | Type | Description |
|-|-|-|
| `State` | AnimatorState | Exposes the underlying State object. |

## Methods
### Graph Positioning
| Method | Return Type | Description |
|-|-|-|
| `LeftOf(AacFlState otherState)` | `AacFlState ` | Move the state to be left of the other state in the graph. |
| `RightOf(AacFlState otherState)` | `AacFlState ` | Move the state to be right of the other state in the graph. |
| `Over(AacFlState otherState)` | `AacFlState ` | Move the state to be over the other state in the graph. |
| `Under(AacFlState otherState)` | `AacFlState ` | Move the state to be under the other state in the graph. |
| `LeftOf()` | `AacFlState ` | Move the state to be left of the last created state of the state machine this belongs to in the graph. |
| `RightOf()` | `AacFlState ` | Move the state to be right of the last created state of the state machine this belongs to in the graph. |
| `Over()` | `AacFlState ` | Move the state to be over the last created state of the state machine this belongs to in the graph. |
| `Under()` | `AacFlState ` | Move the state to be under the last created state of the state machine this belongs to in the graph. |
| `Shift(AacFlState otherState, int shiftX, int shiftY)` | `AacFlState ` | Move the state to be shifted next to the other state in the graph, in grid units. `shiftX` positive goes right, `shiftY` positive goes down. |
| `Shift(Vector3 otherPosition, int shiftX, int shiftY)` | `AacFlState ` | Move the state to be shifted next to another position in the graph, in grid units. `shiftX` positive goes right, `shiftY` positive goes down. Deprecated for `Shift(Vector2 otherPosition, int shiftX, int shiftY)`|
| `Shift(Vector2 otherPosition, int shiftX, int shiftY)` | `AacFlState ` | Move the state to be shifted next to another position in the graph, in grid units. `shiftX` positive goes right, `shiftY` positive goes down. |

### Attributes
| Method | Return Type | Description |
|-|-|-|
| `WithAnimation(Motion clip)` | `AacFlState ` | Set a specific raw Motion for the state. This could be a blend tree. |
| `WithAnimation(AacFlClip clip)` /// Set a specific clip for the state. See `(AacFlBase).NewClip()` | `AacFlState ` | Set a specific clip for the state. See `(AacFlBase).NewClip()` and similar. |
| `MotionTime(AacFlFloatParameter floatParam)` | `AacFlState ` | Set the Motion Time, formerly known as Normalized Time. |
| `WithSpeed(AacFlFloatParameter parameter)` | `AacFlState ` | Set the Speed. |
| `WithWriteDefaultsSetTo(bool shouldWriteDefaults)` | `AacFlState ` | Set Write Defaults. If you need to do this to many states, consider changing the AacConfiguration DefaultsProvider when creating Animator As Code. |

### Transitions
| Method | Return Type | Description |
|-|-|-|
| `TransitionsTo(AacFlState destination)` | `AacFlTransition ` | Create a new transition from this state to the destination state. |
| `TransitionsFromAny()` | `AacFlTransition ` | Create a new transition from Any to this state. |
| `TransitionsFromEntry()` | `AacFlEntryTransition ` | Create a new transition from Entry to this state. Note that the first created state is the default state, so generally this function does not need to be invoked onto the first created state. Calling this function will not define this state to be the default state. |
| `AutomaticallyMovesTo(AacFlState destination)` | `AacFlState ` | Create a transition with no exit time to the destination state, and does not return the transition. |
| `Exits()` | `AacFlTransition ` | Create a transition from this state to the exit. |

### Avatar Parameter Driver state behaviours
| Method | Return Type | Description |
|-|-|-|
| `Drives(AacFlIntParameter parameter, int value)` | `AacFlState ` | Drive the Int parameter to value. Create an Avatar Parameter Driver state behaviour if it does not exist. |
| `Drives(AacFlFloatParameter parameter, float value)` | `AacFlState ` | Drive the Float parameter to value. Create an Avatar Parameter Driver state behaviour if it does not exist. |
| `DrivingIncreases(AacFlFloatParameter parameter, float additiveValue)` | `AacFlState ` | Drive the Float parameter, incrementing it by `additiveValue`. Create an Avatar Parameter Driver state behaviour if it does not exist. |
| `DrivingDecreases(AacFlFloatParameter parameter, float positiveValueToDecreaseBy)` | `AacFlState ` | Drive the Float parameter, decreasing it by the amount of `positiveValueToDecreaseBy`. Create an Avatar Parameter Driver state behaviour if it does not exist. |
| `DrivingIncreases(AacFlIntParameter parameter, int additiveValue)` | `AacFlState ` | Drive the Int parameter, incrementing it by `additiveValue`. Create an Avatar Parameter Driver state behaviour if it does not exist. |
| `DrivingDecreases(AacFlIntParameter parameter, int positiveValueToDecreaseBy)` | `AacFlState ` | Drive the Int parameter, decreasing it by the amount of `positiveValueToDecreaseBy`. Create an Avatar Parameter Driver state behaviour if it does not exist. |
| `DrivingRandomizesLocally(AacFlFloatParameter parameter, float min, float max)` | `AacFlState ` | Drive the Float parameter value to be random between min and max. Set the driver to be Local only. Create an Avatar Parameter Driver state behaviour if it does not exist. |
| `DrivingRandomizesLocally(AacFlIntParameter parameter, int min, int max)` | `AacFlState ` | Drive the Int parameter value to be random between min and max. Set the driver to be Local only. Create an Avatar Parameter Driver state behaviour if it does not exist. |
| `DrivingRandomizesLocally(AacFlBoolParameter parameter, float chance)` | `AacFlState ` | Drive the Bool parameter value to be random with the specified chance of being true. Set the driver to be Local only. Create an Avatar Parameter Driver state behaviour if it does not exist. |
| `Drives(AacFlBoolParameter parameter, bool value)` | `AacFlState ` | Drive the Bool parameter to value. Create an Avatar Parameter Driver state behaviour if it does not exist. |
| `Drives(AacFlBoolParameterGroup parameters, bool value)` | `AacFlState ` | Drive the Bool parameter to value. Create an Avatar Parameter Driver state behaviour if it does not exist. |
| `DrivingLocally()` | `AacFlState ` | Set the driver to be Local only. Create an Avatar Parameter Driver state behaviour if it does not exist. |
| `DrivingCopies(AacFlIntParameter source, AacFlIntParameter destination)` | `AacFlState ` | Drive the destination Int parameter to be the same as the source Int parameter. Create an Avatar Parameter Driver state behaviour if it does not exist. |
| `DrivingCopies(AacFlFloatParameter source, AacFlFloatParameter destination)` | `AacFlState ` | Drive the destination Float parameter to be the same as the source Float parameter. Create an Avatar Parameter Driver state behaviour if it does not exist. |
| `DrivingCopies(AacFlBoolParameter source, AacFlBoolParameter destination)` | `AacFlState ` | Drive the destination Bool parameter to be the same as the source Bool parameter. Create an Avatar Parameter Driver state behaviour if it does not exist. |
| `DrivingRemaps(AacFlBoolParameter source, float sourceMin, float sourceMax, AacFlBoolParameter destination, float destinationMin, float destinationMax)` | `AacFlState ` | Drive the destination Bool parameter to be the same as the source Bool parameter. Create an Avatar Parameter Driver state behaviour if it does not exist. |
| `DrivingCasts(AacFlFloatParameter source, AacFlIntParameter destination)` | `AacFlState ` | Drive the destination Int parameter to be the same as the source Float parameter. Create an Avatar Parameter Driver state behaviour if it does not exist. |
| `DrivingCasts(AacFlIntParameter source, AacFlFloatParameter destination)` | `AacFlState ` | Drive the destination Float parameter to be the same as the source Int parameter. Create an Avatar Parameter Driver state behaviour if it does not exist. |
| `DrivingCasts(AacFlBoolParameter source, AacFlFloatParameter destination)` | `AacFlState ` | Drive the destination Float parameter to be the same as the source Bool parameter. Create an Avatar Parameter Driver state behaviour if it does not exist. |
| `DrivingCasts(AacFlFloatParameter source, AacFlBoolParameter destination)` | `AacFlState ` | Drive the destination Bool parameter to be the same as the source Float parameter. Create an Avatar Parameter Driver state behaviour if it does not exist. |
| `DrivingCasts(AacFlBoolParameter source, AacFlIntParameter destination)` | `AacFlState ` | Drive the destination Int parameter to be the same as the source Bool parameter. Create an Avatar Parameter Driver state behaviour if it does not exist. |
| `DrivingCasts(AacFlIntParameter source, AacFlBoolParameter destination)` | `AacFlState ` | Drive the destination Bool parameter to be the same as the source Int parameter. Create an Avatar Parameter Driver state behaviour if it does not exist. |
| `DrivingCasts(AacFlBoolParameter source, float sourceMin, float sourceMax, AacFlFloatParameter destination, float destinationMin, float destinationMax)` | `AacFlState ` | Drive the destination Float parameter to be the same as the source Bool parameter, but remapped from sourceMin to sourceMax to destinationMin to destinationMax. Create an Avatar Parameter Driver state behaviour if it does not exist. |
| `DrivingCasts(AacFlFloatParameter source, float sourceMin, float sourceMax, AacFlBoolParameter destination, float destinationMin, float destinationMax)` | `AacFlState ` | Drive the destination Bool parameter to be the same as the source Float parameter, but remapped from sourceMin to sourceMax to destinationMin to destinationMax. Create an Avatar Parameter Driver state behaviour if it does not exist. |
| `DrivingCasts(AacFlBoolParameter source, float sourceMin, float sourceMax, AacFlIntParameter destination, int destinationMin, int destinationMax)` | `AacFlState ` | Drive the destination Int parameter to be the same as the source Bool parameter, but remapped from sourceMin to sourceMax to destinationMin to destinationMax. Create an Avatar Parameter Driver state behaviour if it does not exist. |
| `DrivingCasts(AacFlIntParameter source, int sourceMin, int sourceMax, AacFlBoolParameter destination, float destinationMin, float destinationMax)` | `AacFlState ` | Drive the destination Bool parameter to be the same as the source Int parameter, but remapped from sourceMin to sourceMax to destinationMin to destinationMax. Create an Avatar Parameter Driver state behaviour if it does not exist. |

### Other state behaviours
| Method | Return Type | Description |
|-|-|-|
| `PrintsToLogUsingTrackingBehaviour(string value)` | `AacFlState ` | Use an Animator Tracking Control to print logs to the avatar wearer. Create an Animator Tracking Control state behaviour if it does not exist. |
| `TrackingTracks(TrackingElement element)` | `AacFlState ` | Use an Animator Tracking Control to set the element to be tracking. Create an Animator Tracking Control state behaviour if it does not exist. |
| `TrackingAnimates(TrackingElement element)` | `AacFlState ` | Use an Animator Tracking Control to set the element to be animating. Create an Animator Tracking Control state behaviour if it does not exist. |
| `TrackingSets(TrackingElement element, VRC_AnimatorTrackingControl.TrackingType trackingType)` | `AacFlState ` | Use an Animator Tracking Control to set the element to be the value of `trackingType`. Create an Animator Tracking Control state behaviour if it does not exist. |
| `LocomotionEnabled()` | `AacFlState ` | Enable locomotion. Create an Animator Locomotion Control if it does not exist. |
| `LocomotionDisabled()` | `AacFlState ` | Disable locomotion. Create an Animator Locomotion Control if it does not exist. |
