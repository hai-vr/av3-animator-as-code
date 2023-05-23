using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Animations;
using static AnimatorAsCode.Framework.AacFlConditionSimple;
using static UnityEditor.Animations.AnimatorConditionMode;

// ReSharper disable once CheckNamespace
namespace AnimatorAsCode.Framework
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

        public static AacFlConditionSimple ForEach(
            string[] subjects,
            Action<string, AacFlCondition> action
        )
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

    /// <summary> Base idea of a parameter </summary>
    public abstract class AacFlParameter
    {
        public string Name { get; }

        protected AacFlParameter(string name)
        {
            Name = name;
        }
    }

    /// <summary> Float parameter </summary>
    public class AacFlFloatParameter : AacFlParameter
    {
        internal static AacFlFloatParameter Internally(string name) =>
            new AacFlFloatParameter(name);

        protected AacFlFloatParameter(string name)
            : base(name) { }

        /// <summary> Is true if this parameter is greater than the provided float </summary>
        /// <param name="other">The other parameter</param>
        /// <returns>Condition</returns>
        public IAacFlCondition IsGreaterThan(float other) =>
            Just(condition => condition.Add(Name, Greater, other));

        /// <summary> Is true if this parameter is less than the provided float </summary>
        /// <param name="other">The other parameter</param>
        /// <returns>Condition</returns>
        public IAacFlCondition IsLessThan(float other) =>
            Just(condition => condition.Add(Name, Less, other));
    }

    /// <summary> Integer parameter </summary>
    public class AacFlIntParameter : AacFlParameter
    {
        internal static AacFlIntParameter Internally(string name) => new AacFlIntParameter(name);

        protected AacFlIntParameter(string name)
            : base(name) { }

        /// <summary> Is true if this parameter is greater than the provided int </summary>
        /// <param name="other">The other parameter</param>
        /// <returns>Condition</returns>
        public IAacFlCondition IsGreaterThan(int other) =>
            Just(condition => condition.Add(Name, Greater, other));

        /// <summary> Is true if this parameter is less than the provided int </summary>
        /// <param name="other">The other parameter</param>
        /// <returns>Condition</returns>
        public IAacFlCondition IsLessThan(int other) =>
            Just(condition => condition.Add(Name, Less, other));

        /// <summary> Is true if this parameter is equal to the provided int </summary>
        /// <param name="other">The other parameter</param>
        /// <returns>Condition</returns>
        public IAacFlCondition IsEqualTo(int other) =>
            Just(condition => condition.Add(Name, AnimatorConditionMode.Equals, other));

        /// <summary> Is true if this parameter is not equal to the provided int </summary>
        /// <param name="other">The other parameter</param>
        /// <returns>Condition</returns>
        public IAacFlCondition IsNotEqualTo(int other) =>
            Just(condition => condition.Add(Name, NotEqual, other));
    }

    /// <summary> Enum parameter </summary>
    public class AacFlEnumIntParameter<TEnum> : AacFlIntParameter
        where TEnum : Enum
    {
        internal static AacFlEnumIntParameter<TInEnum> Internally<TInEnum>(string name)
            where TInEnum : Enum => new AacFlEnumIntParameter<TInEnum>(name);

        protected AacFlEnumIntParameter(string name)
            : base(name) { }

        /// <summary> Is true if this parameter is equal to the provided enum </summary>
        /// <param name="other">The other parameter</param>
        /// <returns>Condition</returns>
        public IAacFlCondition IsEqualTo(TEnum other) => IsEqualTo((int)(object)other);

        /// <summary> Is true if this parameter is not equal to the provided enum </summary>
        /// <param name="other">The other parameter</param>
        /// <returns>Condition</returns>
        public IAacFlCondition IsNotEqualTo(TEnum other) => IsNotEqualTo((int)(object)other);
    }

    /// <summary> Bool parameter </summary>
    public class AacFlBoolParameter : AacFlParameter
    {
        internal static AacFlBoolParameter Internally(string name) => new AacFlBoolParameter(name);

        protected AacFlBoolParameter(string name)
            : base(name) { }

        /// <summary> Is true if this parameter is true </summary>
        /// <returns>Condition</returns>
        public IAacFlCondition IsTrue() => Just(condition => condition.Add(Name, If, 0));

        /// <summary> Is true if this parameter is false </summary>
        /// <returns>Condition</returns>
        public IAacFlCondition IsFalse() => Just(condition => condition.Add(Name, IfNot, 0));

        /// <summary> Is true if this parameter is equal to the provided bool </summary>
        /// <param name="other">The other parameter</param>
        /// <returns>Condition</returns>
        public IAacFlCondition IsEqualTo(bool other) =>
            Just(condition => condition.Add(Name, other ? If : IfNot, 0));

        /// <summary> Is true if this parameter is not equal to the provided bool </summary>
        /// <param name="other">The other parameter</param>
        /// <returns>Condition</returns>
        public IAacFlCondition IsNotEqualTo(bool other) =>
            Just(condition => condition.Add(Name, other ? IfNot : If, 0));
    }

    /// <summary> Parameter group of floats </summary>
    public class AacFlFloatParameterGroup
    {
        internal static AacFlFloatParameterGroup Internally(params string[] names) =>
            new AacFlFloatParameterGroup(names);

        private readonly string[] _names;

        private AacFlFloatParameterGroup(params string[] names)
        {
            _names = names;
        }

        /// <summary> Converts this group to a list of float parameters </summary>
        /// <returns>List of float parameters</returns>
        public List<AacFlBoolParameter> ToList() =>
            _names.Select(AacFlBoolParameter.Internally).ToList();

        /// <summary> Is true if the entire group is greater than the provided float </summary>
        /// <param name="other">The other parameter</param>
        /// <returns>Condition</returns>
        public IAacFlCondition AreGreaterThan(float other) =>
            ForEach(_names, (name, condition) => condition.Add(name, Greater, other));

        /// <summary> Is true if the entire group is less than the provided float </summary>
        /// <param name="other">The other parameter</param>
        /// <returns>Condition</returns>
        public IAacFlCondition AreLessThan(float other) =>
            ForEach(_names, (name, condition) => condition.Add(name, Less, other));
    }

    /// <summary> Parameter group of ints </summary>
    public class AacFlIntParameterGroup
    {
        internal static AacFlIntParameterGroup Internally(params string[] names) =>
            new AacFlIntParameterGroup(names);

        private readonly string[] _names;

        private AacFlIntParameterGroup(params string[] names)
        {
            _names = names;
        }

        /// <summary> Converts this group to a list of int parameters </summary>
        /// <returns>List of int parameters</returns>
        public List<AacFlBoolParameter> ToList() =>
            _names.Select(AacFlBoolParameter.Internally).ToList();

        /// <summary> Is true if the entire group is greater than the provided float </summary>
        /// <param name="other">The other parameter</param>
        /// <returns>Condition</returns>
        public IAacFlCondition AreGreaterThan(float other) =>
            ForEach(_names, (name, condition) => condition.Add(name, Greater, other));

        /// <summary> Is true if the entire group is less than the provided float </summary>
        /// <param name="other">The other parameter</param>
        /// <returns>Condition</returns>
        public IAacFlCondition AreLessThan(float other) =>
            ForEach(_names, (name, condition) => condition.Add(name, Less, other));

        /// <summary> Is true if the entire group is equal to the provided float </summary>
        /// <param name="other">The other parameter</param>
        /// <returns>Condition</returns>
        public IAacFlCondition AreEqualTo(float other) =>
            ForEach(
                _names,
                (name, condition) => condition.Add(name, AnimatorConditionMode.Equals, other)
            );

        /// <summary> Is true if the entire group is not equal to the provided float </summary>
        /// <param name="other">The other parameter</param>
        /// <returns>Condition</returns>
        public IAacFlCondition AreNotEqualTo(float other) =>
            ForEach(_names, (name, condition) => condition.Add(name, NotEqual, other));
    }

    /// <summary> Parameter group of bools </summary>
    public class AacFlBoolParameterGroup
    {
        internal static AacFlBoolParameterGroup Internally(params string[] names) =>
            new AacFlBoolParameterGroup(names);

        private readonly string[] _names;

        private AacFlBoolParameterGroup(params string[] names)
        {
            _names = names;
        }

        /// <summary> Converts this group to a list of bool parameters </summary>
        /// <returns>List of bool parameters</returns>
        public List<AacFlBoolParameter> ToList() =>
            _names.Select(AacFlBoolParameter.Internally).ToList();

        /// <summary> Is true if the entire group is true </summary>
        /// <returns>Condition</returns>
        public IAacFlCondition AreTrue() =>
            ForEach(_names, (name, condition) => condition.Add(name, If, 0));

        /// <summary> Is true if the entire group is false </summary>
        /// <returns>Condition</returns>
        public IAacFlCondition AreFalse() =>
            ForEach(_names, (name, condition) => condition.Add(name, IfNot, 0));

        /// <summary> Is true if the entire group is equal to the provided bool </summary>
        /// <param name="other">The other parameter</param>
        /// <returns>Condition</returns>
        public IAacFlCondition AreEqualTo(bool other) =>
            ForEach(_names, (name, condition) => condition.Add(name, other ? If : IfNot, 0));

        /// <summary> Is true if the entire group is false, except the parameter defined in exceptThisMustBeTrue must be true </summary>
        /// <param name="exceptThisMustBeTrue">The parameter that must be true</param>
        /// <returns>Condition</returns>
        public IAacFlCondition AreFalseExcept(AacFlBoolParameter exceptThisMustBeTrue)
        {
            var group = new AacFlBoolParameterGroup(exceptThisMustBeTrue.Name);
            return AreFalseExcept(group);
        }

        /// <summary> Is true if the entire group is false, except the parameters defined in exceptTheseMustBeTrue must be true </summary>
        /// <param name="exceptTheseMustBeTrue">The parameters that must be true</param>
        /// <returns>Condition</returns>
        public IAacFlCondition AreFalseExcept(params AacFlBoolParameter[] exceptTheseMustBeTrue)
        {
            var group = new AacFlBoolParameterGroup(
                exceptTheseMustBeTrue.Select(parameter => parameter.Name).ToArray()
            );
            return AreFalseExcept(group);
        }

        /// <summary> Is true if the entire group is false, except the parameter group defined in exceptTheseMustBeTrue must be true </summary>
        /// <param name="exceptTheseMustBeTrue">The parameter group that must be true</param>
        /// <returns>Condition</returns>
        public IAacFlCondition AreFalseExcept(AacFlBoolParameterGroup exceptTheseMustBeTrue) =>
            Just(condition =>
            {
                foreach (
                    var name in _names.Where(name => !exceptTheseMustBeTrue._names.Contains(name))
                )
                {
                    condition.Add(name, IfNot, 0);
                }
                foreach (var name in exceptTheseMustBeTrue._names)
                {
                    condition.Add(name, If, 0);
                }
            });

        /// <summary> Is true if the entire group is true, except the parameter defined in exceptThisMustBeFalse must be false </summary>
        /// <param name="exceptThisMustBeFalse">The parameter that must be false</param>
        /// <returns>Condition</returns>
        public IAacFlCondition AreTrueExcept(AacFlBoolParameter exceptThisMustBeFalse)
        {
            var group = new AacFlBoolParameterGroup(exceptThisMustBeFalse.Name);
            return AreTrueExcept(group);
        }

        /// <summary> Is true if the entire group is true, except the parameters defined in exceptTheseMustBeFalse must be false </summary>
        /// <param name="exceptTheseMustBeFalse">The parameters that must be false</param>
        /// <returns>Condition</returns>
        public IAacFlCondition AreTrueExcept(params AacFlBoolParameter[] exceptTheseMustBeFalse)
        {
            var group = new AacFlBoolParameterGroup(
                exceptTheseMustBeFalse.Select(parameter => parameter.Name).ToArray()
            );
            return AreTrueExcept(group);
        }

        /// <summary> Is true if the entire group is true, except the parameter group defined in exceptTheseMustBeFalse must be false </summary>
        /// <param name="exceptTheseMustBeFalse">The parameter group that must be false</param>
        /// <returns>Condition</returns>
        public IAacFlCondition AreTrueExcept(AacFlBoolParameterGroup exceptTheseMustBeFalse) =>
            Just(condition =>
            {
                foreach (
                    var name in _names.Where(name => !exceptTheseMustBeFalse._names.Contains(name))
                )
                {
                    condition.Add(name, If, 0);
                }
                foreach (var name in exceptTheseMustBeFalse._names)
                {
                    condition.Add(name, IfNot, 0);
                }
            });

        /// <summary> Is true if any of the parameters in the group are true </summary>
        /// <returns>Condition</returns>
        public IAacFlOrCondition IsAnyTrue()
        {
            return IsAnyEqualTo(true);
        }

        /// <summary> Is true if any of the parameters in the group are false </summary>
        /// <returns>Condition</returns>
        public IAacFlOrCondition IsAnyFalse()
        {
            return IsAnyEqualTo(false);
        }

        /// <summary> Is true if any of the parameters in the group are equal to the provided bool </summary>
        /// <param name="value">The value to compare to</param>
        /// <returns>Condition</returns>
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

        public List<AacFlTransitionContinuation> ApplyTo(
            AacFlNewTransitionContinuation firstContinuation
        )
        {
            var pendingContinuations = new List<AacFlTransitionContinuation>();

            var newContinuation = firstContinuation;
            for (var index = 0; index < _names.Length; index++)
            {
                var name = _names[index];
                var pendingContinuation = newContinuation.When(
                    AacFlBoolParameter.Internally(name).IsEqualTo(_value)
                );
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
