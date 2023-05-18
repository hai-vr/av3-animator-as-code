# Create an animation

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
