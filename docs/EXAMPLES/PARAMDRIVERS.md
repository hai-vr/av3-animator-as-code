# Using Parameter Drivers

This example shows:

- Driving parameters using VRC Parameter Driver behavior
- VRChat Parameters
- Creating systems with multiple FX layers
- Complex transitions

```mermaid
---
title: FX Controller
---
flowchart TD
    subgraph Main FX Layer
    Entry1[Entry]:::entryState --> lackOfChangeDetected[Animate To NoChange]:::defaultState
    lackOfChangeDetected -->|>75% of anim \n dirtyCheckParameter| changeDetected[Animate To Changing]
    lackOfChangeDetected --> noChange
    noChange -->|dirtyCheckParameter| changeDetected
    changing[Changing] -->|dirtyCheckParameter| stillChanging[Still Changing]
    stillChanging --> changing
    stillChanging ~~~|Sets dirtyCheckParameter to false| stillChanging
    changing[Changing] -->|100% of anim| lackOfChangeDetected
    changeDetected -->|100% of anim| changing
    Exit1:::exitState
    AnyState1:::anyState
    end

    subgraph Detection
    Entry2[Entry]:::entryState --> reevaluating[Reevaluating]:::defaultState
    reevaluating -->|"GestureLeft == (0-8) && GestureRight == (0-8)"| state["Gesture {left} {right}"]
    state -->|"GestureLeft != (0-8) || GestureRight != (0-8)"| reevaluating
    state ~~~|"Drives AAC_INTERNAL_GesturingIcon_DirtyCheck to true"| state
    end
```

[toggle](https://raw.githubusercontent.com/Happyrobot33/av3-animator-as-code/main/Packages/com.happyrobot33.animatorascode/Editor/Framework/Examples/GenExample3_Gesturing.cs ":include :type=code csharp")
