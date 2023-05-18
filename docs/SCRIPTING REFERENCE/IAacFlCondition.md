# IAacFlCondition, IAacFlOrCondition
*Namespace: AnimatorAsCode.Framework*

## AacFlFloatParameter
| Condition | Description |
|-|-|
| `IsGreaterThan(float other)` | Float is greater than `other`; be mindful of minifloat imprecision over the network. |
| `IsLessThan(float other)` | Float is less than `other`; be mindful of minifloat imprecision over the network. |

## AacFlIntParameter
| Condition | Description |
|-|-|
| `IsGreaterThan(int other)` | Int is strictly greater than `other` |
| `IsLessThan(int other)` | Int is strictly less than `other` |
| `IsEqualTo(int other)` | Int is equal to `other` |
| `IsNotEqualTo(int other)` | Int is not equal to `other` |

## AacFlEnumIntParameter<TEnum> where TEnum : Enum
| Condition | Description |
|-|-|
| `IsEqualTo(TEnum other)` | Int is equal to `(int)other` |
| `IsNotEqualTo(TEnum other)` | Int is not equal to `(int)other` |
| `IsGreaterThan(TEnum other)` | Int is strictly greater than `(int)other` |
| `IsLessThan(TEnum other)` | Int is strictly less than `(int)other` |
| `IsEqualTo(int other)` | (inherited from AacFlIntParameter) |
| `IsNotEqualTo(int other)` | (inherited from AacFlIntParameter) |

## AacFlBoolParameter
| Condition | Description |
|-|-|
| `IsTrue()` | Bool is true |
| `IsFalse()` | Bool is false |
| `IsEqualTo(bool other)` | Bool is equal to `other` |
| `IsNotEqualTo(bool other)` | Bool is not equal to `other` |

## AacFlFloatParameterGroup
| Condition | Description |
|-|-|
| `AreGreaterThan(float other)` | All of the Floats are greater than `other`; be mindful of minifloat imprecision over the network. |
| `AreLessThan(float other)` | All of the Floats are less than `other`; be mindful of minifloat imprecision over the network. |

## AacFlIntParameterGroup
| Condition | Description |
|-|-|
| `AreGreaterThan(int other)` | All of the Ints are strictly greater than `other` |
| `AreLessThan(int other)` | All of the Ints are strictly less than `other` |
| `AreEqualTo(int other)` | All of the Ints are equal to `other` |
| `AreNotEqualTo(int other)` | All of the Ints are not equal to `other` |

## AacFlBoolParameterGroup
| Condition | Description |
|-|-|
| `AreTrue()` | All of the Bools are true |
| `AreFalse()` | All of the Bools are false |
| `AreEqualTo(bool other)` | All of the Bools are equal to `other` |
| `AreFalseExcept(AacFlBoolParameter exceptThisMustBeTrue)` | All of the Bools except `exceptThisMustBeTrue` are false, and the Bool of `exceptThisMustBeTrue` must be true. |
| `AreFalseExcept(params AacFlBoolParameter[] exceptTheseMustBeTrue)` | All of the Bools except those in `exceptTheseMustBeTrue` are false, and all of the Bools in `exceptTheseMustBeTrue` must be true. |
| `AreFalseExcept(AacFlBoolParameterGroup exceptTheseMustBeTrue)` | All of the Bools except those in `exceptTheseMustBeTrue` are false, and all of the Bools in `exceptTheseMustBeTrue` must be true. |
| `AreTrueExcept(AacFlBoolParameter exceptThisMustBeFalse)` | All of the Bools except `exceptThisMustBeTrue` are true, and the Bool of `exceptThisMustBeTrue` must be false. |
| `AreTrueExcept(params AacFlBoolParameter[] exceptTheseMustBeFalse)` | All of the Bools except those in `exceptTheseMustBeTrue` are true, and all of the Bools in `exceptTheseMustBeTrue` must be false. |
| `AreTrueExcept(AacFlBoolParameterGroup exceptTheseMustBeFalse)` | All of the Bools except those in `exceptTheseMustBeTrue` are true, and all of the Bools in `exceptTheseMustBeTrue` must be false. |
| `IsAnyTrue()` -> returns `IAacFlOrCondition`, can only be used inside `.When(...)` | Generates multiple transitions, verifying whether any Bool is true |
| `IsAnyFalse()` -> returns `IAacFlOrCondition`, can only be used inside `.When(...)` | Generates multiple transitions, verifying whether any Bool is false |
