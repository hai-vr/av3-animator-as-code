# Animator As Code

**Animator As Code** is a small Unity Editor facility to generate Avatars 3.0 Animator layers and animations from a [fluent builder](https://en.wikipedia.org/wiki/Fluent_interface) syntax written in C#.

Describing your animators as code provides the following advantages:

- you do not need to edit your animations by hand every time you add remove or change the location of a component in your hierarchy
- you will not need to edit a hundred transitions by hand if you need to rectify your animator

It is written with VRChat Avatars 3.0 use cases in mind; the API is opinionated to facilitate writing such animators in a concise way, hopefully requiring as little additional tweaking.

# Interested? Join my Discord Server

This is a work in progress, I am looking for feedback!

[Join the Invitation Discord Server!](https://discord.com/invite/58fWAUTYF8)

# Install

There are currently no releases.

Clone the repository within a subfolder of your Unity project, or download the source code and install in any subfolder of your project.

The project can be located within `Assets/AnimatorAsCodeFramework` but you can choose any location.

# Examples

## #0 Toggle a GameObject

This example shows:

- Creating a FX layer
- Creating states
- Creating animations
- Declaring a Bool parameter
- Creating transitions with conditions
- 
![sx_2022-02-25_00-47-15_dDwKV4bsE2](https://user-images.githubusercontent.com/60819407/155912947-7e5ac988-71f7-44cd-b526-2e245faffcd9.png)

```csharp
public class GenExample0_ToggleGo : MonoBehaviour
{
    public VRCAvatarDescriptor avatar;
    public AnimatorController assetContainer;
    public string assetKey;
    public GameObject item;
}

private void Create()
{
    var my = (GenExample0_ToggleGo) target;
    // The avatar is used here:
    // - to find the FX playable layer animator, where a new layer will be created.
    // - to resolve the relative animation path to the item.
    // The generated animation files are stored in the asset container.
    var aac = AacExample.AnimatorAsCode("Example 0", my.avatar, my.assetContainer, my.assetKey, AacExample.Options().WriteDefaultsOff());

    // Create a layer in the FX animator.
    // Additional layers can be created in the FX animator (see later in the manual).
    var fx = aac.CreateMainFxLayer();

    // The first created state is the default one connected to the "Entry" node.
    // States are automatically placed on the grid (see later in the manual).
    var hidden = fx.NewState("Hidden")
        // Animation assets are generated as sub-assets of the asset container.
        // The animation path to my.skinnedMesh is relative to my.avatar
        .WithAnimation(aac.NewClip().Toggling(my.item, false));
    var shown = fx.NewState("Shown")
        .WithAnimation(aac.NewClip().Toggling(my.item, true));

    // Creates a Bool parameter in the FX layer.
    // Parameters are added to the Animator if a parameter with the same name
    // does not exist yet.
    var itemParam = fx.BoolParameter("EnableItem");

    // Transitions are created with a set of default values
    // That can be changed in the Generator settings (see later in the manual).
    hidden.TransitionsTo(shown).When(itemParam.IsTrue());
    shown.TransitionsTo(hidden).When(itemParam.IsFalse());
}
```

## #1 Toggle a SkinnedMeshRenderer with two conditions

This example shows:

- Simple toggle animations
- Groups of Bool parameters
- Simple transitions

![sx_2022-02-25_00-39-05_DbvwrouqFc](https://user-images.githubusercontent.com/60819407/155912964-ffdfe363-6c1f-41d4-b474-9f18e1749c4d.png)

```csharp
public class GenExample1_ToggleSmr : MonoBehaviour
{
    public VRCAvatarDescriptor avatar;
    public AnimatorController assetContainer;
    public string assetKey;
    public SkinnedMeshRenderer skinnedMesh;
}

private void Create()
{
    var my = (GenExample1_ToggleSmr) target;
    var aac = AacExample.AnimatorAsCode(SystemName, my.avatar, my.assetContainer, my.assetKey);
    
    var fx = aac.CreateMainFxLayer();
    var hidden = fx.NewState("Hidden")
        // The runtime type of my.skinnedMesh is used within the animation.
        // In this case, the "SkinnedMeshRenderer" component is disabled.
        .WithAnimation(aac.NewClip().TogglingComponent(my.skinnedMesh, false));
    var shown = fx.NewState("Shown")
        .WithAnimation(aac.NewClip().TogglingComponent(my.skinnedMesh, true));
    
    // This creates two Bool parameters in the animator.
    // The resulting value can be used in conditions.
    var accessoriesParams = fx.BoolParameters("EnableAccessories", "AccessoryThing");
    
    // The following line creates one transition.
    // The conditions are "EnableAccessories is true" and "AccessoryThing is true"
    hidden.TransitionsTo(shown).When(accessoriesParams.AreTrue());
    
    // The following line creates two transitions:
    // - The first transition is "EnableAccessories is false"
    // - The second transition is "AccessoryThing is false"
    shown.TransitionsTo(hidden).When(accessoriesParams.IsAnyFalse());
}
```

## #2 Animate a SkinnedMesh with Motion time

This example shows:

- Describing animations with simplified keyframes
- Animating a state with Motion Time (formerly known as Normalized Time)

![sx_2022-02-25_01-25-53_2KNRPmra4o](https://user-images.githubusercontent.com/60819407/155912980-53ba1528-0872-4b65-a491-2e9442f48bae.png)


```csharp
public class GenExample2_Animate : MonoBehaviour
{
    public VRCAvatarDescriptor avatar;
    public AnimatorController assetContainer;
    public string assetKey;
    public SkinnedMeshRenderer wedgeMesh;
}

private void Create()
{
    var my = (GenExample2_Animate) target;
    var aac = AacExample.AnimatorAsCode(SystemName, my.avatar, my.assetContainer, my.assetKey);
    
    var fx = aac.CreateMainFxLayer();
    
    fx.NewState("Motion")
        .WithAnimation(aac.NewClip().Animating(clip =>
        {
            clip.Animates(my.wedgeMesh, "blendShape.Wedge").WithFrameCountUnit(keyframes =>
                keyframes.Easing(0, 100f).Easing(28, 0).Easing(29, 0).Easing(30, 0).Easing(31, 0).Easing(32, 0).Easing(60, 100f)
            );
            clip.Animates(my.wedgeMesh, "material._Metallic").WithFrameCountUnit(keyframes =>
                keyframes.Constant(0, 1f).Constant(28, 0).Constant(60, 0)
            );
        }))
        .MotionTime(fx.FloatParameter("WedgeAmount"));
}
```

## #3 Using Parameter Drivers

This example shows:

- Driving parameters using VRC Parameter Driver behavior
- VRChat Parameters
- Creating systems with multiple FX layers
- Complex transitions
- *Show code...*

    ```csharp
    public class GenExample3_Gesturing : MonoBehaviour
    {
        public VRCAvatarDescriptor avatar;
        public AnimatorController assetContainer;
        public string assetKey;
        public SkinnedMeshRenderer iconMesh;
    }
    
    private GenExample3_Gesturing my;
    private AacFlBase aac;
    
    private void Create()
    {
        my = (GenExample3_Gesturing) target;
        aac = AacExample.AnimatorAsCode(SystemName, my.avatar, my.assetContainer, my.assetKey);
        
        CreateMainLayer();
        CreateSupportingLayer();
    }
    
    private void CreateMainLayer()
    {
        var layer = aac.CreateMainFxLayer();
        
        var dirtyCheckParameter = layer.BoolParameter("AAC_INTERNAL_GesturingIcon_DirtyCheck");
        
        // ### Create states
        var lackOfChangeDetected = layer.NewState("Animate To NoChange")
            .WithAnimation(IconAppears());
        
        // By default, states have an animation that animates a dummy object for 1 frame.
        var noChange = layer.NewState("NoChange", 1, 0).RightOf();
        
        var changeDetected = layer.NewState("Animate To Changing").Under()
            .WithAnimation(IconDisappears());
        
        // This creates a clip that animates a dummy object for 1.5f seconds.
        var changing = layer.NewState("Changing", 0, 1).LeftOf()
            .WithAnimation(aac.DummyClipLasting(1.5f, AacFlUnit.Seconds));
        
        // When this state is entered, the parameter is driven to the value of false.
        var stillChanging = layer.NewState("Still Changing", 0, 2).Under()
            .Drives(dirtyCheckParameter, false);
        
        // ------
        
        // ### Create transitions
        lackOfChangeDetected.TransitionsTo(changeDetected).AfterAnimationIsAtLeastAtPercent(0.7f).When(dirtyCheckParameter.IsTrue());
        
        // The transition duration is 30% of the animation duration.
        lackOfChangeDetected.TransitionsTo(noChange).AfterAnimationFinishes().WithTransitionDurationPercent(0.3f);
        
        noChange.TransitionsTo(changeDetected).When(dirtyCheckParameter.IsTrue());
        
        // By using AfterAnimationFinishes, the transition will trigger after the animation
        // for the icon appearing finishes.
        changeDetected.TransitionsTo(changing).AfterAnimationFinishes();
        
        changing.TransitionsTo(stillChanging).When(dirtyCheckParameter.IsTrue());
        // By using AfterAnimationFinishes, the transition will trigger after 1.5 seconds,
        // which is the length of the animation in Changing.
        changing.TransitionsTo(lackOfChangeDetected).AfterAnimationFinishes();
        
        // The transition will immediately happen upon entering, by using Exit time set to 0.
        stillChanging.AutomaticallyMovesTo(changing);
    }
    
    private void CreateSupportingLayer()
    {
        // Create an additional FX layer.
        var layer = aac.CreateSupportingFxLayer("Detection");
        var reevaluating = layer.NewState("Reevaluating", -1, 0);
        
        foreach (var left in Enumerable.Range(0, 8))
        {
            foreach (var right in Enumerable.Range(0, 8))
            {
                var state = layer.NewState($"Gesture {left} {right}", left, right)
                    // When this state is entered, the parameter is driven to the value of true.
                    .Drives(layer.BoolParameter("AAC_INTERNAL_GesturingIcon_DirtyCheck"), true);
        
                reevaluating.TransitionsTo(state)
                    // Use ".Av3" to access VRChat standard parameters.
                    // Accessing these parameters will create the corresponding parameter in the animator.
                    .When(layer.Av3().GestureLeft.IsEqualTo(left))
                    .And(layer.Av3().GestureRight.IsEqualTo(right));
                state.TransitionsTo(reevaluating)
                    .When(layer.Av3().GestureLeft.IsNotEqualTo(left))
                    .Or()
                    .When(layer.Av3().GestureRight.IsNotEqualTo(right));
            }
        }
    }
    
    private AacFlClip IconAppears()
    {
        return aac.NewClip().Animating(clip =>
        {
            clip.Animates(my.iconMesh, "blendShape.Wedge")
                .WithFrameCountUnit(keyframes => keyframes.Easing(0, 0f).Easing(10, 100f));
        });
    }
    
    private AacFlClip IconDisappears()
    {
        return aac.NewClip().Animating(clip =>
        {
            clip.Animates(my.iconMesh, "blendShape.Wedge")
                .WithFrameCountUnit(keyframes => keyframes.Easing(0f, 100f).Easing(10, 0f));
        });
    }
    ```


---

# Reference

## Typical animator creation steps

Animator As Code is generally used in the following steps:

- Declare an Animator As Code
- Create one or multiple layers
- Create the states
    - Create the animations at the same time
- Create the transitions

## Declare an Animator As Code (AAC)

In order to use Animator As Code (AAC), first, declare it with a configuration.

The AAC configuration requires the following:

- A system name.
    - Animator As Code describes systems. A system can have multiple layers, not only across playable layer animators, but also within a single playable layer animator. All created layers will be prefixed with this system name.
    - It is up to you to decide where the boundaries of the system lies.
- An avatar descriptor, animator root, and default value root.
    - This is used to select the playable layer animators to use.
    - This is also used to select the root transform that animations will use for relative paths.
    - This is used to collect the default values for some animations.
    - *In general, all three of them are the same object, but this is not mandatory.*
- An asset container.
    - Animations are generated by Animator As Code, and the large quantity of generated assets can be quite messy. To limit littering your project, such assets will be generated as sub-assets of a container. The container is of type Animator Controller, but it doesn’t need to have any layers within it.
- An asset key.
    - The asset key a prefix that all generated assets will use. When creating the layers, all generated assets that uses that prefix will be removed upon invocation of `aac.ClearPreviousAssets()`.
- A provider of defaults.
    - Animator As Code is opinionated, and sometimes you want to tweak the default values. The provider of defaults is executed when a state is created, an animation is created, a transition is created. This will let you tweak the generation process.

To declare it:

```csharp
string systemName;
VRCAvatarDescriptor avatar;
AnimatorController assetContainer;
string assetKey;

var aac = AacV0.Create(new AacConfiguration
{
    SystemName = systemName,
    AvatarDescriptor = avatar,
    AnimatorRoot = avatar.transform,
    DefaultValueRoot = avatar.transform,
    AssetContainer = assetContainer,
    AssetKey = assetKey,
    DefaultsProvider = new AacDefaultsProvider(writeDefaults: false)
});
// Remove all previously generated assets from the asset container
// that match the asset key.
aac.ClearPreviousAssets();
```

## Create one or multiple layers

Using AAC, create layers in your animators. A system can have multiple layers across animators.

There is one main layer, and multiple supporting layers, per animator.

- The main layer will be named exactly after your system name.
- The supporting layers will be prefixed by the system name, and appended with a suffix of your choice.

You are in no obligation to create a main layer. If you think several layers of the same animators are of equal importance, you can declare them as being supporting layers.

```csharp
var fx = aac.CreateMainFxLayer();
var detection = aac.CreateSupportingFxLayer("Detection");
```

## Create states

Using your layer, you may now create states. Your state will be configured with defaults based on your AAC configuration, most notably, the Write Defaults setting.

By default, states have a dummy animation that lasts one frame (1/60th of a second).

It is intended to create the animation clip directly while declaring the state, this will be explained later on.

```csharp
var hidden = fx.NewState("Hidden")
    .WithAnimation(aac.NewClip().Toggling(my.item, false));
```

## Creating parameters for use within layers

Using your layer, you can create parameters.

The parameter will be added to the animator the layer belongs to.

If you need to reuse a parameter across multiple layers, you need to invoke it on all relevant layers.

*(Once done, you are not forced to reuse the resulting parameter instance on the same layer it was created from)*

```csharp

fx.NewState("Motion")
    // This creates a Float parameter on the FX layer.
    .MotionTime(fx.FloatParameter("WedgeAmount"))
    // ...
```

If a parameter with the same name already exists in the animator, it will not be created again.

*This is also true if the animator already has a parameter with the same name but with a different type, and no error will be raised.*

There are also dedicated functions to obtain Avatars 3.0 parameters, such as.

```csharp
var gestureLeftWeight = fx.Av3().GestureLeftWeight
```

## Forcing the value of parameters

In some rare cases you may wish to override the default value of animator parameters, for example in use for special conditions, for use in blend trees as a constant value.

Using your layer:

```csharp
fx.OverrideValue(fx.FloatParameter("SmoothingAmount"), 0.7f)
```

## Visually organize your states

By default, a newly created state will be visually placed under the previously created state.

For more control, states have `LeftOf`, `RightOf`, `Over`, `Under`, `Shift` operators, which let you move a state to be visually next to another state.

The value is in grid units.

![Untitled](https://user-images.githubusercontent.com/60819407/155912996-89369e2d-435d-4619-b5ee-3ac4b6f8827c.png)

```csharp
var init = fx.NewState("Init"); // This is the first state. By default it is at (0, 0)
var a = fx.NewState("A"); // This will be placed under Init.
var b = fx.NewState("B"); // This will be placed under A.
var c = fx.NewState("C").RightOf(a); // This will be placed right of A.
var d = fx.NewState("D"); // This will be placed under C.
var e = fx.NewState("E").RightOf(); // This will be placed right of D.
var alternate = fx.NewState("Alternate").Over(c); // This will be placed over C.

// This will be placed relative to Alternate: 2 blocks over, and 1 to the right.
var reset = fx.NewState("Reset").Shift(alternate, 1, -2);
```

## Create an animation

By default, states have a dummy animation that lasts one frame (1/60th of a second). If you want the state to play an animation of your choice, there is usually an invocation to the `.WithAnimation(...)` function:

```csharp
var hidden = fx.NewState("Hidden")
    .WithAnimation(aac.NewClip().Toggling(my.item, false));
```

The invocation uses AAC to create a new clip inside the asset container. You don’t need to specify a name for this clip, it is irrelevant.

Animations use object references instead of paths. They are converted to paths in the animation clip asset by resolving the relative path to the animator root of the AAC configuration.

Most of the functions lets you create single-frame constant animations.

For more complex animations, use the `.Animating(...)` function.

```csharp
fx.NewState("Motion")
    .WithAnimation(aac.NewClip().Animating(clip =>
    {
        clip.Animates(my.wedgeMesh, "blendShape.Wedge").WithFrameCountUnit(keyframes =>
            keyframes.Easing(0, 100f).Easing(28, 0).Easing(29, 0).Easing(30, 0).Easing(31, 0).Easing(32, 0).Easing(60, 100f)
        );
        clip.Animates(my.wedgeMesh, "material._Metallic").WithFrameCountUnit(keyframes =>
            keyframes.Constant(0, 1f).Constant(28, 0).Constant(60, 0)
        );
    }))
```

## Create transitions and define conditions

Using the states, you can create transitions between states. Your transition will be configured with defaults based on your AAC configuration, defined within the DefaultsProvider.

To create a transition from Any, Exit, or Entry, there are some functions in the states like `.TransitionsFromAny()` or `.Exits()`.

Start defining conditions for that transition using the `.When(...)` operator.

Parameters have functions that generate conditions once invoked.

```csharp
reevaluating.TransitionsTo(state)
    // Use ".Av3" to access VRChat standard parameters.
    // Accessing these parameters will create the corresponding parameter in the animator.
    .When(layer.Av3().GestureLeft.IsEqualTo(left))
    .And(layer.Av3().GestureRight.IsEqualTo(right));
state.TransitionsTo(reevaluating)
    .When(layer.Av3().GestureLeft.IsNotEqualTo(left))
    .Or()
    .When(layer.Av3().GestureRight.IsNotEqualTo(right));
```

## When(), And(), and Or() operators

In animator transitions, all conditions must be verified for the transition to occur; this is effectively a “AND” of all of the conditions (A && B && C).

This is done by using the `.And(...)` operator: `.When(A).And(B).And(C)`

To represent “OR”, new transitions need to be created; ((A && B && C) || (D && E)) results in:

- Transition 1: (A && B && C)
- Transition 2: (D && E)

This is done by using the `.Or(...)` operator: `.When(A).And(B).And(C).Or().When(D).And(E)`

From this limitation, conditions with nested OR cannot be expressed easily, such as:

- 🚫 (F && (G || H) && (J || K))

## Conditions that generate multiple transitions

Some conditions generate multiple transitions, such as `boolParameters.IsAnyFalse()`

In order to still let you express these expressions easily, the following applies:

- These conditions can only be used in a `.When(...)` operator, and not within a `.And(...)`
- If such a condition is used, calling `.And(...)` will apply the condition to all transitions generated by the `.When(...)` operator up until the next `Or(...)` operator if any exists.
    - For example:
      `.When(fx.BoolParameters(I, J).IsAnyFalse()).And(K.IsTrue())`
    - Is equivalent to:
      `.When(I.IsFalse()).And(K.IsTrue()).Or().When(J.IsFalse()).And(K.IsTrue())`

## Use the WhenConditions() operator to build in a `for` loop

The presence of the `.When(...)` operator can make it difficult to build conditions iteratively in a `for` loop.

For this purpose, use the `.WhenConditions()` operator. This will let you build conditions using the `.And(...)` operator.

```csharp
var conditions = state.TransitionsFromEntry().WhenConditions();
for (var i = 0; i < numberOfBits; i++)
{
   conditions.And(parameter[i].IsEqualTo(bitMask[i]));
}
```

## Create many-to-one transitions using a `foreach` loop

There is no facility to create multiple identical transitions from multiple states to another state.

Use a regular `foreach` loop to achieve this.

```csharp
foreach (var cancelWhenNotAllowed in new[] {auto, reverse, manual, custom, done})
{
    cancelWhenNotAllowed.TransitionsTo(idle).When(allowSystemParameter.IsFalse());
}
```

---

# Reference manual

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
- `AacFlState DrivingIncreases(AacFlIntParameter parameter, int additiveValue)` /// Drive the Int parameter, incrementing it by `additiveValue`. Create an Avatar Parameter Driver state behaviour if it does not exist.
- `AacFlState DrivingDecreases(AacFlIntParameter parameter, int positiveValueToDecreaseBy)` /// Drive the Int parameter, decreasing it by the amount of `positiveValueToDecreaseBy`. Create an Avatar Parameter Driver state behaviour if it does not exist.
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
