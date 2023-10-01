#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

namespace AnimatorAsCodeFramework.Examples
{
    public class GenExampleManual_PlacingStates : MonoBehaviour
    {
        public GameObject avatar;
        public AnimatorController assetContainer;
        public string assetKey;
    }

    [CustomEditor(typeof(GenExampleManual_PlacingStates), true)]
    public class GenExampleManual_PlacingStatesEditor : Editor
    {
        private const string SystemName = "Example Manual - Placing States";

        public override void OnInspectorGUI()
        {
            AacExample.InspectorTemplate(this, serializedObject, "assetKey", Create);
        }

        private void Create()
        {
            var my = (GenExampleManual_PlacingStates) target;
            var aac = AacExample.AnimatorAsCode(SystemName, my.avatar, my.assetContainer, my.assetKey);

            var ctrl = aac.NewAnimatorController();
            var fx = ctrl.NewLayer();

            var init = fx.NewState("Init"); // This is the first state. By default it is at (0, 0)
            var a = fx.NewState("A"); // This will be placed under Init.
            var b = fx.NewState("B"); // This will be placed under A.
            var c = fx.NewState("C").RightOf(a); // This will be placed right of A.
            var d = fx.NewState("D"); // This will be placed under C.
            var e = fx.NewState("E").RightOf(); // This will be placed right of D.
            var alternate = fx.NewState("Alternate").Over(c); // This will be placed over C.

            // This will be placed relative to Alternate: 2 blocks over, and 1 to the right.
            var reset = fx.NewState("Reset").Shift(alternate, 1, -2);
        }

        private void Remove()
        {
            var my = (GenExampleManual_PlacingStates) target;
            var aac = AacExample.AnimatorAsCode(SystemName, my.avatar, my.assetContainer, my.assetKey);
        }
    }
}
#endif