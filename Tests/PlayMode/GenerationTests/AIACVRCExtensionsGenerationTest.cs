using AnimatorAsCode.V0.Extensions.VRChat;
using NUnit.Framework;
using VRC.SDK3.Avatars.Components;
using VRC.SDKBase;

namespace av3_animator_as_code.Tests.PlayMode.GenerationTests
{
    public class AIACVRCExtensionsGenerationTest : AbstractSimpleSingleLayerAnimatorInternalAC
    {
        [Test]
        public void It_generates_a_driver_that_sets_bool_to_true()
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
        public void It_generates_a_driver_that_sets_bool_to_false()
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
        
        [Test]
        public void It_generates_a_driver_that_sets_int_to_300()
        {
            var aac = TestAac();
            var controller = NewPersistentController();
            var fx = aac.CreateMainArbitraryControllerLayer(controller);

            // Exercise
            var first = fx.NewState("First");
            first.Drives(fx.IntParameter("MyInt"), 300);

            // Verify
            var eState = controller.layers[0].stateMachine.states[0].state;
            Assert.AreEqual(1, eState.behaviours.Length);
            
            var eBehaviour = (VRCAvatarParameterDriver)eState.behaviours[0];
            Assert.AreEqual(typeof(VRCAvatarParameterDriver), eBehaviour.GetType());
            Assert.AreEqual(1, eBehaviour.parameters.Count);
            
            var eParameter = eBehaviour.parameters[0];
            Assert.AreEqual("MyInt", eParameter.name);
            Assert.AreEqual(VRC_AvatarParameterDriver.ChangeType.Set, eParameter.type);
            Assert.AreEqual(300f, eParameter.value);
        }
        
        [Test]
        public void It_generates_a_driver_that_sets_float_to_400_point_56()
        {
            var aac = TestAac();
            var controller = NewPersistentController();
            var fx = aac.CreateMainArbitraryControllerLayer(controller);

            // Exercise
            var first = fx.NewState("First");
            first.Drives(fx.FloatParameter("MyFloat"), 400.56f);

            // Verify
            var eState = controller.layers[0].stateMachine.states[0].state;
            Assert.AreEqual(1, eState.behaviours.Length);
            
            var eBehaviour = (VRCAvatarParameterDriver)eState.behaviours[0];
            Assert.AreEqual(typeof(VRCAvatarParameterDriver), eBehaviour.GetType());
            Assert.AreEqual(1, eBehaviour.parameters.Count);
            
            var eParameter = eBehaviour.parameters[0];
            Assert.AreEqual("MyFloat", eParameter.name);
            Assert.AreEqual(VRC_AvatarParameterDriver.ChangeType.Set, eParameter.type);
            Assert.AreEqual(400.56f, eParameter.value);
        }
        
        [Test]
        public void It_generates_a_driver_that_increases_int_by_2()
        {
            var aac = TestAac();
            var controller = NewPersistentController();
            var fx = aac.CreateMainArbitraryControllerLayer(controller);

            // Exercise
            var first = fx.NewState("First");
            first.DrivingIncreases(fx.IntParameter("MyInt"), 2);

            // Verify
            var eState = controller.layers[0].stateMachine.states[0].state;
            Assert.AreEqual(1, eState.behaviours.Length);
            
            var eBehaviour = (VRCAvatarParameterDriver)eState.behaviours[0];
            Assert.AreEqual(typeof(VRCAvatarParameterDriver), eBehaviour.GetType());
            Assert.AreEqual(1, eBehaviour.parameters.Count);
            
            var eParameter = eBehaviour.parameters[0];
            Assert.AreEqual("MyInt", eParameter.name);
            Assert.AreEqual(VRC_AvatarParameterDriver.ChangeType.Add, eParameter.type);
            Assert.AreEqual(2f, eParameter.value);
        }
        
        [Test]
        public void It_generates_a_driver_that_decreases_int_by_2_through_adding_its_opposite()
        {
            var aac = TestAac();
            var controller = NewPersistentController();
            var fx = aac.CreateMainArbitraryControllerLayer(controller);

            // Exercise
            var first = fx.NewState("First");
            first.DrivingDecreases(fx.IntParameter("MyInt"), 2);

            // Verify
            var eState = controller.layers[0].stateMachine.states[0].state;
            Assert.AreEqual(1, eState.behaviours.Length);
            
            var eBehaviour = (VRCAvatarParameterDriver)eState.behaviours[0];
            Assert.AreEqual(typeof(VRCAvatarParameterDriver), eBehaviour.GetType());
            Assert.AreEqual(1, eBehaviour.parameters.Count);
            
            var eParameter = eBehaviour.parameters[0];
            Assert.AreEqual("MyInt", eParameter.name);
            Assert.AreEqual(VRC_AvatarParameterDriver.ChangeType.Add, eParameter.type);
            Assert.AreEqual(-2f, eParameter.value);
        }
        
        [Test]
        public void It_generates_a_driver_that_increases_float_by_3_point_4()
        {
            var aac = TestAac();
            var controller = NewPersistentController();
            var fx = aac.CreateMainArbitraryControllerLayer(controller);

            // Exercise
            var first = fx.NewState("First");
            first.DrivingIncreases(fx.FloatParameter("MyFloat"), 3.4f);

            // Verify
            var eState = controller.layers[0].stateMachine.states[0].state;
            Assert.AreEqual(1, eState.behaviours.Length);
            
            var eBehaviour = (VRCAvatarParameterDriver)eState.behaviours[0];
            Assert.AreEqual(typeof(VRCAvatarParameterDriver), eBehaviour.GetType());
            Assert.AreEqual(1, eBehaviour.parameters.Count);
            
            var eParameter = eBehaviour.parameters[0];
            Assert.AreEqual("MyFloat", eParameter.name);
            Assert.AreEqual(VRC_AvatarParameterDriver.ChangeType.Add, eParameter.type);
            Assert.AreEqual(3.4f, eParameter.value);
        }
        
        [Test]
        public void It_generates_a_driver_that_decreases_float_by_3_point_4_through_adding_its_opposite()
        {
            var aac = TestAac();
            var controller = NewPersistentController();
            var fx = aac.CreateMainArbitraryControllerLayer(controller);

            // Exercise
            var first = fx.NewState("First");
            first.DrivingDecreases(fx.FloatParameter("MyFloat"), 3.4f);

            // Verify
            var eState = controller.layers[0].stateMachine.states[0].state;
            Assert.AreEqual(1, eState.behaviours.Length);
            
            var eBehaviour = (VRCAvatarParameterDriver)eState.behaviours[0];
            Assert.AreEqual(typeof(VRCAvatarParameterDriver), eBehaviour.GetType());
            Assert.AreEqual(1, eBehaviour.parameters.Count);
            
            var eParameter = eBehaviour.parameters[0];
            Assert.AreEqual("MyFloat", eParameter.name);
            Assert.AreEqual(VRC_AvatarParameterDriver.ChangeType.Add, eParameter.type);
            Assert.AreEqual(-3.4f, eParameter.value);
        }
        
        [Test]
        public void It_generates_a_driver_that_copies_a_bool_to_another_bool()
        {
            var aac = TestAac();
            var controller = NewPersistentController();
            var fx = aac.CreateMainArbitraryControllerLayer(controller);

            // Exercise
            var first = fx.NewState("First");
            first.DrivingCopies(fx.BoolParameter("MyBoolSource"), fx.BoolParameter("MyBoolDestination"));

            // Verify
            var eState = controller.layers[0].stateMachine.states[0].state;
            Assert.AreEqual(1, eState.behaviours.Length);
            
            var eBehaviour = (VRCAvatarParameterDriver)eState.behaviours[0];
            Assert.AreEqual(typeof(VRCAvatarParameterDriver), eBehaviour.GetType());
            Assert.AreEqual(1, eBehaviour.parameters.Count);
            
            var eParameter = eBehaviour.parameters[0];
            Assert.AreEqual("MyBoolDestination", eParameter.name);
            Assert.AreEqual(VRC_AvatarParameterDriver.ChangeType.Copy, eParameter.type);
            Assert.IsFalse(eParameter.convertRange);
            Assert.AreEqual("MyBoolSource", eParameter.source);
        }
        
        [Test]
        public void It_generates_a_driver_that_copies_a_bool_to_an_int()
        {
            var aac = TestAac();
            var controller = NewPersistentController();
            var fx = aac.CreateMainArbitraryControllerLayer(controller);

            // Exercise
            var first = fx.NewState("First");
            first.DrivingCopies(fx.BoolParameter("MyBoolSource"), fx.IntParameter("MyIntDestination"));

            // Verify
            var eState = controller.layers[0].stateMachine.states[0].state;
            Assert.AreEqual(1, eState.behaviours.Length);
            
            var eBehaviour = (VRCAvatarParameterDriver)eState.behaviours[0];
            Assert.AreEqual(typeof(VRCAvatarParameterDriver), eBehaviour.GetType());
            Assert.AreEqual(1, eBehaviour.parameters.Count);
            
            var eParameter = eBehaviour.parameters[0];
            Assert.AreEqual("MyIntDestination", eParameter.name);
            Assert.AreEqual(VRC_AvatarParameterDriver.ChangeType.Copy, eParameter.type);
            Assert.IsFalse(eParameter.convertRange);
            Assert.AreEqual("MyBoolSource", eParameter.source);
        }
        
        [Test]
        public void It_generates_a_driver_that_copies_a_bool_to_a_float()
        {
            var aac = TestAac();
            var controller = NewPersistentController();
            var fx = aac.CreateMainArbitraryControllerLayer(controller);

            // Exercise
            var first = fx.NewState("First");
            first.DrivingCopies(fx.BoolParameter("MyBoolSource"), fx.FloatParameter("MyFloatDestination"));

            // Verify
            var eState = controller.layers[0].stateMachine.states[0].state;
            Assert.AreEqual(1, eState.behaviours.Length);
            
            var eBehaviour = (VRCAvatarParameterDriver)eState.behaviours[0];
            Assert.AreEqual(typeof(VRCAvatarParameterDriver), eBehaviour.GetType());
            Assert.AreEqual(1, eBehaviour.parameters.Count);
            
            var eParameter = eBehaviour.parameters[0];
            Assert.AreEqual("MyFloatDestination", eParameter.name);
            Assert.AreEqual(VRC_AvatarParameterDriver.ChangeType.Copy, eParameter.type);
            Assert.IsFalse(eParameter.convertRange);
            Assert.AreEqual("MyBoolSource", eParameter.source);
        }
        
        [Test]
        public void It_generates_a_driver_that_copies_an_int_to_another_int()
        {
            var aac = TestAac();
            var controller = NewPersistentController();
            var fx = aac.CreateMainArbitraryControllerLayer(controller);

            // Exercise
            var first = fx.NewState("First");
            first.DrivingCopies(fx.IntParameter("MyIntSource"), fx.IntParameter("MyIntDestination"));

            // Verify
            var eState = controller.layers[0].stateMachine.states[0].state;
            Assert.AreEqual(1, eState.behaviours.Length);
            
            var eBehaviour = (VRCAvatarParameterDriver)eState.behaviours[0];
            Assert.AreEqual(typeof(VRCAvatarParameterDriver), eBehaviour.GetType());
            Assert.AreEqual(1, eBehaviour.parameters.Count);
            
            var eParameter = eBehaviour.parameters[0];
            Assert.AreEqual("MyIntDestination", eParameter.name);
            Assert.AreEqual(VRC_AvatarParameterDriver.ChangeType.Copy, eParameter.type);
            Assert.IsFalse(eParameter.convertRange);
            Assert.AreEqual("MyIntSource", eParameter.source);
        }
        
        [Test]
        public void It_generates_a_driver_that_copies_a_float_to_another_float()
        {
            var aac = TestAac();
            var controller = NewPersistentController();
            var fx = aac.CreateMainArbitraryControllerLayer(controller);

            // Exercise
            var first = fx.NewState("First");
            first.DrivingCopies(fx.FloatParameter("MyFloatSource"), fx.FloatParameter("MyFloatDestination"));

            // Verify
            var eState = controller.layers[0].stateMachine.states[0].state;
            Assert.AreEqual(1, eState.behaviours.Length);
            
            var eBehaviour = (VRCAvatarParameterDriver)eState.behaviours[0];
            Assert.AreEqual(typeof(VRCAvatarParameterDriver), eBehaviour.GetType());
            Assert.AreEqual(1, eBehaviour.parameters.Count);
            
            var eParameter = eBehaviour.parameters[0];
            Assert.AreEqual("MyFloatDestination", eParameter.name);
            Assert.AreEqual(VRC_AvatarParameterDriver.ChangeType.Copy, eParameter.type);
            Assert.IsFalse(eParameter.convertRange);
            Assert.AreEqual("MyFloatSource", eParameter.source);
        }
        
        [Test]
        public void It_generates_a_driver_that_remaps_a_bool_to_another_bool_to_invert_its_value()
        {
            var aac = TestAac();
            var controller = NewPersistentController();
            var fx = aac.CreateMainArbitraryControllerLayer(controller);

            // Exercise
            var first = fx.NewState("First");
            first.DrivingRemaps(fx.BoolParameter("MyBoolSource"), false, true, fx.BoolParameter("MyBoolDestination"), true, false);

            // Verify
            var eState = controller.layers[0].stateMachine.states[0].state;
            Assert.AreEqual(1, eState.behaviours.Length);
            
            var eBehaviour = (VRCAvatarParameterDriver)eState.behaviours[0];
            Assert.AreEqual(typeof(VRCAvatarParameterDriver), eBehaviour.GetType());
            Assert.AreEqual(1, eBehaviour.parameters.Count);
            
            var eParameter = eBehaviour.parameters[0];
            Assert.AreEqual("MyBoolDestination", eParameter.name);
            Assert.AreEqual(VRC_AvatarParameterDriver.ChangeType.Copy, eParameter.type);
            Assert.IsTrue(eParameter.convertRange);
            Assert.AreEqual("MyBoolSource", eParameter.source);
            Assert.AreEqual(0f, eParameter.sourceMin);
            Assert.AreEqual(1f, eParameter.sourceMax);
            Assert.AreEqual(1f, eParameter.destMin);
            Assert.AreEqual(0f, eParameter.destMax);
        }
        
        [Test]
        public void It_generates_a_driver_that_remaps_an_int_percentage_to_a_joystick_float()
        {
            var aac = TestAac();
            var controller = NewPersistentController();
            var fx = aac.CreateMainArbitraryControllerLayer(controller);

            // Exercise
            var first = fx.NewState("First");
            first.DrivingRemaps(fx.IntParameter("MyIntSource"), 0, 100, fx.FloatParameter("MyFloatDestination"), -1f, 1f);

            // Verify
            var eState = controller.layers[0].stateMachine.states[0].state;
            Assert.AreEqual(1, eState.behaviours.Length);
            
            var eBehaviour = (VRCAvatarParameterDriver)eState.behaviours[0];
            Assert.AreEqual(typeof(VRCAvatarParameterDriver), eBehaviour.GetType());
            Assert.AreEqual(1, eBehaviour.parameters.Count);
            
            var eParameter = eBehaviour.parameters[0];
            Assert.AreEqual("MyFloatDestination", eParameter.name);
            Assert.AreEqual(VRC_AvatarParameterDriver.ChangeType.Copy, eParameter.type);
            Assert.IsTrue(eParameter.convertRange);
            Assert.AreEqual("MyIntSource", eParameter.source);
            Assert.AreEqual(0f, eParameter.sourceMin);
            Assert.AreEqual(100f, eParameter.sourceMax);
            Assert.AreEqual(-1f, eParameter.destMin);
            Assert.AreEqual(1f, eParameter.destMax);
        }
        
        [Test]
        public void It_generates_a_driver_that_remaps_an_analog_float_to_an_inverse_joystick_float()
        {
            var aac = TestAac();
            var controller = NewPersistentController();
            var fx = aac.CreateMainArbitraryControllerLayer(controller);

            // Exercise
            var first = fx.NewState("First");
            first.DrivingRemaps(fx.FloatParameter("MyFloatSource"), 0, 1f, fx.FloatParameter("MyFloatDestination"), 1f, -1f);

            // Verify
            var eState = controller.layers[0].stateMachine.states[0].state;
            Assert.AreEqual(1, eState.behaviours.Length);
            
            var eBehaviour = (VRCAvatarParameterDriver)eState.behaviours[0];
            Assert.AreEqual(typeof(VRCAvatarParameterDriver), eBehaviour.GetType());
            Assert.AreEqual(1, eBehaviour.parameters.Count);
            
            var eParameter = eBehaviour.parameters[0];
            Assert.AreEqual("MyFloatDestination", eParameter.name);
            Assert.AreEqual(VRC_AvatarParameterDriver.ChangeType.Copy, eParameter.type);
            Assert.IsTrue(eParameter.convertRange);
            Assert.AreEqual("MyFloatSource", eParameter.source);
            Assert.AreEqual(0f, eParameter.sourceMin);
            Assert.AreEqual(1f, eParameter.sourceMax);
            Assert.AreEqual(1f, eParameter.destMin);
            Assert.AreEqual(-1f, eParameter.destMax);
        }
    }
}