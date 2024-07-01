using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Animations;
using static AnimatorAsCode.V1.AacFlConditionSimple;
using static UnityEditor.Animations.AnimatorConditionMode;

// ReSharper disable once CheckNamespace
namespace AnimatorAsCode.V1
{
    class AacFlConditionSimple : IAacFlCondition
    {
        private readonly Action<AacFlCondition> _action;

        public AacFlConditionSimple(Action<AacFlCondition> action)
        {
            _action = action;
        }

        public static AacFlConditionSimple Just(Action<AacFlCondition> action)
        {
            return new AacFlConditionSimple(action);
        }

        public static AacFlConditionSimple ForEach(string[] subjects, Action<string, AacFlCondition> action)
        {
            return new AacFlConditionSimple(condition =>
            {
                foreach (var subject in subjects)
                {
                    action.Invoke(subject, condition);
                }
            });
        }

        public void ApplyTo(AacFlCondition appender)
        {
            _action.Invoke(appender);
        }
    }

    public abstract class AacFlParameter
    {
        /// Expose the name of this parameter.
        public string Name { get; }

        protected AacFlParameter(string name)
        {
            Name = name;
        }
    }

    public abstract class AacFlParameter<TParam> : AacFlParameter
    {
        protected AacFlParameter(string name) : base(name)
        {
        }

        /// This function is used for internal purposes:<br/>
        /// Provide a float representation of the parameter.<br/>
        /// This is used mainly to derive parameter driver values on the VRChat platform.
        public abstract float ValueToFloat(TParam value);
    }
    
    public abstract class AacFlNumericParameter<TParam> : AacFlParameter<TParam>
    {
        protected AacFlNumericParameter(string name) : base(name)
        {
        }
    }

    public class AacFlFloatParameter : AacFlNumericParameter<float>
    {
        internal static AacFlFloatParameter Internally(string name) => new AacFlFloatParameter(name);
        protected AacFlFloatParameter(string name) : base(name) { }
        
        /// Float is greater than other.<br/>
        /// When used on some platforms, you need to be careful as the remote value may not be the same as the local value.
        public IAacFlCondition IsGreaterThan(float other) => Just(condition => condition.Add(Name, Greater, other));
        
        /// Float is less than other.<br/>
        /// When used on some platforms, you need to be careful as the remote value may not be the same as the local value.
        public IAacFlCondition IsLessThan(float other) => Just(condition => condition.Add(Name, Less, other));

        /// This function is used for internal purposes:<br/>
        /// Returns the same value as the parameter.
        public override float ValueToFloat(float value)
        {
            return value;
        }
    }

    public class AacFlIntParameter : AacFlNumericParameter<int>
    {
        internal static AacFlIntParameter Internally(string name) => new AacFlIntParameter(name);
        protected AacFlIntParameter(string name) : base(name) { }
        
        /// Int is strictly greater than `other`
        public IAacFlCondition IsGreaterThan(int other) => Just(condition => condition.Add(Name, Greater, other));
        
        /// Int is strictly less than `other`
        public IAacFlCondition IsLessThan(int other) => Just(condition => condition.Add(Name, Less, other));
        
        /// Int is equal to `other`
        public IAacFlCondition IsEqualTo(int other) => Just(condition => condition.Add(Name, AnimatorConditionMode.Equals, other));
        
        /// Int is not equal to `other`
        public IAacFlCondition IsNotEqualTo(int other) => Just(condition => condition.Add(Name, NotEqual, other));

        /// This function is used for internal purposes:<br/>
        /// Returns the int value as a float.
        public override float ValueToFloat(int value)
        {
            return value;
        }
    }

    public class AacFlEnumIntParameter<TEnum> : AacFlIntParameter where TEnum : Enum
    {
        internal static AacFlEnumIntParameter<TInEnum> Internally<TInEnum>(string name) where TInEnum : Enum => new AacFlEnumIntParameter<TInEnum>(name);
        protected AacFlEnumIntParameter(string name) : base(name)
        {
        }

        /// Int is equal to `(int)other`
        public IAacFlCondition IsEqualTo(TEnum other) => IsEqualTo((int)(object)other);
        
        /// Int is not equal to `(int)other`
        public IAacFlCondition IsNotEqualTo(TEnum other) => IsNotEqualTo((int)(object)other);
    }

    public class AacFlBoolParameter : AacFlParameter<bool>
    {
        internal static AacFlBoolParameter Internally(string name) => new AacFlBoolParameter(name);
        protected AacFlBoolParameter(string name) : base(name) { }
        
        /// Bool is true
        public IAacFlCondition IsTrue() => Just(condition => condition.Add(Name, If, 0));
        
        /// Bool is false
        public IAacFlCondition IsFalse() => Just(condition => condition.Add(Name, IfNot, 0));
        
        /// Bool is equal to `other`
        public IAacFlCondition IsEqualTo(bool other) => Just(condition => condition.Add(Name, other ? If : IfNot, 0));
        
        /// Bool is not equal to `other`
        public IAacFlCondition IsNotEqualTo(bool other) => Just(condition => condition.Add(Name, other ? IfNot : If, 0));

        /// This function is used for internal purposes:<br/>
        /// Returns 1 when the value is true, 0 otherwise.
        public override float ValueToFloat(bool value)
        {
            return value ? 1f : 0f;
        }
    }

    public class AacFlFloatParameterGroup
    {
        internal static AacFlFloatParameterGroup Internally(params string[] names) => new AacFlFloatParameterGroup(names);
        private readonly string[] _names;
        private AacFlFloatParameterGroup(params string[] names) { _names = names; }
        public List<AacFlFloatParameter> ToList() => _names.Select(AacFlFloatParameter.Internally).ToList();

        /// All of the Floats are greater than `other`.<br/>
        /// When used on some platforms, you need to be careful as the remote value may not be the same as the local value.
        public IAacFlCondition AreGreaterThan(float other) => ForEach(_names, (name, condition) => condition.Add(name, Greater, other));
        /// All of the Floats are less than `other`.<br/>
        /// When used on some platforms, you need to be careful as the remote value may not be the same as the local value.
        public IAacFlCondition AreLessThan(float other) => ForEach(_names, (name, condition) => condition.Add(name, Less, other));
    }

    public class AacFlIntParameterGroup
    {
        internal static AacFlIntParameterGroup Internally(params string[] names) => new AacFlIntParameterGroup(names);
        private readonly string[] _names;
        private AacFlIntParameterGroup(params string[] names) { _names = names; }
        public List<AacFlIntParameter> ToList() => _names.Select(AacFlIntParameter.Internally).ToList();

        /// All of the Ints are strictly greater than `other`
        public IAacFlCondition AreGreaterThan(float other) => ForEach(_names, (name, condition) => condition.Add(name, Greater, other));
        /// All of the Ints are strictly less than `other`
        public IAacFlCondition AreLessThan(float other) => ForEach(_names, (name, condition) => condition.Add(name, Less, other));
        /// All of the Ints are equal to `other`
        public IAacFlCondition AreEqualTo(float other) => ForEach(_names, (name, condition) => condition.Add(name, AnimatorConditionMode.Equals, other));
        /// All of the Ints are not equal to `other`
        public IAacFlCondition AreNotEqualTo(float other) => ForEach(_names, (name, condition) => condition.Add(name, NotEqual, other));
    }

    public class AacFlBoolParameterGroup
    {
        internal static AacFlBoolParameterGroup Internally(params string[] names) => new AacFlBoolParameterGroup(names);
        private readonly string[] _names;
        private AacFlBoolParameterGroup(params string[] names) { _names = names; }
        public List<AacFlBoolParameter> ToList() => _names.Select(AacFlBoolParameter.Internally).ToList();

        /// All of the Bools are true
        public IAacFlCondition AreTrue() => ForEach(_names, (name, condition) => condition.Add(name, If, 0));
        
        /// All of the Bools are false
        public IAacFlCondition AreFalse() => ForEach(_names, (name, condition) => condition.Add(name, IfNot, 0));
        
        /// All of the Bools are equal to `other`
        public IAacFlCondition AreEqualTo(bool other) => ForEach(_names, (name, condition) => condition.Add(name, other ? If : IfNot, 0));

        /// All the Bools except `exceptThisMustBeTrue` are false, and the Bool of `exceptThisMustBeTrue` must be true.
        public IAacFlCondition AreFalseExcept(AacFlBoolParameter exceptThisMustBeTrue)
        {
            var group = new AacFlBoolParameterGroup(exceptThisMustBeTrue.Name);
            return AreFalseExcept(group);
        }

        /// All the Bools except those in `exceptTheseMustBeTrue` are false, and all of the Bools in `exceptTheseMustBeTrue` must be true.
        public IAacFlCondition AreFalseExcept(params AacFlBoolParameter[] exceptTheseMustBeTrue)
        {
            var group = new AacFlBoolParameterGroup(exceptTheseMustBeTrue.Select(parameter => parameter.Name).ToArray());
            return AreFalseExcept(group);
        }

        /// All the Bools except those in `exceptTheseMustBeTrue` are false, and all of the Bools in `exceptTheseMustBeTrue` must be true.
        public IAacFlCondition AreFalseExcept(AacFlBoolParameterGroup exceptTheseMustBeTrue) => Just(condition =>
        {
            foreach (var name in _names.Where(name => !exceptTheseMustBeTrue._names.Contains(name)))
            {
                condition.Add(name, IfNot, 0);
            }
            foreach (var name in exceptTheseMustBeTrue._names)
            {
                condition.Add(name, If, 0);
            }
        });

        /// All the Bools except `exceptThisMustBeTrue` are true, and the Bool of `exceptThisMustBeTrue` must be false.
        public IAacFlCondition AreTrueExcept(AacFlBoolParameter exceptThisMustBeFalse)
        {
            var group = new AacFlBoolParameterGroup(exceptThisMustBeFalse.Name);
            return AreTrueExcept(group);
        }

        /// All the Bools except those in `exceptTheseMustBeTrue` are true, and all of the Bools in `exceptTheseMustBeTrue` must be false.
        public IAacFlCondition AreTrueExcept(params AacFlBoolParameter[] exceptTheseMustBeFalse)
        {
            var group = new AacFlBoolParameterGroup(exceptTheseMustBeFalse.Select(parameter => parameter.Name).ToArray());
            return AreTrueExcept(group);
        }

        /// All the Bools except those in `exceptTheseMustBeTrue` are true, and all of the Bools in `exceptTheseMustBeTrue` must be false.
        public IAacFlCondition AreTrueExcept(AacFlBoolParameterGroup exceptTheseMustBeFalse) => Just(condition =>
        {
            foreach (var name in _names.Where(name => !exceptTheseMustBeFalse._names.Contains(name)))
            {
                condition.Add(name, If, 0);
            }
            foreach (var name in exceptTheseMustBeFalse._names)
            {
                condition.Add(name, IfNot, 0);
            }
        });

        /// Generates multiple transitions, verifying whether any Bool is true. This can only be used inside `.When(...)`
        public IAacFlOrCondition IsAnyTrue()
        {
            return IsAnyEqualTo(true);
        }

        /// Generates multiple transitions, verifying whether any Bool is false. This can only be used inside `.When(...)`
        public IAacFlOrCondition IsAnyFalse()
        {
            return IsAnyEqualTo(false);
        }

        private IAacFlOrCondition IsAnyEqualTo(bool value)
        {
            return new AacFlBoolParameterIsAnyOrCondition(_names, value);
        }
    }

    internal class AacFlBoolParameterIsAnyOrCondition : IAacFlOrCondition
    {
        private readonly string[] _names;
        private readonly bool _value;

        public AacFlBoolParameterIsAnyOrCondition(string[] names, bool value)
        {
            _names = names;
            _value = value;
        }

        public List<AacFlTransitionContinuation> ApplyTo(AacFlNewTransitionContinuation firstContinuation)
        {
            var pendingContinuations = new List<AacFlTransitionContinuation>();

            var newContinuation = firstContinuation;
            for (var index = 0; index < _names.Length; index++)
            {
                var name = _names[index];
                var pendingContinuation = newContinuation.When(AacFlBoolParameter.Internally(name).IsEqualTo(_value));
                pendingContinuations.Add(pendingContinuation);
                if (index < _names.Length - 1)
                {
                    newContinuation = pendingContinuation.Or();
                }
            }

            return pendingContinuations;
        }
    }
}
