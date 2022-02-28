## Animator As Code (AacV0)
- `static AacFlBase Create(AacConfiguration configuration)` /// Create an Animator As Code (AAC) base.

# Base (AacFlBase)
- `AacFlClip NewClip()` /// Create a new clip. The asset is generated into the container.
- `AacFlClip CopyClip(AnimationClip originalClip)` /// Create a new clip that is a copy of `originalClip`. The asset is generated into the container.
- `BlendTree NewBlendTreeAsRaw()` /// Create a new BlendTree asset. The asset is generated into the container.
- `AacFlClip NewClip(string name)` /// Create a new clip with a name. However, the name is only used as a suffix for the asset. The asset is generated into the container. FIXME: This is quite pointless because the name is mangled anyways.
- `AacFlClip DummyClipLasting(float numberOf, AacFlUnit unit)` /// Create a new clip which animates a dummy transform for a specific duration specified in an unit (Frames or Seconds).
- `void RemoveAllMainLayers()` /// Remove all main layers matching that system from all animators of the Avatar descriptor.
- `void RemoveAllSupportingLayers(string suffix)` /// Remove all supporting layers matching that system and suffix from all animators of the Avatar descriptor.
- `AacFlLayer CreateMainFxLayer()` /// Create the main Fx layer of that system, clearing the previous one of the same system. You are not obligated to have a main layer.
- `AacFlLayer CreateMainGestureLayer()` /// Create the main Gesture layer of that system, clearing the previous one of the same system. You are not obligated to have a main layer.
- `AacFlLayer CreateMainActionLayer()` /// Create the main Action layer of that system, clearing the previous one of the same system. You are not obligated to have a main layer.
- `AacFlLayer CreateMainIdleLayer()` /// Create the main Idle layer of that system, clearing the previous one of the same system. You are not obligated to have a main layer.
- `AacFlLayer CreateMainLocomotionLayer()` /// Create the main Locomotion layer of that system, clearing the previous one of the same system. You are not obligated to have a main layer.
- `AacFlLayer CreateMainAv3Layer(VRCAvatarDescriptor.AnimLayerType animLayerType)` /// Create the main layer of that system for a specific type of layer, clearing the previous one of the same system. You are not obligated to have a main layer.
- `AacFlLayer CreateSupportingFxLayer(string suffix)` /// Create a supporting Fx layer for that system and suffix, clearing the previous one of the same system and suffix. You can create multiple supporting layers with different suffixes, and you are not obligated to have a main layer to create a supporting layer.
- `AacFlLayer CreateSupportingGestureLayer(string suffix)` /// Create a supporting Gesture layer for that system and suffix, clearing the previous one of the same system and suffix. You can create multiple supporting layers with different suffixes, and you are not obligated to have a main layer to create a supporting layer.
- `AacFlLayer CreateSupportingActionLayer(string suffix)` /// Create a supporting Action layer for that system and suffix, clearing the previous one of the same system and suffix. You can create multiple supporting layers with different suffixes, and you are not obligated to have a main layer to create a supporting layer.
- `AacFlLayer CreateSupportingIdleLayer(string suffix)` /// Create a supporting Idle layer for that system and suffix, clearing the previous one of the same system and suffix. You can create multiple supporting layers with different suffixes, and you are not obligated to have a main layer to create a supporting layer.
- `AacFlLayer CreateSupportingLocomotionLayer(string suffix)` /// Create a supporting Locomotion layer for that system and suffix, clearing the previous one of the same system and suffix. You can create multiple supporting layers with different suffixes, and you are not obligated to have a main layer to create a supporting layer.
- `AacFlLayer CreateSupportingAv3Layer(VRCAvatarDescriptor.AnimLayerType animLayerType, string suffix)` /// Create a supporting layer for that system and suffix, clearing the previous one of the same system and suffix. You can create multiple supporting layers with different suffixes, and you are not obligated to have a main layer to create a supporting layer.
- `AacFlLayer CreateMainArbitraryControllerLayer(AnimatorController controller)` /// Create a main layer for an arbitrary AnimatorController, clearing the previous one of the same system. You are not obligated to have a main layer.
- `AacFlLayer CreateSupportingArbitraryControllerLayer(AnimatorController controller, string suffix)` /// Create a supporting layer for an arbitrary AnimatorController, clearing the previous one of the same system and suffix. You can create multiple supporting layers with different suffixes, and you are not obligated to have a main layer to create a supporting layer.
- `AacFlLayer CreateFirstArbitraryControllerLayer(AnimatorController controller)` /// Clears the topmost layer of an arbitrary AnimatorController, and returns it.
- `AacVrcAssetLibrary VrcAssets()` /// Return an AacVrcAssetLibrary, which lets you select various assets from VRChat.
- `void ClearPreviousAssets()` /// Removes all assets from the asset container matching the specified asset key.

# Layer (AacFlLayer)
- `AacFlState NewState(string name)` /// Create a new state, initially positioned below the last generated state of this layer.
- `AacFlState NewState(string name, int x, int y)` /// Create a new state at a specific position `x` and `y`, in grid units. The grid size is defined in the DefaultsProvider of the AacConfiguration of AAC. `x` positive goes right, `y` positive goes down.
- `AacFlTransition AnyTransitionsTo(AacFlState destination)` /// Create a transition from Any to the `destination` state.
- `AacFlEntryTransition EntryTransitionsTo(AacFlState destination)` /// Create a transition from the Entry to the `destination` state.
- `AacFlBoolParameter BoolParameter(string parameterName)` /// Create a Bool parameter in the animator.
- `AacFlBoolParameter TriggerParameterAsBool(string parameterName)` /// Create a Trigger parameter in the animator, but returns a Bool parameter for use in AAC.
- `AacFlFloatParameter FloatParameter(string parameterName)` /// Create a Float parameter in the animator.
- `AacFlIntParameter IntParameter(string parameterName)` /// Create an Int parameter in the animator.
- `AacFlBoolParameterGroup BoolParameters(params string[] parameterNames)` /// Create multiple Bool parameters in the animator, and returns a group of multiple Bools.
- `AacFlBoolParameterGroup TriggerParametersAsBools(params string[] parameterNames)` /// Create multiple Trigger parameters in the animator, but returns a group of multiple Bools for use in AAC.
- `AacFlFloatParameterGroup FloatParameters(params string[] parameterNames)` /// Create multiple Float parameters in the animator, and returns a group of multiple Bools.
- `AacFlIntParameterGroup IntParameters(params string[] parameterNames)` /// Create multiple Int parameters in the animator, and returns a group of multiple Bools.
- `AacFlBoolParameterGroup BoolParameters(params AacFlBoolParameter[] parameters)` /// Combine multiple Int parameters into a group.
- `AacFlBoolParameterGroup TriggerParametersAsBools(params AacFlBoolParameter[] parameters)` /// Combine multiple Trigger parameters into a group. FIXME: This is a pointless function because BoolParameters already exists.
- `AacFlFloatParameterGroup FloatParameters(params AacFlFloatParameter[] parameters)` /// Combine multiple Int parameters into a group.
- `AacFlIntParameterGroup IntParameters(params AacFlIntParameter[] parameters)` /// Combine multiple Int parameters into a group.
- `AacAv3 Av3()` /// Return an AacAv3 object, letting you select various standard Avatars 3.0 Animator Parameters. Subsequently invoking its functions will create invoked parameters in that layer.
- `void OverrideValue(AacFlBoolParameter toBeForced, bool value)` /// Set the Bool value of `toBeForced` parameter to `value` in the animator.
- `void OverrideValue(AacFlFloatParameter toBeForced, float value)` /// Set the Float value of `toBeForced` parameter to `value` in the animator.
- `void OverrideValue(AacFlIntParameter toBeForced, int value)` /// Set the Int value of `toBeForced` parameter to `value` in the animator.
- `AacFlLayer WithAvatarMask(AvatarMask avatarMask)` /// Set the Avatar Mask of the layer.
- `void WithAvatarMaskNoTransforms()` /// Set the Avatar Mask of the layer to be an Avatar Mask which denies all transforms. The asset is generated into the container.
- `void ResolveAvatarMask(Transform[] paths)` /// Set the Avatar Mask of the layer to be an Avatar Mask that allows the specified transforms. If `paths` is an empty array, all transforms are denied, which is effectively the same as calling `.WithAvatarMaskNoTransforms()`. The asset is generated into the container.

# Avatars 3.0 (AacAv3)

## Parameters

- `AacFlBoolParameter IsLocal` -> Bool /// Create a Bool parameter in the animator named IsLocal.
- `AacFlEnumIntParameter<Av3Viseme> Viseme` /// Create an Int parameter in the animator named Viseme.
- `AacFlEnumIntParameter<Av3Gesture> GestureLeft` /// Create an Int parameter in the animator named GestureLeft.
- `AacFlEnumIntParameter<Av3Gesture> GestureRight` /// Create an Int parameter in the animator named GestureRight.
- `AacFlFloatParameter GestureLeftWeight` -> Float /// Create a Float parameter in the animator named GestureLeftWeight.
- `AacFlFloatParameter GestureRightWeight` -> Float /// Create a Float parameter in the animator named GestureRightWeight.
- `AacFlFloatParameter AngularY` -> Float /// Create a Float parameter in the animator named AngularY.
- `AacFlFloatParameter VelocityX` -> Float /// Create a Float parameter in the animator named VelocityX.
- `AacFlFloatParameter VelocityY` -> Float /// Create a Float parameter in the animator named VelocityY.
- `AacFlFloatParameter VelocityZ` -> Float /// Create a Float parameter in the animator named VelocityZ.
- `AacFlFloatParameter Upright` -> Float /// Create a Float parameter in the animator named Upright.
- `AacFlBoolParameter Grounded` -> Bool /// Create a Bool parameter in the animator named Grounded.
- `AacFlBoolParameter Seated` -> Bool /// Create a Bool parameter in the animator named Seated.
- `AacFlBoolParameter AFK` -> Bool /// Create a Bool parameter in the animator named AFK.
- `AacFlIntParameter TrackingType` -> Int /// Create an Int parameter in the animator named TrackingType.
- `AacFlIntParameter VRMode` -> Int /// Create an Int parameter in the animator named VRMode.
- `AacFlBoolParameter MuteSelf` -> Bool /// Create a Bool parameter in the animator named MuteSelf.
- `AacFlBoolParameter InStation` -> Bool /// Create a Bool parameter in the animator named InStation.
- `AacFlFloatParameter Voice` -> Float /// Create a Float parameter in the animator named Voice.

## Supporting conditions

- `IAacFlCondition ItIsRemote()` /// Verify that IsLocal is false. Create a Bool parameter in the animator named IsLocal.
- `IAacFlCondition ItIsLocal()` /// Verify that IsLocal is true. Create a Bool parameter in the animator named IsLocal.

# Asset Library (AacVrcAssetLibrary)

- `AvatarMask LeftHandAvatarMask()` /// Left Hand avatar mask asset.
- `AvatarMask RightHandAvatarMask()` /// Right Hand avatar mask asset.
- `AnimationClip ProxyForGesture(AacAv3.Av3Gesture gesture, bool masculine)` /// Gesture proxy animation asset. There are two idle animations for the neutral hand, the bool selects one or the other.

# State (AacFlState)

- `AnimatorState State;` /// Expose the underlying State object.

## Graph Positioning

- `AacFlState LeftOf(AacFlState otherState)` /// Move the state to be left of the other state in the graph.
- `AacFlState RightOf(AacFlState otherState)` /// Move the state to be right of the other state in the graph.
- `AacFlState Over(AacFlState otherState)` /// Move the state to be over the other state in the graph.
- `AacFlState Under(AacFlState otherState)` /// Move the state to be under the other state in the graph.
- `AacFlState LeftOf()` /// Move the state to be left of the last created state of the state machine this belongs to in the graph.
- `AacFlState RightOf()` /// Move the state to be right of the last created state of the state machine this belongs to in the graph.
- `AacFlState Over()` /// Move the state to be over the last created state of the state machine this belongs to in the graph.
- `AacFlState Under()` /// Move the state to be under the last created state of the state machine this belongs to in the graph.
- `AacFlState Shift(AacFlState otherState, int shiftX, int shiftY)` /// Move the state to be shifted next to the other state in the graph, in grid units. `shiftX` positive goes right, `shiftY` positive goes down.
- `AacFlState Shift(Vector3 otherPosition, int shiftX, int shiftY)` /// Move the state to be shifted next to another position in the graph, in grid units. `shiftX` positive goes right, `shiftY` positive goes down. // FIXME: Vector3 is really odd as a type.

## Attributes

- `AacFlState WithAnimation(Motion clip)` /// Set a specific raw Motion for the state. This could be a blend tree.
- `AacFlState WithAnimation(AacFlClip clip)` /// Set a specific clip for the state. See `(AacFlBase).NewClip()` and similar.
- `AacFlState MotionTime(AacFlFloatParameter floatParam)` /// Set the Motion Time, formerly known as Normalized Time.
- `AacFlState WithSpeed(AacFlFloatParameter parameter)` /// Set the Speed.
- `AacFlState WithWriteDefaultsSetTo(bool shouldWriteDefaults)` /// Set Write Defaults. If you need to do this to many states, consider changing the AacConfiguration DefaultsProvider when creating Animator As Code.

## Transitions

- `AacFlTransition TransitionsTo(AacFlState destination)` /// Create a new transition from this state to the destination state.
- `AacFlTransition TransitionsFromAny()` /// Create a new transition from Any to this state.
- `AacFlEntryTransition TransitionsFromEntry()` /// Create a new transition from Entry to this state. Note that the first created state is the default state, so generally this function does not need to be invoked onto the first created state. Calling this function will not define this state to be the default state.
- `AacFlState AutomaticallyMovesTo(AacFlState destination)` /// Create a transition with no exit time to the destination state, and does not return the transition.
- `AacFlTransition Exits()` /// Create a transition from this state to the exit.

## Avatar Parameter Driver state behaviour

- `AacFlState Drives(AacFlIntParameter parameter, int value)` /// Drive the Int parameter to value. Create an Avatar Parameter Driver state behaviour if it does not exist.
- `AacFlState Drives(AacFlFloatParameter parameter, float value)` /// Drive the Float parameter to value. Create an Avatar Parameter Driver state behaviour if it does not exist.
- `AacFlState DrivingIncreases(AacFlFloatParameter parameter, float additiveValue)` /// Drive the Float parameter, incrementing it by `additiveValue`. Create an Avatar Parameter Driver state behaviour if it does not exist.
- `AacFlState DrivingDecreases(AacFlFloatParameter parameter, float positiveValueToDecreaseBy)` /// Drive the Float parameter, decreasing it by the amount of `positiveValueToDecreaseBy`. Create an Avatar Parameter Driver state behaviour if it does not exist.
- `AacFlState DrivingIncreases(AacFlIntParameter parameter, float additiveValue)` /// Drive the Int parameter, incrementing it by `additiveValue`. Create an Avatar Parameter Driver state behaviour if it does not exist.
- `AacFlState DrivingDecreases(AacFlIntParameter parameter, float positiveValueToDecreaseBy)` /// Drive the Int parameter, decreasing it by the amount of `positiveValueToDecreaseBy`. Create an Avatar Parameter Driver state behaviour if it does not exist.
- `AacFlState DrivingRandomizesLocally(AacFlFloatParameter parameter, float min, float max)` /// Drive the Float parameter value to be random between min and max. Set the driver to be Local only. Create an Avatar Parameter Driver state behaviour if it does not exist.
- `AacFlState DrivingRandomizesLocally(AacFlIntParameter parameter, int min, int max)` /// Drive the Int parameter value to be random between min and max. Set the driver to be Local only. Create an Avatar Parameter Driver state behaviour if it does not exist.
- `AacFlState DrivingRandomizesLocally(AacFlBoolParameter parameter, float chance)` /// Drive the Bool parameter value to be random with the specified chance of being true. Set the driver to be Local only. Create an Avatar Parameter Driver state behaviour if it does not exist.
- `AacFlState Drives(AacFlBoolParameter parameter, bool value)` /// Drive the Bool parameter to value. Create an Avatar Parameter Driver state behaviour if it does not exist.
- `AacFlState Drives(AacFlBoolParameterGroup parameters, bool value)` /// Drive the Bool parameter to value. Create an Avatar Parameter Driver state behaviour if it does not exist.
- `AacFlState DrivingLocally()` /// Set the driver to be Local only. Create an Avatar Parameter Driver state behaviour if it does not exist.

## Other state behaviours

- `AacFlState PrintsToLogUsingTrackingBehaviour(string value)` /// Use an Animator Tracking Control to print logs to the avatar wearer. Create an Animator Tracking Control state behaviour if it does not exist.
- `AacFlState TrackingTracks(TrackingElement element)` /// Use an Animator Tracking Control to set the element to be tracking. Create an Animator Tracking Control state behaviour if it does not exist.
- `AacFlState TrackingAnimates(TrackingElement element)` /// Use an Animator Tracking Control to set the element to be animating. Create an Animator Tracking Control state behaviour if it does not exist.
- `AacFlState TrackingSets(TrackingElement element, VRC_AnimatorTrackingControl.TrackingType trackingType)` /// Use an Animator Tracking Control to set the element to be the value of `trackingType`. Create an Animator Tracking Control state behaviour if it does not exist.
- `AacFlState LocomotionEnabled()` /// Enable locomotion. Create an Animator Locomotion Control if it does not exist.
- `AacFlState LocomotionDisabled()` /// Disable locomotion. Create an Animator Locomotion Control if it does not exist.

# Clip (AacFlClip)

- `AnimationClip Clip;`  /// Expose the underlying Clip object.

## Attributes

- `AacFlClip Looping()` /// Set the clip to be looping.
- `AacFlClip NonLooping()` /// Set the clip to be non-looping.

## Single-frame Animations

- `AacFlClip Toggling(GameObject[] gameObjectsWithNulls, bool value)` /// Enable or disable GameObjects. This lasts one frame. The array can safely contain null values.
- `AacFlClip BlendShape(SkinnedMeshRenderer renderer, string blendShapeName, float value)` /// Change a blendShape of a skinned mesh. This lasts one frame.
- `AacFlClip BlendShape(SkinnedMeshRenderer[] rendererWithNulls, string blendShapeName, float value)` /// Change a blendShape of multiple skinned meshes. This lasts one frame. The array can safely contain null values.
- `AacFlClip Scaling(GameObject[] gameObjectsWithNulls, Vector3 scale)` /// Scale GameObjects. This lasts one frame. The array can safely contain null values. // FIXME: This is weird, this should be a Transform array, and also this needs a single-object overload.
- `AacFlClip Toggling(GameObject gameObject, bool value)` ///  Enable or disable a GameObject. This lasts one frame.
- `AacFlClip TogglingComponent(Component[] componentsWithNulls, bool value)` /// Toggle several components. This lasts one frame. The runtime type of each individual component will be used. The array can safely contain null values.
- `AacFlClip TogglingComponent(Component component, bool value)` /// Toggle a component. This lasts one frame. The runtime type of the component will be used.
- `AacFlClip SwappingMaterial(Renderer renderer, int slot, Material material)` /// Swap a material of a Renderer on the specified slot (indexed at 0). This lasts one frame.
- `AacFlClip SwappingMaterial(ParticleSystem particleSystem, int slot, Material material)` /// Swap a material of a Particle System on the specified slot (indexed at 0). This lasts one frame. This effectively takes the ParticleSystemRenderer of the component.

## Clip Editing

- `AacFlClip Animating(Action<AacFlEditClip> action)` /// Start editing the clip with a lambda expression.

# Clip editing (AacFlEditClip)

- `AnimationClip Clip;`  /// Expose the underlying Clip object.

- `AacFlSettingCurve Animates(string path, Type type, string propertyName)` /// Animates a path the traditional way.
- `AacFlSettingCurve Animates(Transform transform, Type type, string propertyName)` /// Animates an object in the hierarchy relative to the animator root, the traditional way.
- `AacFlSettingCurve Animates(GameObject gameObject)` /// Animates the active property of a GameObject, toggling it.
- `AacFlSettingCurve Animates(Component anyComponent, string property)` /// Animates the enabled property of a component, toggling it. The runtime type of the component will be used.
- `AacFlSettingCurve Animates(Component[] anyComponents, string property)` /// Animates the enabled property of several components, toggling them. The runtime type of the component will be used. // FIXME: Safety is not provived on nulls. It should probably be, for convenience.
- `AacFlSettingCurve AnimatesAnimator(AacFlParameter floatParameter)` /// Animates a Float parameter of the animator (may sometimes be referred to as an Animated Animator Parameter, or AAP).
- `AacFlSettingCurveColor AnimatesColor(Component anyComponent, string property)` /// Animates a color property of a component. The runtime type of the component will be used.
- `AacFlSettingCurveColor AnimatesColor(Component[] anyComponents, string property)` /// Animates a color property of several components. The runtime type of the component will be used. // FIXME: Safety is not provived on nulls. It should probably be, for convenience.
- `EditorCurveBinding BindingFromComponent(Component anyComponent, string propertyName)` /// Returns an EditorCurveBinding of a component, relative to the animator root. The runtime type of the component will be used. This is meant to be used in conjunction with traditional animation APIs.

# Curve of type Float (AacFlSettingCurve)
- `void WithOneFrame(float desiredValue)` /// Define the curve to be exactly one frame by defining two constant keyframes, usually lasting 1/60th of a second, with the desired value.
- `void WithFixedSeconds(float seconds, float desiredValue)` /// Define the curve to last a specific amount of seconds by defining two constant keyframes, with the desired value.
- `void WithSecondsUnit(Action<AacFlSettingKeyframes> action)` /// Start defining the keyframes with a lambda expression, expressing the unit to be in seconds.
- `void WithFrameCountUnit(Action<AacFlSettingKeyframes> action)` /// Start defining the keyframes with a lambda expression, expressing the unit in frames.
- `void WithUnit(AacFlUnit unit, Action<AacFlSettingKeyframes> action)` /// Start defining the keyframes with a lambda expression, expressing the unit.

# Curve of type Color (AacFlSettingCurveColor)

- `void WithOneFrame(Color desiredValue)` /// Define the curve to be exactly one frame by defining two constant keyframes, usually lasting 1/60th of a second, with the desired color value.
- `void WithKeyframes(AacFlUnit unit, Action<AacFlSettingKeyframesColor> action)` /// Start defining the keyframes with a lambda expression, expressing the unit.

# Keyframes of type Float (AacFlSettingKeyframes)
- `AacFlSettingKeyframes Easing(float timeInUnit, float value)` /// Create a curved spline keyframe. The unit is defined by the function that invokes this lambda expression.
- `AacFlSettingKeyframes Linear(float timeInUnit, float value)` /// Create a linear keyframe. The unit is defined by the function that invokes this lambda expression.
- `AacFlSettingKeyframes Constant(float timeInUnit, float value)` /// Create a constant keyframe. The unit is defined by the function that invokes this lambda expression.

# Keyframes of type Color (AacFlSettingKeyframesColor)
- `AacFlSettingKeyframesColor Easing(int frame, Color value)` /// Create a curved spline keyframe. The unit is defined by the function that invokes this lambda expression.
- `AacFlSettingKeyframesColor Linear(float frame, Color value)` /// Create a linear keyframe. The unit is defined by the function that invokes this lambda expression.
- `AacFlSettingKeyframesColor Constant(int frame, Color value)` /// Create a constant keyframe. The unit is defined by the function that invokes this lambda expression.


# Transitions

`AnimatorTransitionBase Transition;`  /// Expose the underlying Transition object (from AacFlNewTransitionContinuation).

## Attributes (AacFlTransition : AacFlNewTransitionContinuation)

- `AacFlTransition WithSourceInterruption()` /// Set interruption source to be Source. // FIXME: Clunky interface.
- `AacFlTransition WithTransitionDurationSeconds(float transitionDuration)` /// Set a fixed transition duration in seconds.
- `AacFlTransition WithTransitionDurationPercent(float transitionDurationNormalized)` /// Set a non-fixed transition duration in a normalized amount. // FIXME: Percent is a misnomer.
- `AacFlTransition WithOrderedInterruption()` /// Enable ordered interruption.
- `AacFlTransition WithNoOrderedInterruption()` /// Disable ordered interruption.
- `AacFlTransition WithTransitionToSelf()` /// Enable transition to self.
- `AacFlTransition WithNoTransitionToSelf()` /// Disable transition to self.
- `AacFlTransition AfterAnimationFinishes()` /// Set an exit time at 1, where the animation finishes.
- `AacFlTransition AfterAnimationIsAtLeastAtPercent(float exitTimeNormalized)` /// Set the exit time at a specific normalized amount. // FIXME: Percent is a misnomer.

## Starting Conditions (AacFlNewTransitionContinuation) & (AacFlEntryTransition : AacFlNewTransitionContinuation)

- `AacFlTransitionContinuation When(IAacFlCondition action)` /// Start defining conditions.
- `AacFlMultiTransitionContinuation When(IAacFlOrCondition actionsWithOr)` /// Start defining conditions from a condition which may generate multiple transitions. Any And operator subsequently invoked as a result will apply to all transitions that would be generated by that condition, until the Or operator, if any.
- `AacFlTransitionContinuation When(Action<AacFlTransitionContinuationWithoutOr> actionsWithoutOr)` /// Start defining conditions using a lambda expression that cannot contain Or operators.
- `AacFlTransitionContinuationOnlyOr When(Action<AacFlNewTransitionContinuation> actionsWithOr)` /// Start defining conditions using a lambda expression that can contain Or operators, however the next operator, if any, must be Or.
- `AacFlTransitionContinuation WhenConditions()` /// Provides an handle to start defining conditions using the And operator, for use in loops. The Or operator may be invoked at any point.

## Single-transition continuation (AacFlTransitionContinuation : AacFlTransitionContinuationAbstractWithOr)
- `AacFlTransitionContinuation And(IAacFlCondition action)` /// Add another condition to this transition. A transition requires all of its conditions to pass.
- `AacFlTransitionContinuation And(Action<AacFlTransitionContinuationWithoutOr> actionsWithoutOr)` /// Add another condition to this transition. A transition requires all of its conditions to pass.

# Multi-transition continuation (AacFlMultiTransitionContinuation : AacFlTransitionContinuationAbstractWithOr)
- `AacFlMultiTransitionContinuation And(IAacFlCondition action)` /// Add another condition to all of the transitions generated by the previous conditions since the last When operator invocation. A transition requires all of its conditions to pass.
- `AacFlMultiTransitionContinuation And(Action<AacFlTransitionContinuationWithoutOr> actionsWithoutOr)` /// Add another condition to all of the transitions generated by the previous conditions since the last When operator invocation. A transition requires all of its conditions to pass.

# Continuation (AacFlTransitionContinuationAbstractWithOr)
- `AacFlNewTransitionContinuation Or()` /// Creates a single new transition, copying the attributes of the one this was invoked on. This has no conditions yet, so invoking a When operator immediately after this is expected here.

# Continuation wihtout Or (AacFlTransitionContinuationWithoutOr)
- `AacFlTransitionContinuationWithoutOr And(IAacFlCondition action)` /// Add another condition to this transition. A transition requires all of its conditions to pass.
- `AacFlTransitionContinuationWithoutOr AndWhenever(Action<AacFlTransitionContinuationWithoutOr> action)` /// Add additional conditions using a lambda expression that cannot contain Or operators. A transition requires all of its conditions to pass.

# Conditions (IAacFlCondition, IAacFlOrCondition)

### AacFlFloatParameter
- `IsGreaterThan(float other)` /// Float is greater than `other`; be mindful of minifloat imprecision over the network.
- `IsLessThan(float other)` /// Float is less than `other`; be mindful of minifloat imprecision over the network.

### AacFlIntParameter
- `IsGreaterThan(int other)` /// Int is strictly greater than `other`
- `IsLessThan(int other)` /// Int is strictly less than `other`
- `IsEqualTo(int other)` /// Int is equal to `other`
- `IsNotEqualTo(int other)` /// Int is not equal to `other`

### AacFlEnumIntParameter<TEnum> where TEnum : Enum
- `IsEqualTo(TEnum other)` /// Int is not equal to `(int)other`
- `IsNotEqualTo(TEnum other)` /// Int is equal to `(int)other`
- `IsGreaterThan(int other)` /// (inherited from AacFlIntParameter) Int is strictly greater than `other`
- `IsLessThan(int other)` /// (inherited from AacFlIntParameter) Int is strictly less than `other`
- `IsEqualTo(int other)` /// (inherited from AacFlIntParameter)
- `IsNotEqualTo(int other)` /// (inherited from AacFlIntParameter)

### AacFlBoolParameter
- `IsTrue()` /// Bool is true
- `IsFalse()` /// Bool is false
- `IsEqualTo(bool other)` /// Bool is equal to `other`
- `IsNotEqualTo(bool other)` /// Bool is not equal to `other`

### AacFlFloatParameterGroup
- `AreGreaterThan(float other)` /// All of the Floats are greater than `other`; be mindful of minifloat imprecision over the network.
- `AreLessThan(float other)` /// All of the Floats are less than `other`; be mindful of minifloat imprecision over the network.

### AacFlIntParameterGroup
- `AreGreaterThan(float other)` /// All of the Ints are strictly greater than `other`
- `AreLessThan(float other)` /// All of the Ints are strictly less than `other`
- `AreEqualTo(float other)` /// All of the Ints are equal to `other`
- `AreNotEqualTo(float other)` /// All of the Ints are not equal to `other`

### AacFlBoolParameterGroup
- `AreTrue()` /// All of the Bools are true
- `AreFalse()` /// All of the Bools are false
- `AreEqualTo(bool other)` /// All of the Bools are equal to `other`
- `AreFalseExcept(AacFlBoolParameter exceptThisMustBeTrue)` /// All of the Bools except `exceptThisMustBeTrue` are false, and the Bool of `exceptThisMustBeTrue` must be true.
- `AreFalseExcept(params AacFlBoolParameter[] exceptTheseMustBeTrue)` /// All of the Bools except those in `exceptTheseMustBeTrue` are false, and all of the Bools in `exceptTheseMustBeTrue` must be true.
- `AreFalseExcept(AacFlBoolParameterGroup exceptTheseMustBeTrue)` /// All of the Bools except those in `exceptTheseMustBeTrue` are false, and all of the Bools in `exceptTheseMustBeTrue` must be true.
- `AreTrueExcept(AacFlBoolParameter exceptThisMustBeFalse)` /// All of the Bools except `exceptThisMustBeTrue` are true, and the Bool of `exceptThisMustBeTrue` must be false.
- `AreTrueExcept(params AacFlBoolParameter[] exceptTheseMustBeFalse)` /// All of the Bools except those in `exceptTheseMustBeTrue` are true, and all of the Bools in `exceptTheseMustBeTrue` must be false.
- `AreTrueExcept(AacFlBoolParameterGroup exceptTheseMustBeFalse)` /// All of the Bools except those in `exceptTheseMustBeTrue` are true, and all of the Bools in `exceptTheseMustBeTrue` must be false.
- `IsAnyTrue()` -> returns `IAacFlOrCondition`, can only be used inside `.When(...)` /// Generates multiple transitions, verifying whether any Bool is true
- `IsAnyFalse()` -> returns `IAacFlOrCondition`, can only be used inside `.When(...)` /// Generates multiple transitions, verifying whether any Bool is false
