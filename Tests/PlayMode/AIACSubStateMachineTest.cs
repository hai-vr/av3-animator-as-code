using NUnit.Framework;
using UnityEditor.Animations;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;

namespace av3_animator_as_code.Tests.PlayMode
{
    public class AIACSubStateMachineTest : AbstractSimpleSingleLayerAIAC
    {
        [Test]
        public void It_should_transition_to_a_SSM()
        {
            var aac = TestAac();
            var controller = new AnimatorController();
            var fx = aac.CreateMainArbitraryControllerLayer(controller);

            // Exercise
            var first = fx.NewState("First");
            var ssm = fx.NewSubStateMachine("SSM");
            ssm.NewState("SSM One");
            
            first.TransitionsTo(ssm).AfterAnimationFinishes();

            var animator = _root.AddComponent<Animator>();
            animator.runtimeAnimatorController = controller;
            animator.enabled = false;

            // Verify
            // Frame 0
            Assert.IsTrue(Info0(animator).IsName("First"));
            
            // Frame 1
            animator.Update(1 / 60f);
            Assert.IsTrue(Info0(animator).IsName("SSM One"));
        }
        
        [Test]
        public void It_should_exit_out_of_a_SSM()
        {
            var aac = TestAac();
            var controller = new AnimatorController();
            var fx = aac.CreateMainArbitraryControllerLayer(controller);

            // Exercise
            var first = fx.NewState("First");
            var second = fx.NewState("Second");
            var ssm = fx.NewSubStateMachine("SSM");
            var one = ssm.NewState("SSM One");
            
            first.TransitionsTo(ssm).AfterAnimationFinishes();
            one.Exits().AfterAnimationFinishes();
            ssm.TransitionsTo(second);

            var animator = _root.AddComponent<Animator>();
            animator.runtimeAnimatorController = controller;
            animator.enabled = false;

            // Verify
            // Frame 0
            Assert.IsTrue(Info0(animator).IsName("First"));
            
            // Frame 1
            animator.Update(1 / 60f);
            Assert.IsTrue(Info0(animator).IsName("SSM One"));
            
            // Frame 2
            animator.Update(1 / 60f);
            Assert.IsTrue(Info0(animator).IsName("Second"));
        }
        
        [Test]
        public void It_should_restart_the_SSM()
        {
            var aac = TestAac();
            var controller = new AnimatorController();
            var fx = aac.CreateMainArbitraryControllerLayer(controller);

            // Exercise
            var first = fx.NewState("First");
            var ssm = fx.NewSubStateMachine("SSM");
            var one = ssm.NewState("SSM One");
            var two = ssm.NewState("SSM Two");
            
            first.TransitionsTo(ssm).AfterAnimationFinishes();
            one.TransitionsTo(two).AfterAnimationFinishes();
            two.Exits().AfterAnimationFinishes();
            ssm.Restarts();

            var animator = _root.AddComponent<Animator>();
            animator.runtimeAnimatorController = controller;
            animator.enabled = false;

            // Verify
            // Frame 0
            Assert.IsTrue(Info0(animator).IsName("First"));
            
            // Frame 1
            animator.Update(1 / 60f);
            Assert.IsTrue(Info0(animator).IsName("SSM One"));
            
            // Frame 2
            animator.Update(1 / 60f);
            Assert.IsTrue(Info0(animator).IsName("SSM Two"));
            
            // Frame 3
            animator.Update(1 / 60f);
            Assert.IsTrue(Info0(animator).IsName("SSM One"));
        }
        
        [Test]
        public void It_should_enter_a_SSM_conditionally__Entry_created_through_SSM_instance()
        {
            var aac = TestAac();
            var controller = new AnimatorController();
            var fx = aac.CreateMainArbitraryControllerLayer(controller);

            // Exercise
            var first = fx.NewState("First");
            var ssm = fx.NewSubStateMachine("SSM");
            ssm.NewState("SSM One");

            var two = ssm.NewState("SSM Two");
            first.TransitionsTo(ssm).AfterAnimationFinishes();
            ssm.EntryTransitionsTo(two).When(fx.BoolParameter("MyBool").IsTrue());

            var animator = _root.AddComponent<Animator>();
            animator.runtimeAnimatorController = controller;
            animator.enabled = false;

            // Verify
            // Frame 0
            Assert.IsTrue(Info0(animator).IsName("First"));
            
            // Frame 1
            animator.SetBool("MyBool", true);
            animator.Update(1 / 60f);
            Assert.IsTrue(Info0(animator).IsName("SSM Two"));
        }
        
        [Test]
        public void It_should_enter_a_SSM_conditionally__Entry_created_through_state_instance()
        {
            var aac = TestAac();
            var controller = new AnimatorController();
            var fx = aac.CreateMainArbitraryControllerLayer(controller);

            // Exercise
            var first = fx.NewState("First");
            var ssm = fx.NewSubStateMachine("SSM");
            ssm.NewState("SSM One");

            var two = ssm.NewState("SSM Two");
            first.TransitionsTo(ssm).AfterAnimationFinishes();
            two.TransitionsFromEntry().When(fx.BoolParameter("MyBool").IsTrue()); 

            var animator = _root.AddComponent<Animator>();
            animator.runtimeAnimatorController = controller;
            animator.enabled = false;

            // Verify
            // Frame 0
            Assert.IsTrue(Info0(animator).IsName("First"));
            
            // Frame 1
            animator.SetBool("MyBool", true);
            animator.Update(1 / 60f);
            Assert.IsTrue(Info0(animator).IsName("SSM Two"));
        }
        
        [Test]
        public void It_should_enter_the_default_SSM_entry_state_when_conditions_do_not_pass()
        {
            var aac = TestAac();
            var controller = new AnimatorController();
            var fx = aac.CreateMainArbitraryControllerLayer(controller);

            // Exercise
            var first = fx.NewState("First");
            var ssm = fx.NewSubStateMachine("SSM");
            ssm.NewState("SSM One");

            var two = ssm.NewState("SSM Two");
            first.TransitionsTo(ssm).AfterAnimationFinishes();
            ssm.EntryTransitionsTo(two).When(fx.BoolParameter("MyBool").IsTrue());

            var animator = _root.AddComponent<Animator>();
            animator.runtimeAnimatorController = controller;
            animator.enabled = false;

            // Verify
            // Frame 0
            Assert.IsTrue(Info0(animator).IsName("First"));
            
            // Frame 1
            animator.SetBool("MyBool", false);
            animator.Update(1 / 60f);
            Assert.IsTrue(Info0(animator).IsName("SSM One"));
        }

        private static AnimatorStateInfo Info0(Animator animator)
        {
            return animator.GetCurrentAnimatorStateInfo(0);
        }
    }
}