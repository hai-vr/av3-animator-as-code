using System.Collections;
using AnimatorAsCode.V0;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.TestTools;

namespace av3_animator_as_code.Tests.PlayMode
{
    public class AnimatorInternalACSubStateMachineTest : AbstractSimpleSingleLayerAnimatorInternalAC
    {
        [UnityTest]
        public IEnumerator It_should_transition_to_a_SSM()
        {
            var aac = TestAac();
            var controller = new AnimatorController();
            var fx = aac.CreateMainArbitraryControllerLayer(controller);

            // Exercise
            var first = fx.NewState("First");
            var ssm = fx.NewSubStateMachine("SSM");
            var one = ssm.NewState("SSM One");
            first.TransitionsTo(ssm).AfterAnimationFinishes();

            var animator = _root.AddComponent<Animator>();
            animator.runtimeAnimatorController = controller;
            animator.enabled = false;

            // Verify
            // Frame 0
            Assert.IsTrue(Info0(animator).IsName("First"));
            animator.Update(1 / 60f);
            
            // Frame 1
            Assert.IsTrue(Info0(animator).IsName("SSM One"));
            yield break;
        }

        private static AnimatorStateInfo Info0(Animator animator)
        {
            return animator.GetCurrentAnimatorStateInfo(0);
        }
    }
}