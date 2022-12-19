using AnimatorAsCode.V0.Extensions.VRChat;
using NUnit.Framework;
using VRC.SDK3.Avatars.Components;
using VRC.SDKBase;

namespace av3_animator_as_code.Tests.PlayMode.GenerationTests
{
    public class AIACVRCExtensionsGenerationTest : AbstractSimpleSingleLayerAnimatorInternalAC
    {
        [Test]
        public void It_generates_a_drive_that_sets_bool_to_true()
        {
            var aac = TestAac();
            var controller = NewPersistentController();
            var fx = aac.CreateMainArbitraryControllerLayer(controller);

            // Exercise
            var first = fx.NewState("First");
            first.Drives(fx.BoolParameter("MyBool"), true);

            // Verify
            var eState = controller.layers[0].stateMachine.states[0].state;
            Assert.AreEqual(1, eState.behaviours.Length);
            
            var eBehaviour = (VRCAvatarParameterDriver)eState.behaviours[0];
            Assert.AreEqual(typeof(VRCAvatarParameterDriver), eBehaviour.GetType());
            Assert.AreEqual(1, eBehaviour.parameters.Count);
            
            var eParameter = eBehaviour.parameters[0];
            Assert.AreEqual("MyBool", eParameter.name);
            Assert.AreEqual(VRC_AvatarParameterDriver.ChangeType.Set, eParameter.type);
            Assert.AreEqual(1f, eParameter.value);
        }
        
        [Test]
        public void It_generates_a_drive_that_sets_bool_to_false()
        {
            var aac = TestAac();
            var controller = NewPersistentController();
            var fx = aac.CreateMainArbitraryControllerLayer(controller);

            // Exercise
            var first = fx.NewState("First");
            first.Drives(fx.BoolParameter("MyBool"), false);

            // Verify
            var eState = controller.layers[0].stateMachine.states[0].state;
            Assert.AreEqual(1, eState.behaviours.Length);
            
            var eBehaviour = (VRCAvatarParameterDriver)eState.behaviours[0];
            Assert.AreEqual(typeof(VRCAvatarParameterDriver), eBehaviour.GetType());
            Assert.AreEqual(1, eBehaviour.parameters.Count);
            
            var eParameter = eBehaviour.parameters[0];
            Assert.AreEqual("MyBool", eParameter.name);
            Assert.AreEqual(VRC_AvatarParameterDriver.ChangeType.Set, eParameter.type);
            Assert.AreEqual(0f, eParameter.value);
        }
    }
}