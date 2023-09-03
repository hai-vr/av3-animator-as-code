using System;
using AnimatorAsCode.V1;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace av3_animator_as_code.Tests.V1VRC.PlayMode
{
    public abstract class AbstractSimpleSingleLayerAIAC
    {
        protected GameObject _root;
        private AnimatorController _container;
        private string _containerPath;
        private AnimatorController _controller;
        private string _controllerPath;

        [SetUp]
        public void SetUp()
        {
            _root = new GameObject("Root");
            _container = new AnimatorController();
            _containerPath = $"Assets/temp_aac_test__container_{Guid.NewGuid()}.asset";
            AssetDatabase.CreateAsset(_container, _containerPath);
        }
        
        [TearDown]
        public void TearDown()
        {
            Object.Destroy(_root);
            AssetDatabase.DeleteAsset(_containerPath);
            if (_controllerPath != null) AssetDatabase.DeleteAsset(_controllerPath);
            _root = null;
            _container = null;
            _containerPath = null;
            _controller = null;
            _controllerPath = null;
        }

        protected AnimatorController NewPersistentController()
        {
            _controller = new AnimatorController();
            _controllerPath = $"Assets/temp_aac_test__ctrl_{Guid.NewGuid()}.asset";
            AssetDatabase.CreateAsset(_controller, _controllerPath);
            return _controller;
        }

        protected AacFlBase TestAac()
        {
            return AacV0.Create(new AacConfiguration
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