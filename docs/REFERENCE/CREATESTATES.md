# Create states

Using your layer, you may now create states. Your state will be configured with defaults based on your AAC configuration, most notably, the Write Defaults setting.

By default, states have a dummy animation that lasts one frame (1/60th of a second).

It is intended to create the animation clip directly while declaring the state, this will be explained later on.

```csharp
var hidden = fx.NewState("Hidden")
    .WithAnimation(aac.NewClip().Toggling(my.item, false));
```
