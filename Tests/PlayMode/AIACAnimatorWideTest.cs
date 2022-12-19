using NUnit.Framework;
using UnityEditor.Animations;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;

namespace av3_animator_as_code.Tests.PlayMode
{
    public class AIACAnimatorWideTest : AbstractSimpleSingleLayerAIAC
    {
        [Test]
        public void It_should_override_bool()
        {
            var aac = TestAac();
            var controller = new AnimatorController();
            var fx = aac.CreateMainArbitraryControllerLayer(controller);

            // Exercise
            var first = fx.NewState("First");
            var second = fx.NewState("Second");
            var myBool = fx.BoolParameter("MyBool");
            first.TransitionsTo(second).When(myBool.IsTrue());
            fx.OverrideValue(myBool, true);

            var animator = _root.AddComponent<Animator>();
            animator.runtimeAnimatorController = controller;
            animator.enabled = false;

            // Verify
            // Frame 0
            Assert.IsTrue(Info0(animator).IsName("First"));
            
            // Frame 1
            animator.Update(1 / 60f);
            Assert.IsTrue(Info0(animator).IsName("Second"));
        }
        
        [Test]
        public void It_should_override_int()
        {
            var aac = TestAac();
            var controller = new AnimatorController();
            var fx = aac.CreateMainArbitraryControllerLayer(controller);

            // Exercise
            var first = fx.NewState("First");
            var second = fx.NewState("Second");
            var myInt = fx.IntParameter("MyInt");
            first.TransitionsTo(second).When(myInt.IsGreaterThan(1));
            fx.OverrideValue(myInt, 2);

            var animator = _root.AddComponent<Animator>();
            animator.runtimeAnimatorController = controller;
            animator.enabled = false;

            // Verify
            // Frame 0
            Assert.IsTrue(Info0(animator).IsName("First"));
            
            // Frame 1
            animator.Update(1 / 60f);
            Assert.IsTrue(Info0(animator).IsName("Second"));
        }
        
        [Test]
        public void It_should_override_float()
        {
            var aac = TestAac();
            var controller = new AnimatorController();
            var fx = aac.CreateMainArbitraryControllerLayer(controller);

            // Exercise
            var first = fx.NewState("First");
            var second = fx.NewState("Second");
            var myFloat = fx.FloatParameter("MyFloat");
            first.TransitionsTo(second).When(myFloat.IsGreaterThan(0.49f));
            fx.OverrideValue(myFloat, 0.5f);

            var animator = _root.AddComponent<Animator>();
            animator.runtimeAnimatorController = controller;
            animator.enabled = false;

            // Verify
            // Frame 0
            Assert.IsTrue(Info0(animator).IsName("First"));
            
            // Frame 1
            animator.Update(1 / 60f);
            Assert.IsTrue(Info0(animator).IsName("Second"));
        }

        private static AnimatorStateInfo Info0(Animator animator)
        {
            return animator.GetCurrentAnimatorStateInfo(0);
        }
    }
}