#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

namespace AnimatorAsCodeFramework.Examples
{
    public class GenExample0_ToggleGo : MonoBehaviour
    {
        public GameObject avatar;
        public AnimatorController assetContainer;
        public string assetKey;
        public GameObject item;
    }

    [CustomEditor(typeof(GenExample0_ToggleGo), true)]
    public class GenExample0_ToggleGoEditor : Editor
    {
        private const string SystemName = "Example 0";

        public override void OnInspectorGUI()
        {
            AacExample.InspectorTemplate(this, serializedObject, "assetKey", Create);
        }

        private void Create()
        {
            var my = (GenExample0_ToggleGo) target;
            // The avatar is used here:
            // - to find the FX playable layer animator, where a new layer will be created.
            // - to resolve the relative animation path to the item.
            // The generated animation files are stored in the asset container.
            var aac = AacExample.AnimatorAsCode(SystemName, my.avatar, my.assetContainer, my.assetKey, AacExample.Options().WriteDefaultsOff());

            // Create a layer in the FX animator.
            // Additional layers can be created in the FX animator (see later in the manual).
            var ctrl = aac.NewAnimatorController();
            var fx = ctrl.NewLayer();

            // The first created state is the default one connected to the "Entry" node.
            // States are automatically placed on the grid (see later in the manual).
            var hidden = fx.NewState("Hidden")
                // Animation assets are generated as sub-assets of the asset container.
                // The animation path to my.skinnedMesh is relative to my.avatar
                .WithAnimation(aac.NewClip().Toggling(my.item, false));
            var shown = fx.NewState("Shown")
                .WithAnimation(aac.NewClip().Toggling(my.item, true));

            // Creates a Bool parameter in the FX layer.
            // Parameters are added to the Animator if a parameter with the same name
            // does not exist yet.
            var itemParam = fx.BoolParameter("EnableItem");

            // Transitions are created with a set of default values
            // That can be changed in the Generator settings (see later in the manual).
            hidden.TransitionsTo(shown).When(itemParam.IsTrue());
            shown.TransitionsTo(hidden).When(itemParam.IsFalse());
        }
    }
}
#endif