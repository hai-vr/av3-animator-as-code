using AnimatorAsCode.V0;
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
        
        [Test]
        public void It_should_apply_motion_time()
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
            fx.NewState("First")
                .WithAnimation(aac.NewClip().Animating(clip =>
                {
                    clip.Animates(child.transform, "m_LocalScale.x").WithSecondsUnit(keyframes => keyframes.Linear(0f, 0f).Linear(1f, 10f));
                }))
                .MotionTime(fx.FloatParameter("MyFloat"));

            var animator = _root.AddComponent<Animator>();
            animator.runtimeAnimatorController = controller;
            animator.enabled = false;

            // Verify
            // Frame 0
            animator.SetFloat("MyFloat", 0.5f);
            animator.Update(1 / 60f);
            Assert.AreEqual(5f, child.transform.localScale.x);
        }
        
        [Test]
        public void It_should_apply_speed()
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
            fx.NewState("First")
                .WithAnimation(aac.NewClip().Animating(clip =>
                {
                    clip.Animates(child.transform, "m_LocalScale.x").WithSecondsUnit(keyframes => keyframes.Linear(0f, 0f).Linear(1f, 10f));
                }))
                .WithSpeed(fx.FloatParameter("MyFloat"));

            var animator = _root.AddComponent<Animator>();
            animator.runtimeAnimatorController = controller;
            animator.enabled = false;

            // Verify
            // Frame 0
            animator.SetFloat("MyFloat", 2f); // At double the speed
            animator.Update(15 / 60f); // Advance by a quarter of a second
            Assert.AreEqual(5f, child.transform.localScale.x);
        }
        
        [Test]
        public void It_should_apply_speed_set_to_constant()
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
            fx.NewState("First")
                .WithAnimation(aac.NewClip().Animating(clip =>
                {
                    clip.Animates(child.transform, "m_LocalScale.x").WithSecondsUnit(keyframes => keyframes.Linear(0f, 0f).Linear(1f, 10f));
                }))
                .WithSpeedSetTo(2f); // At double the speed

            var animator = _root.AddComponent<Animator>();
            animator.runtimeAnimatorController = controller;
            animator.enabled = false;

            // Verify
            // Frame 0
            animator.Update(15 / 60f); // Advance by a quarter of a second
            Assert.AreEqual(5f, child.transform.localScale.x);
        }

        private static AnimatorStateInfo Info0(Animator animator)
        {
            return animator.GetCurrentAnimatorStateInfo(0);
        }
    }
}