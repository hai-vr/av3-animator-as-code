using System.Linq;
using UnityEditor.Animations;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace AnimatorAsCode.Framework
{
    public partial class AacAnimatorGenerator
    {
        private readonly AnimatorController _animatorController;
        private readonly AnimationClip _emptyClip;
        private readonly IAacDefaultsProvider _defaultsProvider;

        internal AacAnimatorGenerator(
            AnimatorController animatorController,
            AnimationClip emptyClip,
            IAacDefaultsProvider defaultsProvider
        )
        {
            _animatorController = animatorController;
            _emptyClip = emptyClip;
            _defaultsProvider = defaultsProvider;
        }

        internal void CreateParamsAsNeeded(params AacFlParameter[] parameters)
        {
            foreach (var parameter in parameters)
            {
                switch (parameter)
                {
                    case AacFlIntParameter _:
                        CreateParamIfNotExists(parameter.Name, AnimatorControllerParameterType.Int);
                        break;
                    case AacFlFloatParameter _:
                        CreateParamIfNotExists(
                            parameter.Name,
                            AnimatorControllerParameterType.Float
                        );
                        break;
                    case AacFlBoolParameter _:
                        CreateParamIfNotExists(
                            parameter.Name,
                            AnimatorControllerParameterType.Bool
                        );
                        break;
                }
            }
        }

        internal void CreateTriggerParamsAsNeeded(params AacFlBoolParameter[] parameters)
        {
            foreach (var parameter in parameters)
            {
                CreateParamIfNotExists(parameter.Name, AnimatorControllerParameterType.Trigger);
            }
        }

        private void CreateParamIfNotExists(string paramName, AnimatorControllerParameterType type)
        {
            if (
                _animatorController.parameters.FirstOrDefault(param => param.name == paramName)
                == null
            )
            {
                _animatorController.AddParameter(paramName, type);
            }
        }

        // DEPRECATED: This causes the editor window to glitch by deselecting, which is jarring for experimentation
        internal AacStateMachine CreateOrRemakeLayerAtSameIndex(
            string layerName,
            float weightWhenCreating,
            AvatarMask maskWhenCreating = null
        )
        {
            var originalIndexToPreserveOrdering = FindIndexOf(layerName);
            if (originalIndexToPreserveOrdering != -1)
            {
                _animatorController.RemoveLayer(originalIndexToPreserveOrdering);
            }

            AddLayerWithWeight(layerName, weightWhenCreating, maskWhenCreating);
            if (originalIndexToPreserveOrdering != -1)
            {
                var items = _animatorController.layers.ToList();
                var last = items[items.Count - 1];
                items.RemoveAt(items.Count - 1);
                items.Insert(originalIndexToPreserveOrdering, last);
                _animatorController.layers = items.ToArray();
            }

            var layer = TryGetLayer(layerName);
            var machinist = new AacStateMachine(
                layer.stateMachine,
                _emptyClip,
                new AacBackingAnimator(this),
                _defaultsProvider
            );
            return machinist
                .WithAnyStatePosition(0, 7)
                .WithEntryPosition(0, -1)
                .WithExitPosition(7, -1);
        }

        internal AacStateMachine CreateOrClearLayerAtSameIndex(
            string layerName,
            float weightWhenCreating,
            AvatarMask maskWhenCreating = null
        )
        {
            var originalIndexToPreserveOrdering = FindIndexOf(layerName);
            if (originalIndexToPreserveOrdering != -1)
            {
                RecursivelyClearChildrenMachines(
                    _animatorController.layers[originalIndexToPreserveOrdering].stateMachine
                );
                _animatorController.layers[originalIndexToPreserveOrdering]
                    .stateMachine
                    .stateMachines = new ChildAnimatorStateMachine[0];
                _animatorController.layers[originalIndexToPreserveOrdering].stateMachine.states =
                    new ChildAnimatorState[0];
                _animatorController.layers[originalIndexToPreserveOrdering]
                    .stateMachine
                    .entryTransitions = new AnimatorTransition[0];
                _animatorController.layers[originalIndexToPreserveOrdering]
                    .stateMachine
                    .anyStateTransitions = new AnimatorStateTransition[0];
            }
            else
            {
                _animatorController.AddLayer(_animatorController.MakeUniqueLayerName(layerName));
                originalIndexToPreserveOrdering = _animatorController.layers.Length - 1;
            }

            var layers = _animatorController.layers;
            layers[originalIndexToPreserveOrdering].avatarMask = maskWhenCreating;
            layers[originalIndexToPreserveOrdering].defaultWeight = weightWhenCreating;
            _animatorController.layers = layers;

            var layer = TryGetLayer(layerName);
            var machinist = new AacStateMachine(
                layer.stateMachine,
                _emptyClip,
                new AacBackingAnimator(this),
                _defaultsProvider
            );
            return machinist
                .WithAnyStatePosition(0, 7)
                .WithEntryPosition(0, -1)
                .WithExitPosition(7, -1);
        }

        private void RecursivelyClearChildrenMachines(AnimatorStateMachine parentMachine)
        {
            // TODO: RemoveStateMachine might already be recursive
            foreach (var childStateMachineHolder in parentMachine.stateMachines)
            {
                RecursivelyClearChildrenMachines(childStateMachineHolder.stateMachine);
                parentMachine.RemoveStateMachine(childStateMachineHolder.stateMachine);
            }
        }

        private int FindIndexOf(string layerName)
        {
            return _animatorController.layers
                .ToList()
                .FindIndex(layer1 => layer1.name == layerName);
        }

        private AnimatorControllerLayer TryGetLayer(string layerName)
        {
            return _animatorController.layers.FirstOrDefault(it => it.name == layerName);
        }

        private void AddLayerWithWeight(
            string layerName,
            float weightWhenCreating,
            AvatarMask maskWhenCreating
        )
        {
            _animatorController.AddLayer(_animatorController.MakeUniqueLayerName(layerName));

            var mutatedLayers = _animatorController.layers;
            mutatedLayers[mutatedLayers.Length - 1].defaultWeight = weightWhenCreating;
            mutatedLayers[mutatedLayers.Length - 1].avatarMask = maskWhenCreating;
            _animatorController.layers = mutatedLayers;
        }
    }
}
