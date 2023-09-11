using System;
using System.Linq;
using UnityEditor.Animations;
using VRC.SDK3.Avatars.Components;

// ReSharper disable once CheckNamespace
namespace AnimatorAsCode.V1.VRCDestructiveWorkflow
{
    // ReSharper disable once InconsistentNaming
    public static class AacVRCDestructiveWorkflowExtensions
    {
        private interface IAdditionalDataAvatarDescriptor {}
        
        public static AacConfiguration WithAvatarDescriptor(this AacConfiguration that, VRCAvatarDescriptor avatarDescriptor)
        {
            return that.WithAdditonalData(typeof(IAdditionalDataAvatarDescriptor), avatarDescriptor);
        }
        
        /// Create the main Fx layer of that system, clearing the previous one of the same system. You are not obligated to have a main layer.
        public static AacFlLayer CreateMainFxLayer(this AacFlBase that) => DoCreateMainLayerOnController(that, VRCAvatarDescriptor.AnimLayerType.FX);
        
        /// Create the main Gesture layer of that system, clearing the previous one of the same system. You are not obligated to have a main layer.
        public static AacFlLayer CreateMainGestureLayer(this AacFlBase that) => DoCreateMainLayerOnController(that, VRCAvatarDescriptor.AnimLayerType.Gesture);
        
        /// Create the main Action layer of that system, clearing the previous one of the same system. You are not obligated to have a main layer.
        public static AacFlLayer CreateMainActionLayer(this AacFlBase that) => DoCreateMainLayerOnController(that, VRCAvatarDescriptor.AnimLayerType.Action);
        
        /// Create the main Idle layer of that system, clearing the previous one of the same system. You are not obligated to have a main layer.
        public static AacFlLayer CreateMainIdleLayer(this AacFlBase that) => DoCreateMainLayerOnController(that, VRCAvatarDescriptor.AnimLayerType.Additive);
        
        /// Create the main Locomotion layer of that system, clearing the previous one of the same system. You are not obligated to have a main layer.
        public static AacFlLayer CreateMainLocomotionLayer(this AacFlBase that) => DoCreateMainLayerOnController(that, VRCAvatarDescriptor.AnimLayerType.Base);
        
        /// Create the main layer of that system for a specific type of layer, clearing the previous one of the same system. You are not obligated to have a main layer.
        public static AacFlLayer CreateMainAv3Layer(this AacFlBase that, VRCAvatarDescriptor.AnimLayerType animLayerType) => DoCreateMainLayerOnController(that, animLayerType);

        /// Create a supporting Fx layer for that system and suffix, clearing the previous one of the same system and suffix. You can create multiple supporting layers with different suffixes, and you are not obligated to have a main layer to create a supporting layer.
        public static AacFlLayer CreateSupportingFxLayer(this AacFlBase that, string suffix) => DoCreateSupportingLayerOnController(that, VRCAvatarDescriptor.AnimLayerType.FX, suffix);
        
        /// Create a supporting Gesture layer for that system and suffix, clearing the previous one of the same system and suffix. You can create multiple supporting layers with different suffixes, and you are not obligated to have a main layer to create a supporting layer.
        public static AacFlLayer CreateSupportingGestureLayer(this AacFlBase that, string suffix) => DoCreateSupportingLayerOnController(that, VRCAvatarDescriptor.AnimLayerType.Gesture, suffix);
        
        /// Create a supporting Action layer for that system and suffix, clearing the previous one of the same system and suffix. You can create multiple supporting layers with different suffixes, and you are not obligated to have a main layer to create a supporting layer.
        public static AacFlLayer CreateSupportingActionLayer(this AacFlBase that, string suffix) => DoCreateSupportingLayerOnController(that, VRCAvatarDescriptor.AnimLayerType.Action, suffix);
        
        /// Create a supporting Idle layer for that system and suffix, clearing the previous one of the same system and suffix. You can create multiple supporting layers with different suffixes, and you are not obligated to have a main layer to create a supporting layer.
        public static AacFlLayer CreateSupportingIdleLayer(this AacFlBase that, string suffix) => DoCreateSupportingLayerOnController(that, VRCAvatarDescriptor.AnimLayerType.Additive, suffix);
        
        /// Create a supporting Locomotion layer for that system and suffix, clearing the previous one of the same system and suffix. You can create multiple supporting layers with different suffixes, and you are not obligated to have a main layer to create a supporting layer.
        public static AacFlLayer CreateSupportingLocomotionLayer(this AacFlBase that, string suffix) => DoCreateSupportingLayerOnController(that, VRCAvatarDescriptor.AnimLayerType.Base, suffix);
        
        /// Create a supporting layer for that system and suffix, clearing the previous one of the same system and suffix. You can create multiple supporting layers with different suffixes, and you are not obligated to have a main layer to create a supporting layer.
        public static AacFlLayer CreateSupportingAv3Layer(this AacFlBase that, VRCAvatarDescriptor.AnimLayerType animLayerType, string suffix) => DoCreateSupportingLayerOnController(that, animLayerType, suffix);

        private static AacFlLayer DoCreateMainLayerOnController(AacFlBase that, VRCAvatarDescriptor.AnimLayerType animType)
        {
            var animator = AnimatorOf(AvatarDescriptor(that), animType);
            var layerName = that.InternalConfiguration().DefaultsProvider.ConvertLayerName(that.InternalConfiguration().SystemName);

            return that.InternalDoCreateLayer(animator, layerName);
        }

        private static AacFlLayer DoCreateSupportingLayerOnController(AacFlBase that, VRCAvatarDescriptor.AnimLayerType animType, string suffix)
        {
            var animator = AnimatorOf(AvatarDescriptor(that), animType);
            var layerName = that.InternalConfiguration().DefaultsProvider.ConvertLayerNameWithSuffix(that.InternalConfiguration().SystemName, suffix);

            return that.InternalDoCreateLayer(animator, layerName);
        }

        internal static AnimatorController AnimatorOf(VRCAvatarDescriptor ad, VRCAvatarDescriptor.AnimLayerType animLayerType)
        {
            return (AnimatorController) ad.baseAnimationLayers.First(it => it.type == animLayerType).animatorController;
        }

        /// Remove all main layers matching that system from all animators of the Avatar descriptor.
        public static void RemoveAllMainLayers(this AacFlBase that)
        {
            var layerName = that.InternalConfiguration().SystemName;
            RemoveLayerOnAllControllers(that, that.InternalConfiguration().DefaultsProvider.ConvertLayerName(layerName));
        }

        /// Remove all supporting layers matching that system and suffix from all animators of the Avatar descriptor.
        public static void RemoveAllSupportingLayers(this AacFlBase that, string suffix)
        {
            var layerName = that.InternalConfiguration().SystemName;
            RemoveLayerOnAllControllers(that, that.InternalConfiguration().DefaultsProvider.ConvertLayerNameWithSuffix(layerName, suffix));
        }

        private static void RemoveLayerOnAllControllers(AacFlBase that, string layerName)
        {
            var layers = AvatarDescriptor(that).baseAnimationLayers.Select(layer => layer.animatorController).Where(layer => layer != null).Distinct().ToList();
            foreach (var customAnimLayer in layers)
            {
                new AacAnimatorRemoval((AnimatorController) customAnimLayer).RemoveLayer(that.InternalConfiguration().DefaultsProvider.ConvertLayerName(layerName));
            }
        }

        private static VRCAvatarDescriptor AvatarDescriptor(AacFlBase that)
        {
            var foundAvatarDescriptor =
                that.InternalConfiguration().TryGetAdditionalData(typeof(IAdditionalDataAvatarDescriptor), out var avatarDescriptor);
            if (!foundAvatarDescriptor)
            {
                throw new InvalidOperationException(
                    "Could not find avatar descriptor in configuration. Invoke .WithAvatarDescriptor(...) on the configuration object");
            }

            return (VRCAvatarDescriptor)avatarDescriptor;
        }
    }
}