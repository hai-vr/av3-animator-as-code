using System.Collections;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.TestTools;

namespace av3_animator_as_code.Tests.PlayMode
{
    public class AnimatorInternalACTest : AbstractSimpleSingleLayerAnimatorInternalAC
    {
        [UnityTest]
        public IEnumerator It_should_create_layer_with_state()
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
            Assert.IsTrue(info.IsTag(init.State.tag));
            yield break;
        }
    }
}