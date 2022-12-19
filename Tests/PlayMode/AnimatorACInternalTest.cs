using NUnit.Framework;
using UnityEditor.Animations;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;

namespace av3_animator_as_code.Tests.PlayMode
{
    public class AIACStateTest : AbstractSimpleSingleLayerAnimatorInternalAC
    {
        [Test]
        public void It_should_create_layer_with_state()
        {
            var aac = TestAac();
            var controller = new AnimatorController();
            var fx = aac.CreateMainArbitraryControllerLayer(controller);

            // Exercise
            var init = fx.NewState("Init");

            var animator = _root.AddComponent<Animator>();
            animator.runtimeAnimatorController = controller;

            // Verify
            Assert.AreEqual(1, animator.layerCount);
            var info = animator.GetCurrentAnimatorStateInfo(0);
            Assert.IsTrue(info.IsName("Init"));
        }
        
        [Test]
        public void It_should_cause_write_defaults_OFF_to_preserve_animated_object_state()
        {
            var aac = TestAac();
            var controller = new AnimatorController();
            var fx = aac.CreateMainArbitraryControllerLayer(controller);

            // Exercise
            var child = new GameObject
            {
                name = "Child",
                transform = { parent = _root.transform }
            };
            var first = fx.NewState("First")
                .WithWriteDefaultsSetTo(false)
                .WithAnimation(aac.NewClip().Toggling(child, false));
            var second = fx.NewState("Second")
                .WithWriteDefaultsSetTo(false);
            first.TransitionsTo(second).AfterAnimationFinishes();

            var animator = _root.AddComponent<Animator>();
            animator.runtimeAnimatorController = controller;
            animator.enabled = false;

            // Verify
            // Frame 0
            animator.Update(0f);
            Assert.IsTrue(Info0(animator).IsName("First"));
            Assert.IsFalse(child.gameObject.activeSelf);
            
            // Frame 1
            animator.Update(1 / 60f);
            Assert.IsTrue(Info0(animator).IsName("Second"));
            Assert.IsFalse(child.gameObject.activeSelf);
        }
        
        [Test]
        public void It_should_cause_write_defaults_ON_to_revert_animated_object_state()
        {
            var aac = TestAac();
            var controller = new AnimatorController();
            var fx = aac.CreateMainArbitraryControllerLayer(controller);

            // Exercise
            var child = new GameObject
            {
                name = "Child",
                transform = { parent = _root.transform }
            };
            var first = fx.NewState("First")
                .WithWriteDefaultsSetTo(true)
                .WithAnimation(aac.NewClip().Toggling(child, false));
            var second = fx.NewState("Second")
                .WithWriteDefaultsSetTo(true);
            first.TransitionsTo(second).AfterAnimationFinishes();

            var animator = _root.AddComponent<Animator>();
            animator.runtimeAnimatorController = controller;
            animator.enabled = false;

            // Verify
            // Frame 0
            animator.Update(0f);
            Assert.IsTrue(Info0(animator).IsName("First"));
            Assert.IsFalse(child.gameObject.activeSelf);
            
            // Frame 1
            animator.Update(1 / 60f);
            Assert.IsTrue(Info0(animator).IsName("Second"));
            Assert.IsTrue(child.gameObject.activeSelf); // The object turns back on
        }

        private static AnimatorStateInfo Info0(Animator animator)
        {
            return animator.GetCurrentAnimatorStateInfo(0);
        }
    }
}