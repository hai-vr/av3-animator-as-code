# Creating parameters for use within layers

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

# Forcing the value of parameters

In some rare cases you may wish to override the default value of animator parameters, for example in use for special conditions, for use in blend trees as a constant value.

Using your layer:

```csharp
fx.OverrideValue(fx.FloatParameter("SmoothingAmount"), 0.7f)
```
