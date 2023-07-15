#if VRC_SDK_VRCSDK3_AVATARS
using System;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using VRC.SDK3.Avatars.Components;

namespace AnimatorAsCode.Framework
{
    public partial class Aac
    {
        internal static AnimatorController AnimatorOf(
            VRCAvatarDescriptor ad,
            VRCAvatarDescriptor.AnimLayerType animLayerType
        )
        {
            return (AnimatorController)
                ad.baseAnimationLayers.First(it => it.type == animLayerType).animatorController;
        }
    }

    public partial struct AacConfiguration
    {
        public VRCAvatarDescriptor AvatarDescriptor;
    }

    public partial class AacFlBase
    {
        public void RemoveAllMainLayers()
        {
            var layerName = _configuration.SystemName;
            RemoveLayerOnAllControllers(
                _configuration.DefaultsProvider.ConvertLayerName(layerName)
            );
        }

        public void RemoveAllSupportingLayers(string suffix)
        {
            var layerName = _configuration.SystemName;
            RemoveLayerOnAllControllers(
                _configuration.DefaultsProvider.ConvertLayerNameWithSuffix(layerName, suffix)
            );
        }

        private void RemoveLayerOnAllControllers(string layerName)
        {
            var layers = _configuration.AvatarDescriptor.baseAnimationLayers
                .Select(layer => layer.animatorController)
                .Where(layer => layer != null)
                .Distinct()
                .ToList();
            foreach (var customAnimLayer in layers)
            {
                new AacAnimatorRemoval((AnimatorController)customAnimLayer).RemoveLayer(
                    _configuration.DefaultsProvider.ConvertLayerName(layerName)
                );
            }
        }

        public AacFlLayer CreateMainFxLayer() =>
            DoCreateMainLayerOnController(VRCAvatarDescriptor.AnimLayerType.FX);

        public AacFlLayer CreateMainGestureLayer() =>
            DoCreateMainLayerOnController(VRCAvatarDescriptor.AnimLayerType.Gesture);

        public AacFlLayer CreateMainActionLayer() =>
            DoCreateMainLayerOnController(VRCAvatarDescriptor.AnimLayerType.Action);

        public AacFlLayer CreateMainIdleLayer() =>
            DoCreateMainLayerOnController(VRCAvatarDescriptor.AnimLayerType.Additive);

        public AacFlLayer CreateMainLocomotionLayer() =>
            DoCreateMainLayerOnController(VRCAvatarDescriptor.AnimLayerType.Base);

        public AacFlLayer CreateMainAv3Layer(VRCAvatarDescriptor.AnimLayerType animLayerType) =>
            DoCreateMainLayerOnController(animLayerType);

        public AacFlLayer CreateSupportingFxLayer(string suffix) =>
            DoCreateSupportingLayerOnController(VRCAvatarDescriptor.AnimLayerType.FX, suffix);

        public AacFlLayer CreateSupportingGestureLayer(string suffix) =>
            DoCreateSupportingLayerOnController(VRCAvatarDescriptor.AnimLayerType.Gesture, suffix);

        public AacFlLayer CreateSupportingActionLayer(string suffix) =>
            DoCreateSupportingLayerOnController(VRCAvatarDescriptor.AnimLayerType.Action, suffix);

        public AacFlLayer CreateSupportingIdleLayer(string suffix) =>
            DoCreateSupportingLayerOnController(VRCAvatarDescriptor.AnimLayerType.Additive, suffix);

        public AacFlLayer CreateSupportingLocomotionLayer(string suffix) =>
            DoCreateSupportingLayerOnController(VRCAvatarDescriptor.AnimLayerType.Base, suffix);

        public AacFlLayer CreateSupportingAv3Layer(
            VRCAvatarDescriptor.AnimLayerType animLayerType,
            string suffix
        ) => DoCreateSupportingLayerOnController(animLayerType, suffix);

        private AacFlLayer DoCreateMainLayerOnController(VRCAvatarDescriptor.AnimLayerType animType)
        {
            var animator = Aac.AnimatorOf(_configuration.AvatarDescriptor, animType);
            var layerName = _configuration.DefaultsProvider.ConvertLayerName(
                _configuration.SystemName
            );

            return DoCreateLayer(animator, layerName);
        }

        private AacFlLayer DoCreateSupportingLayerOnController(
            VRCAvatarDescriptor.AnimLayerType animType,
            string suffix
        )
        {
            var animator = Aac.AnimatorOf(_configuration.AvatarDescriptor, animType);
            var layerName = _configuration.DefaultsProvider.ConvertLayerNameWithSuffix(
                _configuration.SystemName,
                suffix
            );

            return DoCreateLayer(animator, layerName);
        }
    }
}
#endif
