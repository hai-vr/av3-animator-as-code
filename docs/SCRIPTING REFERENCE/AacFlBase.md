# AacFlBase
*Namespace: AnimatorAsCode.Framework*

## Methods 
| Method | Return Type | Description |
|-|-|-|
| `NewClip()` | `AacFlClip` | Create a new clip. The asset is generated into the container. |
| `CopyClip(AnimationClip originalClip)` | `AacFlClip` | Create a new clip that is a copy of `originalClip`. The asset is generated into the container. |
| `NewBlendTreeAsRaw()` | N/A | Create a new BlendTree asset. The asset is generated into the container. |
| `NewClip(string name)` | `AacFlClip` | Create a new clip with a name. However, the name is only used as a suffix for the asset. The asset is generated into the container. FIXME: This is quite pointless because the name is mangled anyways. |
| `DummyClipLasting(float numberOf, AacFlUnit unit)` | `AacFlClip` | Create a new clip which animates a dummy transform for a specific duration specified in an unit (Frames or Seconds). |
| `RemoveAllMainLayers()` | N/A | Remove all main layers matching that system from all animators of the Avatar descriptor. |
| `RemoveAllSupportingLayers(string suffix)` | N/A | Remove all supporting layers matching that system and suffix from all animators of the Avatar descriptor. |
| `CreateMainFxLayer()` | `AacFlLayer` | Create the main Fx layer of that system, clearing the previous one of the same system. You are not obligated to have a main layer. |
| `CreateMainGestureLayer()` | `AacFlLayer` | Create the main Gesture layer of that system, clearing the previous one of the same system. You are not obligated to have a main layer. |
| `CreateMainActionLayer()` | `AacFlLayer` | Create the main Action layer of that system, clearing the previous one of the same system. You are not obligated to have a main layer. |
| `CreateMainIdleLayer()` | `AacFlLayer` | Create the main Idle layer of that system, clearing the previous one of the same system. You are not obligated to have a main layer. |
| `CreateMainLocomotionLayer()` | `AacFlLayer` | Create the main Locomotion layer of that system, clearing the previous one of the same system. You are not obligated to have a main layer. |
| `CreateMainAv3Layer(VRCAvatarDescriptor.AnimLayerType animLayerType)` | `AacFlLayer` | Create the main layer of that system for a specific type of layer, clearing the previous one of the same system. You are not obligated to have a main layer. |
| `CreateSupportingFxLayer(string suffix)` | `AacFlLayer` | Create a supporting Fx layer for that system and suffix, clearing the previous one of the same system and suffix. You can create multiple supporting layers with different suffixes, and you are not obligated to have a main layer to create a supporting layer. |
| `CreateSupportingGestureLayer(string suffix)` | `AacFlLayer` | Create a supporting Gesture layer for that system and suffix, clearing the previous one of the same system and suffix. You can create multiple supporting layers with different suffixes, and you are not obligated to have a main layer to create a supporting layer. |
| `CreateSupportingActionLayer(string suffix)` | `AacFlLayer` | Create a supporting Action layer for that system and suffix, clearing the previous one of the same system and suffix. You can create multiple supporting layers with different suffixes, and you are not obligated to have a main layer to create a supporting layer. |
| `CreateSupportingIdleLayer(string suffix)` | `AacFlLayer` | Create a supporting Idle layer for that system and suffix, clearing the previous one of the same system and suffix. You can create multiple supporting layers with different suffixes, and you are not obligated to have a main layer to create a supporting layer. |
| `CreateSupportingLocomotionLayer(string suffix)` | `AacFlLayer` | Create a supporting Locomotion layer for that system and suffix, clearing the previous one of the same system and suffix. You can create multiple supporting layers with different suffixes, and you are not obligated to have a main layer to create a supporting layer. |
| `CreateSupportingAv3Layer(VRCAvatarDescriptor.AnimLayerType animLayerType, string suffix)` | `AacFlLayer` | Create a supporting layer for that system and suffix, clearing the previous one of the same system and suffix. You can create multiple supporting layers with different suffixes, and you are not obligated to have a main layer to create a supporting layer. |
| `CreateMainArbitraryControllerLayer(AnimatorController controller)` | `AacFlLayer` | Create a main layer for an arbitrary AnimatorController, clearing the previous one of the same system. You are not obligated to have a main layer. |
| `CreateSupportingArbitraryControllerLayer(AnimatorController controller, string suffix)` | `AacFlLayer` | Create a supporting layer for an arbitrary AnimatorController, clearing the previous one of the same system and suffix. You can create multiple supporting layers with different suffixes, and you are not obligated to have a main layer to create a supporting layer. |
| `CreateFirstArbitraryControllerLayer(AnimatorController controller)` | `AacFlLayer` | Clears the topmost layer of an arbitrary AnimatorController, and returns it. |
| `VrcAssets()` | `AacVrcAssetLibrary` | Return an AacVrcAssetLibrary, which lets you select various assets from VRChat. |
| `ClearPreviousAssets()` | N/A | Removes all assets from the asset container matching the specified asset key. |
