# AacFlClip
*Namespace: AnimatorAsCode.Framework*

## Parameters
| Parameter | Type | Description |
|-|-|-|
| `Clip` | `AnimationClip` | Exposes the underlying clip object. |

## Methods
| Method | Return Type | Description |
|-|-|-|
| `Looping()` | `AacFlClip ` | Set the clip to loop. |
| `NonLooping()` | `AacFlClip ` | Set the clip to not loop. |

### Single-Frame Animations
| Method | Return Type | Description |
|-|-|-|
| `Toggling(GameObject[] gameObjectsWithNulls, bool value)` | `AacFlClip ` | Enable or disable GameObjects. This lasts one frame. The array can safely contain null values. |
| `BlendShape(SkinnedMeshRenderer renderer, string blendShapeName, float value)` | `AacFlClip ` | Change a blendShape of a skinned mesh. This lasts one frame. |
| `BlendShape(SkinnedMeshRenderer[] rendererWithNulls, string blendShapeName, float value)` | `AacFlClip ` | Change a blendShape of multiple skinned meshes. This lasts one frame. The array can safely contain null values. |
| `Scaling(GameObject[] gameObjectsWithNulls, Vector3 scale)` | `AacFlClip ` | Scale GameObjects. This lasts one frame. The array can safely contain null values. // FIXME: This is weird, this should be a Transform array, and also this needs a single-object overload. |
| `Toggling(GameObject gameObject, bool value)` | `AacFlClip ` |  Enable or disable a GameObject. This lasts one frame. |
| `TogglingComponent(Component[] componentsWithNulls, bool value)` | `AacFlClip ` | Toggle several components. This lasts one frame. The runtime type of each individual component will be used. The array can safely contain null values. |
| `TogglingComponent(Component component, bool value)` | `AacFlClip ` | Toggle a component. This lasts one frame. The runtime type of the component will be used. |
| `SwappingMaterial(Renderer renderer, int slot, Material material)` | `AacFlClip ` | Swap a material of a Renderer on the specified slot (indexed at 0). This lasts one frame. |
| `SwappingMaterial(ParticleSystem particleSystem, int slot, Material material)` | `AacFlClip ` | Swap a material of a Particle System on the specified slot (indexed at 0). This lasts one frame. This effectively takes the ParticleSystemRenderer of the component. |

### Clip Editing
| Method | Return Type | Description |
|-|-|-|
| `Animating(Action<AacFlEditClip> action)` | `AacFlClip ` | Start editing the clip with a lambda expression. |
