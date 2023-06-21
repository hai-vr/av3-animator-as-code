using System;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine;
using System.Reflection;

// ReSharper disable once CheckNamespace
namespace AnimatorAsCode.Framework
{
    /// <summary> A state in a state machine. </summary>
    /// <inheritdoc cref="AacStateMachine.GridPosition(int,int)" path="/remarks"/>
    public partial class AacFlState
    {
        private static readonly PropertyInfo PropPushUndo = typeof(AnimatorState).GetProperty(
            "pushUndo",
            BindingFlags.NonPublic | BindingFlags.Instance
        );

        /// <summary> The underlying state. </summary>
        public readonly AnimatorState State;
        private readonly AnimatorStateMachine _machine;
        private readonly IAacDefaultsProvider _defaultsProvider;
        private readonly AacBackingAnimator _backingAnimator;

        /// <summary> Creates a new state. </summary>
        /// <param name="state"> The underlying state. </param>
        /// <param name="machine"> The state machine this state belongs to. </param>
        /// <param name="defaultsProvider"> The defaults provider. </param>
        public AacFlState(
            AnimatorState state,
            AnimatorStateMachine machine,
            IAacDefaultsProvider defaultsProvider
        )
        {
            PropPushUndo.SetValue(state, false);
            State = state;
            _machine = machine;
            _defaultsProvider = defaultsProvider;
        }

        /// <summary> Sets the position of this state to the left of another state. </summary>
        /// <param name="otherState"> The other state. </param>
        /// <returns> This instance. </returns>
        public AacFlState LeftOf(AacFlState otherState) => MoveNextTo(otherState, -1, 0);

        /// <summary> Sets the position of this state to the right of another state. </summary>
        /// <inheritdoc cref="LeftOf(AacFlState)"/>
        public AacFlState RightOf(AacFlState otherState) => MoveNextTo(otherState, 1, 0);

        /// <summary> Sets the position of this state to above another state. </summary>
        /// <inheritdoc cref="LeftOf(AacFlState)"/>
        public AacFlState Over(AacFlState otherState) => MoveNextTo(otherState, 0, -1);

        /// <summary> Sets the position of this state to below another state. </summary>
        /// <inheritdoc cref="LeftOf(AacFlState)"/>
        public AacFlState Under(AacFlState otherState) => MoveNextTo(otherState, 0, 1);

        /// <summary> Sets the position of this state to the left of the second to last state. </summary>
        /// <returns> This instance. </returns>
        public AacFlState LeftOf() => MoveNextTo(null, -1, 0);

        /// <summary> Sets the position of this state to the right of the second to last state. </summary>
        /// <inheritdoc cref="LeftOf()"/>
        public AacFlState RightOf() => MoveNextTo(null, 1, 0);

        /// <summary> Sets the position of this state to above the second to last state. </summary>
        /// <inheritdoc cref="LeftOf()"/>
        public AacFlState Over() => MoveNextTo(null, 0, -1);

        /// <summary> Sets the position of this state to below the second to last state. </summary>
        /// <inheritdoc cref="LeftOf()"/>
        public AacFlState Under() => MoveNextTo(null, 0, 1);

        /// <summary> Sets the position of this state relative to another state. </summary>
        /// <inheritdoc cref="AacStateMachine.GridPosition(int,int)" path="/remarks"/>
        /// <param name="otherState"> The other state. </param>
        /// <param name="shiftX"> The x shift. </param>
        /// <param name="shiftY"> The y shift. </param>
        /// <returns> This instance. </returns>
        public AacFlState Shift(AacFlState otherState, int shiftX, int shiftY) =>
            MoveNextTo(otherState, shiftX, shiftY);

        private AacFlState MoveNextTo(AacFlState otherStateOrSecondToLastWhenNull, int x, int y)
        {
            if (otherStateOrSecondToLastWhenNull == null)
            {
                var other = _machine.states[_machine.states.Length - 2];
                Shift((Vector2)other.position, x, y);

                return this;
            }
            else
            {
                var other = _machine.states.First(
                    animatorState => animatorState.state == otherStateOrSecondToLastWhenNull.State
                );
                Shift((Vector2)other.position, x, y);

                return this;
            }
        }

        /// <summary> Sets the position of this state relative to a position on the grid. </summary>
        /// <inheritdoc cref="AacStateMachine.GridPosition(int,int)" path="/remarks"/>
        /// <param name="otherPosition"> The other position. </param>
        /// <param name="shiftX"> The x shift. </param>
        /// <param name="shiftY"> The y shift. </param>
        /// <returns> This instance. </returns>
        [Obsolete("Use Shift(Vector2, int, int) instead.")]
        public AacFlState Shift(Vector3 otherPosition, int shiftX, int shiftY)
        {
            return Shift((Vector2)otherPosition, shiftX, shiftY);
        }

        /// <summary> Sets the position of this state relative to a position on the grid. </summary>
        /// <inheritdoc cref="AacStateMachine.GridPosition(int,int)" path="/remarks"/>
        /// <param name="otherPosition"> The other position. </param>
        /// <param name="shiftX"> The x shift. </param>
        /// <param name="shiftY"> The y shift. </param>
        /// <returns> This instance. </returns>
        public AacFlState Shift(Vector2 otherPosition, int shiftX, int shiftY)
        {
            Vector3 otherPositionV3 = new Vector3(otherPosition.x, otherPosition.y, 0);
            var states = _machine.states;
            for (var index = 0; index < states.Length; index++)
            {
                var childAnimatorState = states[index];
                if (childAnimatorState.state == State)
                {
                    var cms = childAnimatorState;
                    cms.position =
                        otherPositionV3
                        + new Vector3(
                            shiftX * _defaultsProvider.Grid().x,
                            shiftY * _defaultsProvider.Grid().y,
                            0
                        );
                    states[index] = cms;
                    break;
                }
            }
            _machine.states = states;
            return this;
        }

        /// <summary> Sets the animation clip of this state. </summary>
        /// <param name="clip"> The clip. </param>
        /// <returns> This instance. </returns>
        public AacFlState WithAnimation(Motion clip)
        {
            State.motion = clip;
            return this;
        }

        /// <inheritdoc cref="WithAnimation(Motion)"/>
        public AacFlState WithAnimation(AacFlClip clip)
        {
            State.motion = clip.Clip;
            return this;
        }

        /// <summary> Create a transition from this state to another state. </summary>
        /// <param name="destination"> The destination state. </param>
        /// <returns> The transition. </returns>
        public AacFlTransition TransitionsTo(AacFlState destination)
        {
            return new AacFlTransition(
                ConfigureTransition(State.AddTransition(destination.State)),
                _machine,
                State,
                destination.State
            );
        }

        /// <summary> Create a transition from the any state to this state. </summary>
        /// <returns> The transition. </returns>
        public AacFlTransition TransitionsFromAny()
        {
            return new AacFlTransition(
                ConfigureTransition(_machine.AddAnyStateTransition(State)),
                _machine,
                null,
                State
            );
        }

        /// <summary> Create a transition from the entry state to this state. </summary>
        /// <returns> The transition. </returns>
        public AacFlEntryTransition TransitionsFromEntry()
        {
            return new AacFlEntryTransition(
                _machine.AddEntryTransition(State),
                _machine,
                null,
                State
            );
        }

        /// <summary> Create a transition with no exit time to the destination state. </summary>
        /// <param name="destination"> The destination state. </param>
        /// <returns> This instance. </returns>
        public AacFlState AutomaticallyMovesTo(AacFlState destination)
        {
            var transition = ConfigureTransition(State.AddTransition(destination.State));
            transition.hasExitTime = true;
            return this;
        }

        /// <summary> Create a transition with to the exit state. </summary>
        /// <returns> This instance. </returns>
        public AacFlTransition Exits()
        {
            return new AacFlTransition(
                ConfigureTransition(State.AddExitTransition()),
                _machine,
                State,
                null
            );
        }

        private AnimatorStateTransition ConfigureTransition(AnimatorStateTransition transition)
        {
            _defaultsProvider.ConfigureTransition(transition);
            return transition;
        }

        /// <summary> Sets the state's write defaults toggle to the given value. </summary>
        /// <param name="shouldWriteDefaults"> Should Write Defaults be on. </param>
        /// <returns> The current state. </returns>
        public AacFlState WithWriteDefaultsSetTo(bool shouldWriteDefaults)
        {
            State.writeDefaultValues = shouldWriteDefaults;
            return this;
        }

        /// <summary> Set the Motion Time, formerly known as Normalized Time. </summary>
        /// <param name="floatParam"> The float parameter to use. </param>
        /// <returns> The current state. </returns>
        public AacFlState MotionTime(AacFlFloatParameter floatParam)
        {
            State.timeParameterActive = true;
            State.timeParameter = floatParam.Name;

            return this;
        }

        /// <summary> Set the speed of the animation. </summary>
        /// <param name="parameter"> The float parameter to use. </param>
        /// <returns> The current state. </returns>
        public AacFlState WithSpeed(AacFlFloatParameter parameter)
        {
            State.speedParameter = parameter.Name;
            State.speedParameterActive = true;

            return this;
        }
    }
}
