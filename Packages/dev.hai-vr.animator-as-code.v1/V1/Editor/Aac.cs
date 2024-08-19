using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using Object = UnityEngine.Object;

// ReSharper disable once CheckNamespace
namespace AnimatorAsCode.V1
{
    /// Starter class for Animator As Code V1. Call this to obtain an instance of AacFlBase.<br/><br/>
    /// The intent of the namespace AnimatorAsCode.V1, and the class AacV1,
    /// is to allow Animator As Code V1 to be simultaneously installed inside projects where:<br/>
    /// - Instances of Animator As Code V0 may exist,<br/>
    /// - Instances of Animator As Code V1 derivatives may exist,<br/>
    /// - Future instances of Animator As Code V2 might exist.<br/><br/>
    /// For this reason, in case of a breaking change, there should be different versions of Animator As Code released
    /// with a different package name, namespace, and initializer class name, so that the user may install dependencies
    /// that rely on different versions of Animator As Code in the same project.
    public static class AacV1
    {
        /// Create an Animator As Code (AAC) base.
        public static AacFlBase Create(AacConfiguration configuration)
        {
            return new AacFlBase(configuration);
        }
    }

    public struct AacConfiguration
    {
        /// A name that will be used as the prefix of all animators layers created.
        public string SystemName;
        // Please consult "https://docs.hai-vr.dev/docs/products/animator-as-code/migrating-v0-to-v1" on how to migrate this property
        // public VRCAvatarDescriptor AvatarDescriptor;
        /// A reference to the animator root. All relative paths will be made relative to this animator root.
        public Transform AnimatorRoot;
        /// Unused. A reference to a root, where default values will be sampled from.
        public Transform DefaultValueRoot;
        /// A persistent asset where all created assets will be added into, based on the value of ContainerMode.
        public Object AssetContainer;
        /// Defines whether created assets should be added to the AssetContainer.
        public Container ContainerMode;
        /// A prefix which will be used in name of all assets, so that the created assets can be removed during subsequent executions of AnimatorAsCode, assuming that your process is destructive.
        public string AssetKey;
        /// An object that will provide default values. When in doubt, use `new AacDefaultsProvider(...)`
        public IAacDefaultsProvider DefaultsProvider;
        
        private Dictionary<Type, object> _additionalData; // Nullable

        /// For use by users of extension functions: Store additional data in this configuration.<br/>
        /// This additional data can be used during the operation of those extension functions.<br/>
        /// Example: Use this to store a reference to a platform-specific avatar component.
        public AacConfiguration WithAdditionalData<T>(T value)
        {
            var conf = this;
            if (_additionalData == null) conf._additionalData = new Dictionary<Type, object>();
            conf._additionalData[typeof(T)] = value;
            return conf;
        }

        /// Attempts to retrieve additional data stored by `WithAdditionalData`.<br/>
        /// Returns true when such data is available.
        public bool TryGetAdditionalData<T>(out T value) where T : class
        {
            if (_additionalData != null && _additionalData.TryGetValue(typeof(T), out var result))
            {
                value = (T)result;
                return true;
            }

            value = null;
            return false;
        }

        internal Object AsPersistentContainerRequired()
        {
            return ContainerMode != Container.Never ? AssetContainer : null;
        }

        internal Object AsRegularContainer()
        {
            return ContainerMode == Container.Everything ? AssetContainer : null;
        }

        /// Values that define how created assets should be added to the AssetContainer.
        public enum Container
        {
            /// Store all created assets in the AssetContainer.
            Everything,
            /// Only store created assets in the AssetContainer if that asset requires persistence.<br/>
            /// Right now, only AnimatorController assets require persistence.
            OnlyWhenPersistenceRequired,
            /// Do not store any assets in the AssetContainer.<br/>
            /// In this case, the value provided in the AssetContainer of the configuration does not matter.
            Never
        }
    }

    public class AacFlLayer
    {
        /// Exposes the underlying AnimatorAsCode StateMachine object of this layer.
        public AacFlStateMachine StateMachine => _stateMachine;

        private readonly AnimatorController _animatorController;
        private readonly AacConfiguration _configuration;
        private readonly string _fullLayerName;
        private readonly AacFlStateMachine _stateMachine;
        
        private readonly List<string> _floatParameters = new List<string>();
        private readonly List<string> _intParameters = new List<string>();
        private readonly List<string> _boolParameters = new List<string>();
        private readonly Dictionary<string, float> _floatOverrides = new Dictionary<string, float>();
        private readonly Dictionary<string, int> _intOverrides = new Dictionary<string, int>();
        private readonly Dictionary<string, bool> _boolOverrides = new Dictionary<string, bool>();

        internal AacFlLayer(AnimatorController animatorController, AacConfiguration configuration, AacFlStateMachine stateMachine, string fullLayerName)
        {
            _animatorController = animatorController;
            _configuration = configuration;
            _fullLayerName = fullLayerName;
            _stateMachine = stateMachine;
        }

        /// Create a new state, initially positioned below the last generated state of this layer.<br/>
        /// 🔺 If the name is already used, a number will be appended at the end.
        public AacFlState NewState(string name)
        {
            var lastState = _stateMachine.LastNodePosition();
            var state = _stateMachine.NewState(name, 0, 0).Shift(lastState, 0, 1);
            return state;
        }

        /// Create a new state at a specific position x and y, in grid units. The grid size is defined in the DefaultsProvider of the AacConfiguration of AAC. x positive goes right, y positive goes down.<br/>
        /// 🔺 If the name is already used, a number will be appended at the end.
        public AacFlState NewState(string name, int x, int y)
        {
            return _stateMachine.NewState(name, x, y);
        }

        /// Create a new state machine, initially positioned below the last generated state of this layer.<br/>
        /// 🔺 If the name is already used, a number will be appended at the end.
        public AacFlStateMachine NewSubStateMachine(string name)
        {
            return _stateMachine.NewSubStateMachine(name);
        }

        /// Create a new state machine at a specific position `x` and `y`, in grid units. The grid size is defined in the DefaultsProvider of the AacConfiguration of AAC. `x` positive goes right, `y` positive goes down.<br/>
        /// 🔺 If the name is already used, a number will be appended at the end.
        public AacFlStateMachine NewSubStateMachine(string name, int x, int y)
        {
            return _stateMachine.NewSubStateMachine(name, x, y);
        }

        /// Create a transition from Any to the `destination` state.
        public AacFlTransition AnyTransitionsTo(AacFlState destination)
        {
            return _stateMachine.AnyTransitionsTo(destination);
        }

        /// Create a transition from Any to the `destination` state machine.
        public AacFlTransition AnyTransitionsTo(AacFlStateMachine destination)
        {
            return _stateMachine.AnyTransitionsTo(destination);
        }

        // Create a transition from the Entry to the `destination` state.
        public AacFlEntryTransition EntryTransitionsTo(AacFlState destination)
        {
            return _stateMachine.EntryTransitionsTo(destination);
        }
        
        // Create a transition from the Entry to the `destination` state machine.
        public AacFlEntryTransition EntryTransitionsTo(AacFlStateMachine destination)
        {
            return _stateMachine.EntryTransitionsTo(destination);
        }

        /// Create a Bool parameter in the animator.
        public AacFlBoolParameter BoolParameter(string parameterName)
        {
            _boolParameters.Add(parameterName);
            return _stateMachine.InternalBackingAnimator().BoolParameter(parameterName);
        }
        
        /// Create a Trigger parameter in the animator, but returns a Bool parameter for use in AAC.
        public AacFlBoolParameter TriggerParameterAsBool(string parameterName)
        {
            _boolParameters.Add(parameterName);
            return _stateMachine.InternalBackingAnimator().TriggerParameter(parameterName);
        }
        
        /// Create a Float parameter in the animator.
        public AacFlFloatParameter FloatParameter(string parameterName)
        {
            _floatParameters.Add(parameterName);
            return _stateMachine.InternalBackingAnimator().FloatParameter(parameterName);
        }
        
        /// Create an Int parameter in the animator.
        public AacFlIntParameter IntParameter(string parameterName)
        {
            _intParameters.Add(parameterName);
            return _stateMachine.InternalBackingAnimator().IntParameter(parameterName);
        }
        
        /// Create multiple Bool parameters in the animator, and returns a group of multiple Bools.
        public AacFlBoolParameterGroup BoolParameters(params string[] parameterNames)
        {
            _boolParameters.AddRange(parameterNames);
            return _stateMachine.InternalBackingAnimator().BoolParameters(parameterNames);
        }
        
        /// Create multiple Trigger parameters in the animator, but returns a group of multiple Bools for use in AAC.
        public AacFlBoolParameterGroup TriggerParametersAsBools(params string[] parameterNames)
        {
            _boolParameters.AddRange(parameterNames);
            return _stateMachine.InternalBackingAnimator().TriggerParameters(parameterNames);
        }
        
        /// Create multiple Float parameters in the animator, and returns a group of multiple Floats.
        public AacFlFloatParameterGroup FloatParameters(params string[] parameterNames)
        {
            _floatParameters.AddRange(parameterNames);
            return _stateMachine.InternalBackingAnimator().FloatParameters(parameterNames);
        }
        
        /// Create multiple Int parameters in the animator, and returns a group of multiple Ints.
        public AacFlIntParameterGroup IntParameters(params string[] parameterNames)
        {
            _intParameters.AddRange(parameterNames);
            return _stateMachine.InternalBackingAnimator().IntParameters(parameterNames);
        }
        
        /// Combine multiple Bool parameters into a group.
        public AacFlBoolParameterGroup BoolParameters(params AacFlBoolParameter[] parameters)
        {
            _boolParameters.AddRange(parameters.Select(parameter => parameter.Name));
            return _stateMachine.InternalBackingAnimator().BoolParameters(parameters);
        }
        
        /// Combine multiple Bool parameters into a group. There is no difference with BoolParameters(...) and is provided only for semantic purposes.
        public AacFlBoolParameterGroup TriggerParametersAsBools(params AacFlBoolParameter[] parameters)
        {
            _boolParameters.AddRange(parameters.Select(parameter => parameter.Name));
            return _stateMachine.InternalBackingAnimator().TriggerParameters(parameters);
        }
        
        /// Combine multiple Float parameters into a group.
        public AacFlFloatParameterGroup FloatParameters(params AacFlFloatParameter[] parameters)
        {
            _floatParameters.AddRange(parameters.Select(parameter => parameter.Name));
            return _stateMachine.InternalBackingAnimator().FloatParameters(parameters);
        }
        
        /// Combine multiple Int parameters into a group.
        public AacFlIntParameterGroup IntParameters(params AacFlIntParameter[] parameters)
        {
            _intParameters.AddRange(parameters.Select(parameter => parameter.Name));
            return _stateMachine.InternalBackingAnimator().IntParameters(parameters);
        }

        /// Set the Bool value of `toBeForced` parameter to `value` in the animator.
        public AacFlLayer OverrideValue(AacFlBoolParameter toBeForced, bool value)
        {
            _boolOverrides[toBeForced.Name] = value;
            var parameters = _animatorController.parameters;
            foreach (var param in parameters)
            {
                if (param.name == toBeForced.Name)
                {
                    param.defaultBool = value;
                }
            }

            _animatorController.parameters = parameters;

            return this;
        }

        /// Set the Float value of `toBeForced` parameter to `value` in the animator.
        public AacFlLayer OverrideValue(AacFlFloatParameter toBeForced, float value)
        {
            _floatOverrides[toBeForced.Name] = value;
            var parameters = _animatorController.parameters;
            foreach (var param in parameters)
            {
                if (param.name == toBeForced.Name)
                {
                    param.defaultFloat = value;
                }
            }

            _animatorController.parameters = parameters;

            return this;
        }

        /// Set the Int value of `toBeForced` parameter to `value` in the animator.
        public AacFlLayer OverrideValue(AacFlIntParameter toBeForced, int value)
        {
            _intOverrides[toBeForced.Name] = value;
            var parameters = _animatorController.parameters;
            foreach (var param in parameters)
            {
                if (param.name == toBeForced.Name)
                {
                    param.defaultInt = value;
                }
            }

            _animatorController.parameters = parameters;

            return this;
        }

        /// Set the Avatar Mask of the layer.
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

        /// Set the Avatar Mask of the layer to be an Avatar Mask which denies all transforms. The asset is generated into the container.
        public AacFlLayer WithAvatarMaskNoTransforms()
        {
            ResolveAvatarMask(new Transform[0]);

            return this;
        }

        /// Set the Avatar Mask of the layer to be an Avatar Mask that allows the specified transforms. If `paths` is an empty array, all transforms are denied, which is effectively the same as calling `.WithAvatarMaskNoTransforms()`. The asset is generated into the container.
        public AacFlLayer ResolveAvatarMask(Transform[] paths)
        {
            var avatarMask = AacInternals.NewAvatarMask(_configuration, _fullLayerName);

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
                    avatarMask.SetTransformPath(index, AacInternals.ResolveRelativePath(_configuration.AnimatorRoot, transform));
                }
            }

            for (int i = 0; i < (int) AvatarMaskBodyPart.LastBodyPart; i++)
            {
                avatarMask.SetHumanoidBodyPartActive((AvatarMaskBodyPart) i, false);
            }

            WithAvatarMask(avatarMask);

            return this;
        }

        /// Set the Default State of the layer.
        public AacFlLayer WithDefaultState(AacFlState newDefaultState)
        {
            _stateMachine.WithDefaultState(newDefaultState);
            return this;
        }

        /// <b>FOR USE ONLY BY EXTENSION FUNCTIONS:</b><br/>
        /// Exposes the internal state machine.
        public AacFlStateMachine InternalStateMachine()
        {
            return _stateMachine;
        }

        /// Copy all created parameters to the other layer, and overrides it with a value if a value was stored.
        public AacFlLayer CopyParametersAndOverridesTo(AacFlLayer otherLayer)
        {
            foreach (var key in _floatParameters)
            {
                var param = otherLayer.FloatParameter(key);
                if (_floatOverrides.TryGetValue(key, out var value)) otherLayer.OverrideValue(param, value);
            }
            foreach (var key in _intParameters)
            {
                var param = otherLayer.IntParameter(key);
                if (_intOverrides.TryGetValue(key, out var value)) otherLayer.OverrideValue(param, value);
            }
            foreach (var key in _boolParameters)
            {
                var param = otherLayer.BoolParameter(key);
                if (_boolOverrides.TryGetValue(key, out var value)) otherLayer.OverrideValue(param, value);
            }

            return this;
        }
        
        // Change the layer weight of this layer.
        public AacFlLayer WithWeight(float weight)
        {
            var finalFullLayerName = _fullLayerName;
            _animatorController.layers = _animatorController.layers
                .Select(layer =>
                {
                    if (layer.name == finalFullLayerName)
                    {
                        layer.defaultWeight = weight;
                    }

                    return layer;
                })
                .ToArray();

            return this;
        }
        
        // Change the blending mode of this layer.
        public AacFlLayer WithBlendingMode(AnimatorLayerBlendingMode blendingMode)
        {
            var finalFullLayerName = _fullLayerName;
            _animatorController.layers = _animatorController.layers
                .Select(layer =>
                {
                    if (layer.name == finalFullLayerName)
                    {
                        layer.blendingMode = blendingMode;
                    }

                    return layer;
                })
                .ToArray();

            return this;
        }
    }

    /// Provides methods for use by extension functions, exposing methods departing from normal fluent interface usage.
    /// These methods are entering a staging phase as of V1.1.0. It is not recommended to use them.
    public static class AacAccessorForExtensions
    {
        /// NOT FOR PUBLIC USE: This method is entering a staging phase as of V1.1.0. It is not recommended to use it.
        public static AacConfiguration AccessConfiguration(AacFlBase aacBase)
        {
            return aacBase.AccessConfiguration();
        }

        /// NOT FOR PUBLIC USE: This method is entering a staging phase as of V1.1.0. It is not recommended to use it.
        public static AacFlLayer AccessCreateLayer(AacFlBase aacBase, AnimatorController animator, string layerName)
        {
            return aacBase.AccessCreateLayer(animator, layerName);
        }
    }

    public class AacFlBase
    {
        private readonly AacConfiguration _configuration;

        /// NOT FOR PUBLIC USE: Internal use only so that destructive workflow can access this. Maybe this can be improved
        [Obsolete("This will be made private/internal in V1.2.0. Use AacAccessorForExtensions.AccessConfiguration(...) instead")]
        public AacConfiguration InternalConfiguration()
        {
            return _configuration;
        }

        internal AacConfiguration AccessConfiguration()
        {
            return _configuration;
        }

        internal AacFlBase(AacConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// Create a new clip. The asset is generated into the container.
        public AacFlClip NewClip()
        {
            var clip = AacInternals.NewClip(_configuration, Guid.NewGuid().ToString());
            return new AacFlClip(_configuration, clip);
        }

        /// Create a new clip that is a copy of `originalClip`. The asset is generated into the container.
        public AacFlClip CopyClip(AnimationClip originalClip)
        {
            var newClip = UnityEngine.Object.Instantiate(originalClip);
            var clip = AacInternals.RegisterClip(_configuration, Guid.NewGuid().ToString(), newClip);
            return new AacFlClip(_configuration, clip);
        }
        
        /// Create a new BlendTree asset. The asset is generated into the container.
        public AacFlNonInitializedBlendTree NewBlendTree()
        {
            return new AacFlNonInitializedBlendTree(AacInternals.NewBlendTreeAsRaw(_configuration, Guid.NewGuid().ToString()));
        }

        /// Create a new BlendTree asset and returns a native BlendTree object. The asset is generated into the container. You may use NewBlendTree() instead to obtain a fluent interface.
        public BlendTree NewBlendTreeAsRaw()
        {
            return AacInternals.NewBlendTreeAsRaw(_configuration, Guid.NewGuid().ToString());
        }

        /// Create a new clip with a name. However, the name is only used as a suffix for the asset. The asset is generated into the container.
        public AacFlClip NewClip(string name)
        {
            var clip = AacInternals.NewClip(_configuration, name);
            return new AacFlClip(_configuration, clip);
        }

        /// Create a new clip which animates a dummy transform for a specific duration specified in an unit (Frames or Seconds).
        public AacFlClip DummyClipLasting(float numberOf, AacFlUnit unit)
        {
            var dummyClip = AacInternals.NewClip(_configuration, $"D({numberOf} {Enum.GetName(typeof(AacFlUnit), unit)})");

            return new AacFlClip(_configuration, dummyClip)
                .Animating(clip => clip.Animates("_ignored", typeof(GameObject), "m_IsActive")
                    .WithUnit(unit, keyframes => keyframes.Constant(0, 0f).Constant(numberOf, 0f)));
        }

        /// Duplicate a new asset into the container and return it. For example, use this to create modified material variants. This asset will be removed the same way as other generated assets.
        public T DuplicateAsset<T>(T assetToDuplicate) where T : Object
        {
            return AacInternals.DuplicateAssetIntoContainer(_configuration, assetToDuplicate);
        }

        // For backwards compatibility, generates an animation with 2 keyframes that are at a distance of 1 / 60 of 1 frame,
        // that is 1 / 3600 seconds.
        // This was due to DummyClipLasting returning a clip with unit conversion applied twice,
        // resulting in an "undesired" animation which was functional anyways.
        // Since this dummy clip is used internally on all uninitalized states,
        // preserve this behaviour so that existing animators don't break due to this change.
        private AacFlClip AnomalousSingleKeyframeClip()
        {
            var numberOf = 1f;
            var unit = AacFlUnit.Frames;
            
            var dummyClip = AacInternals.NewClip(_configuration, $"D({numberOf} {Enum.GetName(typeof(AacFlUnit), unit)})");

            var duration = unit == AacFlUnit.Frames ? numberOf / 60f : numberOf;
            return new AacFlClip(_configuration, dummyClip)
                .Animating(clip => clip.Animates("_ignored", typeof(GameObject), "m_IsActive")
                    .WithUnit(unit, keyframes => keyframes.Constant(0, 0f).Constant(duration, 0f)));
        }
        
        /// Create a new animator controller. The asset is generated into the container.
        public AacFlController NewAnimatorController()
        {
            var animatorController = AacInternals.NewAnimatorController(_configuration, Guid.NewGuid().ToString());
            return new AacFlController(_configuration, animatorController, this);
        }

        /// Create a new animator controller with a name. However, the name is only used as a suffix for the asset. The asset is generated into the container.
        public AacFlController NewAnimatorController(string name)
        {
            var animatorController = AacInternals.NewAnimatorController(_configuration, name);
            return new AacFlController(_configuration, animatorController, this);
        }
        
        //---- ## Destructive workflow

        /// Destructive workflow: Create a main layer for an arbitrary AnimatorController, clearing the previous one of the same system. You are not obligated to have a main layer.
        public AacFlLayer CreateMainArbitraryControllerLayer(AnimatorController controller) => DoCreateLayer(controller, _configuration.DefaultsProvider.ConvertLayerName(_configuration.SystemName));
        
        /// Destructive workflow: Create a supporting layer for an arbitrary AnimatorController, clearing the previous one of the same system and suffix. You can create multiple supporting layers with different suffixes, and you are not obligated to have a main layer to create a supporting layer.
        public AacFlLayer CreateSupportingArbitraryControllerLayer(AnimatorController controller, string suffix) => DoCreateLayer(controller, _configuration.DefaultsProvider.ConvertLayerNameWithSuffix(_configuration.SystemName, suffix));
        
        /// Destructive workflow: Clears the topmost layer of an arbitrary AnimatorController, and returns it.
        public AacFlLayer CreateFirstArbitraryControllerLayer(AnimatorController controller) => DoCreateLayer(controller, controller.layers[0].name);

        /// NOT FOR PUBLIC USE: Internal use only so that destructive workflow can access this. Maybe this can be improved
        [Obsolete("This will be made private/internal in V1.2.0. Use AacAccessorForExtensions.CreateLayer(...) instead")]
        public AacFlLayer InternalDoCreateLayer(AnimatorController animator, string layerName)
        {
            return DoCreateLayer(animator, layerName);
        }
        
        internal AacFlLayer AccessCreateLayer(AnimatorController animator, string layerName)
        {
            return DoCreateLayer(animator, layerName);
        }

        private AacFlLayer DoCreateLayer(AnimatorController animator, string layerName)
        {
            var ag = new AacAnimatorGenerator(animator, CreateEmptyClip().Clip, _configuration.DefaultsProvider, _configuration.AnimatorRoot);
            var machine = ag.CreateOrClearLayerAtSameIndex(layerName, 1f);

            return new AacFlLayer(animator, _configuration, machine, layerName);
        }

        internal AacFlLayer DoCreateLayerWithoutDeleting(AnimatorController animator, string layerName)
        {
            var ag = new AacAnimatorGenerator(animator, CreateEmptyClip().Clip, _configuration.DefaultsProvider, _configuration.AnimatorRoot);
            var machine = ag.CreateOrClearLayerAtSameIndex(layerName, 1f, null, false);

            return new AacFlLayer(animator, _configuration, machine, layerName);
        }

        private AacFlClip CreateEmptyClip()
        {
            var emptyClip = AnomalousSingleKeyframeClip();
            return emptyClip;
        }

        /// Removes all assets from the asset container matching the specified asset key.
        public void ClearPreviousAssets()
        {
            var allSubAssets = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(_configuration.AsPersistentContainerRequired()));
            foreach (var subAsset in allSubAssets)
            {
                if (
                    (
                        subAsset.name.StartsWith($"{AacInternals.AutoGeneratedPrefix}{_configuration.AssetKey}__")
                        || subAsset.name.StartsWith($"{AacInternals.AutoGeneratedLegacyPrefix}{_configuration.AssetKey}__")
                    )
                    
                    && (subAsset is AnimationClip || subAsset is BlendTree || subAsset is AvatarMask))
                {
                    AssetDatabase.RemoveObjectFromAsset(subAsset);
                }
            }
        }

        /// If you are not creating an animator, this returns an object from which you can obtain animator parameter objects. You should use this class if you are creating BlendTree assets without any animator controllers to back it. Otherwise, it is strongly recommended to obtain animator parameter objects directly from the layer objects instead of using NoAnimator(), as the use of NoAnimator() will not result in the registration of any parameters inside the animator controller.
        public AacFlNoAnimator NoAnimator()
        {
            return new AacFlNoAnimator();
        }
    }

    public class AacFlNoAnimator
    {
        private readonly List<string> _floatParameters = new List<string>();
        private readonly List<string> _intParameters = new List<string>();
        private readonly List<string> _boolParameters = new List<string>();
        private readonly Dictionary<string, float> _floatOverrides = new Dictionary<string, float>();
        private readonly Dictionary<string, int> _intOverrides = new Dictionary<string, int>();
        private readonly Dictionary<string, bool> _boolOverrides = new Dictionary<string, bool>();

        internal AacFlNoAnimator()
        {
        }

        /// Create a Float parameter, for use without a backing animator.
        public AacFlFloatParameter FloatParameter(string parameterName)
        {
            _floatParameters.Add(parameterName);
            return AacFlFloatParameter.Internally(parameterName);
        }

        /// Create an Int parameter, for use without a backing animator.
        public AacFlIntParameter IntParameter(string parameterName)
        {
            _intParameters.Add(parameterName);
            return AacFlIntParameter.Internally(parameterName);
        }

        /// Create a Bool parameter, for use without a backing animator.
        public AacFlBoolParameter BoolParameter(string parameterName)
        {
            _boolParameters.Add(parameterName);
            return AacFlBoolParameter.Internally(parameterName);
        }

        /// Stores the Float value of `toBeForced` parameter to `value`, which will be used in the CopyParametersAndOverridesTo() function.
        public AacFlNoAnimator OverrideValue(AacFlFloatParameter toBeForced, float value)
        {
            _floatOverrides[toBeForced.Name] = value;
            return this;
        }

        /// Stores the Int value of `toBeForced` parameter to `value`, which will be used in the CopyParametersAndOverridesTo() function.
        public AacFlNoAnimator OverrideValue(AacFlIntParameter toBeForced, int value)
        {
            _intOverrides[toBeForced.Name] = value;
            return this;
        }

        /// Stores the Bool value of `toBeForced` parameter to `value`, which will be used in the CopyParametersAndOverridesTo() function.
        public AacFlNoAnimator OverrideValue(AacFlBoolParameter toBeForced, bool value)
        {
            _boolOverrides[toBeForced.Name] = value;
            return this;
        }

        /// Copy all created parameters to the other layer, and overrides it with a value if a value was stored.
        public AacFlNoAnimator CopyParametersAndOverridesTo(AacFlLayer otherLayer)
        {
            foreach (var key in _floatParameters)
            {
                var param = otherLayer.FloatParameter(key);
                if (_floatOverrides.TryGetValue(key, out var value)) otherLayer.OverrideValue(param, value);
            }
            foreach (var key in _intParameters)
            {
                var param = otherLayer.IntParameter(key);
                if (_intOverrides.TryGetValue(key, out var value)) otherLayer.OverrideValue(param, value);
            }
            foreach (var key in _boolParameters)
            {
                var param = otherLayer.BoolParameter(key);
                if (_boolOverrides.TryGetValue(key, out var value)) otherLayer.OverrideValue(param, value);
            }

            return this;
        }
    }

    public class AacFlController
    {
        /// Exposes the underlying Unity AnimatorController.
        [PublicAPI] public AnimatorController AnimatorController { get; }

        private readonly AacConfiguration _configuration;
        private readonly AacFlBase _base;

        internal AacFlController(AacConfiguration configuration, AnimatorController animatorAnimatorController, AacFlBase originalBase)
        {
            AnimatorController = animatorAnimatorController;
            _configuration = configuration;
            _base = originalBase;
        }
        
        /// Create a new layer with a specific suffix. You cannot create multiple layers with the same suffix on the same controller.
        public AacFlLayer NewLayer(string suffix) => _base.DoCreateLayerWithoutDeleting(AnimatorController, _configuration.DefaultsProvider.ConvertLayerNameWithSuffix(_configuration.SystemName, suffix));
        
        /// Create a new layer. You cannot invoke this method multiple times on the same controller.
        public AacFlLayer NewLayer() => _base.DoCreateLayerWithoutDeleting(AnimatorController, _configuration.SystemName);
    }

    /// Removes animators from an animator controller.<br/>
    /// This class is exposed for use by extension functions that need to remove layers.
    public class AacAnimatorRemoval
    {
        private readonly AnimatorController _animatorController;

        [PublicAPI] public AacAnimatorRemoval(AnimatorController animatorController)
        {
            _animatorController = animatorController;
        }

        /// Remove a single layer having that exact name.
        public void RemoveLayer(string layerName)
        {
            var index = FindIndexOf(layerName);
            if (index == -1) return;

            AacInternals.NoUndo(_animatorController, () => _animatorController.RemoveLayer(index));
        }

        private int FindIndexOf(string layerName)
        {
            return _animatorController.layers.ToList().FindIndex(layer => layer.name == layerName);
        }
    }

    public class AacAnimatorGenerator
    {
        private readonly AnimatorController _animatorController;
        private readonly AnimationClip _emptyClip;
        private readonly IAacDefaultsProvider _defaultsProvider;
        private readonly Transform _animatorRoot;

        internal AacAnimatorGenerator(AnimatorController animatorController, AnimationClip emptyClip, IAacDefaultsProvider defaultsProvider, Transform animatorRoot)
        {
            _animatorController = animatorController;
            _emptyClip = emptyClip;
            _defaultsProvider = defaultsProvider;
            _animatorRoot = animatorRoot;
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
                        CreateParamIfNotExists(parameter.Name, AnimatorControllerParameterType.Float);
                        break;
                    case AacFlBoolParameter _:
                        CreateParamIfNotExists(parameter.Name, AnimatorControllerParameterType.Bool);
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
            if (_animatorController.parameters.FirstOrDefault(param => param.name == paramName) == null)
            {
                AacInternals.NoUndo(_animatorController, () => _animatorController.AddParameter(paramName, type));
            }
        }

        // DEPRECATED: This causes the editor window to glitch by deselecting, which is jarring for experimentation
        internal AacFlStateMachine CreateOrRemakeLayerAtSameIndex(string layerName, float weightWhenCreating, AvatarMask maskWhenCreating = null)
        {
            var originalIndexToPreserveOrdering = FindIndexOf(layerName);
            if (originalIndexToPreserveOrdering != -1)
            {
                AacInternals.NoUndo(_animatorController, () => _animatorController.RemoveLayer(originalIndexToPreserveOrdering));
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
            var machinist = new AacFlStateMachine(layer.stateMachine, _emptyClip, new AacBackingAnimator(this), _defaultsProvider, _animatorRoot);
            return machinist
                .WithAnyStatePosition(0, 7)
                .WithEntryPosition(0, -1)
                .WithExitPosition(7, -1);
        }

        internal AacFlStateMachine CreateOrClearLayerAtSameIndex(string layerName, float weightWhenCreating, AvatarMask maskWhenCreating = null, bool allowDeletion = true)
        {
            var originalIndexToPreserveOrdering = FindIndexOf(layerName);
            if (originalIndexToPreserveOrdering != -1)
            {
                if (!allowDeletion)
                {
                    throw new InvalidOperationException($"Cannot create layer with name {layerName} as it already exists. When creating layers using NewAnimatorController, you may only use unique names.");
                }
                RecursivelyClearChildrenMachines(_animatorController.layers[originalIndexToPreserveOrdering].stateMachine);

                _animatorController.layers[originalIndexToPreserveOrdering].stateMachine.stateMachines = new ChildAnimatorStateMachine[0];
                _animatorController.layers[originalIndexToPreserveOrdering].stateMachine.states = new ChildAnimatorState[0];
                _animatorController.layers[originalIndexToPreserveOrdering].stateMachine.entryTransitions = new AnimatorTransition[0];
                _animatorController.layers[originalIndexToPreserveOrdering].stateMachine.anyStateTransitions = new AnimatorStateTransition[0];
            }
            else
            {
                AacInternals.NoUndo(_animatorController, () => _animatorController.AddLayer(_animatorController.MakeUniqueLayerName(layerName)));
                originalIndexToPreserveOrdering = _animatorController.layers.Length - 1;
            }

            var layers = _animatorController.layers;
            layers[originalIndexToPreserveOrdering].avatarMask = maskWhenCreating;
            layers[originalIndexToPreserveOrdering].defaultWeight = weightWhenCreating;
            _animatorController.layers = layers;

            var layer = TryGetLayer(layerName);
            var machinist = new AacFlStateMachine(layer.stateMachine, _emptyClip, new AacBackingAnimator(this), _defaultsProvider, _animatorRoot);
            _defaultsProvider.ConfigureStateMachine(layer.stateMachine);
            return machinist;
        }

        private void RecursivelyClearChildrenMachines(AnimatorStateMachine parentMachine)
        {
            // TODO: RemoveStateMachine might already be recursive
            foreach (var childStateMachineHolder in parentMachine.stateMachines)
            {
                RecursivelyClearChildrenMachines(childStateMachineHolder.stateMachine);
                AacInternals.NoUndo(parentMachine, () => parentMachine.RemoveStateMachine(childStateMachineHolder.stateMachine));
            }
        }

        private int FindIndexOf(string layerName)
        {
            return _animatorController.layers.ToList().FindIndex(layer1 => layer1.name == layerName);
        }

        private AnimatorControllerLayer TryGetLayer(string layerName)
        {
            return _animatorController.layers.FirstOrDefault(it => it.name == layerName);
        }

        private void AddLayerWithWeight(string layerName, float weightWhenCreating, AvatarMask maskWhenCreating)
        {
            AacInternals.NoUndo(_animatorController, () => _animatorController.AddLayer(_animatorController.MakeUniqueLayerName(layerName)));

            var mutatedLayers = _animatorController.layers;
            mutatedLayers[mutatedLayers.Length - 1].defaultWeight = weightWhenCreating;
            mutatedLayers[mutatedLayers.Length - 1].avatarMask = maskWhenCreating;
            _animatorController.layers = mutatedLayers;
        }
    }
}