using System.Collections;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.TestTools;

namespace av3_animator_as_code.Tests.PlayMode
{
    public class AnimatorInternalACBoolTransitionTest : AbstractSimpleSingleLayerAnimatorInternalAC
    {
        [UnityTest]
        public IEnumerator It_should_transition_when_bool_is_true()
        {
            var controller = new AnimatorController();
            var aac = TestAac();
            var fx = aac.CreateMainArbitraryControllerLayer(controller);

            // Exercise
            var first = fx.NewState("First");
            var second = fx.NewState("Second");
            var myBool = fx.BoolParameter("MyBool");
            first.TransitionsTo(second).When(myBool.IsTrue());

            var animator = _root.AddComponent<Animator>();
            animator.runtimeAnimatorController = controller;

            // Verify
            // Frame 0
            Assert.AreEqual(1, animator.layerCount);
            Assert.IsTrue(Info0(animator).IsName("First"));
            
            // Frame 1
            yield return null;
            Assert.IsTrue(Info0(animator).IsName("First"));
            
            // Frame 2
            animator.SetBool("MyBool", true);
            yield return null;
            Assert.IsTrue(Info0(animator).IsName("Second"));
        }

        [UnityTest]
        public IEnumerator It_should_transition_when_bool_is_false()
        {
            var controller = new AnimatorController();
            var aac = TestAac();
            var fx = aac.CreateMainArbitraryControllerLayer(controller);

            // Exercise
            var first = fx.NewState("First");
            var second = fx.NewState("Second");
            var myBool = fx.BoolParameter("MyBool");
            first.TransitionsTo(second).When(myBool.IsFalse());

            var animator = _root.AddComponent<Animator>();
            animator.runtimeAnimatorController = controller;

            // Verify
            // Frame 0
            Assert.AreEqual(1, animator.layerCount);
            Assert.IsTrue(Info0(animator).IsName("First"));
            
            // Frame 1
            animator.SetBool("MyBool", true);
            yield return null;
            Assert.IsTrue(Info0(animator).IsName("First"));
            
            // Frame 2
            animator.SetBool("MyBool", false);
            yield return null;
            Assert.IsTrue(Info0(animator).IsName("Second"));
        }

        private static AnimatorStateInfo Info0(Animator animator)
        {
            return animator.GetCurrentAnimatorStateInfo(0);
        }
    }
}