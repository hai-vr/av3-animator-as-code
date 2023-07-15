# Toggling a Skinned Mesh Renderer with two conditions
This example shows:

- Simple toggle animations
- Groups of Bool parameters
- Simple transitions

```mermaid
flowchart TD
    Entry:::entryState --> Hidden:::defaultState
    Hidden -->|"EnableAccessories && AccessoryThing"| Shown
    Shown -->|"!EnableAccessories"| Hidden
    Shown -->|"!AccessoryThing"| Hidden
    Exit:::exitState
    AnyState:::anyState
```

[toggle](https://raw.githubusercontent.com/Happyrobot33/av3-animator-as-code/main/Packages/com.happyrobot33.animatorascode/Editor/Framework/Examples/GenExample1_ToggleSmr.cs ":include :type=code csharp")
