using System;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace AnimatorAsCode.Framework
{
    public partial class AacFlBase
    {
        private readonly AacConfiguration _configuration;

        internal AacFlBase(AacConfiguration configuration)
        {
            _configuration = configuration;
        }

        public AacFlClip NewClip()
        {
            var clip = Aac.NewClip(_configuration, Guid.NewGuid().ToString());
            return new AacFlClip(_configuration, clip);
        }

        public AacFlClip CopyClip(AnimationClip originalClip)
        {
            var newClip = UnityEngine.Object.Instantiate(originalClip);
            var clip = Aac.RegisterClip(_configuration, Guid.NewGuid().ToString(), newClip);
            return new AacFlClip(_configuration, clip);
        }

        public BlendTree NewBlendTreeAsRaw()
        {
            return Aac.NewBlendTreeAsRaw(_configuration, Guid.NewGuid().ToString());
        }

        public AacFlClip NewClip(string name)
        {
            var clip = Aac.NewClip(_configuration, name);
            return new AacFlClip(_configuration, clip);
        }

        public AacFlClip DummyClipLasting(float numberOf, AacFlUnit unit)
        {
            var dummyClip = Aac.NewClip(
                _configuration,
                $"D({numberOf} {Enum.GetName(typeof(AacFlUnit), unit)})"
            );

            var duration = unit == AacFlUnit.Frames ? numberOf / 60f : numberOf;
            return new AacFlClip(_configuration, dummyClip).Animating(
                clip =>
                    clip.Animates("_ignored", typeof(GameObject), "m_IsActive")
                        .WithUnit(
                            unit,
                            keyframes => keyframes.Constant(0, 0f).Constant(duration, 0f)
                        )
            );
        }

        public AacFlLayer CreateMainArbitraryControllerLayer(AnimatorController controller) =>
            DoCreateLayer(
                controller,
                _configuration.DefaultsProvider.ConvertLayerName(_configuration.SystemName)
            );

        public AacFlLayer CreateSupportingArbitraryControllerLayer(
            AnimatorController controller,
            string suffix
        ) =>
            DoCreateLayer(
                controller,
                _configuration.DefaultsProvider.ConvertLayerNameWithSuffix(
                    _configuration.SystemName,
                    suffix
                )
            );

        public AacFlLayer CreateFirstArbitraryControllerLayer(AnimatorController controller) =>
            DoCreateLayer(controller, controller.layers[0].name);

        private AacFlLayer DoCreateLayer(AnimatorController animator, string layerName)
        {
            var ag = new AacAnimatorGenerator(
                animator,
                CreateEmptyClip().Clip,
                _configuration.DefaultsProvider
            );
            var machine = ag.CreateOrClearLayerAtSameIndex(layerName, 1f);

            return new AacFlLayer(animator, _configuration, machine, layerName);
        }

        private AacFlClip CreateEmptyClip()
        {
            var emptyClip = DummyClipLasting(1, AacFlUnit.Frames);
            return emptyClip;
        }

        public AacVrcAssetLibrary VrcAssets()
        {
            return new AacVrcAssetLibrary();
        }

        public void ClearPreviousAssets()
        {
            var allSubAssets = AssetDatabase.LoadAllAssetsAtPath(
                AssetDatabase.GetAssetPath(_configuration.AssetContainer)
            );
            foreach (var subAsset in allSubAssets)
            {
                if (
                    subAsset != _configuration.AssetContainer
                    && (
                        subAsset is AnimationClip || subAsset is BlendTree || subAsset is AvatarMask
                    )
                )
                {
                    //make sure not null
                    if (subAsset != null)
                    {
                        AssetDatabase.RemoveObjectFromAsset(subAsset);
                    }
                }
            }
        }
    }
}
