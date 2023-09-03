using System;
using UnityEditor;
using UnityEngine;
using VRC.SDK3.Avatars.Components;
using VRC.SDKBase;

// ReSharper disable once CheckNamespace
namespace AnimatorAsCode.V1.VRC
{
    public static class AacVRCExtensions
    {
        /// Return an AacVrcAssetLibrary, which lets you select various assets from VRChat.
        public static AacVrcAssetLibrary VrcAssets(this AacFlBase that)
        {
            return new AacVrcAssetLibrary();
        }
        
        public static AacAv3 Av3(this AacFlLayer that)
        {
            return new AacAv3(that.InternalStateMachine().InternalBackingAnimator());
        }

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

        public static TNode TrackingTracks<TNode>(this TNode node, AacAv3.Av3TrackingElement element) where TNode : AacAnimatorNode<TNode>
        {
            var tracking = node.EnsureBehaviour<VRCAnimatorTrackingControl>();
            SettingElementTo(tracking, element, VRC_AnimatorTrackingControl.TrackingType.Tracking);

            return node;
        }

        public static TNode TrackingAnimates<TNode>(this TNode node, AacAv3.Av3TrackingElement element) where TNode : AacAnimatorNode<TNode>
        {
            var tracking = node.EnsureBehaviour<VRCAnimatorTrackingControl>();
            SettingElementTo(tracking, element, VRC_AnimatorTrackingControl.TrackingType.Animation);

            return node;
        }

        public static TNode TrackingSets<TNode>(this TNode node, AacAv3.Av3TrackingElement element, VRC_AnimatorTrackingControl.TrackingType trackingType) where TNode : AacAnimatorNode<TNode>
        {
            var tracking = node.EnsureBehaviour<VRCAnimatorTrackingControl>();
            SettingElementTo(tracking, element, trackingType);

            return node;
        }
        
        private static void SettingElementTo(VRCAnimatorTrackingControl tracking, AacAv3.Av3TrackingElement element, VRC_AnimatorTrackingControl.TrackingType target)
        {
            switch (element)
            {
                case AacAv3.Av3TrackingElement.Head:
                    tracking.trackingHead = target;
                    break;
                case AacAv3.Av3TrackingElement.LeftHand:
                    tracking.trackingLeftHand = target;
                    break;
                case AacAv3.Av3TrackingElement.RightHand:
                    tracking.trackingRightHand = target;
                    break;
                case AacAv3.Av3TrackingElement.Hip:
                    tracking.trackingHip = target;
                    break;
                case AacAv3.Av3TrackingElement.LeftFoot:
                    tracking.trackingLeftFoot = target;
                    break;
                case AacAv3.Av3TrackingElement.RightFoot:
                    tracking.trackingRightFoot = target;
                    break;
                case AacAv3.Av3TrackingElement.LeftFingers:
                    tracking.trackingLeftFingers = target;
                    break;
                case AacAv3.Av3TrackingElement.RightFingers:
                    tracking.trackingRightFingers = target;
                    break;
                case AacAv3.Av3TrackingElement.Eyes:
                    tracking.trackingEyes = target;
                    break;
                case AacAv3.Av3TrackingElement.Mouth:
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

    public class AacAv3
    {
        private readonly AacBackingAnimator _backingAnimator;

        internal AacAv3(AacBackingAnimator backingAnimator)
        {
            _backingAnimator = backingAnimator;
        }

        // ReSharper disable InconsistentNaming
        public AacFlBoolParameter IsLocal => _backingAnimator.BoolParameter("IsLocal");
        public AacFlEnumIntParameter<Av3Viseme> Viseme => _backingAnimator.EnumParameter<Av3Viseme>("Viseme");
        public AacFlEnumIntParameter<Av3Gesture> GestureLeft => _backingAnimator.EnumParameter<Av3Gesture>("GestureLeft");
        public AacFlEnumIntParameter<Av3Gesture> GestureRight => _backingAnimator.EnumParameter<Av3Gesture>("GestureRight");
        public AacFlFloatParameter GestureLeftWeight => _backingAnimator.FloatParameter("GestureLeftWeight");
        public AacFlFloatParameter GestureRightWeight => _backingAnimator.FloatParameter("GestureRightWeight");
        public AacFlFloatParameter AngularY => _backingAnimator.FloatParameter("AngularY");
        public AacFlFloatParameter VelocityX => _backingAnimator.FloatParameter("VelocityX");
        public AacFlFloatParameter VelocityY => _backingAnimator.FloatParameter("VelocityY");
        public AacFlFloatParameter VelocityZ => _backingAnimator.FloatParameter("VelocityZ");
        public AacFlFloatParameter Upright => _backingAnimator.FloatParameter("Upright");
        public AacFlBoolParameter Grounded => _backingAnimator.BoolParameter("Grounded");
        public AacFlBoolParameter Seated => _backingAnimator.BoolParameter("Seated");
        public AacFlBoolParameter AFK => _backingAnimator.BoolParameter("AFK");
        public AacFlIntParameter TrackingType => _backingAnimator.IntParameter("TrackingType");
        public AacFlIntParameter VRMode => _backingAnimator.IntParameter("VRMode");
        public AacFlBoolParameter MuteSelf => _backingAnimator.BoolParameter("MuteSelf");
        public AacFlBoolParameter InStation => _backingAnimator.BoolParameter("InStation");
        public AacFlFloatParameter Voice => _backingAnimator.FloatParameter("Voice");
        // ReSharper restore InconsistentNaming

        public IAacFlCondition ItIsRemote() => IsLocal.IsFalse();
        public IAacFlCondition ItIsLocal() => IsLocal.IsTrue();
    
        public enum Av3TrackingElement
        {
            Head,
            LeftHand,
            RightHand,
            Hip,
            LeftFoot,
            RightFoot,
            LeftFingers,
            RightFingers,
            Eyes,
            Mouth
        }

        public enum Av3Gesture
        {
            // Specify all the values explicitly because they should be dictated by VRChat, not enumeration order.
            Neutral = 0,
            Fist = 1,
            HandOpen = 2,
            Fingerpoint = 3,
            Victory = 4,
            RockNRoll = 5,
            HandGun = 6,
            ThumbsUp = 7
        }

        public enum Av3Viseme
        {
            // Specify all the values explicitly because they should be dictated by VRChat, not enumeration order.
            // ReSharper disable InconsistentNaming
            sil = 0,
            pp = 1,
            ff = 2,
            th = 3,
            dd = 4,
            kk = 5,
            ch = 6,
            ss = 7,
            nn = 8,
            rr = 9,
            aa = 10,
            e = 11,
            ih = 12,
            oh = 13,
            ou = 14
            // ReSharper restore InconsistentNaming
        }
    }

    public class AacVrcAssetLibrary
    {
        public AvatarMask LeftHandAvatarMask()
        {
            return AssetDatabase.LoadAssetAtPath<AvatarMask>("Packages/com.vrchat.avatars/Samples/AV3 Demo Assets/Animation/Masks/vrc_Hand Left.mask");
        }

        public AvatarMask RightHandAvatarMask()
        {
            return AssetDatabase.LoadAssetAtPath<AvatarMask>("Packages/com.vrchat.avatars/Samples/AV3 Demo Assets/Animation/Masks/vrc_Hand Right.mask");
        }

        public AnimationClip ProxyForGesture(AacAv3.Av3Gesture gesture, bool masculine)
        {
            return AssetDatabase.LoadAssetAtPath<AnimationClip>("Packages/com.vrchat.avatars/Samples/AV3 Demo Assets/Animation/ProxyAnim/" + ResolveProxyFilename(gesture, masculine));
        }

        private static string ResolveProxyFilename(AacAv3.Av3Gesture gesture, bool masculine)
        {
            switch (gesture)
            {
                case AacAv3.Av3Gesture.Neutral: return masculine ? "proxy_hands_idle.anim" : "proxy_hands_idle2.anim";
                case AacAv3.Av3Gesture.Fist: return "proxy_hands_fist.anim";
                case AacAv3.Av3Gesture.HandOpen: return "proxy_hands_open.anim";
                case AacAv3.Av3Gesture.Fingerpoint: return "proxy_hands_point.anim";
                case AacAv3.Av3Gesture.Victory: return "proxy_hands_peace.anim";
                case AacAv3.Av3Gesture.RockNRoll: return "proxy_hands_rock.anim";
                case AacAv3.Av3Gesture.HandGun: return "proxy_hands_gun.anim";
                case AacAv3.Av3Gesture.ThumbsUp: return "proxy_hands_thumbs_up.anim";
                default:
                    throw new ArgumentOutOfRangeException(nameof(gesture), gesture, null);
            }
        }
    }
}