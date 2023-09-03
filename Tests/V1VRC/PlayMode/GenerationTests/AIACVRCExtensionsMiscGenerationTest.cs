using AnimatorAsCode.V1.VRC;
using NUnit.Framework;
using VRC.SDK3.Avatars.Components;
using VRC.SDKBase;

namespace av3_animator_as_code.Tests.V1VRC.PlayMode.GenerationTests
{
    public class AIACVRCExtensionsMiscGenerationTest : AbstractSimpleSingleLayerAIAC
    {
        [Test]
        public void It_generates_a_tracking_that_tracks_head()
        {
            var aac = TestAac();
            var controller = NewPersistentController();
            var fx = aac.CreateMainArbitraryControllerLayer(controller);

            // Exercise
            var first = fx.NewState("First");
            var whichElement = TrackingElement.Head;
            first.TrackingTracks(whichElement);

            // Verify
            var eState = controller.layers[0].stateMachine.states[0].state;
            Assert.AreEqual(1, eState.behaviours.Length);
            
            var eBehaviour = (VRCAnimatorTrackingControl)eState.behaviours[0];
            Assert.AreEqual(typeof(VRCAnimatorTrackingControl), eBehaviour.GetType());
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.Tracking, eBehaviour.trackingHead);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingLeftHand);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingRightHand);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingHip);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingLeftFoot);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingRightFoot);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingLeftFingers);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingRightFingers);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingEyes);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingMouth);
        }
        
        [Test]
        public void It_generates_a_tracking_that_animates_head()
        {
            var aac = TestAac();
            var controller = NewPersistentController();
            var fx = aac.CreateMainArbitraryControllerLayer(controller);

            // Exercise
            var first = fx.NewState("First");
            var whichElement = TrackingElement.Head;
            first.TrackingAnimates(whichElement);

            // Verify
            var eState = controller.layers[0].stateMachine.states[0].state;
            Assert.AreEqual(1, eState.behaviours.Length);
            
            var eBehaviour = (VRCAnimatorTrackingControl)eState.behaviours[0];
            Assert.AreEqual(typeof(VRCAnimatorTrackingControl), eBehaviour.GetType());
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.Animation, eBehaviour.trackingHead);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingLeftHand);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingRightHand);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingHip);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingLeftFoot);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingRightFoot);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingLeftFingers);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingRightFingers);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingEyes);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingMouth);
        }
        
        [Test]
        public void It_generates_a_tracking_that_sets_head_to_tracking()
        {
            var aac = TestAac();
            var controller = NewPersistentController();
            var fx = aac.CreateMainArbitraryControllerLayer(controller);

            // Exercise
            var first = fx.NewState("First");
            var whichElement = TrackingElement.Head;
            first.TrackingSets(whichElement, VRC_AnimatorTrackingControl.TrackingType.Tracking);

            // Verify
            var eState = controller.layers[0].stateMachine.states[0].state;
            Assert.AreEqual(1, eState.behaviours.Length);
            
            var eBehaviour = (VRCAnimatorTrackingControl)eState.behaviours[0];
            Assert.AreEqual(typeof(VRCAnimatorTrackingControl), eBehaviour.GetType());
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.Tracking, eBehaviour.trackingHead);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingLeftHand);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingRightHand);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingHip);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingLeftFoot);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingRightFoot);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingLeftFingers);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingRightFingers);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingEyes);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingMouth);
        }
        
        [Test]
        public void It_generates_a_tracking_that_sets_Head_to_animation()
        {
            var aac = TestAac();
            var controller = NewPersistentController();
            var fx = aac.CreateMainArbitraryControllerLayer(controller);

            // Exercise
            var first = fx.NewState("First");
            var whichElement = TrackingElement.Head;
            first.TrackingSets(whichElement, VRC_AnimatorTrackingControl.TrackingType.Animation);

            // Verify
            var eState = controller.layers[0].stateMachine.states[0].state;
            Assert.AreEqual(1, eState.behaviours.Length);
            
            var eBehaviour = (VRCAnimatorTrackingControl)eState.behaviours[0];
            Assert.AreEqual(typeof(VRCAnimatorTrackingControl), eBehaviour.GetType());
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.Animation, eBehaviour.trackingHead);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingLeftHand);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingRightHand);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingHip);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingLeftFoot);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingRightFoot);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingLeftFingers);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingRightFingers);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingEyes);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingMouth);
        }
        
        [Test]
        public void It_generates_a_tracking_that_sets_LeftHand_to_animation()
        {
            var aac = TestAac();
            var controller = NewPersistentController();
            var fx = aac.CreateMainArbitraryControllerLayer(controller);

            // Exercise
            var first = fx.NewState("First");
            var whichElement = TrackingElement.LeftHand;
            first.TrackingSets(whichElement, VRC_AnimatorTrackingControl.TrackingType.Animation);

            // Verify
            var eState = controller.layers[0].stateMachine.states[0].state;
            Assert.AreEqual(1, eState.behaviours.Length);
            
            var eBehaviour = (VRCAnimatorTrackingControl)eState.behaviours[0];
            Assert.AreEqual(typeof(VRCAnimatorTrackingControl), eBehaviour.GetType());
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingHead);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.Animation, eBehaviour.trackingLeftHand);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingRightHand);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingHip);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingLeftFoot);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingRightFoot);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingLeftFingers);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingRightFingers);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingEyes);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingMouth);
        }
        
        [Test]
        public void It_generates_a_tracking_that_sets_RightHand_to_animation()
        {
            var aac = TestAac();
            var controller = NewPersistentController();
            var fx = aac.CreateMainArbitraryControllerLayer(controller);

            // Exercise
            var first = fx.NewState("First");
            var whichElement = TrackingElement.RightHand;
            first.TrackingSets(whichElement, VRC_AnimatorTrackingControl.TrackingType.Animation);

            // Verify
            var eState = controller.layers[0].stateMachine.states[0].state;
            Assert.AreEqual(1, eState.behaviours.Length);
            
            var eBehaviour = (VRCAnimatorTrackingControl)eState.behaviours[0];
            Assert.AreEqual(typeof(VRCAnimatorTrackingControl), eBehaviour.GetType());
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingHead);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingLeftHand);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.Animation, eBehaviour.trackingRightHand);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingHip);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingLeftFoot);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingRightFoot);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingLeftFingers);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingRightFingers);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingEyes);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingMouth);
        }
        
        [Test]
        public void It_generates_a_tracking_that_sets_Hip_to_animation()
        {
            var aac = TestAac();
            var controller = NewPersistentController();
            var fx = aac.CreateMainArbitraryControllerLayer(controller);

            // Exercise
            var first = fx.NewState("First");
            var whichElement = TrackingElement.Hip;
            first.TrackingSets(whichElement, VRC_AnimatorTrackingControl.TrackingType.Animation);

            // Verify
            var eState = controller.layers[0].stateMachine.states[0].state;
            Assert.AreEqual(1, eState.behaviours.Length);
            
            var eBehaviour = (VRCAnimatorTrackingControl)eState.behaviours[0];
            Assert.AreEqual(typeof(VRCAnimatorTrackingControl), eBehaviour.GetType());
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingHead);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingLeftHand);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingRightHand);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.Animation, eBehaviour.trackingHip);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingLeftFoot);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingRightFoot);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingLeftFingers);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingRightFingers);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingEyes);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingMouth);
        }
        
        [Test]
        public void It_generates_a_tracking_that_sets_LeftFoot_to_animation()
        {
            var aac = TestAac();
            var controller = NewPersistentController();
            var fx = aac.CreateMainArbitraryControllerLayer(controller);

            // Exercise
            var first = fx.NewState("First");
            var whichElement = TrackingElement.LeftFoot;
            first.TrackingSets(whichElement, VRC_AnimatorTrackingControl.TrackingType.Animation);

            // Verify
            var eState = controller.layers[0].stateMachine.states[0].state;
            Assert.AreEqual(1, eState.behaviours.Length);
            
            var eBehaviour = (VRCAnimatorTrackingControl)eState.behaviours[0];
            Assert.AreEqual(typeof(VRCAnimatorTrackingControl), eBehaviour.GetType());
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingHead);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingLeftHand);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingRightHand);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingHip);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.Animation, eBehaviour.trackingLeftFoot);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingRightFoot);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingLeftFingers);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingRightFingers);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingEyes);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingMouth);
        }
        
        [Test]
        public void It_generates_a_tracking_that_sets_RightFoot_to_animation()
        {
            var aac = TestAac();
            var controller = NewPersistentController();
            var fx = aac.CreateMainArbitraryControllerLayer(controller);

            // Exercise
            var first = fx.NewState("First");
            var whichElement = TrackingElement.RightFoot;
            first.TrackingSets(whichElement, VRC_AnimatorTrackingControl.TrackingType.Animation);

            // Verify
            var eState = controller.layers[0].stateMachine.states[0].state;
            Assert.AreEqual(1, eState.behaviours.Length);
            
            var eBehaviour = (VRCAnimatorTrackingControl)eState.behaviours[0];
            Assert.AreEqual(typeof(VRCAnimatorTrackingControl), eBehaviour.GetType());
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingHead);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingLeftHand);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingRightHand);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingHip);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingLeftFoot);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.Animation, eBehaviour.trackingRightFoot);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingLeftFingers);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingRightFingers);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingEyes);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingMouth);
        }
        
        [Test]
        public void It_generates_a_tracking_that_sets_LeftFingers_to_animation()
        {
            var aac = TestAac();
            var controller = NewPersistentController();
            var fx = aac.CreateMainArbitraryControllerLayer(controller);

            // Exercise
            var first = fx.NewState("First");
            var whichElement = TrackingElement.LeftFingers;
            first.TrackingSets(whichElement, VRC_AnimatorTrackingControl.TrackingType.Animation);

            // Verify
            var eState = controller.layers[0].stateMachine.states[0].state;
            Assert.AreEqual(1, eState.behaviours.Length);
            
            var eBehaviour = (VRCAnimatorTrackingControl)eState.behaviours[0];
            Assert.AreEqual(typeof(VRCAnimatorTrackingControl), eBehaviour.GetType());
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingHead);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingLeftHand);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingRightHand);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingHip);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingLeftFoot);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingRightFoot);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.Animation, eBehaviour.trackingLeftFingers);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingRightFingers);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingEyes);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingMouth);
        }
        
        [Test]
        public void It_generates_a_tracking_that_sets_RightFingers_to_animation()
        {
            var aac = TestAac();
            var controller = NewPersistentController();
            var fx = aac.CreateMainArbitraryControllerLayer(controller);

            // Exercise
            var first = fx.NewState("First");
            var whichElement = TrackingElement.RightFingers;
            first.TrackingSets(whichElement, VRC_AnimatorTrackingControl.TrackingType.Animation);

            // Verify
            var eState = controller.layers[0].stateMachine.states[0].state;
            Assert.AreEqual(1, eState.behaviours.Length);
            
            var eBehaviour = (VRCAnimatorTrackingControl)eState.behaviours[0];
            Assert.AreEqual(typeof(VRCAnimatorTrackingControl), eBehaviour.GetType());
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingHead);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingLeftHand);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingRightHand);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingHip);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingLeftFoot);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingRightFoot);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingLeftFingers);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.Animation, eBehaviour.trackingRightFingers);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingEyes);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingMouth);
        }
        
        [Test]
        public void It_generates_a_tracking_that_sets_Eyes_to_animation()
        {
            var aac = TestAac();
            var controller = NewPersistentController();
            var fx = aac.CreateMainArbitraryControllerLayer(controller);

            // Exercise
            var first = fx.NewState("First");
            var whichElement = TrackingElement.Eyes;
            first.TrackingSets(whichElement, VRC_AnimatorTrackingControl.TrackingType.Animation);

            // Verify
            var eState = controller.layers[0].stateMachine.states[0].state;
            Assert.AreEqual(1, eState.behaviours.Length);
            
            var eBehaviour = (VRCAnimatorTrackingControl)eState.behaviours[0];
            Assert.AreEqual(typeof(VRCAnimatorTrackingControl), eBehaviour.GetType());
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingHead);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingLeftHand);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingRightHand);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingHip);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingLeftFoot);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingRightFoot);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingLeftFingers);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingRightFingers);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.Animation, eBehaviour.trackingEyes);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingMouth);
        }
        
        [Test]
        public void It_generates_a_tracking_that_sets_Mouth_to_animation()
        {
            var aac = TestAac();
            var controller = NewPersistentController();
            var fx = aac.CreateMainArbitraryControllerLayer(controller);

            // Exercise
            var first = fx.NewState("First");
            var whichElement = TrackingElement.Mouth;
            first.TrackingSets(whichElement, VRC_AnimatorTrackingControl.TrackingType.Animation);

            // Verify
            var eState = controller.layers[0].stateMachine.states[0].state;
            Assert.AreEqual(1, eState.behaviours.Length);
            
            var eBehaviour = (VRCAnimatorTrackingControl)eState.behaviours[0];
            Assert.AreEqual(typeof(VRCAnimatorTrackingControl), eBehaviour.GetType());
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingHead);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingLeftHand);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingRightHand);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingHip);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingLeftFoot);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingRightFoot);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingLeftFingers);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingRightFingers);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingEyes);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.Animation, eBehaviour.trackingMouth);
        }
        
        [Test]
        public void It_generates_a_tracking_that_sets_Head_and_Both_Hands_to_animation()
        {
            var aac = TestAac();
            var controller = NewPersistentController();
            var fx = aac.CreateMainArbitraryControllerLayer(controller);

            // Exercise
            var first = fx.NewState("First");
            first.TrackingSets(TrackingElement.Head, VRC_AnimatorTrackingControl.TrackingType.Animation);
            first.TrackingSets(TrackingElement.LeftHand, VRC_AnimatorTrackingControl.TrackingType.Animation);
            first.TrackingSets(TrackingElement.RightHand, VRC_AnimatorTrackingControl.TrackingType.Animation);

            // Verify
            var eState = controller.layers[0].stateMachine.states[0].state;
            Assert.AreEqual(1, eState.behaviours.Length);
            
            var eBehaviour = (VRCAnimatorTrackingControl)eState.behaviours[0];
            Assert.AreEqual(typeof(VRCAnimatorTrackingControl), eBehaviour.GetType());
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.Animation, eBehaviour.trackingHead);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.Animation, eBehaviour.trackingLeftHand);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.Animation, eBehaviour.trackingRightHand);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingHip);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingLeftFoot);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingRightFoot);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingLeftFingers);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingRightFingers);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingEyes);
            Assert.AreEqual(VRC_AnimatorTrackingControl.TrackingType.NoChange, eBehaviour.trackingMouth);
        }
    }
}