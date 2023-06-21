#if UNITY_EDITOR
using System.Linq;
using AnimatorAsCode.Framework;
using UnityEngine;
using VRC.SDK3.Avatars.Components;
using UnityEditor;
using UnityEditor.Animations;

namespace AnimatorAsCode.Framework.Examples
{
    public class GenExample4_SubStateMachines : MonoBehaviour
    {
        public VRCAvatarDescriptor avatar;
        public AnimatorController assetContainer;
        public string assetKey;
    }

    [CustomEditor(typeof(GenExample4_SubStateMachines), true)]
    public class GenExample4_SubStateMachinesEditor : Editor
    {
        private const string SystemName = "Example 4";

        private GenExample4_SubStateMachines my;
        private AacFlBase aac;

        public override void OnInspectorGUI()
        {
            AacExample.InspectorTemplate(this, serializedObject, "assetKey", Create, Remove);
        }

        private void Create()
        {
            my = (GenExample4_SubStateMachines)target;
            aac = AacExample.AnimatorAsCode(SystemName, my.avatar, my.assetContainer, my.assetKey);

            CreateMainLayer();
        }

        private void Remove()
        {
            var my = (GenExample4_SubStateMachines)target;
            var aac = AacExample.AnimatorAsCode(
                SystemName,
                my.avatar,
                my.assetContainer,
                my.assetKey
            );

            aac.RemoveAllMainLayers();
            aac.RemoveAllSupportingLayers("Detection");
        }

        private void CreateMainLayer()
        {
            AacFlLayer layer = aac.CreateMainFxLayer();

            AacFlState idle = layer.NewState("Idle");
            AacFlState walk = layer.NewState("Walk");

            AacFlLayer stateGroup = layer.NewStateGroup("StateGroup");

            AacFlState idleSub = stateGroup.NewState("Idle");

            idle.AutomaticallyMovesTo(idleSub);
            idleSub.AutomaticallyMovesTo(walk);
        }
    }
}
#endif
