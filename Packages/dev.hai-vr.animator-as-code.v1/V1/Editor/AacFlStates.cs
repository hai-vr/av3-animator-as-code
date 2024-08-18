using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEditor.Animations;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace AnimatorAsCode.V1
{
    public class AacBackingAnimator
    {
        private readonly AacAnimatorGenerator _generator;

        internal AacBackingAnimator(AacAnimatorGenerator animatorGenerator)
        {
            _generator = animatorGenerator;
        }

        public AacFlBoolParameter BoolParameter(string parameterName)
        {
            var result = AacFlBoolParameter.Internally(parameterName);
            _generator.CreateParamsAsNeeded(result);
            return result;
        }

        public AacFlBoolParameter TriggerParameter(string parameterName)
        {
            var result = AacFlBoolParameter.Internally(parameterName);
            _generator.CreateTriggerParamsAsNeeded(result);
            return result;
        }

        public AacFlFloatParameter FloatParameter(string parameterName)
        {
            var result = AacFlFloatParameter.Internally(parameterName);
            _generator.CreateParamsAsNeeded(result);
            return result;
        }

        public AacFlIntParameter IntParameter(string parameterName)
        {
            var result = AacFlIntParameter.Internally(parameterName);
            _generator.CreateParamsAsNeeded(result);
            return result;
        }

        public AacFlEnumIntParameter<TEnum> EnumParameter<TEnum>(string parameterName) where TEnum : Enum
        {
            var result = AacFlEnumIntParameter<TEnum>.Internally<TEnum>(parameterName);
            _generator.CreateParamsAsNeeded(result);
            return result;
        }

        public AacFlBoolParameterGroup BoolParameters(params string[] parameterNames)
        {
            var result = AacFlBoolParameterGroup.Internally(parameterNames);
            _generator.CreateParamsAsNeeded(result.ToList().ToArray());
            return result;
        }

        public AacFlBoolParameterGroup TriggerParameters(params string[] parameterNames)
        {
            var result = AacFlBoolParameterGroup.Internally(parameterNames);
            _generator.CreateTriggerParamsAsNeeded(result.ToList().ToArray());
            return result;
        }

        public AacFlFloatParameterGroup FloatParameters(params string[] parameterNames)
        {
            var result = AacFlFloatParameterGroup.Internally(parameterNames);
            _generator.CreateParamsAsNeeded(result.ToList().ToArray());
            return result;
        }

        public AacFlIntParameterGroup IntParameters(params string[] parameterNames)
        {
            var result = AacFlIntParameterGroup.Internally(parameterNames);
            _generator.CreateParamsAsNeeded(result.ToList().ToArray());
            return result;
        }

        public AacFlBoolParameterGroup BoolParameters(params AacFlBoolParameter[] parameters)
        {
            var result = AacFlBoolParameterGroup.Internally(parameters.Select(parameter => parameter.Name).ToArray());
            _generator.CreateParamsAsNeeded(parameters);
            return result;
        }

        public AacFlBoolParameterGroup TriggerParameters(params AacFlBoolParameter[] parameters)
        {
            var result = AacFlBoolParameterGroup.Internally(parameters.Select(parameter => parameter.Name).ToArray());
            _generator.CreateTriggerParamsAsNeeded(parameters);
            return result;
        }

        public AacFlFloatParameterGroup FloatParameters(params AacFlFloatParameter[] parameters)
        {
            var result = AacFlFloatParameterGroup.Internally(parameters.Select(parameter => parameter.Name).ToArray());
            _generator.CreateParamsAsNeeded(parameters);
            return result;
        }

        public AacFlIntParameterGroup IntParameters(params AacFlIntParameter[] parameters)
        {
            var result = AacFlIntParameterGroup.Internally(parameters.Select(parameter => parameter.Name).ToArray());
            _generator.CreateParamsAsNeeded(parameters);
            return result;
        }
    }

    public class AacFlStateMachine : AacAnimatorNode<AacFlStateMachine>
    {
        /// Exposes the underlying Unity AnimatorStateMachine object of this state machine.
        [PublicAPI] public AnimatorStateMachine Machine { get; }

        private readonly AnimationClip _emptyClip;
        private readonly AacBackingAnimator _backingAnimator;
        private readonly IAacDefaultsProvider _defaultsProvider;
        private readonly float _gridShiftX;
        private readonly float _gridShiftY;

        private readonly List<AacAnimatorNode> _childNodes;

        private readonly HashSet<string> _stateNames;

        internal AacFlStateMachine(AnimatorStateMachine machine, AnimationClip emptyClip, AacBackingAnimator backingAnimator, IAacDefaultsProvider defaultsProvider, Transform animatorRoot, AacFlStateMachine parent = null)
            : base(parent, defaultsProvider, animatorRoot)
        {
            Machine = machine;
            _emptyClip = emptyClip;
            _backingAnimator = backingAnimator;
            _defaultsProvider = defaultsProvider;
            _stateNames = new HashSet<string>();

            var grid = defaultsProvider.Grid();
            _gridShiftX = grid.x;
            _gridShiftY = grid.y;

            _childNodes = new List<AacAnimatorNode>();
        }

        /// <b>FOR USE ONLY BY EXTENSION FUNCTIONS:</b><br/>
        /// Exposes the internal backing animator.
        public AacBackingAnimator InternalBackingAnimator()
        {
            return _backingAnimator;
        }

        /// Create a new state machine, initially positioned below the last generated state of this layer.<br/>
        /// 🔺 If the name is already used, a number will be appended at the end.
        public AacFlStateMachine NewSubStateMachine(string name)
        {
            var lastState = LastNodePosition();
            return NewSubStateMachine(name, 0, 0).Shift(lastState, 0, 1);
        }

        /// Create a new state machine at a specific position `x` and `y`, in grid units. The grid size is defined in the DefaultsProvider of the AacConfiguration of AAC. `x` positive goes right, `y` positive goes down.<br/>
        /// 🔺 If the name is already used, a number will be appended at the end.
        public AacFlStateMachine NewSubStateMachine(string name, int x, int y)
        {
            var stateMachine = AacInternals.NoUndo(Machine, () => Machine.AddStateMachine(SanitizeAndEnsureNameIsDeduplicated(name), GridPosition(x, y)));
            var aacMachine = new AacFlStateMachine(stateMachine, _emptyClip, _backingAnimator, DefaultsProvider, AnimatorRoot, this);
            _defaultsProvider.ConfigureStateMachine(stateMachine);
            _childNodes.Add(aacMachine);
            return aacMachine;
        }

        public AacFlStateMachine WithEntryPosition(int x, int y)
        {
            Machine.entryPosition = GridPosition(x, y);
            return this;
        }

        public AacFlStateMachine WithExitPosition(int x, int y)
        {
            Machine.exitPosition = GridPosition(x, y);
            return this;
        }

        public AacFlStateMachine WithAnyStatePosition(int x, int y)
        {
            Machine.anyStatePosition = GridPosition(x, y);
            return this;
        }

        public AacFlStateMachine WithParentStateMachinePosition(int x, int y)
        {
            Machine.parentStateMachinePosition = GridPosition(x, y);
            return this;
        }

        /// Create a new state, initially positioned below the last generated state of this layer.<br/>
        /// 🔺 If the name is already used, a number will be appended at the end.
        public AacFlState NewState(string name)
        {
            var lastState = LastNodePosition();
            return NewState(name, 0, 0).Shift(lastState, 0, 1);
        }

        /// Create a new state at a specific position `x` and `y`, in grid units. The grid size is defined in the DefaultsProvider of the AacConfiguration of AAC. `x` positive goes right, `y` positive goes down.<br/>
        /// 🔺 If the name is already used, a number will be appended at the end.
        public AacFlState NewState(string name, int x, int y)
        {
            var state = AacInternals.NoUndo(Machine, () => Machine.AddState(SanitizeAndEnsureNameIsDeduplicated(name), GridPosition(x, y)));
            DefaultsProvider.ConfigureState(state, _emptyClip);
            var aacState = new AacFlState(state, this, DefaultsProvider, AnimatorRoot);
            _childNodes.Add(aacState);

            return aacState;
        }

        private string SanitizeAndEnsureNameIsDeduplicated(string name)
        {
            name = SanitizeName(name);
            
            var suffix = 0;
            var stateName = name;
            while (_stateNames.Contains(stateName))
            {
                stateName = name + " " + (++suffix);
            }

            _stateNames.Add(stateName);
            return stateName;
        }

        private string SanitizeName(string name)
        {
            // If the name contains a period ".", it can cause the animator to misbehave
            // as in, transitions will not work properly.
            // incredible but true
            // Apparently this is because substates use the dot as a separator or something
            // Sanitize the name so that menu state names such as "J. Inner" don't mess things up
            // return Regex.Replace(controlName, "[^A-Za-z0-9]", "");
            return name.Replace(".", "_");
        }

        /// Create a transition from Any to the `destination` state.
        public AacFlTransition AnyTransitionsTo(AacFlState destination)
        {
            // Sub-state machines "cannot" have Any state transitions created directly from them.
            // Internally, Any always comes from the root state machine, but visually in the graph, it will come from the sub-state machine.
            return AnyTransition(destination, RootMachine().Machine);
        }

        /// Create a transition from Any to the `destination` state machine.
        public AacFlTransition AnyTransitionsTo(AacFlStateMachine destination)
        {
            return AnyTransition(destination, Machine);
        }

        /// Create a transition from the Entry to the `destination` state.
        public AacFlEntryTransition EntryTransitionsTo(AacFlState destination)
        {
            return EntryTransition(destination, Machine);
        }

        /// Create a transition from the Entry to the `destination` state machine.
        public AacFlEntryTransition EntryTransitionsTo(AacFlStateMachine destination)
        {
            return EntryTransition(destination, Machine);
        }

        public AacFlEntryTransition TransitionsFromEntry()
        {
            return EntryTransition(this, ParentMachine.Machine);
        }

        public AacFlNewTransitionContinuation TransitionsTo(AacFlState destination)
        {
            return new AacFlNewTransitionContinuation(AacInternals.NoUndo(ParentMachine.Machine, () => ParentMachine.Machine.AddStateMachineTransition(Machine, destination.State)), ParentMachine.Machine, Machine, destination.State);
        }

        public AacFlNewTransitionContinuation TransitionsTo(AacFlStateMachine destination)
        {
            return new AacFlNewTransitionContinuation(AacInternals.NoUndo(ParentMachine.Machine, () => ParentMachine.Machine.AddStateMachineTransition(Machine, destination.Machine)), ParentMachine.Machine, Machine, destination.Machine);
        }

        public AacFlNewTransitionContinuation Restarts()
        {
            return new AacFlNewTransitionContinuation(AacInternals.NoUndo(ParentMachine.Machine, () => ParentMachine.Machine.AddStateMachineTransition(Machine, Machine)), ParentMachine.Machine, Machine, Machine);
        }

        public AacFlNewTransitionContinuation Exits()
        {
            return new AacFlNewTransitionContinuation(AacInternals.NoUndo(ParentMachine.Machine, () => ParentMachine.Machine.AddStateMachineExitTransition(Machine)), ParentMachine.Machine, Machine, null);
        }

        private AacFlTransition AnyTransition(AacFlState destination, AnimatorStateMachine animatorStateMachine)
        {
            return new AacFlTransition(ConfigureTransition(AacInternals.NoUndo(animatorStateMachine, () => animatorStateMachine.AddAnyStateTransition(destination.State))), animatorStateMachine, null, destination.State);
        }

        private AacFlTransition AnyTransition(AacFlStateMachine destination, AnimatorStateMachine animatorStateMachine)
        {
            return new AacFlTransition(ConfigureTransition(AacInternals.NoUndo(animatorStateMachine, () => animatorStateMachine.AddAnyStateTransition(destination.Machine))), animatorStateMachine, null, destination.Machine);
        }

        private AnimatorStateTransition ConfigureTransition(AnimatorStateTransition transition)
        {
            DefaultsProvider.ConfigureTransition(transition);
            return transition;
        }

        private AacFlEntryTransition EntryTransition(AacFlState destination, AnimatorStateMachine animatorStateMachine)
        {
            return new AacFlEntryTransition(AacInternals.NoUndo(animatorStateMachine, () => animatorStateMachine.AddEntryTransition(destination.State)), animatorStateMachine, null, destination.State);
        }

        private AacFlEntryTransition EntryTransition(AacFlStateMachine destination, AnimatorStateMachine animatorStateMachine)
        {
            return new AacFlEntryTransition(AacInternals.NoUndo(animatorStateMachine, () => animatorStateMachine.AddEntryTransition(destination.Machine)), animatorStateMachine, null, destination.Machine);
        }

        internal Vector3 LastNodePosition()
        {
            return _childNodes.LastOrDefault()?.GetPosition() ?? Vector3.right * _gridShiftX * 2;
        }

        private Vector3 GridPosition(int x, int y)
        {
            return new Vector3(x * _gridShiftX, y * _gridShiftY, 0);
        }

        internal IReadOnlyList<AacAnimatorNode> GetChildNodes()
        {
            return _childNodes;
        }

        protected internal override Vector3 GetPosition()
        {
            return ParentMachine.Machine.stateMachines.First(x => x.stateMachine == Machine).position;
        }

        protected internal override void SetPosition(Vector3 position)
        {
            var stateMachines = ParentMachine.Machine.stateMachines;
            for (var i = 0; i < stateMachines.Length; i++)
            {
                var m = stateMachines[i];
                if (m.stateMachine == Machine)
                {
                    m.position = position;
                    stateMachines[i] = m;
                    break;
                }
            }
            ParentMachine.Machine.stateMachines = stateMachines;
        }

        public AacFlStateMachine WithDefaultState(AacFlState newDefaultState)
        {
            Machine.defaultState = newDefaultState.State;
            return this;
        }

        public override TBehaviour EnsureBehaviour<TBehaviour>()
        {
            foreach (var behaviour in Machine.behaviours)
                if (behaviour is TBehaviour myBehaviour)
                    return myBehaviour;

            return AacInternals.NoUndo(Machine, () => Machine.AddStateMachineBehaviour<TBehaviour>());
        }

        public override TBehaviour CreateNewBehaviour<TBehaviour>()
        {
            return AacInternals.NoUndo(Machine, () => Machine.AddStateMachineBehaviour<TBehaviour>());
        }
    }

    public class AacFlState : AacAnimatorNode<AacFlState>
    {
        /// Exposes the underlying Unity AnimatorState of this state.
        [PublicAPI] public AnimatorState State { get; }
        
        private readonly AnimatorStateMachine _machine;

        internal AacFlState(AnimatorState state, AacFlStateMachine parentMachine, IAacDefaultsProvider defaultsProvider, Transform animatorRoot) : base(parentMachine, defaultsProvider, animatorRoot)
        {
            State = state;
            _machine = parentMachine.Machine;
        }

        public AacFlState WithAnimation(Motion clip)
        {
            State.motion = clip;
            return this;
        }

        public AacFlState WithAnimation(AacFlClip clip)
        {
            State.motion = clip.Clip;
            return this;
        }

        public AacFlState WithAnimation(AacFlBlendTree blendTree)
        {
            State.motion = blendTree.BlendTree;
            return this;
        }

        /// Create a new transition from this state to the `destination` state.
        public AacFlTransition TransitionsTo(AacFlState destination)
        {
            return new AacFlTransition(ConfigureTransition(AacInternals.NoUndo(State, () => State.AddTransition(destination.State))), _machine, State, destination.State);
        }

        /// Create a new transition from this state to the `destination` state machine.
        public AacFlTransition TransitionsTo(AacFlStateMachine destination)
        {
            return new AacFlTransition(ConfigureTransition(AacInternals.NoUndo(State, () => State.AddTransition(destination.Machine))), _machine, State, destination.Machine);
        }

        /// Create a new transition from Any to this state.
        public AacFlTransition TransitionsFromAny()
        {
            // Sub-state machines "cannot" have Any state transitions created directly from them.
            // Internally, Any always comes from the root state machine, but visually in the graph, it will come from the sub-state machine.
            var rootMachine = RootMachine().Machine;
            return new AacFlTransition(ConfigureTransition(AacInternals.NoUndo(State, () => rootMachine.AddAnyStateTransition(State))), rootMachine, null, State);
        }

        /// Create a new transition from Entry to this state. Note that the first created state is the default state, so generally this function does not need to be invoked onto the first created state.<br/>
        /// Calling this function will not define this state to be the default state.
        public AacFlEntryTransition TransitionsFromEntry()
        {
            return new AacFlEntryTransition(AacInternals.NoUndo(State, () => _machine.AddEntryTransition(State)), _machine, null, State);
        }

        /// Create a transition with no exit time to the `destination` state.<br/>
        /// Calling this function does not return the transition.
        public AacFlState AutomaticallyMovesTo(AacFlState destination)
        {
            var transition = ConfigureTransition(AacInternals.NoUndo(State, () => State.AddTransition(destination.State)));
            transition.hasExitTime = true;
            return this;
        }

        /// Create a transition with no exit time to the `destination` state machine.<br/>
        /// Calling this function does not return the transition.
        public AacFlState AutomaticallyMovesTo(AacFlStateMachine destination)
        {
            var transition = ConfigureTransition(AacInternals.NoUndo(State, () => State.AddTransition(destination.Machine)));
            transition.hasExitTime = true;
            return this;
        }

        /// Create a transition from this state to the exit.
        public AacFlTransition Exits()
        {
            return new AacFlTransition(ConfigureTransition(AacInternals.NoUndo(State, () => State.AddExitTransition())), _machine, State, null);
        }

        private AnimatorStateTransition ConfigureTransition(AnimatorStateTransition transition)
        {
            DefaultsProvider.ConfigureTransition(transition);
            return transition;
        }

        /// Set Write Defaults. If you need to do this to many states, consider changing the AacConfiguration DefaultsProvider when creating the AnimatorAsCode instance.
        public AacFlState WithWriteDefaultsSetTo(bool shouldWriteDefaults)
        {
            State.writeDefaultValues = shouldWriteDefaults;
            return this;
        }

        /// Obsolete. Use `WithMotionTime()` instead.<br/>
        /// Set the Motion Time to a parameter. This was formerly known as Normalized Time.<br/>
        /// <br/>
        /// This function is preserved for compatibility.<br/>
        /// The behaviour is identical to `WithMotionTime()`.
        [Obsolete("This function was renamed to WithMotionTime(AacFlFloatParameter)")]
        public AacFlState MotionTime(AacFlFloatParameter floatParam)
        {
            return WithMotionTime(floatParam);
        }
        
        /// Set the Motion Time to a parameter. This was formerly known as Normalized Time.
        public AacFlState WithMotionTime(AacFlFloatParameter floatParam)
        {
            State.timeParameterActive = true;
            State.timeParameter = floatParam.Name;

            return this;
        }

        /// Set the Cycle Offset to a parameter.
        public AacFlState WithCycleOffset(AacFlFloatParameter floatParam)
        {
            State.cycleOffsetParameterActive = true;
            State.cycleOffsetParameter = floatParam.Name;

            return this;
        }

        /// Set the Cycle Offset to a specific value.
        public AacFlState WithCycleOffsetSetTo(float cycleOffset)
        {
            State.cycleOffsetParameterActive = false;
            State.cycleOffset = cycleOffset;

            return this;
        }

        /// Set the Speed to a parameter.
        public AacFlState WithSpeed(AacFlFloatParameter parameter)
        {
            State.speedParameterActive = true;
            State.speedParameter = parameter.Name;

            return this;
        }

        /// Set the Speed to a specific value.
        public AacFlState WithSpeedSetTo(float speed)
        {
            State.speedParameterActive = false;
            State.speed = speed;

            return this;
        }

        protected internal override Vector3 GetPosition()
        {
            return _machine.states.First(x => x.state == State).position;
        }

        protected internal override void SetPosition(Vector3 position)
        {
            var states = _machine.states;
            for (var i = 0; i < states.Length; i++)
            {
                var m = states[i];
                if (m.state == State)
                {
                    m.position = position;
                    states[i] = m;
                    break;
                }
            }
            _machine.states = states;
        }

        public override TBehaviour EnsureBehaviour<TBehaviour>()
        {
            foreach (var behaviour in State.behaviours)
                if (behaviour is TBehaviour myBehaviour)
                    return myBehaviour;

            return AacInternals.NoUndo(State, () => State.AddStateMachineBehaviour<TBehaviour>());
        }

        public override TBehaviour CreateNewBehaviour<TBehaviour>()
        {
            return AacInternals.NoUndo(State, () => State.AddStateMachineBehaviour<TBehaviour>());
        }
    }

    public class AacFlTransition : AacFlNewTransitionContinuation
    {
        private readonly AnimatorStateTransition _transition;

        internal AacFlTransition(AnimatorStateTransition transition, AnimatorStateMachine machine, AacTransitionEndpoint sourceNullableIfAny, AacTransitionEndpoint destinationNullableIfExits) : base(transition, machine, sourceNullableIfAny, destinationNullableIfExits)
        {
            _transition = transition;
        }

        /// Set interruption source to be Source.
        public AacFlTransition WithSourceInterruption()
        {
            _transition.interruptionSource = TransitionInterruptionSource.Source;
            return this;
        }

        /// Set interruption source set to that value.
        public AacFlTransition WithInterruption(TransitionInterruptionSource interruptionSource)
        {
            _transition.interruptionSource = interruptionSource;
            return this;
        }

        /// Set a fixed transition duration in seconds.
        public AacFlTransition WithTransitionDurationSeconds(float transitionDuration)
        {
            _transition.duration = transitionDuration;
            return this;
        }

        /// Enable ordered interruption.
        public AacFlTransition WithOrderedInterruption()
        {
            _transition.orderedInterruption = true;
            return this;
        }

        /// Disable ordered interruption.
        public AacFlTransition WithNoOrderedInterruption()
        {
            _transition.orderedInterruption = false;
            return this;
        }

        /// Enable transition to self.
        public AacFlTransition WithTransitionToSelf()
        {
            _transition.canTransitionToSelf = true;
            return this;
        }

        /// Disable transition to self.
        public AacFlTransition WithNoTransitionToSelf()
        {
            _transition.canTransitionToSelf = false;
            return this;
        }

        /// Set an exit time at 1, where the animation finishes.
        public AacFlTransition AfterAnimationFinishes()
        {
            _transition.hasExitTime = true;
            _transition.exitTime = 1;

            return this;
        }

        /// Set an exit time at 0, so that it may transition almost immediately.
        public AacFlTransition Automatically()
        {
            _transition.hasExitTime = true;
            _transition.exitTime = 0;

            return this;
        }
        
        /// Set the exit time at a specific normalized amount.
        public AacFlTransition AfterAnimationIsAtLeastAtNormalized(float exitTimeNormalized)
        {
            return AfterAnimationIsAtLeastAtPercent(exitTimeNormalized);
        }
        
        /// Set a non-fixed transition duration in a normalized amount.
        public AacFlTransition WithTransitionDurationNormalized(float transitionDurationNormalized)
        {
            return WithTransitionDurationPercent(transitionDurationNormalized);
        }

        /// Set the exit time at a specific normalized amount.<br/>
        /// <br/>
        /// Note: Percent is a misnomer. You are expected to provide a value expressed as a normalized value (where 1 represents the clip duration).<br/>
        /// This function behaves identically to `AfterAnimationIsAtLeastAtNormalized(float)`
        public AacFlTransition AfterAnimationIsAtLeastAtPercent(float exitTimeNormalized)
        {
            _transition.hasExitTime = true;
            _transition.exitTime = exitTimeNormalized;

            return this;
        }

        /// Set a non-fixed transition duration in a normalized amount.<br/>
        /// <br/>
        /// Note: Percent is a misnomer. You are expected to provide a value expressed as a normalized value (where 1 represents the clip duration).<br/>
        /// This function behaves identically to `WithTransitionDurationNormalized(float)`
        public AacFlTransition WithTransitionDurationPercent(float transitionDurationNormalized)
        {
            _transition.hasFixedDuration = false;
            _transition.duration = transitionDurationNormalized;

            return this;
        }
    }

    public class AacFlEntryTransition : AacFlNewTransitionContinuation
    {
        internal AacFlEntryTransition(AnimatorTransition transition, AnimatorStateMachine machine, AnimatorState sourceNullableIfAny, AacTransitionEndpoint destinationNullableIfExits) : base(transition, machine, sourceNullableIfAny, destinationNullableIfExits)
        {
        }
    }

    public interface IAacFlCondition
    {
        void ApplyTo(AacFlCondition appender);
    }

    public interface IAacFlOrCondition
    {
        List<AacFlTransitionContinuation> ApplyTo(AacFlNewTransitionContinuation firstContinuation);
    }

    public class AacFlCondition
    {
        private readonly AnimatorTransitionBase _transition;

        internal AacFlCondition(AnimatorTransitionBase transition)
        {
            _transition = transition;
        }

        public AacFlCondition Add(string parameter, AnimatorConditionMode mode, float threshold)
        {
            AacInternals.NoUndo(_transition, () => _transition.AddCondition(mode, threshold, parameter));
            return this;
        }
    }

    public class AacFlNewTransitionContinuation
    {
        /// Exposes the underlying Unity AnimatorTransitionBase of this transition.
        [PublicAPI] public AnimatorTransitionBase Transition { get; }
        
        private readonly AnimatorStateMachine _machine;
        private readonly AacTransitionEndpoint _sourceNullableIfAny;
        private readonly AacTransitionEndpoint _destinationNullableIfExits;

        internal AacFlNewTransitionContinuation(AnimatorTransitionBase transition, AnimatorStateMachine machine, AacTransitionEndpoint sourceNullableIfAny, AacTransitionEndpoint destinationNullableIfExits)
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

        /// Applies a series of conditions to this transition, but this series of conditions cannot include an Or operator.
        public AacFlTransitionContinuation When(Action<AacFlTransitionContinuationWithoutOr> actionsWithoutOr)
        {
            actionsWithoutOr(new AacFlTransitionContinuationWithoutOr(Transition));
            return AsContinuationWithOr();
        }

        /// Applies a series of conditions, and this series may contain Or operators. However, the result can not be followed by an And operator. It can only be an Or operator.
        public AacFlTransitionContinuationOnlyOr When(Action<AacFlNewTransitionContinuation> actionsWithOr)
        {
            actionsWithOr(this);
            return AsContinuationOnlyOr();
        }

        /// Applies a series of conditions, and this series may contain Or operators. All And operators that follow will apply to all the conditions generated by this series, until the next Or operator.
        public AacFlMultiTransitionContinuation When(IAacFlOrCondition actionsWithOr)
        {
            var pendingContinuations = actionsWithOr.ApplyTo(this);
            return new AacFlMultiTransitionContinuation(Transition, _machine, _sourceNullableIfAny, _destinationNullableIfExits, pendingContinuations);
        }

        /// Provides a handle to start defining conditions using the And operator, for use in loops. The Or operator may be invoked at any point.
        public AacFlTransitionContinuation WhenConditions()
        {
            return AsContinuationWithOr();
        }

        private AacFlTransitionContinuation AsContinuationWithOr()
        {
            return new AacFlTransitionContinuation(Transition, _machine, _sourceNullableIfAny, _destinationNullableIfExits);
        }

        private AacFlTransitionContinuationOnlyOr AsContinuationOnlyOr()
        {
            return new AacFlTransitionContinuationOnlyOr(Transition, _machine, _sourceNullableIfAny, _destinationNullableIfExits);
        }
    }

    public class AacFlTransitionContinuation : AacFlTransitionContinuationAbstractWithOr
    {
        internal AacFlTransitionContinuation(AnimatorTransitionBase transition, AnimatorStateMachine machine, AacTransitionEndpoint sourceNullableIfAny, AacTransitionEndpoint destinationNullableIfExits) : base(transition, machine, sourceNullableIfAny, destinationNullableIfExits)
        {
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
        public AacFlTransitionContinuation And(Action<AacFlTransitionContinuationWithoutOr> actionsWithoutOr)
        {
            actionsWithoutOr(new AacFlTransitionContinuationWithoutOr(Transition));
            return this;
        }
    }

    public class AacFlMultiTransitionContinuation : AacFlTransitionContinuationAbstractWithOr
    {
        private readonly List<AacFlTransitionContinuation> _pendingContinuations;

        internal AacFlMultiTransitionContinuation(AnimatorTransitionBase transition, AnimatorStateMachine machine, AacTransitionEndpoint sourceNullableIfAny, AacTransitionEndpoint destinationNullableIfExits, List<AacFlTransitionContinuation> pendingContinuations) : base(transition, machine, sourceNullableIfAny, destinationNullableIfExits)
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
        public AacFlMultiTransitionContinuation And(Action<AacFlTransitionContinuationWithoutOr> actionsWithoutOr)
        {
            foreach (var pendingContinuation in _pendingContinuations)
            {
                pendingContinuation.And(actionsWithoutOr);
            }

            return this;
        }
    }

    public class AacFlTransitionContinuationOnlyOr : AacFlTransitionContinuationAbstractWithOr
    {
        internal AacFlTransitionContinuationOnlyOr(AnimatorTransitionBase transition, AnimatorStateMachine machine, AacTransitionEndpoint sourceNullableIfAny, AacTransitionEndpoint destinationNullableIfExits) : base(transition, machine, sourceNullableIfAny, destinationNullableIfExits)
        {
        }
    }

    public abstract class AacFlTransitionContinuationAbstractWithOr
    {
        protected readonly AnimatorTransitionBase Transition;
        private readonly AnimatorStateMachine _machine;
        private readonly AacTransitionEndpoint _sourceNullableIfAny;
        private readonly AacTransitionEndpoint _destinationNullableIfExits;

        protected AacFlTransitionContinuationAbstractWithOr(AnimatorTransitionBase transition, AnimatorStateMachine machine, AacTransitionEndpoint sourceNullableIfAny, AacTransitionEndpoint destinationNullableIfExits)
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
            return new AacFlNewTransitionContinuation(NewTransitionFromTemplate(), _machine, _sourceNullableIfAny, _destinationNullableIfExits);
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
                if (_sourceNullableIfAny == null)
                {
                    if (_destinationNullableIfExits.TryGetState(out var state))
                        newTransition = AacInternals.NoUndo(_machine, () => _machine.AddEntryTransition(state));
                    else if (_destinationNullableIfExits.TryGetStateMachine(out var stateMachine))
                        newTransition = AacInternals.NoUndo(_machine, () => _machine.AddEntryTransition(stateMachine));
                    else
                        throw new InvalidOperationException("_destinationNullableIfExits is not null but does not contain an AnimatorState or AnimatorStateMachine");
                }
                // source will never be a state if we're cloning an AnimatorTransition
                else if (_sourceNullableIfAny.TryGetStateMachine(out var stateMachine))
                {
                    if (_destinationNullableIfExits == null)
                        newTransition = AacInternals.NoUndo(_machine, () => _machine.AddStateMachineExitTransition(stateMachine));
                    else if (_destinationNullableIfExits.TryGetState(out var destinationState))
                        newTransition = AacInternals.NoUndo(_machine, () => _machine.AddStateMachineTransition(stateMachine, destinationState));
                    else if (_destinationNullableIfExits.TryGetStateMachine(out var destinationStateMachine))
                        newTransition = AacInternals.NoUndo(_machine, () => _machine.AddStateMachineTransition(stateMachine, destinationStateMachine));
                    else
                        throw new InvalidOperationException("_destinationNullableIfExits is not null but does not contain an AnimatorState or AnimatorStateMachine");
                }
                else
                    throw new InvalidOperationException("_sourceNullableIfAny is not null but does not contain an AnimatorStateMachine");
            }
            return newTransition;
        }

        private AnimatorStateTransition NewTransition()
        {
            AnimatorState state;
            AnimatorStateMachine stateMachine;

            if (_sourceNullableIfAny == null)
            {
                if (_destinationNullableIfExits.TryGetState(out state))
                    return AacInternals.NoUndo(_machine, () => _machine.AddAnyStateTransition(state));
                if (_destinationNullableIfExits.TryGetStateMachine(out stateMachine))
                    return AacInternals.NoUndo(_machine, () => _machine.AddAnyStateTransition(stateMachine));
                throw new InvalidOperationException("Transition has no source nor destination.");
            }

            // source will never be a state machine if we're cloning an AnimatorStateTransition
            if (_sourceNullableIfAny.TryGetState(out var sourceState))
            {
                if (_destinationNullableIfExits == null)
                {
                    return AacInternals.NoUndo(sourceState, () => sourceState.AddExitTransition());
                }

                if (_destinationNullableIfExits.TryGetState(out state))
                {
                    return AacInternals.NoUndo(sourceState, () => sourceState.AddTransition(state));
                }

                if (_destinationNullableIfExits.TryGetStateMachine(out stateMachine))
                {
                    return AacInternals.NoUndo(sourceState, () => sourceState.AddTransition(stateMachine));
                }

                throw new InvalidOperationException("_destinationNullableIfExits is not null but does not contain an AnimatorState or AnimatorStateMachine");
            }
            throw new InvalidOperationException("_sourceNullableIfAny is not null but does not contain an AnimatorState");
        }
    }

    public class AacFlTransitionContinuationWithoutOr
    {
        private readonly AnimatorTransitionBase _transition;

        internal AacFlTransitionContinuationWithoutOr(AnimatorTransitionBase transition)
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
        public AacFlTransitionContinuationWithoutOr AndWhenever(Action<AacFlTransitionContinuationWithoutOr> action)
        {
            action(this);
            return this;
        }
    }

    public class AacTransitionEndpoint
    {
        private readonly AnimatorState _state;
        private readonly AnimatorStateMachine _stateMachine;

        private AacTransitionEndpoint(AnimatorState state)
        {
            _state = state;
        }

        private AacTransitionEndpoint(AnimatorStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public static implicit operator AacTransitionEndpoint(AnimatorState state)
        {
            return new AacTransitionEndpoint(state);
        }

        public static implicit operator AacTransitionEndpoint(AnimatorStateMachine stateMachine)
        {
            return new AacTransitionEndpoint(stateMachine);
        }

        public bool TryGetState(out AnimatorState state)
        {
            state = _state;
            return _state != null;
        }

        public bool TryGetStateMachine(out AnimatorStateMachine stateMachine)
        {
            stateMachine = _stateMachine;
            return _stateMachine != null;
        }
    }
}
