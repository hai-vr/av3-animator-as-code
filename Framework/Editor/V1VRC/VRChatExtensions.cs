using System;
using VRC.SDK3.Avatars.Components;
using VRC.SDKBase;

// ReSharper disable once CheckNamespace
namespace AnimatorAsCode.V1.VRC
{
    public static class VRChatExtensions
    {
        /// <summary>
        /// Set <i>parameter</i> to a given <i>value</i>. For unsynced parameters, also see <i>DrivingLocally</i>.
        /// </summary>
        public static TNode Drives<TNode, TParam>(this TNode node, AacFlParameter<TParam> parameter, TParam value) where TNode : AacAnimatorNode<TNode>
        {
            var driver = node.EnsureBehaviour<VRCAvatarParameterDriver>();
            driver.parameters.Add(new VRC_AvatarParameterDriver.Parameter
            {
                type = VRC_AvatarParameterDriver.ChangeType.Set,
                name = parameter.Name, value = parameter.ValueToFloat(value)
            });
            return node;
        }

        /// <summary>
        /// Set <i>parameter</i> by increasing its current value by <i>additiveValue</i>.
        /// </summary>
        public static TNode DrivingIncreases<TNode, TParam>(this TNode node, AacFlNumericParameter<TParam> parameter, TParam additiveValue) where TNode : AacAnimatorNode<TNode>
        {
            var driver = node.EnsureBehaviour<VRCAvatarParameterDriver>();
            driver.parameters.Add(new VRC_AvatarParameterDriver.Parameter
            {
                type = VRC_AvatarParameterDriver.ChangeType.Add,
                name = parameter.Name, value = parameter.ValueToFloat(additiveValue)
            });
            return node;
        }

        /// <summary>
        /// Set <i>parameter</i> by decreasing its current value by <i>positiveValueToDecreaseBy</i>.
        /// </summary>
        public static TNode DrivingDecreases<TNode, TParam>(this TNode node, AacFlNumericParameter<TParam> parameter, TParam positiveValueToDecreaseBy) where TNode : AacAnimatorNode<TNode>
        {
            var driver = node.EnsureBehaviour<VRCAvatarParameterDriver>();
            driver.parameters.Add(new VRC_AvatarParameterDriver.Parameter
            {
                type = VRC_AvatarParameterDriver.ChangeType.Add,
                name = parameter.Name, 
                value = parameter.ValueToFloat(positiveValueToDecreaseBy) * -1
            });
            return node;
        }
        
        /// <summary>
        /// Copies <i>sourceParameter</i> to <i>destParameter</i> with the given custom ranges.
        /// https://docs.vrchat.com/docs/state-behaviors#copy
        /// </summary>
        public static TNode DrivingRemaps<TNode, TSource, TDest>(this TNode node, AacFlParameter<TSource> sourceParameter, TSource sourceMin, TSource sourceMax, AacFlParameter<TDest> destParameter, TDest destMin, TDest destMax) where TNode : AacAnimatorNode<TNode>
        {
            var driver = node.EnsureBehaviour<VRCAvatarParameterDriver>();
            driver.EnsureParameter(new VRC_AvatarParameterDriver.Parameter
            {
                name = destParameter.Name,
                type = VRC_AvatarParameterDriver.ChangeType.Copy,
                source = sourceParameter.Name,
                convertRange = true,
                sourceMin = sourceParameter.ValueToFloat(sourceMin),
                sourceMax = sourceParameter.ValueToFloat(sourceMax),
                destMin = destParameter.ValueToFloat(destMin),
                destMax = destParameter.ValueToFloat(destMax)
            });
            return node;
        }

        /// <summary>
        /// Copies <i>sourceParameter</i> to <i>destParameter</i> with no custom ranges.
        /// https://docs.vrchat.com/docs/state-behaviors#copy
        /// </summary>
        public static TNode DrivingCopies<TNode, TSource, TDest>(this TNode node, AacFlParameter<TSource> sourceParameter, AacFlParameter<TDest> destParameter) where TNode : AacAnimatorNode<TNode>
        {
            var driver = node.EnsureBehaviour<VRCAvatarParameterDriver>();
            driver.EnsureParameter(new VRC_AvatarParameterDriver.Parameter
            {
                name = destParameter.Name,
                type = VRC_AvatarParameterDriver.ChangeType.Copy,
                source = sourceParameter.Name,
            });
            return node;
        }

        /// <summary>
        /// Sets <i>parameter</i> to a random value between <i>min</i> and <i>max</i>.
        /// This is only ran for the person wearing the avatar, and usually needs to be backed by a synced variable.
        /// https://docs.vrchat.com/docs/state-behaviors#random
        /// </summary>
        public static TNode DrivingRandomizesLocally<TNode, TParam>(this TNode node, AacFlNumericParameter<TParam> parameter, TParam min, TParam max) where TNode : AacAnimatorNode<TNode>
        {
            var driver = node.EnsureBehaviour<VRCAvatarParameterDriver>();
            driver.parameters.Add(new VRC_AvatarParameterDriver.Parameter
            {
                type = VRC_AvatarParameterDriver.ChangeType.Random,
                name = parameter.Name, valueMin = parameter.ValueToFloat(min), valueMax = parameter.ValueToFloat(max)
            });
            driver.localOnly = true;
            return node;
        }

        /// <summary>
        /// Sets <i>parameter</i> to either true or false, with the given <i>chance</i>.
        /// This is only ran for the person wearing the avatar, and usually needs to be backed by a synced variable.
        /// https://docs.vrchat.com/docs/state-behaviors#random
        /// </summary>
        public static TNode DrivingRandomizesLocally<TNode>(this TNode node, AacFlBoolParameter parameter, float chance) where TNode : AacAnimatorNode<TNode>
        {
            var driver = node.EnsureBehaviour<VRCAvatarParameterDriver>();
            driver.parameters.Add(new VRC_AvatarParameterDriver.Parameter
            {
                type = VRC_AvatarParameterDriver.ChangeType.Random,
                name = parameter.Name, chance = chance
            });
            driver.localOnly = true;
            return node;
        }
        
        /// <summary>
        /// Sets <i>parameter</i> to a random value between <i>min</i> and <i>max</i>.
        /// For use with unsynced parameters. <b>Warning: The resulting value will be different for everyone seeing your avatar.</b>
        /// https://docs.vrchat.com/docs/state-behaviors#random
        /// </summary>
        public static TNode DrivingRandomizesUnsynced<TNode, TParam>(this TNode node, AacFlNumericParameter<TParam> parameter, TParam min, TParam max) where TNode : AacAnimatorNode<TNode>
        {
            var driver = node.EnsureBehaviour<VRCAvatarParameterDriver>();
            driver.parameters.Add(new VRC_AvatarParameterDriver.Parameter
            {
                type = VRC_AvatarParameterDriver.ChangeType.Random,
                name = parameter.Name, valueMin = parameter.ValueToFloat(min), valueMax = parameter.ValueToFloat(max)
            });
            return node;
        }

        /// <summary>
        /// Sets <i>parameter</i> to either true or false, with the given <i>chance</i>.
        /// For use with unsynced parameters. <b>Warning: The resulting value will be different for everyone seeing your avatar.</b>
        /// https://docs.vrchat.com/docs/state-behaviors#random
        /// </summary>
        public static TNode DrivingRandomizesUnsynced<TNode>(this TNode node, AacFlBoolParameter parameter, float chance) where TNode : AacAnimatorNode<TNode>
        {
            var driver = node.EnsureBehaviour<VRCAvatarParameterDriver>();
            driver.parameters.Add(new VRC_AvatarParameterDriver.Parameter
            {
                type = VRC_AvatarParameterDriver.ChangeType.Random,
                name = parameter.Name, chance = chance
            });
            return node;
        }

        public static TNode Drives<TNode>(this TNode node,AacFlBoolParameterGroup parameters, bool value) where TNode : AacAnimatorNode<TNode>
        {
            var driver = node.EnsureBehaviour<VRCAvatarParameterDriver>();
            foreach (var parameter in parameters.ToList())
            {
                driver.parameters.Add(new VRC_AvatarParameterDriver.Parameter
                {
                    name = parameter.Name, value = value ? 1 : 0
                });
            }
            return node;
        }
        
        /// <summary>
        /// Only set this parameter for the person wearing the avatar (recommended for synced parameters).
        /// </summary>
        public static TNode DrivingLocally<TNode>(this TNode node) where TNode : AacAnimatorNode<TNode>
        {
            var driver = node.EnsureBehaviour<VRCAvatarParameterDriver>();
            driver.localOnly = true;
            return node;
        }

        private static void EnsureParameter(this VRC_AvatarParameterDriver driver, VRC_AvatarParameterDriver.Parameter p)
        {
            for (var i = 0; i < driver.parameters.Count; i++)
            {
                if (driver.parameters[i].name != p.name) continue;
                
                driver.parameters[i] = p;
                return;
            }
            driver.parameters.Add(p);
        }
        
        public static TNode PrintsToLogUsingTrackingBehaviour<TNode>(this TNode node, string value) where TNode : AacAnimatorNode<TNode>
        {
            var tracking = node.EnsureBehaviour<VRCAnimatorTrackingControl>();
            tracking.debugString = value;

            return node;
        }

        public static TNode TrackingTracks<TNode>(this TNode node, TrackingElement element) where TNode : AacAnimatorNode<TNode>
        {
            var tracking = node.EnsureBehaviour<VRCAnimatorTrackingControl>();
            SettingElementTo(tracking, element, VRC_AnimatorTrackingControl.TrackingType.Tracking);

            return node;
        }

        public static TNode TrackingAnimates<TNode>(this TNode node, TrackingElement element) where TNode : AacAnimatorNode<TNode>
        {
            var tracking = node.EnsureBehaviour<VRCAnimatorTrackingControl>();
            SettingElementTo(tracking, element, VRC_AnimatorTrackingControl.TrackingType.Animation);

            return node;
        }

        public static TNode TrackingSets<TNode>(this TNode node, TrackingElement element, VRC_AnimatorTrackingControl.TrackingType trackingType) where TNode : AacAnimatorNode<TNode>
        {
            var tracking = node.EnsureBehaviour<VRCAnimatorTrackingControl>();
            SettingElementTo(tracking, element, trackingType);

            return node;
        }
        
        private static void SettingElementTo(VRCAnimatorTrackingControl tracking, TrackingElement element, VRC_AnimatorTrackingControl.TrackingType target)
        {
            switch (element)
            {
                case TrackingElement.Head:
                    tracking.trackingHead = target;
                    break;
                case TrackingElement.LeftHand:
                    tracking.trackingLeftHand = target;
                    break;
                case TrackingElement.RightHand:
                    tracking.trackingRightHand = target;
                    break;
                case TrackingElement.Hip:
                    tracking.trackingHip = target;
                    break;
                case TrackingElement.LeftFoot:
                    tracking.trackingLeftFoot = target;
                    break;
                case TrackingElement.RightFoot:
                    tracking.trackingRightFoot = target;
                    break;
                case TrackingElement.LeftFingers:
                    tracking.trackingLeftFingers = target;
                    break;
                case TrackingElement.RightFingers:
                    tracking.trackingRightFingers = target;
                    break;
                case TrackingElement.Eyes:
                    tracking.trackingEyes = target;
                    break;
                case TrackingElement.Mouth:
                    tracking.trackingMouth = target;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(element), element, null);
            }
        }

        public static TNode LocomotionEnabled<TNode>(this TNode node) where TNode : AacAnimatorNode<TNode>
        {
            var locomotionControl = node.EnsureBehaviour<VRCAnimatorLocomotionControl>();
            locomotionControl.disableLocomotion = false;

            return node;
        }

        public static TNode LocomotionDisabled<TNode>(this TNode node) where TNode : AacAnimatorNode<TNode>
        {
            var locomotionControl = node.EnsureBehaviour<VRCAnimatorLocomotionControl>();
            locomotionControl.disableLocomotion = true;

            return node;
        }

        public static TNode PlayableEnables<TNode>(this TNode node, VRC_PlayableLayerControl.BlendableLayer blendable, float blendDurationSeconds = 0f) where TNode : AacAnimatorNode<TNode>
        {
            var playable = node.EnsureBehaviour<VRCPlayableLayerControl>();
            PlayableSets(playable, blendable, blendDurationSeconds, 1.0f);
            return node;
        }

        public static TNode PlayableDisables<TNode>(this TNode node, VRC_PlayableLayerControl.BlendableLayer blendable, float blendDurationSeconds = 0f) where TNode : AacAnimatorNode<TNode>
        {
            var playable = node.EnsureBehaviour<VRCPlayableLayerControl>();
            PlayableSets(playable, blendable, blendDurationSeconds, 0.0f);
            return node;
        }

        private static void PlayableSets(VRCPlayableLayerControl playable, VRC_PlayableLayerControl.BlendableLayer blendable, float blendDurationSeconds, float weight)
        {
            playable.layer = blendable;
            playable.goalWeight = weight;
            playable.blendDuration = blendDurationSeconds;
        }

        public static TNode PoseSpaceEntered<TNode>(this TNode node, float delaySeconds = 0f) where TNode : AacAnimatorNode<TNode>
        {
            var temporaryPoseSpace = node.EnsureBehaviour<VRCAnimatorTemporaryPoseSpace>();
            temporaryPoseSpace.enterPoseSpace = true;
            temporaryPoseSpace.fixedDelay = true;
            temporaryPoseSpace.delayTime = delaySeconds;

            return node;
        }

        public static TNode PoseSpaceExited<TNode>(this TNode node, float delaySeconds = 0f) where TNode : AacAnimatorNode<TNode>
        {
            var temporaryPoseSpace = node.EnsureBehaviour<VRCAnimatorTemporaryPoseSpace>();
            temporaryPoseSpace.enterPoseSpace = false;
            temporaryPoseSpace.fixedDelay = true;
            temporaryPoseSpace.delayTime = delaySeconds;

            return node;
        }

        public static TNode PoseSpaceEnteredPercent<TNode>(this TNode node, float delayNormalized) where TNode : AacAnimatorNode<TNode>
        {
            var temporaryPoseSpace = node.EnsureBehaviour<VRCAnimatorTemporaryPoseSpace>();
            temporaryPoseSpace.enterPoseSpace = true;
            temporaryPoseSpace.fixedDelay = false;
            temporaryPoseSpace.delayTime = delayNormalized;

            return node;
        }

        public static TNode PoseSpaceExitedPercent<TNode>(this TNode node, float delayNormalized) where TNode : AacAnimatorNode<TNode>
        {
            var temporaryPoseSpace = node.EnsureBehaviour<VRCAnimatorTemporaryPoseSpace>();
            temporaryPoseSpace.enterPoseSpace = false;
            temporaryPoseSpace.fixedDelay = false;
            temporaryPoseSpace.delayTime = delayNormalized;
            return node;
        }
    }
}