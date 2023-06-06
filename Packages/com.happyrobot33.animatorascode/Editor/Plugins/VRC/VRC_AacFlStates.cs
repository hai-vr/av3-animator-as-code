#if VRC_SDK_VRCSDK3_AVATARS
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine;
using VRC.SDK3.Avatars.Components;
using VRC.SDKBase;

using System.Reflection;

namespace AnimatorAsCode.Framework
{
    public partial class AacFlState
    {
        private VRCAvatarParameterDriver _driver;
        private VRCAnimatorTrackingControl _tracking;
        private VRCAnimatorLocomotionControl _locomotionControl;

        /// <summary> Drives the parameter provided to the value provided. </summary>
        /// <param name="parameter"> The parameter. </param>
        /// <param name="value"> The value. </param>
        /// <returns> This instance. </returns>
        public AacFlState Drives(AacFlIntParameter parameter, int value)
        {
            CreateDriverBehaviorIfNotExists();
            _driver.parameters.Add(
                new VRC_AvatarParameterDriver.Parameter
                {
                    type = VRC_AvatarParameterDriver.ChangeType.Set,
                    name = parameter.Name,
                    value = value
                }
            );
            return this;
        }

        /// <inheritdoc cref="Drives(AacFlIntParameter,int)"/>
        public AacFlState Drives(AacFlFloatParameter parameter, float value)
        {
            CreateDriverBehaviorIfNotExists();
            _driver.parameters.Add(
                new VRC_AvatarParameterDriver.Parameter
                {
                    type = VRC_AvatarParameterDriver.ChangeType.Set,
                    name = parameter.Name,
                    value = value
                }
            );
            return this;
        }

        /// <summary> Adds the value provided to the parameter provided. </summary>
        /// <param name="parameter"> The parameter. </param>
        /// <param name="additiveValue"> The additive value. </param>
        /// <returns> This instance. </returns>
        public AacFlState DrivingIncreases(AacFlFloatParameter parameter, float additiveValue)
        {
            CreateDriverBehaviorIfNotExists();
            _driver.parameters.Add(
                new VRC_AvatarParameterDriver.Parameter
                {
                    type = VRC_AvatarParameterDriver.ChangeType.Add,
                    name = parameter.Name,
                    value = additiveValue
                }
            );
            return this;
        }

        /// <summary> Subtracts the value provided from the parameter provided. </summary>
        /// <param name="parameter"> The parameter. </param>
        /// <param name="positiveValueToDecreaseBy"> The positive value to decrease by. </param>
        /// <returns> This instance. </returns>
        public AacFlState DrivingDecreases(
            AacFlFloatParameter parameter,
            float positiveValueToDecreaseBy
        )
        {
            CreateDriverBehaviorIfNotExists();
            _driver.parameters.Add(
                new VRC_AvatarParameterDriver.Parameter
                {
                    type = VRC_AvatarParameterDriver.ChangeType.Add,
                    name = parameter.Name,
                    value = -positiveValueToDecreaseBy
                }
            );
            return this;
        }

        /// <inheritdoc cref="DrivingIncreases(AacFlFloatParameter,float)"/>
        public AacFlState DrivingIncreases(AacFlIntParameter parameter, int additiveValue)
        {
            CreateDriverBehaviorIfNotExists();
            _driver.parameters.Add(
                new VRC_AvatarParameterDriver.Parameter
                {
                    type = VRC_AvatarParameterDriver.ChangeType.Add,
                    name = parameter.Name,
                    value = additiveValue
                }
            );
            return this;
        }

        /// <inheritdoc cref="DrivingDecreases(AacFlFloatParameter,float)"/>
        public AacFlState DrivingDecreases(
            AacFlIntParameter parameter,
            int positiveValueToDecreaseBy
        )
        {
            CreateDriverBehaviorIfNotExists();
            _driver.parameters.Add(
                new VRC_AvatarParameterDriver.Parameter
                {
                    type = VRC_AvatarParameterDriver.ChangeType.Add,
                    name = parameter.Name,
                    value = -positiveValueToDecreaseBy
                }
            );
            return this;
        }

        /// <summary> Randomizes the parameter provided between the values provided. </summary>
        /// <param name="parameter"> The parameter. </param>
        /// <param name="min"> The minimum value. </param>
        /// <param name="max"> The maximum value. </param>
        /// <returns> This instance. </returns>
        public AacFlState DrivingRandomizes(AacFlIntParameter parameter, int min, int max)
        {
            CreateDriverBehaviorIfNotExists();
            _driver.parameters.Add(
                new VRC_AvatarParameterDriver.Parameter
                {
                    type = VRC_AvatarParameterDriver.ChangeType.Random,
                    name = parameter.Name,
                    valueMin = min,
                    valueMax = max
                }
            );
            return this;
        }

        /// <inheritdoc cref="DrivingRandomizes(AacFlIntParameter,int,int)"/>
        /// <remarks> This version only applys to the local client </remarks>
        public AacFlState DrivingRandomizesLocally(
            AacFlFloatParameter parameter,
            float min,
            float max
        )
        {
            CreateDriverBehaviorIfNotExists();
            _driver.parameters.Add(
                new VRC_AvatarParameterDriver.Parameter
                {
                    type = VRC_AvatarParameterDriver.ChangeType.Random,
                    name = parameter.Name,
                    valueMin = min,
                    valueMax = max
                }
            );
            _driver.localOnly = true;
            return this;
        }

        /// <summary> Randomizes the parameter to true based on the chance provided. </summary>
        /// <inheritdoc cref="DrivingRandomizesLocally(AacFlFloatParameter,float,float)" path="/remarks"/>
        /// <param name="parameter"> The parameter. </param>
        /// <param name="chance"> The chance. </param>
        /// <returns> This instance. </returns>
        public AacFlState DrivingRandomizesLocally(AacFlBoolParameter parameter, float chance)
        {
            CreateDriverBehaviorIfNotExists();
            _driver.parameters.Add(
                new VRC_AvatarParameterDriver.Parameter
                {
                    type = VRC_AvatarParameterDriver.ChangeType.Random,
                    name = parameter.Name,
                    chance = chance
                }
            );
            _driver.localOnly = true;
            return this;
        }

        /// <inheritdoc cref="DrivingRandomizesLocally(AacFlFloatParameter, float, float)"/>
        public AacFlState DrivingRandomizesLocally(AacFlIntParameter parameter, int min, int max)
        {
            CreateDriverBehaviorIfNotExists();
            _driver.parameters.Add(
                new VRC_AvatarParameterDriver.Parameter
                {
                    type = VRC_AvatarParameterDriver.ChangeType.Random,
                    name = parameter.Name,
                    valueMin = min,
                    valueMax = max
                }
            );
            _driver.localOnly = true;
            return this;
        }

        /// <inheritdoc cref="Drives(AacFlFloatParameter,float)"/>
        public AacFlState Drives(AacFlBoolParameter parameter, bool value)
        {
            CreateDriverBehaviorIfNotExists();
            _driver.parameters.Add(
                new VRC_AvatarParameterDriver.Parameter
                {
                    name = parameter.Name,
                    value = value ? 1 : 0
                }
            );
            return this;
        }

        /// <inheritdoc cref="Drives(AacFlFloatParameter,float)"/>
        public AacFlState Drives(AacFlBoolParameterGroup parameters, bool value)
        {
            CreateDriverBehaviorIfNotExists();
            foreach (var parameter in parameters.ToList())
            {
                _driver.parameters.Add(
                    new VRC_AvatarParameterDriver.Parameter
                    {
                        name = parameter.Name,
                        value = value ? 1 : 0
                    }
                );
            }
            return this;
        }

        /// <summary> Sets the parameter driver to local only. </summary>
        /// <returns> This instance. </returns>
        public AacFlState DrivingLocally()
        {
            CreateDriverBehaviorIfNotExists();
            _driver.localOnly = true;
            return this;
        }

        /// <summary> Drive the destination parameter to the source parameter. </summary>
        /// <param name="source"> The source parameter. </param>
        /// <param name="destination"> The destination parameter. </param>
        /// <returns> This instance. </returns>
        public AacFlState DrivingCopies(AacFlIntParameter source, AacFlIntParameter destination)
        {
            CreateDriverBehaviorIfNotExists();
            _driver.parameters.Add(
                new VRC_AvatarParameterDriver.Parameter
                {
                    type = VRC_AvatarParameterDriver.ChangeType.Copy,
                    source = source.Name,
                    name = destination.Name,
                    convertRange = false
                }
            );
            return this;
        }

        /// <inheritdoc cref="DrivingCopies(AacFlIntParameter,AacFlIntParameter)"/>
        public AacFlState DrivingCopies(AacFlBoolParameter source, AacFlBoolParameter destination)
        {
            CreateDriverBehaviorIfNotExists();
            _driver.parameters.Add(
                new VRC_AvatarParameterDriver.Parameter
                {
                    type = VRC_AvatarParameterDriver.ChangeType.Copy,
                    source = source.Name,
                    name = destination.Name,
                    convertRange = false
                }
            );
            return this;
        }

        /// <inheritdoc cref="DrivingCopies(AacFlIntParameter,AacFlIntParameter)"/>
        public AacFlState DrivingCopies(AacFlFloatParameter source, AacFlFloatParameter destination)
        {
            CreateDriverBehaviorIfNotExists();
            _driver.parameters.Add(
                new VRC_AvatarParameterDriver.Parameter
                {
                    type = VRC_AvatarParameterDriver.ChangeType.Copy,
                    source = source.Name,
                    name = destination.Name,
                    convertRange = false
                }
            );
            return this;
        }

        /// <summary> Drive the destination parameter to the source parameter. </summary>
        /// <remarks> Remaps the two parameters. </remarks>
        /// <param name="source"> The source parameter. </param>
        /// <param name="sourceMin"> The source minimum. </param>
        /// <param name="sourceMax"> The source maximum. </param>
        /// <param name="destination"> The destination parameter. </param>
        /// <param name="destinationMin"> The destination minimum. </param>
        /// <param name="destinationMax"> The destination maximum. </param>
        /// <returns> This instance. </returns>
        public AacFlState DrivingRemaps(
            AacFlIntParameter source,
            int sourceMin,
            int sourceMax,
            AacFlIntParameter destination,
            int destinationMin,
            int destinationMax
        )
        {
            CreateDriverBehaviorIfNotExists();
            _driver.parameters.Add(
                new VRC_AvatarParameterDriver.Parameter
                {
                    type = VRC_AvatarParameterDriver.ChangeType.Copy,
                    source = source.Name,
                    name = destination.Name,
                    convertRange = true,
                    sourceMin = sourceMin,
                    sourceMax = sourceMax,
                    destMin = destinationMin,
                    destMax = destinationMax
                }
            );
            return this;
        }

        /// <inheritdoc cref="DrivingRemaps(AacFlIntParameter,int,int,AacFlIntParameter,int,int)"/>
        public AacFlState DrivingRemaps(
            AacFlBoolParameter source,
            float sourceMin,
            float sourceMax,
            AacFlBoolParameter destination,
            float destinationMin,
            float destinationMax
        )
        {
            CreateDriverBehaviorIfNotExists();
            _driver.parameters.Add(
                new VRC_AvatarParameterDriver.Parameter
                {
                    type = VRC_AvatarParameterDriver.ChangeType.Copy,
                    source = source.Name,
                    name = destination.Name,
                    convertRange = true,
                    sourceMin = sourceMin,
                    sourceMax = sourceMax,
                    destMin = destinationMin,
                    destMax = destinationMax
                }
            );
            return this;
        }

        /// <summary> Converts the source parameter to the destination parameter type. </summary>
        /// <param name="source"> The source parameter. </param>
        /// <param name="destination"> The destination parameter. </param>
        /// <returns> This instance. </returns>
        public AacFlState DrivingCasts(AacFlFloatParameter source, AacFlIntParameter destination)
        {
            CreateDriverBehaviorIfNotExists();
            _driver.parameters.Add(
                new VRC_AvatarParameterDriver.Parameter
                {
                    type = VRC_AvatarParameterDriver.ChangeType.Copy,
                    source = source.Name,
                    name = destination.Name,
                    convertRange = false
                }
            );
            return this;
        }

        /// <inheritdoc cref="DrivingCasts(AacFlFloatParameter,AacFlIntParameter)"/>
        public AacFlState DrivingCasts(AacFlIntParameter source, AacFlFloatParameter destination)
        {
            CreateDriverBehaviorIfNotExists();
            _driver.parameters.Add(
                new VRC_AvatarParameterDriver.Parameter
                {
                    type = VRC_AvatarParameterDriver.ChangeType.Copy,
                    source = source.Name,
                    name = destination.Name,
                    convertRange = false
                }
            );
            return this;
        }

        /// <inheritdoc cref="DrivingCasts(AacFlFloatParameter,AacFlIntParameter)"/>
        public AacFlState DrivingCasts(AacFlBoolParameter source, AacFlFloatParameter destination)
        {
            CreateDriverBehaviorIfNotExists();
            _driver.parameters.Add(
                new VRC_AvatarParameterDriver.Parameter
                {
                    type = VRC_AvatarParameterDriver.ChangeType.Copy,
                    source = source.Name,
                    name = destination.Name,
                    convertRange = false
                }
            );
            return this;
        }

        /// <inheritdoc cref="DrivingCasts(AacFlFloatParameter,AacFlIntParameter)"/>
        public AacFlState DrivingCasts(AacFlFloatParameter source, AacFlBoolParameter destination)
        {
            CreateDriverBehaviorIfNotExists();
            _driver.parameters.Add(
                new VRC_AvatarParameterDriver.Parameter
                {
                    type = VRC_AvatarParameterDriver.ChangeType.Copy,
                    source = source.Name,
                    name = destination.Name,
                    convertRange = false
                }
            );
            return this;
        }

        /// <inheritdoc cref="DrivingCasts(AacFlFloatParameter,AacFlIntParameter)"/>
        public AacFlState DrivingCasts(AacFlBoolParameter source, AacFlIntParameter destination)
        {
            CreateDriverBehaviorIfNotExists();
            _driver.parameters.Add(
                new VRC_AvatarParameterDriver.Parameter
                {
                    type = VRC_AvatarParameterDriver.ChangeType.Copy,
                    source = source.Name,
                    name = destination.Name,
                    convertRange = false
                }
            );
            return this;
        }

        /// <inheritdoc cref="DrivingCasts(AacFlFloatParameter,AacFlIntParameter)"/>
        public AacFlState DrivingCasts(AacFlIntParameter source, AacFlBoolParameter destination)
        {
            CreateDriverBehaviorIfNotExists();
            _driver.parameters.Add(
                new VRC_AvatarParameterDriver.Parameter
                {
                    type = VRC_AvatarParameterDriver.ChangeType.Copy,
                    source = source.Name,
                    name = destination.Name,
                    convertRange = false
                }
            );
            return this;
        }

        /// <inheritdoc cref="DrivingRemaps(AacFlIntParameter,int,int,AacFlIntParameter,int,int)"/>
        public AacFlState DrivingCasts(
            AacFlBoolParameter source,
            float sourceMin,
            float sourceMax,
            AacFlFloatParameter destination,
            float destinationMin,
            float destinationMax
        )
        {
            CreateDriverBehaviorIfNotExists();
            _driver.parameters.Add(
                new VRC_AvatarParameterDriver.Parameter
                {
                    type = VRC_AvatarParameterDriver.ChangeType.Copy,
                    source = source.Name,
                    name = destination.Name,
                    convertRange = true,
                    sourceMin = sourceMin,
                    sourceMax = sourceMax,
                    destMin = destinationMin,
                    destMax = destinationMax
                }
            );
            return this;
        }

        /// <inheritdoc cref="DrivingRemaps(AacFlIntParameter,int,int,AacFlIntParameter,int,int)"/>
        public AacFlState DrivingCasts(
            AacFlFloatParameter source,
            float sourceMin,
            float sourceMax,
            AacFlBoolParameter destination,
            float destinationMin,
            float destinationMax
        )
        {
            CreateDriverBehaviorIfNotExists();
            _driver.parameters.Add(
                new VRC_AvatarParameterDriver.Parameter
                {
                    type = VRC_AvatarParameterDriver.ChangeType.Copy,
                    source = source.Name,
                    name = destination.Name,
                    convertRange = true,
                    sourceMin = sourceMin,
                    sourceMax = sourceMax,
                    destMin = destinationMin,
                    destMax = destinationMax
                }
            );
            return this;
        }

        /// <inheritdoc cref="DrivingRemaps(AacFlIntParameter,int,int,AacFlIntParameter,int,int)"/>
        public AacFlState DrivingCasts(
            AacFlBoolParameter source,
            float sourceMin,
            float sourceMax,
            AacFlIntParameter destination,
            int destinationMin,
            int destinationMax
        )
        {
            CreateDriverBehaviorIfNotExists();
            _driver.parameters.Add(
                new VRC_AvatarParameterDriver.Parameter
                {
                    type = VRC_AvatarParameterDriver.ChangeType.Copy,
                    source = source.Name,
                    name = destination.Name,
                    convertRange = true,
                    sourceMin = sourceMin,
                    sourceMax = sourceMax,
                    destMin = destinationMin,
                    destMax = destinationMax
                }
            );
            return this;
        }

        /// <inheritdoc cref="DrivingRemaps(AacFlIntParameter,int,int,AacFlIntParameter,int,int)"/>
        public AacFlState DrivingCasts(
            AacFlIntParameter source,
            int sourceMin,
            int sourceMax,
            AacFlBoolParameter destination,
            float destinationMin,
            float destinationMax
        )
        {
            CreateDriverBehaviorIfNotExists();
            _driver.parameters.Add(
                new VRC_AvatarParameterDriver.Parameter
                {
                    type = VRC_AvatarParameterDriver.ChangeType.Copy,
                    source = source.Name,
                    name = destination.Name,
                    convertRange = true,
                    sourceMin = sourceMin,
                    sourceMax = sourceMax,
                    destMin = destinationMin,
                    destMax = destinationMax
                }
            );
            return this;
        }

        /// <inheritdoc cref="DrivingRemaps(AacFlIntParameter,int,int,AacFlIntParameter,int,int)"/>
        public AacFlState DrivingCasts(
            AacFlIntParameter source,
            int sourceMin,
            int sourceMax,
            AacFlFloatParameter destination,
            float destinationMin,
            float destinationMax
        )
        {
            CreateDriverBehaviorIfNotExists();
            _driver.parameters.Add(
                new VRC_AvatarParameterDriver.Parameter
                {
                    type = VRC_AvatarParameterDriver.ChangeType.Copy,
                    source = source.Name,
                    name = destination.Name,
                    convertRange = true,
                    sourceMin = sourceMin,
                    sourceMax = sourceMax,
                    destMin = destinationMin,
                    destMax = destinationMax
                }
            );
            return this;
        }

        private void CreateDriverBehaviorIfNotExists()
        {
            if (_driver != null)
                return;
            _driver = State.AddStateMachineBehaviour<VRCAvatarParameterDriver>();
            _driver.parameters = new List<VRC_AvatarParameterDriver.Parameter>();
        }

        /// <summary> Use an Animator Tracking Control to print logs to the avatar wearer. </summary>
        /// <param name="value"> The value to print. </param>
        /// <returns> The current state. </returns>
        public AacFlState PrintsToLogUsingTrackingBehaviour(string value)
        {
            CreateTrackingBehaviorIfNotExists();
            _tracking.debugString = value;

            return this;
        }

        /// <summary> Set an element to be tracking. </summary>
        /// <param name="element"> The element to track. </param>
        /// <returns> The current state. </returns>
        public AacFlState TrackingTracks(TrackingElement element)
        {
            CreateTrackingBehaviorIfNotExists();
            SettingElementTo(element, VRC_AnimatorTrackingControl.TrackingType.Tracking);

            return this;
        }

        /// <summary> Set an element to be animated. </summary>
        /// <param name="element"> The element to animate. </param>
        /// <returns> The current state. </returns>
        public AacFlState TrackingAnimates(TrackingElement element)
        {
            CreateTrackingBehaviorIfNotExists();
            SettingElementTo(element, VRC_AnimatorTrackingControl.TrackingType.Animation);

            return this;
        }

        public AacFlState TrackingSets(
            TrackingElement element,
            VRC_AnimatorTrackingControl.TrackingType trackingType
        )
        {
            CreateTrackingBehaviorIfNotExists();
            SettingElementTo(element, trackingType);

            return this;
        }

        /// <summary> Enable locomotion. </summary>
        /// <returns> The current state. </returns>
        public AacFlState LocomotionEnabled()
        {
            CreateLocomotionBehaviorIfNotExists();
            _locomotionControl.disableLocomotion = false;

            return this;
        }

        /// <summary> Disable locomotion. </summary>
        /// <returns> The current state. </returns>
        public AacFlState LocomotionDisabled()
        {
            CreateLocomotionBehaviorIfNotExists();
            _locomotionControl.disableLocomotion = true;

            return this;
        }

        private void SettingElementTo(
            TrackingElement element,
            VRC_AnimatorTrackingControl.TrackingType target
        )
        {
            switch (element)
            {
                case TrackingElement.Head:
                    _tracking.trackingHead = target;
                    break;
                case TrackingElement.LeftHand:
                    _tracking.trackingLeftHand = target;
                    break;
                case TrackingElement.RightHand:
                    _tracking.trackingRightHand = target;
                    break;
                case TrackingElement.Hip:
                    _tracking.trackingHip = target;
                    break;
                case TrackingElement.LeftFoot:
                    _tracking.trackingLeftFoot = target;
                    break;
                case TrackingElement.RightFoot:
                    _tracking.trackingRightFoot = target;
                    break;
                case TrackingElement.LeftFingers:
                    _tracking.trackingLeftFingers = target;
                    break;
                case TrackingElement.RightFingers:
                    _tracking.trackingRightFingers = target;
                    break;
                case TrackingElement.Eyes:
                    _tracking.trackingEyes = target;
                    break;
                case TrackingElement.Mouth:
                    _tracking.trackingMouth = target;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(element), element, null);
            }
        }

        private void CreateTrackingBehaviorIfNotExists()
        {
            if (_tracking != null)
                return;
            _tracking = State.AddStateMachineBehaviour<VRCAnimatorTrackingControl>();
        }

        private void CreateLocomotionBehaviorIfNotExists()
        {
            if (_locomotionControl != null)
                return;
            _locomotionControl = State.AddStateMachineBehaviour<VRCAnimatorLocomotionControl>();
        }

        public enum TrackingElement
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
    }
}
#endif
