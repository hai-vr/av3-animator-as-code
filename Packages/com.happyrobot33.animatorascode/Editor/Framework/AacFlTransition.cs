using System;
using System.Collections.Generic;
using UnityEditor.Animations;
using System.Reflection;

// ReSharper disable once CheckNamespace
namespace AnimatorAsCode.Framework
{
    public partial class AacFlTransition : AacFlNewTransitionContinuation
    {
        private static readonly PropertyInfo PropPushUndo =
            typeof(AnimatorTransitionBase).GetProperty(
                "pushUndo",
                BindingFlags.Instance | BindingFlags.NonPublic
            );
        private readonly AnimatorStateTransition _transition;

        public AacFlTransition(
            AnimatorStateTransition transition,
            AnimatorStateMachine machine,
            AnimatorState sourceNullableIfAny,
            AnimatorState destinationNullableIfExits
        )
            : base(transition, machine, sourceNullableIfAny, destinationNullableIfExits)
        {
            _transition = transition;
            PropPushUndo.SetValue(_transition, false);
        }

        public AacFlTransition WithSourceInterruption()
        {
            _transition.interruptionSource = TransitionInterruptionSource.Source;
            return this;
        }

        public AacFlTransition WithTransitionDurationSeconds(float transitionDuration)
        {
            _transition.duration = transitionDuration;
            return this;
        }

        public AacFlTransition WithOrderedInterruption()
        {
            _transition.orderedInterruption = true;
            return this;
        }

        public AacFlTransition WithNoOrderedInterruption()
        {
            _transition.orderedInterruption = false;
            return this;
        }

        public AacFlTransition WithTransitionToSelf()
        {
            _transition.canTransitionToSelf = true;
            return this;
        }

        public AacFlTransition WithNoTransitionToSelf()
        {
            _transition.canTransitionToSelf = false;
            return this;
        }

        public AacFlTransition AfterAnimationFinishes()
        {
            _transition.hasExitTime = true;
            _transition.exitTime = 1;

            return this;
        }

        public AacFlTransition AfterAnimationIsAtLeastAtPercent(float exitTimeNormalized)
        {
            _transition.hasExitTime = true;
            _transition.exitTime = exitTimeNormalized;

            return this;
        }

        public AacFlTransition WithTransitionDurationPercent(float transitionDurationNormalized)
        {
            _transition.hasFixedDuration = false;
            _transition.duration = transitionDurationNormalized;

            return this;
        }
    }

    public partial class AacFlEntryTransition : AacFlNewTransitionContinuation
    {
        public AacFlEntryTransition(
            AnimatorTransition transition,
            AnimatorStateMachine machine,
            AnimatorState sourceNullableIfAny,
            AnimatorState destinationNullableIfExits
        )
            : base(transition, machine, sourceNullableIfAny, destinationNullableIfExits) { }
    }

    public partial interface IAacFlCondition
    {
        void ApplyTo(AacFlCondition appender);
    }

    public partial interface IAacFlOrCondition
    {
        List<AacFlTransitionContinuation> ApplyTo(AacFlNewTransitionContinuation firstContinuation);
    }

    public partial class AacFlCondition
    {
        private static readonly PropertyInfo PropPushUndo =
            typeof(AnimatorTransitionBase).GetProperty(
                "pushUndo",
                BindingFlags.Instance | BindingFlags.NonPublic
            );
        private readonly AnimatorTransitionBase _transition;

        public AacFlCondition(AnimatorTransitionBase transition)
        {
            _transition = transition;
            PropPushUndo.SetValue(_transition, false);
        }

        public AacFlCondition Add(string parameter, AnimatorConditionMode mode, float threshold)
        {
            _transition.AddCondition(mode, threshold, parameter);
            return this;
        }
    }

    public partial class AacFlNewTransitionContinuation
    {
        public readonly AnimatorTransitionBase Transition;
        private readonly AnimatorStateMachine _machine;
        private readonly AnimatorState _sourceNullableIfAny;
        private readonly AnimatorState _destinationNullableIfExits;

        public AacFlNewTransitionContinuation(
            AnimatorTransitionBase transition,
            AnimatorStateMachine machine,
            AnimatorState sourceNullableIfAny,
            AnimatorState destinationNullableIfExits
        )
        {
            Transition = transition;
            _machine = machine;
            _sourceNullableIfAny = sourceNullableIfAny;
            _destinationNullableIfExits = destinationNullableIfExits;
        }

        /// Adds a condition to the transition.
        ///
        /// The settings of the transition can no longer be modified after this point.
        /// <example>
        /// <code>
        /// .When(_aac.BoolParameter(my.myBoolParameterName).IsTrue())
        /// .And(_aac.BoolParameter(my.myIntParameterName).IsGreaterThan(2))
        /// .And(AacAv3.ItIsLocal())
        /// .Or()
        /// .When(_aac.BoolParameters(
        ///     my.myBoolParameterName,
        ///     my.myOtherBoolParameterName
        /// ).AreTrue())
        /// .And(AacAv3.ItIsRemote());
        /// </code>
        /// </example>
        public AacFlTransitionContinuation When(IAacFlCondition action)
        {
            action.ApplyTo(new AacFlCondition(Transition));
            return AsContinuationWithOr();
        }

        /// <summary>
        /// Applies a series of conditions to this transition, but this series of conditions cannot include an Or operator.
        /// </summary>
        /// <param name="actionsWithoutOr"></param>
        /// <returns></returns>
        public AacFlTransitionContinuation When(
            Action<AacFlTransitionContinuationWithoutOr> actionsWithoutOr
        )
        {
            actionsWithoutOr(new AacFlTransitionContinuationWithoutOr(Transition));
            return AsContinuationWithOr();
        }

        /// <summary>
        /// Applies a series of conditions, and this series may contain Or operators. However, the result can not be followed by an And operator. It can only be an Or operator.
        /// </summary>
        /// <param name="actionsWithOr"></param>
        /// <returns></returns>
        public AacFlTransitionContinuationOnlyOr When(
            Action<AacFlNewTransitionContinuation> actionsWithOr
        )
        {
            actionsWithOr(this);
            return AsContinuationOnlyOr();
        }

        /// <summary>
        /// Applies a series of conditions, and this series may contain Or operators. All And operators that follow will apply to all the conditions generated by this series, until the next Or operator.
        /// </summary>
        /// <param name="actionsWithOr"></param>
        /// <returns></returns>
        public AacFlMultiTransitionContinuation When(IAacFlOrCondition actionsWithOr)
        {
            var pendingContinuations = actionsWithOr.ApplyTo(this);
            return new AacFlMultiTransitionContinuation(
                Transition,
                _machine,
                _sourceNullableIfAny,
                _destinationNullableIfExits,
                pendingContinuations
            );
        }

        public AacFlTransitionContinuation WhenConditions()
        {
            return AsContinuationWithOr();
        }

        private AacFlTransitionContinuation AsContinuationWithOr()
        {
            return new AacFlTransitionContinuation(
                Transition,
                _machine,
                _sourceNullableIfAny,
                _destinationNullableIfExits
            );
        }

        private AacFlTransitionContinuationOnlyOr AsContinuationOnlyOr()
        {
            return new AacFlTransitionContinuationOnlyOr(
                Transition,
                _machine,
                _sourceNullableIfAny,
                _destinationNullableIfExits
            );
        }
    }

    public partial class AacFlTransitionContinuation : AacFlTransitionContinuationAbstractWithOr
    {
        public AacFlTransitionContinuation(
            AnimatorTransitionBase transition,
            AnimatorStateMachine machine,
            AnimatorState sourceNullableIfAny,
            AnimatorState destinationNullableIfExits
        )
            : base(transition, machine, sourceNullableIfAny, destinationNullableIfExits) { }

        /// <summary> Set the Transitions name. </summary>
        /// <remarks> This is sometimes reffered to as a tag aswell. </remarks>
        /// <param name="name"> The name to use. </param>
        /// <returns> The current transition. </returns>
        public AacFlTransitionContinuation WithName(string name)
        {
            Transition.name = name;
            return this;
        }

        /// Adds an additional condition to the transition that requires all preceding conditions to be true.
        /// <example>
        /// <code>
        /// .When(_aac.BoolParameter(my.myBoolParameterName).IsTrue())
        /// .And(_aac.BoolParameter(my.myIntParameterName).IsGreaterThan(2))
        /// .And(AacAv3.ItIsLocal())
        /// .Or()
        /// .When(_aac.BoolParameters(
        ///     my.myBoolParameterName,
        ///     my.myOtherBoolParameterName
        /// ).AreTrue())
        /// .And(AacAv3.ItIsRemote());
        /// </code>
        /// </example>
        public AacFlTransitionContinuation And(IAacFlCondition action)
        {
            action.ApplyTo(new AacFlCondition(Transition));
            return this;
        }

        /// <summary>
        /// Applies a series of conditions to this transition. The conditions cannot include an Or operator.
        /// </summary>
        /// <param name="actionsWithoutOr"></param>
        /// <returns></returns>
        public AacFlTransitionContinuation And(
            Action<AacFlTransitionContinuationWithoutOr> actionsWithoutOr
        )
        {
            actionsWithoutOr(new AacFlTransitionContinuationWithoutOr(Transition));
            return this;
        }
    }

    public partial class AacFlMultiTransitionContinuation
        : AacFlTransitionContinuationAbstractWithOr
    {
        private readonly List<AacFlTransitionContinuation> _pendingContinuations;

        public AacFlMultiTransitionContinuation(
            AnimatorTransitionBase transition,
            AnimatorStateMachine machine,
            AnimatorState sourceNullableIfAny,
            AnimatorState destinationNullableIfExits,
            List<AacFlTransitionContinuation> pendingContinuations
        )
            : base(transition, machine, sourceNullableIfAny, destinationNullableIfExits)
        {
            _pendingContinuations = pendingContinuations;
        }

        /// Adds an additional condition to these transitions that requires all preceding conditions to be true.
        /// <example>
        /// <code>
        /// .When(_aac.BoolParameter(my.myBoolParameterName).IsTrue())
        /// .And(_aac.BoolParameter(my.myIntParameterName).IsGreaterThan(2))
        /// .And(AacAv3.ItIsLocal())
        /// .Or()
        /// .When(_aac.BoolParameters(
        ///     my.myBoolParameterName,
        ///     my.myOtherBoolParameterName
        /// ).AreTrue())
        /// .And(AacAv3.ItIsRemote());
        /// </code>
        /// </example>
        public AacFlMultiTransitionContinuation And(IAacFlCondition action)
        {
            foreach (var pendingContinuation in _pendingContinuations)
            {
                pendingContinuation.And(action);
            }

            return this;
        }

        /// <summary>
        /// Applies a series of conditions to these transitions. The conditions cannot include an Or operator.
        /// </summary>
        /// <param name="actionsWithoutOr"></param>
        /// <returns></returns>
        public AacFlMultiTransitionContinuation And(
            Action<AacFlTransitionContinuationWithoutOr> actionsWithoutOr
        )
        {
            foreach (var pendingContinuation in _pendingContinuations)
            {
                pendingContinuation.And(actionsWithoutOr);
            }

            return this;
        }
    }

    public partial class AacFlTransitionContinuationOnlyOr
        : AacFlTransitionContinuationAbstractWithOr
    {
        public AacFlTransitionContinuationOnlyOr(
            AnimatorTransitionBase transition,
            AnimatorStateMachine machine,
            AnimatorState sourceNullableIfAny,
            AnimatorState destinationNullableIfExits
        )
            : base(transition, machine, sourceNullableIfAny, destinationNullableIfExits) { }
    }

    public abstract partial class AacFlTransitionContinuationAbstractWithOr
    {
        protected readonly AnimatorTransitionBase Transition;
        private readonly AnimatorStateMachine _machine;
        private readonly AnimatorState _sourceNullableIfAny;
        private readonly AnimatorState _destinationNullableIfExits;

        public AacFlTransitionContinuationAbstractWithOr(
            AnimatorTransitionBase transition,
            AnimatorStateMachine machine,
            AnimatorState sourceNullableIfAny,
            AnimatorState destinationNullableIfExits
        )
        {
            Transition = transition;
            _machine = machine;
            _sourceNullableIfAny = sourceNullableIfAny;
            _destinationNullableIfExits = destinationNullableIfExits;
        }

        /// <summary>
        /// Creates a new transition with identical settings but having no conditions defined yet.
        /// </summary>
        /// <example>
        /// <code>
        /// .When(_aac.BoolParameter(my.myBoolParameterName).IsTrue())
        /// .And(_aac.BoolParameter(my.myIntParameterName).IsGreaterThan(2))
        /// .And(AacAv3.ItIsLocal())
        /// .Or()
        /// .When(_aac.BoolParameters(
        ///     my.myBoolParameterName,
        ///     my.myOtherBoolParameterName
        /// ).AreTrue())
        /// .And(AacAv3.ItIsRemote());
        /// </code>
        /// </example>
        public AacFlNewTransitionContinuation Or()
        {
            return new AacFlNewTransitionContinuation(
                NewTransitionFromTemplate(),
                _machine,
                _sourceNullableIfAny,
                _destinationNullableIfExits
            );
        }

        private AnimatorTransitionBase NewTransitionFromTemplate()
        {
            AnimatorTransitionBase newTransition;
            if (Transition is AnimatorStateTransition templateStateTransition)
            {
                var stateTransition = NewTransition();
                stateTransition.duration = templateStateTransition.duration;
                stateTransition.offset = templateStateTransition.offset;
                stateTransition.interruptionSource = templateStateTransition.interruptionSource;
                stateTransition.orderedInterruption = templateStateTransition.orderedInterruption;
                stateTransition.exitTime = templateStateTransition.exitTime;
                stateTransition.hasExitTime = templateStateTransition.hasExitTime;
                stateTransition.hasFixedDuration = templateStateTransition.hasFixedDuration;
                stateTransition.canTransitionToSelf = templateStateTransition.canTransitionToSelf;
                newTransition = stateTransition;
            }
            else
            {
                newTransition = _machine.AddEntryTransition(_destinationNullableIfExits);
            }

            return newTransition;
        }

        private AnimatorStateTransition NewTransition()
        {
            if (_sourceNullableIfAny == null)
            {
                return _machine.AddAnyStateTransition(_destinationNullableIfExits);
            }

            if (_destinationNullableIfExits == null)
            {
                return _sourceNullableIfAny.AddExitTransition();
            }

            return _sourceNullableIfAny.AddTransition(_destinationNullableIfExits);
        }
    }

    public partial class AacFlTransitionContinuationWithoutOr
    {
        private readonly AnimatorTransitionBase _transition;

        public AacFlTransitionContinuationWithoutOr(AnimatorTransitionBase transition)
        {
            _transition = transition;
        }

        public AacFlTransitionContinuationWithoutOr And(IAacFlCondition action)
        {
            action.ApplyTo(new AacFlCondition(_transition));
            return this;
        }

        /// <summary>
        /// Applies a series of conditions to this transition. The conditions cannot include an Or operator.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public AacFlTransitionContinuationWithoutOr AndWhenever(
            Action<AacFlTransitionContinuationWithoutOr> action
        )
        {
            action(this);
            return this;
        }
    }
}
