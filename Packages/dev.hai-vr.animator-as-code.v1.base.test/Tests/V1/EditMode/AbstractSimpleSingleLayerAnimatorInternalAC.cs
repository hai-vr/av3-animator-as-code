using System;
using AnimatorAsCode.V1;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace av3_animator_as_code.Tests.V1.EditMode
{
    public abstract class AbstractSimpleSingleLayerAnimatorInternalEditModeAC
    {
        protected GameObject _root;
        private AnimatorController _container;
        private string _containerPath;

        [SetUp]
        public void SetUp()
        {
            _root = new GameObject("Root");
            _container = new AnimatorController();
            _containerPath = $"Assets/temp_aac_test__{Guid.NewGuid()}.asset";
            AssetDatabase.CreateAsset(_container, _containerPath);
        }
        
        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_root);
            AssetDatabase.DeleteAsset(_containerPath);
            _root = null;
            _container = null;
            _containerPath = null;
        }

        protected AacFlBase TestAac()
        {
            return AacV1.Create(new AacConfiguration
            {
                AnimatorRoot = _root.transform,
                AssetContainer = _container,
                AssetKey = "None",
                DefaultsProvider = new AacDefaultsProvider(),
                SystemName = "Test",
                DefaultValueRoot = _root.transform
            });
        }
    }
}