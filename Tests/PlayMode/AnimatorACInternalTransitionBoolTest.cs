using System.Collections;
using AnimatorAsCode.V0;
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
            animator.Update(1 / 60f);
            Assert.IsTrue(Info0(animator).IsName("First"));
            
            // Frame 2
            animator.SetBool("MyBool", true);
            animator.Update(1 / 60f);
            Assert.IsTrue(Info0(animator).IsName("Second"));
            yield break;
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
            animator.Update(1 / 60f);
            Assert.IsTrue(Info0(animator).IsName("First"));
            
            // Frame 2
            animator.SetBool("MyBool", false);
            animator.Update(1 / 60f);
            Assert.IsTrue(Info0(animator).IsName("Second"));
            yield break;
        }

        [UnityTest]
        public IEnumerator It_should_transition_when_both_bools_are_true()
        {
            var controller = new AnimatorController();
            var aac = TestAac();
            var fx = aac.CreateMainArbitraryControllerLayer(controller);

            // Exercise
            var first = fx.NewState("First");
            var second = fx.NewState("Second");
            var myBoolOne = fx.BoolParameter("MyBoolOne");
            var myBoolTwo = fx.BoolParameter("MyBoolTwo");
            first.TransitionsTo(second).When(myBoolOne.IsTrue()).And(myBoolTwo.IsTrue());

            var animator = _root.AddComponent<Animator>();
            animator.runtimeAnimatorController = controller;

            // Verify
            // Frame 0
            Assert.AreEqual(1, animator.layerCount);
            Assert.IsTrue(Info0(animator).IsName("First"));
            
            // Frame 1
            animator.SetBool("MyBoolOne", true);
            animator.Update(1 / 60f);
            Assert.IsTrue(Info0(animator).IsName("First"));
            
            // Frame 2
            animator.SetBool("MyBoolOne", false);
            animator.SetBool("MyBoolTwo", true);
            animator.Update(1 / 60f);
            Assert.IsTrue(Info0(animator).IsName("First"));
            
            // Frame 3
            animator.SetBool("MyBoolOne", true);
            animator.SetBool("MyBoolTwo", true);
            animator.Update(1 / 60f);
            Assert.IsTrue(Info0(animator).IsName("Second"));
            yield break;
        }

        [UnityTest]
        public IEnumerator It_should_transition_when_second_bool_is_true()
        {
            var controller = new AnimatorController();
            var aac = TestAac();
            var fx = aac.CreateMainArbitraryControllerLayer(controller);

            // Exercise
            var first = fx.NewState("First");
            var second = fx.NewState("Second");
            var myBoolOne = fx.BoolParameter("MyBoolOne");
            var myBoolTwo = fx.BoolParameter("MyBoolTwo");
            first.TransitionsTo(second).When(myBoolOne.IsTrue()).Or().When(myBoolTwo.IsTrue());

            var animator = _root.AddComponent<Animator>();
            animator.runtimeAnimatorController = controller;

            // Verify
            // Frame 0
            Assert.AreEqual(1, animator.layerCount);
            Assert.IsTrue(Info0(animator).IsName("First"));
            
            // Frame 1
            animator.Update(1 / 60f);
            Assert.IsTrue(Info0(animator).IsName("First"));
            
            // Frame 2
            animator.SetBool("MyBoolTwo", true);
            animator.Update(1 / 60f);
            Assert.IsTrue(Info0(animator).IsName("Second"));
            yield break;
        }

        [UnityTest]
        public IEnumerator It_should_transition_when_group_of_bools_are_true()
        {
            var controller = new AnimatorController();
            var aac = TestAac();
            var fx = aac.CreateMainArbitraryControllerLayer(controller);

            // Exercise
            var first = fx.NewState("First");
            var second = fx.NewState("Second");
            var myBools = fx.BoolParameters("MyBoolOne", "MyBoolTwo");
            first.TransitionsTo(second).When(myBools.AreTrue());

            var animator = _root.AddComponent<Animator>();
            animator.runtimeAnimatorController = controller;

            // Verify
            // Frame 0
            Assert.AreEqual(1, animator.layerCount);
            Assert.IsTrue(Info0(animator).IsName("First"));
            
            // Frame 1
            animator.Update(1 / 60f);
            Assert.IsTrue(Info0(animator).IsName("First"));
            
            // Frame 2
            animator.SetBool("MyBoolOne", true);
            animator.SetBool("MyBoolTwo", true);
            animator.Update(1 / 60f);
            Assert.IsTrue(Info0(animator).IsName("Second"));
            yield break;
        }

        [UnityTest]
        public IEnumerator It_should_transition_when_group_of_bools_are_false()
        {
            var controller = new AnimatorController();
            var aac = TestAac();
            var fx = aac.CreateMainArbitraryControllerLayer(controller);

            // Exercise
            var first = fx.NewState("First");
            var second = fx.NewState("Second");
            var myBools = fx.BoolParameters("MyBoolOne", "MyBoolTwo");
            first.TransitionsTo(second).When(myBools.AreFalse());

            var animator = _root.AddComponent<Animator>();
            animator.runtimeAnimatorController = controller;

            // Verify
            // Frame 0
            Assert.AreEqual(1, animator.layerCount);
            Assert.IsTrue(Info0(animator).IsName("First"));
            
            // Frame 1
            animator.SetBool("MyBoolOne", true);
            animator.SetBool("MyBoolTwo", true);
            animator.Update(1 / 60f);
            Assert.IsTrue(Info0(animator).IsName("First"));
            
            // Frame 2
            animator.SetBool("MyBoolOne", false);
            animator.SetBool("MyBoolTwo", false);
            animator.Update(1 / 60f);
            Assert.IsTrue(Info0(animator).IsName("Second"));
            yield break;
        }

        [UnityTest]
        public IEnumerator It_should_transition_when_group_of_bools_are_true_except_one_of_them()
        {
            var controller = new AnimatorController();
            var aac = TestAac();
            var fx = aac.CreateMainArbitraryControllerLayer(controller);

            // Exercise
            var first = fx.NewState("First");
            var second = fx.NewState("Second");
            var myBools = fx.BoolParameters("MyBoolOne", "MyBoolTwo", "MyBoolThree");
            first.TransitionsTo(second).When(myBools.AreTrueExcept(fx.BoolParameter("MyBoolThree")));

            var animator = _root.AddComponent<Animator>();
            animator.runtimeAnimatorController = controller;

            // Verify
            // Frame 0
            Assert.AreEqual(1, animator.layerCount);
            Assert.IsTrue(Info0(animator).IsName("First"));
            
            // Frame 1
            animator.SetBool("MyBoolOne", true);
            animator.SetBool("MyBoolTwo", true);
            animator.SetBool("MyBoolThree", true);
            animator.Update(1 / 60f);
            Assert.IsTrue(Info0(animator).IsName("First"));
            
            // Frame 2
            animator.SetBool("MyBoolOne", true);
            animator.SetBool("MyBoolTwo", true);
            animator.SetBool("MyBoolThree", false);
            animator.Update(1 / 60f);
            Assert.IsTrue(Info0(animator).IsName("Second"));
            yield break;
        }

        [UnityTest]
        public IEnumerator It_should_transition_when_group_of_bools_are_true_except_another_group_of_them()
        {
            var controller = new AnimatorController();
            var aac = TestAac();
            var fx = aac.CreateMainArbitraryControllerLayer(controller);

            // Exercise
            var first = fx.NewState("First");
            var second = fx.NewState("Second");
            var myBools = fx.BoolParameters("MyBoolOne", "MyBoolTwo", "MyBoolThree", "MyBoolFour");
            first.TransitionsTo(second).When(myBools.AreTrueExcept(fx.BoolParameters("MyBoolThree", "MyBoolFour")));

            var animator = _root.AddComponent<Animator>();
            animator.runtimeAnimatorController = controller;

            // Verify
            // Frame 0
            Assert.AreEqual(1, animator.layerCount);
            Assert.IsTrue(Info0(animator).IsName("First"));
            
            // Frame 1
            animator.SetBool("MyBoolOne", true);
            animator.SetBool("MyBoolTwo", true);
            animator.SetBool("MyBoolThree", true);
            animator.SetBool("MyBoolFour", false);
            animator.Update(1 / 60f);
            Assert.IsTrue(Info0(animator).IsName("First"));
            
            // Frame 2
            animator.SetBool("MyBoolOne", true);
            animator.SetBool("MyBoolTwo", true);
            animator.SetBool("MyBoolThree", false);
            animator.SetBool("MyBoolFour", false);
            animator.Update(1 / 60f);
            Assert.IsTrue(Info0(animator).IsName("Second"));
            yield break;
        }

        [UnityTest]
        public IEnumerator It_should_transition_when_any_of_group_of_bools_is_true()
        {
            var controller = new AnimatorController();
            var aac = TestAac();
            var fx = aac.CreateMainArbitraryControllerLayer(controller);

            // Exercise
            var first = fx.NewState("First");
            var second = fx.NewState("Second");
            var myBools = fx.BoolParameters("MyBoolOne", "MyBoolTwo");
            first.TransitionsTo(second).When(myBools.IsAnyTrue());

            var animator = _root.AddComponent<Animator>();
            animator.runtimeAnimatorController = controller;

            // Verify
            // Frame 0
            Assert.AreEqual(1, animator.layerCount);
            Assert.IsTrue(Info0(animator).IsName("First"));
            
            // Frame 1
            animator.Update(1 / 60f);
            Assert.IsTrue(Info0(animator).IsName("First"));
            
            // Frame 2
            animator.SetBool("MyBoolTwo", true);
            animator.Update(1 / 60f);
            Assert.IsTrue(Info0(animator).IsName("Second"));
            yield break;
        }

        [UnityTest]
        public IEnumerator It_should_transition_when_any_of_group_of_bools_is_false()
        {
            var controller = new AnimatorController();
            var aac = TestAac();
            var fx = aac.CreateMainArbitraryControllerLayer(controller);

            // Exercise
            var first = fx.NewState("First");
            var second = fx.NewState("Second");
            var myBools = fx.BoolParameters("MyBoolOne", "MyBoolTwo");
            first.TransitionsTo(second).When(myBools.IsAnyFalse());

            var animator = _root.AddComponent<Animator>();
            animator.runtimeAnimatorController = controller;

            // Verify
            // Frame 0
            Assert.AreEqual(1, animator.layerCount);
            Assert.IsTrue(Info0(animator).IsName("First"));
            
            // Frame 1
            animator.SetBool("MyBoolOne", true);
            animator.SetBool("MyBoolTwo", true);
            animator.Update(1 / 60f);
            Assert.IsTrue(Info0(animator).IsName("First"));
            
            // Frame 2
            animator.SetBool("MyBoolTwo", false);
            animator.Update(1 / 60f);
            Assert.IsTrue(Info0(animator).IsName("Second"));
            yield break;
        }

        [UnityTest]
        public IEnumerator It_should_transition_when_any_of_group_of_bools_is_true_and_another_bool_is_also_true()
        {
            var controller = new AnimatorController();
            var aac = TestAac();
            var fx = aac.CreateMainArbitraryControllerLayer(controller);

            // Exercise
            var first = fx.NewState("First");
            var second = fx.NewState("Second");
            var myBools = fx.BoolParameters("MyBoolOne", "MyBoolTwo");
            var myMandatoryBool = fx.BoolParameter("MyBoolMandatory");
            first.TransitionsTo(second).When(myBools.IsAnyTrue()).And(myMandatoryBool.IsTrue());

            var animator = _root.AddComponent<Animator>();
            animator.runtimeAnimatorController = controller;

            // Verify
            // Frame 0
            Assert.AreEqual(1, animator.layerCount);
            Assert.IsTrue(Info0(animator).IsName("First"));
            
            // Frame 1
            animator.Update(1 / 60f);
            Assert.IsTrue(Info0(animator).IsName("First"));
            
            // Frame 2
            animator.SetBool("MyBoolOne", true);
            animator.SetBool("MyBoolTwo", false);
            animator.Update(1 / 60f);
            Assert.IsTrue(Info0(animator).IsName("First"));
            
            // Frame 3
            animator.SetBool("MyBoolOne", false);
            animator.SetBool("MyBoolTwo", true);
            animator.Update(1 / 60f);
            Assert.IsTrue(Info0(animator).IsName("First"));
            
            // Frame 4
            animator.SetBool("MyBoolOne", false);
            animator.SetBool("MyBoolTwo", false);
            animator.SetBool("MyBoolMandatory", true);
            animator.Update(1 / 60f);
            Assert.IsTrue(Info0(animator).IsName("First"));
            
            // Frame 5
            animator.SetBool("MyBoolOne", false);
            animator.SetBool("MyBoolTwo", true);
            animator.SetBool("MyBoolMandatory", true);
            animator.Update(1 / 60f);
            Assert.IsTrue(Info0(animator).IsName("Second"));
            yield break;
        }

        private static AnimatorStateInfo Info0(Animator animator)
        {
            return animator.GetCurrentAnimatorStateInfo(0);
        }
    }
}