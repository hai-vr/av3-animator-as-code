# Create one or multiple layers

Using AAC, create layers in your animators. A system can have multiple layers across animators.

There is one main layer, and multiple supporting layers, per animator.

- The main layer will be named exactly after your system name.
- The supporting layers will be prefixed by the system name, and appended with a suffix of your choice.

You are in no obligation to create a main layer. If you think several layers of the same animators are of equal importance, you can declare them as being supporting layers.

```csharp
var fx = aac.CreateMainFxLayer();
var detection = aac.CreateSupportingFxLayer("Detection");
```
