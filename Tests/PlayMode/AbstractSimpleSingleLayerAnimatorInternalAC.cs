using System;
using System.Collections;
using AnimatorAsCode.V0;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace av3_animator_as_code.Tests.PlayMode
{
    public abstract class AbstractSimpleSingleLayerAnimatorInternalAC
    {
        protected GameObject _root;
        private AnimatorController _container;
        private string _containerPath;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            _root = new GameObject("Root");
            _container = new AnimatorController();
            _containerPath = $"Assets/temp_aac_test__{Guid.NewGuid()}.asset";
            AssetDatabase.CreateAsset(_container, _containerPath);
            yield return null;
        }
        
        [UnityTearDown]
        public IEnumerator TearDown()
        {
            Object.Destroy(_root);
            AssetDatabase.DeleteAsset(_containerPath);
            _root = null;
            _container = null;
            _containerPath = null;
            yield return null;
        }

        protected AacFlBase TestAac()
        {
            return AacV0.Create(new AacConfiguration
            {
                AnimatorRoot = _root.transform,
                AssetContainer = _container,
                AssetKey = "None",
                AvatarDescriptor = null,
                DefaultsProvider = new AacDefaultsProvider(),
                SystemName = "Test",
                DefaultValueRoot = _root.transform
            });
        }
    }
}