using System;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace AnimatorAsCode.Framework
{
    public partial class AacFlLayer
    {
        private readonly AnimatorController _animatorController;
        private readonly AacConfiguration _configuration;
        private readonly string _fullLayerName;
        private readonly AacStateMachine _stateMachine;
        private readonly AacStateMachine _parentStateMachine;

        internal AacFlLayer(
            AnimatorController animatorController,
            AacConfiguration configuration,
            AacStateMachine stateMachine,
            string fullLayerName,
            AacStateMachine parentStateMachine = null
        )
        {
            _animatorController = animatorController;
            _configuration = configuration;
            _fullLayerName = fullLayerName;
            _stateMachine = stateMachine;
            _parentStateMachine = parentStateMachine;
        }

        /// <summary> Create a new state machine inside of the layer </summary>
        /// <param name="name">Name of the state machine</param>
        /// <param name="x">X position of the state machine</param>
        /// <param name="y">Y position of the state machine</param>
        /// <returns>AacFlLayer</returns>
        public AacFlLayer NewStateGroup(string name, float x, float y)
        {
            AacStateMachine stateMachine = _stateMachine.AddSubStateMachine(
                name,
                new Vector2(x, y)
            );
            AacFlLayer layer = new AacFlLayer(
                _animatorController,
                _configuration,
                stateMachine,
                _fullLayerName + "/" + name,
                _stateMachine
            );
            return layer;
        }

        /// <summary> Create a new state machine inside of the layer </summary>
        /// <param name="name">Name of the state machine</param>
        /// <returns>AacFlLayer</returns>
        public AacFlLayer NewStateGroup(string name)
        {
            Vector2 lastState = _stateMachine.LastStatePosition();
            return NewStateGroup(
                name,
                lastState.x,
                lastState.y + _configuration.DefaultsProvider.Grid().y
            );
        }

        public Vector2 Position
        {
            get
            {
                try
                {
                    AacStateMachine sm = _stateMachine;
                    return _parentStateMachine._machine.stateMachines
                        .First(s => s.stateMachine == sm._machine)
                        .position;
                }
                catch (Exception e)
                {
                    Debug.Log("Failed to get position: " + e.Message);
                    return Vector2.zero;
                }
            }
            set
            {
                try
                {
                    AacStateMachine sm = _stateMachine;
                    ChildAnimatorStateMachine[] ChildMachines = _parentStateMachine
                        ._machine
                        .stateMachines;
                    int index = ChildMachines
                        .ToList()
                        .FindIndex(s => s.stateMachine == sm._machine);
                    ChildMachines[index].position = new Vector3(
                        value.x * _configuration.DefaultsProvider.Grid().x,
                        value.y * _configuration.DefaultsProvider.Grid().y,
                        0
                    );

                    //replace back in the list
                    _parentStateMachine._machine.stateMachines = ChildMachines;
                }
                catch (Exception e)
                {
                    Debug.Log("Failed to set position: " + e.Message);
                }
            }
        }

        /// <summary>
        /// Create a new state
        /// </summary>
        /// <param name="name">Name of the state</param>
        /// <returns>AacFlState</returns>
        public AacFlState NewState(string name)
        {
            var lastState = _stateMachine.LastStatePosition();
            var state = _stateMachine.NewState(name, 0, 0).Shift(lastState, 0, 1);
            return state;
        }

        /// <summary>
        /// Create a new state with a position
        /// </summary>
        /// <param name="name">Name of the state</param>
        /// <param name="x">X position of the state</param>
        /// <param name="y">Y position of the state</param>
        /// <returns>AacFlState</returns>
        public AacFlState NewState(string name, int x, int y)
        {
            return _stateMachine.NewState(name, x, y);
        }

        /// <summary>
        /// Create transition from the layers any state to the given state
        /// </summary>
        /// <param name="destination">Destination state</param>
        /// <returns>AacFlAnyStateTransition</returns>
        public AacFlTransition AnyTransitionsTo(AacFlState destination)
        {
            return _stateMachine.AnyTransitionsTo(destination);
        }

        /// <summary>
        /// Create transition from the layers entry state to the given state
        /// </summary>
        /// <param name="destination">Destination state</param>
        /// <returns>AacFlEntryTransition</returns>
        public AacFlEntryTransition EntryTransitionsTo(AacFlState destination)
        {
            return _stateMachine.EntryTransitionsTo(destination);
        }

        public AacFlBoolParameter BoolParameter(string parameterName) =>
            _stateMachine.BackingAnimator().BoolParameter(parameterName);

        public AacFlBoolParameter TriggerParameterAsBool(string parameterName) =>
            _stateMachine.BackingAnimator().TriggerParameter(parameterName);

        public AacFlFloatParameter FloatParameter(string parameterName) =>
            _stateMachine.BackingAnimator().FloatParameter(parameterName);

        public AacFlIntParameter IntParameter(string parameterName) =>
            _stateMachine.BackingAnimator().IntParameter(parameterName);

        public AacFlBoolParameterGroup BoolParameters(params string[] parameterNames) =>
            _stateMachine.BackingAnimator().BoolParameters(parameterNames);

        public AacFlBoolParameterGroup TriggerParametersAsBools(params string[] parameterNames) =>
            _stateMachine.BackingAnimator().TriggerParameters(parameterNames);

        public AacFlFloatParameterGroup FloatParameters(params string[] parameterNames) =>
            _stateMachine.BackingAnimator().FloatParameters(parameterNames);

        public AacFlIntParameterGroup IntParameters(params string[] parameterNames) =>
            _stateMachine.BackingAnimator().IntParameters(parameterNames);

        public AacFlBoolParameterGroup BoolParameters(params AacFlBoolParameter[] parameters) =>
            _stateMachine.BackingAnimator().BoolParameters(parameters);

        public AacFlBoolParameterGroup TriggerParametersAsBools(
            params AacFlBoolParameter[] parameters
        ) => _stateMachine.BackingAnimator().TriggerParameters(parameters);

        public AacFlFloatParameterGroup FloatParameters(params AacFlFloatParameter[] parameters) =>
            _stateMachine.BackingAnimator().FloatParameters(parameters);

        public AacFlIntParameterGroup IntParameters(params AacFlIntParameter[] parameters) =>
            _stateMachine.BackingAnimator().IntParameters(parameters);

        public AacAv3 Av3() => new AacAv3(_stateMachine.BackingAnimator());

        public void OverrideValue(AacFlBoolParameter toBeForced, bool value)
        {
            var parameters = _animatorController.parameters;
            foreach (var param in parameters)
            {
                if (param.name == toBeForced.Name)
                {
                    param.defaultBool = value;
                }
            }

            _animatorController.parameters = parameters;
        }

        public void OverrideValue(AacFlFloatParameter toBeForced, float value)
        {
            var parameters = _animatorController.parameters;
            foreach (var param in parameters)
            {
                if (param.name == toBeForced.Name)
                {
                    param.defaultFloat = value;
                }
            }

            _animatorController.parameters = parameters;
        }

        public void OverrideValue(AacFlIntParameter toBeForced, int value)
        {
            var parameters = _animatorController.parameters;
            foreach (var param in parameters)
            {
                if (param.name == toBeForced.Name)
                {
                    param.defaultInt = value;
                }
            }

            _animatorController.parameters = parameters;
        }

        public AacFlLayer WithAvatarMask(AvatarMask avatarMask)
        {
            var finalFullLayerName = _fullLayerName;
            _animatorController.layers = _animatorController.layers
                .Select(layer =>
                {
                    if (layer.name == finalFullLayerName)
                    {
                        layer.avatarMask = avatarMask;
                    }

                    return layer;
                })
                .ToArray();

            return this;
        }

        public void WithAvatarMaskNoTransforms()
        {
            ResolveAvatarMask(new Transform[0]);
        }

        public void ResolveAvatarMask(Transform[] paths)
        {
            // FIXME: Fragile
            var avatarMask = new AvatarMask();
            avatarMask.name =
                "zAutogenerated__"
                + _configuration.AssetKey
                + "_"
                + _fullLayerName
                + "__AvatarMask";
            avatarMask.hideFlags = HideFlags.None;

            if (paths.Length == 0)
            {
                avatarMask.transformCount = 1;
                avatarMask.SetTransformActive(0, false);
                avatarMask.SetTransformPath(0, "_ignored");
            }
            else
            {
                avatarMask.transformCount = paths.Length;
                for (var index = 0; index < paths.Length; index++)
                {
                    var transform = paths[index];
                    avatarMask.SetTransformActive(index, true);
                    avatarMask.SetTransformPath(
                        index,
                        Aac.ResolveRelativePath(_configuration.AnimatorRoot, transform)
                    );
                }
            }

            for (int i = 0; i < (int)AvatarMaskBodyPart.LastBodyPart; i++)
            {
                avatarMask.SetHumanoidBodyPartActive((AvatarMaskBodyPart)i, false);
            }

            AssetDatabase.AddObjectToAsset(avatarMask, _animatorController);

            WithAvatarMask(avatarMask);
        }
    }
}
