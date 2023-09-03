#if UNITY_EDITOR
using AnimatorAsCode.V1;
using UnityEngine;
using VRC.SDK3.Avatars.Components;
using UnityEditor;
using UnityEditor.Animations;

namespace AnimatorAsCodeFramework.Examples
{
    public class GenExample4_NonDestructiveWorkflow : MonoBehaviour
    {
        public VRCAvatarDescriptor avatar;
        public AnimatorController assetContainer;
        public string assetKey;
        public SkinnedMeshRenderer iconMesh;
    }

    [CustomEditor(typeof(GenExample4_NonDestructiveWorkflow), true)]
    public class GenExample4_NonDestructiveWorkflowEditor : Editor
    {
        private const string SystemName = "Example 3";

        private GenExample4_NonDestructiveWorkflow my;
        private AacFlBase aac;

        public override void OnInspectorGUI()
        {
            AacExample.InspectorTemplate(this, serializedObject, "assetKey", Create, Remove);
        }

        private void Create()
        {
            my = (GenExample4_NonDestructiveWorkflow) target;
            aac = AacExample.AnimatorAsCode(SystemName, my.avatar, my.assetContainer, my.assetKey);

            CreateMainLayer();
        }

        private void Remove()
        {
            // AacExample.AnimatorAsCode invokes ClearPreviousAssets.
            // This will cause the generated controller to be removed.
            AacExample.AnimatorAsCode(SystemName, my.avatar, my.assetContainer, my.assetKey); 
        }

        private void CreateMainLayer()
        {
            // TODO
            // var fx = aac.NewAnimatorController();
            // var layer = fx.CreateLayer();
            // ...
        }
    }
}
#endif