#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

namespace AnimatorAsCodeFramework.Examples
{
    public class GenExample1_ToggleSmr : MonoBehaviour
    {
        public GameObject avatar;
        public AnimatorController assetContainer;
        public string assetKey;
        public SkinnedMeshRenderer skinnedMesh;
    }

    [CustomEditor(typeof(GenExample1_ToggleSmr), true)]
    public class GenExample1_ToggleSmrEditor : Editor
    {
        private const string SystemName = "Example 1";

        public override void OnInspectorGUI()
        {
            AacExample.InspectorTemplate(this, serializedObject, "assetKey", Create);
        }

        private void Create()
        {
            var my = (GenExample1_ToggleSmr) target;
            var aac = AacExample.AnimatorAsCode(SystemName, my.avatar, my.assetContainer, my.assetKey);

            var ctrl = aac.NewAnimatorController();
            var fx = ctrl.NewLayer();
            var hidden = fx.NewState("Hidden")
                // The runtime type of my.skinnedMesh is used within the animation.
                // In this case, the "SkinnedMeshRenderer" component is disabled.
                .WithAnimation(aac.NewClip().TogglingComponent(my.skinnedMesh, false));
            var shown = fx.NewState("Shown")
                .WithAnimation(aac.NewClip().TogglingComponent(my.skinnedMesh, true));

            // This creates two Bool parameters in the animator.
            // The resulting value can be used in conditions.
            var accessoriesParams = fx.BoolParameters("EnableAccessories", "AccessoryThing");

            // The following line creates one transition.
            // The conditions are "EnableAccessories is true" and "AccessoryThing is true"
            hidden.TransitionsTo(shown).When(accessoriesParams.AreTrue());

            // The following line creates two transitions:
            // - The first transition is "EnableAccessories is false"
            // - The second transition is "AccessoryThing is false"
            shown.TransitionsTo(hidden).When(accessoriesParams.IsAnyFalse());

            if (false)
            {
                // Alternatively, you can use the long way:
                var enableAccessoriesParam = fx.BoolParameter("EnableAccessories");
                var thingParam = fx.BoolParameter("ThingParam");

                hidden.TransitionsTo(shown).When(enableAccessoriesParam.IsTrue()).And(thingParam.IsTrue());

                // - The first transition:
                shown.TransitionsTo(hidden).When(enableAccessoriesParam.IsFalse())
                    // - The second transition:
                    .Or().When(thingParam.IsFalse());
            }
        }
    }
}
#endif