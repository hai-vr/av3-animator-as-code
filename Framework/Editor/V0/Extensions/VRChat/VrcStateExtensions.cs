using System;
using VRC.SDK3.Avatars.Components;
using VRC.SDKBase;

namespace AnimatorAsCode.V0.Extensions.VRChat
{
    public static class VrcStateExtensions
    {
        
        public static TNode Drives<TNode>(this TNode node,AacFlIntParameter parameter, int value) where TNode : AacAnimatorNode<TNode>
        {
            var driver = node.EnsureBehaviour<VRCAvatarParameterDriver>();
            driver.parameters.Add(new VRC_AvatarParameterDriver.Parameter
            {
                type = VRC_AvatarParameterDriver.ChangeType.Set,
                name = parameter.Name, value = value
            });
            return node;
        }

        public static TNode Drives<TNode>(this TNode node,AacFlFloatParameter parameter, float value) where TNode : AacAnimatorNode<TNode>
        {
            var driver = node.EnsureBehaviour<VRCAvatarParameterDriver>();
            driver.parameters.Add(new VRC_AvatarParameterDriver.Parameter
            {
                type = VRC_AvatarParameterDriver.ChangeType.Set,
                name = parameter.Name, value = value
            });
            return node;
        }

        public static TNode DrivingIncreases<TNode>(this TNode node,AacFlFloatParameter parameter, float additiveValue) where TNode : AacAnimatorNode<TNode>
        {
            var driver = node.EnsureBehaviour<VRCAvatarParameterDriver>();
            driver.parameters.Add(new VRC_AvatarParameterDriver.Parameter
            {
                type = VRC_AvatarParameterDriver.ChangeType.Add,
                name = parameter.Name, value = additiveValue
            });
            return node;
        }

        public static TNode DrivingDecreases<TNode>(this TNode node,AacFlFloatParameter parameter, float positiveValueToDecreaseBy) where TNode : AacAnimatorNode<TNode>
        {
            var driver = node.EnsureBehaviour<VRCAvatarParameterDriver>();
            driver.parameters.Add(new VRC_AvatarParameterDriver.Parameter
            {
                type = VRC_AvatarParameterDriver.ChangeType.Add,
                name = parameter.Name, value = -positiveValueToDecreaseBy
            });
            return node;
        }

        public static TNode DrivingIncreases<TNode>(this TNode node,AacFlIntParameter parameter, int additiveValue) where TNode : AacAnimatorNode<TNode>
        {
            var driver = node.EnsureBehaviour<VRCAvatarParameterDriver>();
            driver.parameters.Add(new VRC_AvatarParameterDriver.Parameter
            {
                type = VRC_AvatarParameterDriver.ChangeType.Add,
                name = parameter.Name, value = additiveValue
            });
            return node;
        }

        public static TNode DrivingDecreases<TNode>(this TNode node,AacFlIntParameter parameter, int positiveValueToDecreaseBy) where TNode : AacAnimatorNode<TNode>
        {
            var driver = node.EnsureBehaviour<VRCAvatarParameterDriver>();
            driver.parameters.Add(new VRC_AvatarParameterDriver.Parameter
            {
                type = VRC_AvatarParameterDriver.ChangeType.Add,
                name = parameter.Name, value = -positiveValueToDecreaseBy
            });
            return node;
        }
        
        public static TNode DrivingRemaps<TNode>(this TNode node, AacFlParameter sourceParameter, float sourceMin, float sourceMax, AacFlParameter destParameter, float destMin, float destMax) where TNode : AacAnimatorNode<TNode>
        {
            var driver = node.EnsureBehaviour<VRCAvatarParameterDriver>();
            driver.EnsureParameter(new VRC_AvatarParameterDriver.Parameter
            {
                name = destParameter.Name,
                type = VRC_AvatarParameterDriver.ChangeType.Copy,
                source = sourceParameter.Name,
                convertRange = true,
                sourceMin = sourceMin,
                sourceMax = sourceMax,
                destMin = destMin,
                destMax = destMax
            });
            return node;
        }

        public static TNode DrivingCopies<TNode>(this TNode node, AacFlParameter sourceParameter, AacFlParameter destParameter) where TNode : AacAnimatorNode<TNode>
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

        public static TNode DrivingRandomizesLocally<TNode>(this TNode node,AacFlFloatParameter parameter, float min, float max) where TNode : AacAnimatorNode<TNode>
        {
            var driver = node.EnsureBehaviour<VRCAvatarParameterDriver>();
            driver.parameters.Add(new VRC_AvatarParameterDriver.Parameter
            {
                type = VRC_AvatarParameterDriver.ChangeType.Random,
                name = parameter.Name, valueMin = min, valueMax = max
            });
            driver.localOnly = true;
            return node;
        }

        public static TNode DrivingRandomizesLocally<TNode>(this TNode node,AacFlBoolParameter parameter, float chance) where TNode : AacAnimatorNode<TNode>
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

        public static TNode DrivingRandomizesLocally<TNode>(this TNode node,AacFlIntParameter parameter, int min, int max) where TNode : AacAnimatorNode<TNode>
        {
            var driver = node.EnsureBehaviour<VRCAvatarParameterDriver>();
            driver.parameters.Add(new VRC_AvatarParameterDriver.Parameter
            {
                type = VRC_AvatarParameterDriver.ChangeType.Random,
                name = parameter.Name, valueMin = min, valueMax = max
            });
            driver.localOnly = true;
            return node;
        }

        public static TNode Drives<TNode>(this TNode node,AacFlBoolParameter parameter, bool value) where TNode : AacAnimatorNode<TNode>
        {
            var driver = node.EnsureBehaviour<VRCAvatarParameterDriver>();
            driver.parameters.Add(new VRC_AvatarParameterDriver.Parameter
            {
                name = parameter.Name, value = value ? 1 : 0
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