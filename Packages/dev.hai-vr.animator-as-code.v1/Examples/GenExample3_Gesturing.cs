/*
#if UNITY_EDITOR
using System.Linq;
using AnimatorAsCode.V1;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

namespace AnimatorAsCodeFramework.Examples
{
    public class GenExample3_Gesturing : MonoBehaviour
    {
        public GameObject avatar;
        public AnimatorController assetContainer;
        public string assetKey;
        public SkinnedMeshRenderer iconMesh;
    }

    [CustomEditor(typeof(GenExample3_Gesturing), true)]
    public class GenExample3_GesturingEditor : Editor
    {
        private const string SystemName = "Example 3";

        private GenExample3_Gesturing my;
        private AacFlBase aac;
        private AacFlController ctrl;

        public override void OnInspectorGUI()
        {
            AacExample.InspectorTemplate(this, serializedObject, "assetKey", Create);
        }

        private void Create()
        {
            my = (GenExample3_Gesturing) target;
            aac = AacExample.AnimatorAsCode(SystemName, my.avatar, my.assetContainer, my.assetKey);

            ctrl = aac.NewAnimatorController();
            CreateMainLayer();
            CreateSupportingLayer();
        }

        private void CreateMainLayer()
        {
            var layer = ctrl.NewLayer();

            var dirtyCheckParameter = layer.BoolParameter("AAC_INTERNAL_GesturingIcon_DirtyCheck");

            // ### Create states
            var lackOfChangeDetected = layer.NewState("Animate To NoChange")
                .WithAnimation(IconAppears());

            // By default, states have an animation that animates a dummy object for 1 frame.
            var noChange = layer.NewState("NoChange", 1, 0).RightOf();

            var changeDetected = layer.NewState("Animate To Changing").Under()
                .WithAnimation(IconDisappears());

            // This creates a clip that animates a dummy object for 1.5f seconds.
            var changing = layer.NewState("Changing", 0, 1).LeftOf()
                .WithAnimation(aac.DummyClipLasting(1.5f, AacFlUnit.Seconds));

            // When this state is entered, the parameter is driven to the value of false.
            var stillChanging = layer.NewState("Still Changing", 0, 2).Under()
                .Drives(dirtyCheckParameter, false);

            // ------

            // ### Create transitions
            lackOfChangeDetected.TransitionsTo(changeDetected).AfterAnimationIsAtLeastAtPercent(0.7f).When(dirtyCheckParameter.IsTrue());

            // The transition duration is 30% of the animation duration.
            lackOfChangeDetected.TransitionsTo(noChange).AfterAnimationFinishes().WithTransitionDurationPercent(0.3f);

            noChange.TransitionsTo(changeDetected).When(dirtyCheckParameter.IsTrue());

            // By using AfterAnimationFinishes, the transition will trigger after the animation
            // for the icon appearing finishes.
            changeDetected.TransitionsTo(changing).AfterAnimationFinishes();

            changing.TransitionsTo(stillChanging).When(dirtyCheckParameter.IsTrue());
            // By using AfterAnimationFinishes, the transition will trigger after 1.5 seconds,
            // which is the length of the animation in Changing.
            changing.TransitionsTo(lackOfChangeDetected).AfterAnimationFinishes();

            // The transition will immediately happen upon entering, by using Exit time set to 0.
            stillChanging.AutomaticallyMovesTo(changing);
        }

        private void CreateSupportingLayer()
        {
            // Create an additional FX layer.
            var layer = ctrl.NewLayer("Detection");
            var reevaluating = layer.NewState("Reevaluating", -1, 0);

            foreach (var left in Enumerable.Range(0, 8))
            {
                foreach (var right in Enumerable.Range(0, 8))
                {
                    var state = layer.NewState($"Gesture {left} {right}", left, right)
                        // When this state is entered, the parameter is driven to the value of true.
                        .Drives(layer.BoolParameter("AAC_INTERNAL_GesturingIcon_DirtyCheck"), true);

                    reevaluating.TransitionsTo(state)
                        // Use ".Av3" to access VRChat standard parameters.
                        // Accessing these parameters will create the corresponding parameter in the animator.
                        .When(layer.Av3().GestureLeft.IsEqualTo(left))
                        .And(layer.Av3().GestureRight.IsEqualTo(right));
                    state.TransitionsTo(reevaluating)
                        .When(layer.Av3().GestureLeft.IsNotEqualTo(left))
                        .Or()
                        .When(layer.Av3().GestureRight.IsNotEqualTo(right));
                }
            }
        }

        private AacFlClip IconAppears()
        {
            return aac.NewClip().Animating(clip =>
            {
                clip.Animates(my.iconMesh, "blendShape.Wedge")
                    .WithFrameCountUnit(keyframes => keyframes.Easing(0, 0f).Easing(10, 100f));
            });
        }

        private AacFlClip IconDisappears()
        {
            return aac.NewClip().Animating(clip =>
            {
                clip.Animates(my.iconMesh, "blendShape.Wedge")
                    .WithFrameCountUnit(keyframes => keyframes.Easing(0f, 100f).Easing(10, 0f));
            });
        }
    }
}
#endif
*/