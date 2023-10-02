using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

// ReSharper disable once CheckNamespace
namespace AnimatorAsCode.V1
{
    public class AacFlClip
    {
        private readonly AacConfiguration _component;
        public AnimationClip Clip { get; }

        public AacFlClip(AacConfiguration component, AnimationClip clip)
        {
            _component = component;
            Clip = clip;
        }

        public AacFlClip Looping()
        {
            var settings = AnimationUtility.GetAnimationClipSettings(Clip);
            settings.loopTime = true;
            AnimationUtility.SetAnimationClipSettings(Clip, settings);

            return this;
        }

        public AacFlClip NonLooping()
        {
            var settings = AnimationUtility.GetAnimationClipSettings(Clip);
            settings.loopTime = false;
            AnimationUtility.SetAnimationClipSettings(Clip, settings);

            return this;
        }

        public AacFlClip Animating(Action<AacFlEditClip> action)
        {
            action.Invoke(new AacFlEditClip(_component, Clip));
            return this;
        }

        public AacFlClip Toggling(GameObject[] gameObjectsWithNulls, bool value)
        {
            var defensiveObjects = gameObjectsWithNulls.Where(o => o != null); // Allow users to remove an item in the middle of the array
            foreach (var component in defensiveObjects)
            {
                var binding = AacInternals.Binding(_component, typeof(GameObject), component.transform, "m_IsActive");

                AacInternals.SetCurve(Clip, binding, AacInternals.OneFrame(value ? 1f : 0f));
            }

            return this;
        }

        public AacFlClip BlendShape(SkinnedMeshRenderer renderer, string blendShapeName, float value)
        {
            var binding = AacInternals.Binding(_component, typeof(SkinnedMeshRenderer), renderer.transform, $"blendShape.{blendShapeName}");

            AacInternals.SetCurve(Clip, binding, AacInternals.OneFrame(value));

            return this;
        }

        public AacFlClip BlendShape(SkinnedMeshRenderer[] rendererWithNulls, string blendShapeName, float value)
        {
            var defensiveObjects = rendererWithNulls.Where(o => o != null); // Allow users to remove an item in the middle of the array
            foreach (var component in defensiveObjects)
            {
                var binding = AacInternals.Binding(_component, typeof(SkinnedMeshRenderer), component.transform, $"blendShape.{blendShapeName}");

                AacInternals.SetCurve(Clip, binding, AacInternals.OneFrame(value));
            }

            return this;
        }

        public AacFlClip BlendShape(SkinnedMeshRenderer renderer, string blendShapeName, AnimationCurve animationCurve)
        {
            var binding = AacInternals.Binding(_component, typeof(SkinnedMeshRenderer), renderer.transform, $"blendShape.{blendShapeName}");

            AacInternals.SetCurve(Clip, binding, animationCurve);

            return this;
        }

        public AacFlClip Positioning(GameObject[] gameObjectsWithNulls, Vector3 localPosition)
        {
            var defensiveObjects = gameObjectsWithNulls.Where(o => o != null); // Allow users to remove an item in the middle of the array
            foreach (var component in defensiveObjects)
            {
                AacInternals.SetCurve(Clip, AacInternals.Binding(_component, typeof(Transform), component.transform, "m_LocalPosition.x"), AacInternals.OneFrame(localPosition.x));
                AacInternals.SetCurve(Clip, AacInternals.Binding(_component, typeof(Transform), component.transform, "m_LocalPosition.y"), AacInternals.OneFrame(localPosition.y));
                AacInternals.SetCurve(Clip, AacInternals.Binding(_component, typeof(Transform), component.transform, "m_LocalPosition.z"), AacInternals.OneFrame(localPosition.z));
            }

            return this;
        }

        public AacFlClip RotatingUsingEulerInterpolation(GameObject[] gameObjectsWithNulls, Vector3 localEulerAngles)
        {
            var defensiveObjects = gameObjectsWithNulls.Where(o => o != null); // Allow users to remove an item in the middle of the array
            foreach (var component in defensiveObjects)
            {
                // See https://forum.unity.com/threads/new-animationclip-property-names.367288/#post-2384172
                // AacInternals.SetCurve internally uses AnimationClip.SetCurve instead of AnimationUtility.SetEditorCurve, starting from V1
                AacInternals.SetCurve(Clip, AacInternals.Binding(_component, typeof(Transform), component.transform, "m_LocalEuler.x"), AacInternals.OneFrame(localEulerAngles.x));
                AacInternals.SetCurve(Clip, AacInternals.Binding(_component, typeof(Transform), component.transform, "m_LocalEuler.y"), AacInternals.OneFrame(localEulerAngles.y));
                AacInternals.SetCurve(Clip, AacInternals.Binding(_component, typeof(Transform), component.transform, "m_LocalEuler.z"), AacInternals.OneFrame(localEulerAngles.z));
            }

            return this;
        }

        public AacFlClip RotatingUsingQuaternionInterpolation(GameObject[] gameObjectsWithNulls, Quaternion localQuaternionAngles)
        {
            var defensiveObjects = gameObjectsWithNulls.Where(o => o != null); // Allow users to remove an item in the middle of the array
            foreach (var component in defensiveObjects)
            {
                // See https://forum.unity.com/threads/new-animationclip-property-names.367288/#post-2384172
                // AacInternals.SetCurve internally uses AnimationClip.SetCurve instead of AnimationUtility.SetEditorCurve, starting from V1
                AacInternals.SetCurve(Clip, AacInternals.Binding(_component, typeof(Transform), component.transform, "m_LocalRotation.x"), AacInternals.OneFrame(localQuaternionAngles.x));
                AacInternals.SetCurve(Clip, AacInternals.Binding(_component, typeof(Transform), component.transform, "m_LocalRotation.y"), AacInternals.OneFrame(localQuaternionAngles.y));
                AacInternals.SetCurve(Clip, AacInternals.Binding(_component, typeof(Transform), component.transform, "m_LocalRotation.z"), AacInternals.OneFrame(localQuaternionAngles.z));
                AacInternals.SetCurve(Clip, AacInternals.Binding(_component, typeof(Transform), component.transform, "m_LocalRotation.w"), AacInternals.OneFrame(localQuaternionAngles.w));
            }

            return this;
        }

        public AacFlClip Scaling(GameObject[] gameObjectsWithNulls, Vector3 scale)
        {
            var defensiveObjects = gameObjectsWithNulls.Where(o => o != null); // Allow users to remove an item in the middle of the array
            foreach (var component in defensiveObjects)
            {
                AacInternals.SetCurve(Clip, AacInternals.Binding(_component, typeof(Transform), component.transform, "m_LocalScale.x"), AacInternals.OneFrame(scale.x));
                AacInternals.SetCurve(Clip, AacInternals.Binding(_component, typeof(Transform), component.transform, "m_LocalScale.y"), AacInternals.OneFrame(scale.y));
                AacInternals.SetCurve(Clip, AacInternals.Binding(_component, typeof(Transform), component.transform, "m_LocalScale.z"), AacInternals.OneFrame(scale.z));
            }

            return this;
        }

        public AacFlClip Toggling(GameObject gameObject, bool value)
        {
            var binding = AacInternals.Binding(_component, typeof(GameObject), gameObject.transform, "m_IsActive");

            AacInternals.SetCurve(Clip, binding, AacInternals.OneFrame(value ? 1f : 0f));

            return this;
        }

        public AacFlClip TogglingComponent(Component[] componentsWithNulls, bool value)
        {
            var defensiveComponents = componentsWithNulls.Where(o => o != null); // Allow users to remove an item in the middle of the array
            foreach (var component in defensiveComponents)
            {
                var binding = AacInternals.Binding(_component, component.GetType(), component.transform, "m_Enabled");

                AacInternals.SetCurve(Clip, binding, AacInternals.OneFrame(value ? 1f : 0f));
            }

            return this;
        }

        public AacFlClip TogglingComponent(Component component, bool value)
        {
            var binding = AacInternals.Binding(_component, component.GetType(), component.transform, "m_Enabled");

            AacInternals.SetCurve(Clip, binding, AacInternals.OneFrame(value ? 1f : 0f));

            return this;
        }

        public AacFlClip SwappingMaterial(Renderer renderer, int slot, Material material)
        {
            var binding = AacInternals.Binding(_component, renderer.GetType(), renderer.transform, $"m_Materials.Array.data[{slot}]");

            AnimationUtility.SetObjectReferenceCurve(Clip, binding, new[] {
                new ObjectReferenceKeyframe { time = 0f, value = material },
                new ObjectReferenceKeyframe { time = 1/60f, value = material }
            });

            return this;
        }

        public AacFlClip SwappingMaterial(ParticleSystem particleSystem, int slot, Material material)
        {
            var binding = AacInternals.Binding(_component, typeof(ParticleSystemRenderer), particleSystem.transform, $"m_Materials.Array.data[{slot}]");

            AnimationUtility.SetObjectReferenceCurve(Clip, binding, new[] {
                new ObjectReferenceKeyframe { time = 0f, value = material },
                new ObjectReferenceKeyframe { time = 1/60f, value = material }
            });

            return this;
        }
    }

    public class AacFlEditClip
    {
        private readonly AacConfiguration _component;
        public AnimationClip Clip { get; }

        public AacFlEditClip(AacConfiguration component, AnimationClip clip)
        {
            _component = component;
            Clip = clip;
        }

        public AacFlSettingCurve Animates(string path, Type type, string propertyName)
        {
            var binding = new EditorCurveBinding
            {
                path = path,
                type = type,
                propertyName = propertyName
            };
            return new AacFlSettingCurve(Clip, new[] {binding});
        }

        public AacFlSettingCurve Animates(Transform transform, Type type, string propertyName)
        {
            var binding = AacInternals.Binding(_component, type, transform, propertyName);

            return new AacFlSettingCurve(Clip, new[] {binding});
        }

        public AacFlSettingCurve Animates(GameObject gameObject)
        {
            var binding = AacInternals.Binding(_component, typeof(GameObject), gameObject.transform, "m_IsActive");

            return new AacFlSettingCurve(Clip, new[] {binding});
        }

        public AacFlSettingCurve Animates(Component anyComponent, string property)
        {
            var binding = Internal_BindingFromComponent(anyComponent, property);

            return new AacFlSettingCurve(Clip, new[] {binding});
        }

        public AacFlSettingCurve Animates(Component[] anyComponents, string property)
        {
            var that = this;
            var bindings = anyComponents
                .Select(anyComponent => that.Internal_BindingFromComponent(anyComponent, property))
                .ToArray();

            return new AacFlSettingCurve(Clip, bindings);
        }

        public AacFlSettingCurve AnimatesAnimator(AacFlParameter floatParameter)
        {
            var binding = new EditorCurveBinding
            {
                path = "",
                type = typeof(Animator),
                propertyName = floatParameter.Name
            };
            return new AacFlSettingCurve(Clip, new[] {binding});
        }

        public AacFlSettingCurveColor AnimatesColor(Component anyComponent, string property)
        {
            var binding = Internal_BindingFromComponent(anyComponent, property);
            return new AacFlSettingCurveColor(Clip, new[] {binding});
        }

        public AacFlSettingCurveColor AnimatesColor(Component[] anyComponents, string property)
        {
            var that = this;
            var bindings = anyComponents
                .Select(anyComponent => that.Internal_BindingFromComponent(anyComponent, property))
                .ToArray();

            return new AacFlSettingCurveColor(Clip, bindings);
        }

        public AacFlSettingCurveColor AnimatesHDRColor(Component anyComponent, string property)
        {
            var binding = Internal_BindingFromComponent(anyComponent, property);
            return new AacFlSettingCurveColor(Clip, new[] {binding}, true);
        }

        public AacFlSettingCurveObjectReference AnimatesObjectReference(Component anyComponent, string property)
        {
            var binding = Internal_BindingFromComponent(anyComponent, property);
            return new AacFlSettingCurveObjectReference(Clip, new[] {binding});
        }

        public EditorCurveBinding BindingFromComponent(Component anyComponent, string propertyName)
        {
            return Internal_BindingFromComponent(anyComponent, propertyName);
        }

        private EditorCurveBinding Internal_BindingFromComponent(Component anyComponent, string propertyName)
        {
            return AacInternals.Binding(_component, anyComponent.GetType(), anyComponent.transform, propertyName);
        }
    }

    public class AacFlSettingCurve
    {
        private readonly AnimationClip _clip;
        private readonly EditorCurveBinding[] _bindings;

        public AacFlSettingCurve(AnimationClip clip, EditorCurveBinding[] bindings)
        {
            _clip = clip;
            _bindings = bindings;
        }

        public void WithOneFrame(float desiredValue)
        {
            foreach (var binding in _bindings)
            {
                AacInternals.SetCurve(_clip, binding, AacInternals.OneFrame(desiredValue));
            }
        }

        public void WithFixedSeconds(float seconds, float desiredValue)
        {
            foreach (var binding in _bindings)
            {
                AacInternals.SetCurve(_clip, binding, AacInternals.ConstantSeconds(seconds, desiredValue));
            }
        }

        public void WithSecondsUnit(Action<AacFlSettingKeyframes> action)
        {
            InternalWithUnit(AacFlUnit.Seconds, action);
        }

        public void WithFrameCountUnit(Action<AacFlSettingKeyframes> action)
        {
            InternalWithUnit(AacFlUnit.Frames, action);
        }

        public void WithUnit(AacFlUnit unit, Action<AacFlSettingKeyframes> action)
        {
            InternalWithUnit(unit, action);
        }

        private void InternalWithUnit(AacFlUnit unit, Action<AacFlSettingKeyframes> action)
        {
            var mutatedKeyframes = new List<Keyframe>();
            var builder = new AacFlSettingKeyframes(unit, mutatedKeyframes);
            action.Invoke(builder);

            foreach (var binding in _bindings)
            {
                AacInternals.SetCurve(_clip, binding, new AnimationCurve(mutatedKeyframes.ToArray()));
            }
        }

        public void WithAnimationCurve(AnimationCurve animationCurve)
        {
            foreach (var binding in _bindings)
            {
                AacInternals.SetCurve(_clip, binding, animationCurve);
            }
        }
    }

    public class AacFlSettingCurveObjectReference
    {
        private readonly AnimationClip _clip;
        private readonly EditorCurveBinding[] _bindings;

        public AacFlSettingCurveObjectReference(AnimationClip clip, EditorCurveBinding[] bindings)
        {
            _clip = clip;
            _bindings = bindings;
        }

        public void WithOneFrame(Object objectReference)
        {
            foreach (var binding in _bindings)
            {
                AnimationUtility.SetObjectReferenceCurve(_clip, binding, new[]
                {
                    new ObjectReferenceKeyframe { time = 0f, value = objectReference },
                    new ObjectReferenceKeyframe { time = 1/60f, value = objectReference }
                });
            }
        }
    }

    public class AacFlSettingCurveColor
    {
        private readonly AnimationClip _clip;
        private readonly EditorCurveBinding[] _bindings;
        private readonly bool _hdr;

        public AacFlSettingCurveColor(AnimationClip clip, EditorCurveBinding[] bindings, bool hdr = false)
        {
            _clip = clip;
            _bindings = bindings;
            _hdr = hdr;
        }

        public void WithOneFrame(Color desiredValue)
        {
            foreach (var binding in _bindings)
            {
                AacInternals.SetCurve(_clip, AacInternals.ToSubBinding(binding, _hdr ? "x" : "r"), AacInternals.OneFrame(desiredValue.r));
                AacInternals.SetCurve(_clip, AacInternals.ToSubBinding(binding, _hdr ? "y" : "g"), AacInternals.OneFrame(desiredValue.g));
                AacInternals.SetCurve(_clip, AacInternals.ToSubBinding(binding, _hdr ? "z" : "b"), AacInternals.OneFrame(desiredValue.b));
                AacInternals.SetCurve(_clip, AacInternals.ToSubBinding(binding, _hdr ? "w" : "a"), AacInternals.OneFrame(desiredValue.a));
            }
        }

        public void WithKeyframes(AacFlUnit unit, Action<AacFlSettingKeyframesColor> action)
        {
            var mutatedKeyframesR = new List<Keyframe>();
            var mutatedKeyframesG = new List<Keyframe>();
            var mutatedKeyframesB = new List<Keyframe>();
            var mutatedKeyframesA = new List<Keyframe>();
            var builder = new AacFlSettingKeyframesColor(unit, mutatedKeyframesR, mutatedKeyframesG, mutatedKeyframesB, mutatedKeyframesA);
            action.Invoke(builder);

            foreach (var binding in _bindings)
            {
                AacInternals.SetCurve(_clip, AacInternals.ToSubBinding(binding, _hdr ? "x" : "r"), new AnimationCurve(mutatedKeyframesR.ToArray()));
                AacInternals.SetCurve(_clip, AacInternals.ToSubBinding(binding, _hdr ? "y" : "g"), new AnimationCurve(mutatedKeyframesG.ToArray()));
                AacInternals.SetCurve(_clip, AacInternals.ToSubBinding(binding, _hdr ? "z" : "b"), new AnimationCurve(mutatedKeyframesB.ToArray()));
                AacInternals.SetCurve(_clip, AacInternals.ToSubBinding(binding, _hdr ? "w" : "a"), new AnimationCurve(mutatedKeyframesA.ToArray()));
            }
        }
    }

    public class AacFlSettingKeyframes
    {
        private readonly AacFlUnit _unit;
        private readonly List<Keyframe> _mutatedKeyframes;

        public AacFlSettingKeyframes(AacFlUnit unit, List<Keyframe> mutatedKeyframes)
        {
            _unit = unit;
            _mutatedKeyframes = mutatedKeyframes;
        }

        public AacFlSettingKeyframes Easing(float timeInUnit, float value)
        {
            _mutatedKeyframes.Add(new Keyframe(AsSeconds(timeInUnit), value, 0, 0));

            return this;
        }

        public AacFlSettingKeyframes Constant(float timeInUnit, float value)
        {
            _mutatedKeyframes.Add(new Keyframe(AsSeconds(timeInUnit), value, 0, float.PositiveInfinity));

            return this;
        }

        public AacFlSettingKeyframes Linear(float timeInUnit, float value)
        {
            float valueEnd = value;
            float valueStart = _mutatedKeyframes.Count == 0 ? value : _mutatedKeyframes.Last().value;
            float timeEnd = AsSeconds(timeInUnit);
            float timeStart = _mutatedKeyframes.Count == 0 ? value : _mutatedKeyframes.Last().time;
            float num = (float) (((double) valueEnd - (double) valueStart) / ((double) timeEnd - (double) timeStart));
            // FIXME: This can cause NaN tangents which messes everything

            // return new AnimationCurve(new Keyframe[2]
            // {
                // new Keyframe(timeStart, valueStart, 0.0f, num),
                // new Keyframe(timeEnd, valueEnd, num, 0.0f)
            // });

            if (_mutatedKeyframes.Count > 0)
            {
                var lastKeyframe = _mutatedKeyframes.Last();
                lastKeyframe.outTangent = num;
                _mutatedKeyframes[_mutatedKeyframes.Count - 1] = lastKeyframe;
            }
            _mutatedKeyframes.Add(new Keyframe(AsSeconds(timeInUnit), value, num, 0.0f));

            return this;
        }

        private float AsSeconds(float timeInUnit)
        {
            switch (_unit)
            {
                case AacFlUnit.Frames:
                    return timeInUnit / 60f;
                case AacFlUnit.Seconds:
                    return timeInUnit;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public class AacFlSettingKeyframesColor
    {
        private AacFlSettingKeyframes _r;
        private AacFlSettingKeyframes _g;
        private AacFlSettingKeyframes _b;
        private AacFlSettingKeyframes _a;

        public AacFlSettingKeyframesColor(AacFlUnit unit, List<Keyframe> mutatedKeyframesR, List<Keyframe> mutatedKeyframesG, List<Keyframe> mutatedKeyframesB, List<Keyframe> mutatedKeyframesA)
        {
            _r = new AacFlSettingKeyframes(unit, mutatedKeyframesR);
            _g = new AacFlSettingKeyframes(unit, mutatedKeyframesG);
            _b = new AacFlSettingKeyframes(unit, mutatedKeyframesB);
            _a = new AacFlSettingKeyframes(unit, mutatedKeyframesA);
        }

        public AacFlSettingKeyframesColor Easing(int frame, Color value)
        {
            _r.Easing(frame, value.r);
            _g.Easing(frame, value.g);
            _b.Easing(frame, value.b);
            _a.Easing(frame, value.a);

            return this;
        }

        public AacFlSettingKeyframesColor Linear(float frame, Color value)
        {
            _r.Linear(frame, value.r);
            _g.Linear(frame, value.g);
            _b.Linear(frame, value.b);
            _a.Linear(frame, value.a);

            return this;
        }

        public AacFlSettingKeyframesColor Constant(int frame, Color value)
        {
            _r.Constant(frame, value.r);
            _g.Constant(frame, value.g);
            _b.Constant(frame, value.b);
            _a.Constant(frame, value.a);

            return this;
        }
    }

    public enum AacFlUnit
    {
        Frames, Seconds
    }
}
