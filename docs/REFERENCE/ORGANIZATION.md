# Visually organize your states

By default, a newly created state will be visually placed under the previously created state.

For more control, states have `LeftOf`, `RightOf`, `Over`, `Under`, `Shift` operators, which let you move a state to be visually next to another state.

The value is in grid units.

?> For reference, grid states are 200x10 units large.

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
