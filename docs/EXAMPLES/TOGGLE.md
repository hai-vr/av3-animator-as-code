# Toggling a GameObject
This example shows:

- Creating a FX layer
- Creating states
- Creating animations
- Declaring a Bool parameter
- Creating transitions with conditions

```mermaid
flowchart TD
    Entry:::entryState --> Hidden:::defaultState
    Hidden -->|itemParam == true| Shown
    Shown -->|itemParam == false| Hidden
    Exit:::exitState
    AnyState:::anyState
```

<!-- ![sx_2022-02-25_00-47-15_dDwKV4bsE2](https://user-images.githubusercontent.com/60819407/155912947-7e5ac988-71f7-44cd-b526-2e245faffcd9.png) -->

[toggle](https://raw.githubusercontent.com/Happyrobot33/av3-animator-as-code/main/Packages/com.happyrobot33.animatorascode/Editor/Framework/Examples/GenExample0_ToggleGo.cs ":include :type=code csharp")
