using System;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine;
using System.Reflection;

// ReSharper disable once CheckNamespace
namespace AnimatorAsCode.Framework
{
    internal class AacStateMachine
    {
        private static readonly PropertyInfo PropPushUndo =
            typeof(AnimatorStateMachine).GetProperty(
                "pushUndo",
                BindingFlags.NonPublic | BindingFlags.Instance
            );
        public readonly AnimatorStateMachine _machine;
        private readonly AnimationClip _emptyClip;
        private readonly AacBackingAnimator _backingAnimator;
        private readonly IAacDefaultsProvider _defaultsProvider;
        private readonly float _gridShiftX;
        private readonly float _gridShiftY;
        private Type nodeLastType; //this is the last node created's type

        public AacStateMachine(
            AnimatorStateMachine machine,
            AnimationClip emptyClip,
            AacBackingAnimator backingAnimator,
            IAacDefaultsProvider defaultsProvider
        )
        {
            PropPushUndo.SetValue(machine, false);
            _machine = machine;
            _emptyClip = emptyClip;
            _backingAnimator = backingAnimator;
            _defaultsProvider = defaultsProvider;

            var grid = defaultsProvider.Grid();
            _gridShiftX = grid.x;
            _gridShiftY = grid.y;
        }

        internal AacStateMachine AddSubStateMachine(string name, Vector2 position)
        {
            var machine = _machine.AddStateMachine(name, position);
            AacStateMachine result = new AacStateMachine(
                machine,
                _emptyClip,
                _backingAnimator,
                _defaultsProvider
            );
            nodeLastType = typeof(AnimatorStateMachine);
            return result;
        }

        internal AacBackingAnimator BackingAnimator()
        {
            return _backingAnimator;
        }

        /// <summary> Sets the entry state position. </summary>
        /// <inheritdoc cref="GridPosition(int,int)" path="/remarks"/>
        /// <param name="x"> The x position. </param>
        /// <param name="y"> The y position. </param>
        /// <returns> This instance. </returns>
        public AacStateMachine WithEntryPosition(int x, int y)
        {
            _machine.entryPosition = GridPosition(x, y);
            return this;
        }

        /// <summary> Sets the exit state position. </summary>
        /// <inheritdoc cref="GridPosition(int,int)" path="/remarks"/>
        /// <param name="x"> The x position. </param>
        /// <param name="y"> The y position. </param>
        /// <returns> This instance. </returns>
        public AacStateMachine WithExitPosition(int x, int y)
        {
            _machine.exitPosition = GridPosition(x, y);
            return this;
        }

        /// <summary> Sets the any state position. </summary>
        /// <inheritdoc cref="GridPosition(int,int)" path="/remarks"/>
        /// <param name="x"> The x position. </param>
        /// <param name="y"> The y position. </param>
        /// <returns> This instance. </returns>
        public AacStateMachine WithAnyStatePosition(int x, int y)
        {
            _machine.anyStatePosition = GridPosition(x, y);
            return this;
        }

        /// <summary> Creates a new state in this state machine. </summary>
        /// <inheritdoc cref="GridPosition(int,int)" path="/remarks"/>
        /// <param name="name"> The name of the state.</param>
        /// <param name="x"> The x position. </param>
        /// <param name="y"> The y position. </param>
        /// <returns> The created state. </returns>
        public AacFlState NewState(string name, int x, int y)
        {
            var state = _machine.AddState(name, GridPosition(x, y));
            _defaultsProvider.ConfigureState(state, _emptyClip);
            nodeLastType = typeof(AnimatorState);
            return new AacFlState(state, _machine, _defaultsProvider);
        }

        /// <summary> Creates a transition from the any state to the given state. </summary>
        /// <param name="destination"> The destination state. </param>
        /// <returns> The created transition. </returns>
        public AacFlTransition AnyTransitionsTo(AacFlState destination)
        {
            return AnyTransition(destination, _machine);
        }

        /// <summary> Creates a transition from the entry state to the given state. </summary>
        /// <param name="destination"> The destination state. </param>
        /// <returns> The created transition. </returns>
        public AacFlEntryTransition EntryTransitionsTo(AacFlState destination)
        {
            return EntryTransition(destination, _machine);
        }

        private AacFlTransition AnyTransition(
            AacFlState destination,
            AnimatorStateMachine animatorStateMachine
        )
        {
            return new AacFlTransition(
                ConfigureTransition(animatorStateMachine.AddAnyStateTransition(destination.State)),
                animatorStateMachine,
                null,
                destination.State
            );
        }

        private AnimatorStateTransition ConfigureTransition(AnimatorStateTransition transition)
        {
            _defaultsProvider.ConfigureTransition(transition);
            return transition;
        }

        private AacFlEntryTransition EntryTransition(
            AacFlState destination,
            AnimatorStateMachine animatorStateMachine
        )
        {
            return new AacFlEntryTransition(
                animatorStateMachine.AddEntryTransition(destination.State),
                animatorStateMachine,
                null,
                destination.State
            );
        }

        internal Vector2 LastStatePosition()
        {
            if (nodeLastType == typeof(AnimatorState))
            {
                return (Vector2)_machine.states.Last().position;
            }
            else if (nodeLastType == typeof(AnimatorStateMachine))
            {
                return (Vector2)_machine.stateMachines.Last().position;
            }
            else
            {
                return Vector2.zero;
            }
            /* return _machine.states.Length > 0
                ? (Vector2)_machine.states.Last().position
                : Vector2.zero; */
        }

        /// <summary> Returns the grid position for the given x and y. The grid position is calculated by multiplying the x and y with the grid shift. </summary>
        /// <remarks> Grid states are 200x10. </remarks>
        /// <param name="x"> The x position. </param>
        /// <param name="y"> The y position. </param>
        /// <returns> The grid position. </returns>
        private Vector3 GridPosition(int x, int y)
        {
            return new Vector3(x * _gridShiftX, y * _gridShiftY, 0);
        }
    }
}
