using UnityEditor.Animations;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace AnimatorAsCode.Framework
{
    public partial struct AacConfiguration
    {
        public string SystemName;
        public Transform AnimatorRoot;
        public Transform DefaultValueRoot;
        public AnimatorController AssetContainer;
        public string AssetKey;
        public IAacDefaultsProvider DefaultsProvider;
    }
}
