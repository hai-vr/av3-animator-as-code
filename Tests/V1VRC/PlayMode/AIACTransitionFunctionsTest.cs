using AnimatorAsCode.V1;
using NUnit.Framework;
using UnityEditor.Animations;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;

namespace av3_animator_as_code.Tests.V1VRC.PlayMode
{
    public class AIACTransitionFunctionsTest : AbstractSimpleSingleLayerAIAC
    {
        [Test]
        public void It_should_transition_after_animation_finishes()
        {
            var aac = TestAac();
            var controller = new AnimatorController();
            var fx = aac.CreateMainArbitraryControllerLayer(controller);

            // Exercise
            var first = fx.NewState("First");
            var second = fx.NewState("Second");
            first.TransitionsTo(second).AfterAnimationFinishes();

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
        public void It_should_transition_after_animation_finishes_in_2_frames()
        {
            var aac = TestAac();
            var controller = new AnimatorController();
            var fx = aac.CreateMainArbitraryControllerLayer(controller);

            // Exercise
            var first = fx.NewState("First");
            first.WithAnimation(aac.DummyClipLasting(2, AacFlUnit.Frames));
            var second = fx.NewState("Second");
            first.TransitionsTo(second).AfterAnimationFinishes();

            var animator = _root.AddComponent<Animator>();
            animator.runtimeAnimatorController = controller;
            animator.enabled = false;
            
            // Verify
            // Frame 0 & 1
            for (var iter = 0; iter < 2; iter++)
            {
                Assert.IsTrue(Info0(animator).IsName("First"), $"In iteration {iter}");
                animator.Update(1 / 60f);
            }
            
            // Frame 2
            Assert.IsTrue(Info0(animator).IsName("Second"));
        }
        
        [Test]
        public void It_should_transition_after_animation_finishes_in_3_frames()
        {
            var aac = TestAac();
            var controller = new AnimatorController();
            var fx = aac.CreateMainArbitraryControllerLayer(controller);

            // Exercise
            var first = fx.NewState("First");
            first.WithAnimation(aac.DummyClipLasting(3, AacFlUnit.Frames));
            var second = fx.NewState("Second");
            first.TransitionsTo(second).AfterAnimationFinishes();

            var animator = _root.AddComponent<Animator>();
            animator.runtimeAnimatorController = controller;
            animator.enabled = false;
            
            // Verify
            // Frames [0:2]
            for (var iter = 0; iter < 3; iter++)
            {
                Assert.IsTrue(Info0(animator).IsName("First"), $"In iteration {iter}");
                animator.Update(1 / 60f);
            }
            
            // Frame 3
            Assert.IsTrue(Info0(animator).IsName("Second"));
        }

        [Test]
        public void It_should_transition_after_animation_finishes_in_59_frames()
        {
            var aac = TestAac();
            var controller = new AnimatorController();
            var fx = aac.CreateMainArbitraryControllerLayer(controller);

            // Exercise
            var first = fx.NewState("First");
            first.WithAnimation(aac.DummyClipLasting(59, AacFlUnit.Frames));
            var second = fx.NewState("Second");
            first.TransitionsTo(second).AfterAnimationFinishes();

            var animator = _root.AddComponent<Animator>();
            animator.runtimeAnimatorController = controller;
            animator.enabled = false;
            
            // Verify
            // Frame [0:59]
            for (var iter = 0; iter < 59; iter++)
            {
                Assert.IsTrue(Info0(animator).IsName("First"), $"In iteration {iter}");
                animator.Update(1 / 60f);
            }
            
            // Frame 60
            Assert.IsTrue(Info0(animator).IsName("Second"));
        }

        [Test]
        public void It_should_transition_after_animation_finishes_in_60_frames_with_plus_one_error()
        {
            var aac = TestAac();
            var controller = new AnimatorController();
            var fx = aac.CreateMainArbitraryControllerLayer(controller);

            // Exercise
            var first = fx.NewState("First");
            first.WithAnimation(aac.DummyClipLasting(60, AacFlUnit.Frames));
            var second = fx.NewState("Second");
            first.TransitionsTo(second).AfterAnimationFinishes();

            var animator = _root.AddComponent<Animator>();
            animator.runtimeAnimatorController = controller;
            animator.enabled = false;
            
            // Verify
            // Frame [0:61] (!!! !!!)
            var error = 1;
            for (var iter = 0; iter < 60 + error; iter++)
            {
                Assert.IsTrue(Info0(animator).IsName("First"), $"In iteration {iter}");
                animator.Update(1 / 60f);
            }
            
            // Frame 62
            Assert.IsTrue(Info0(animator).IsName("Second"));
        }

        private static AnimatorStateInfo Info0(Animator animator)
        {
            return animator.GetCurrentAnimatorStateInfo(0);
        }
    }
}