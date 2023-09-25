﻿using System;
using System.Reflection;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

// ReSharper disable once CheckNamespace
namespace AnimatorAsCode.V1
{
    internal static class AacInternals
    {
        internal static AnimatorController NewAnimatorController(AacConfiguration component, string suffix)
        {
            var animatorController = new AnimatorController();
            animatorController.name = "zAutogenerated__" + component.AssetKey + "__" + suffix + "_" + Random.Range(0, Int32.MaxValue); // FIXME animation name conflict
            animatorController.hideFlags = HideFlags.None;
            if (component.AssetContainer != null) AssetDatabase.AddObjectToAsset(animatorController, component.AssetContainer);
            return animatorController;
        }

        internal static AnimationClip NewClip(AacConfiguration component, string suffix)
        {
            return RegisterClip(component, suffix, new AnimationClip());
        }
        
        internal static AnimationClip RegisterClip(AacConfiguration component, string suffix, AnimationClip clip)
        {
            clip.name = "zAutogenerated__" + component.AssetKey + "__" + suffix + "_" + Random.Range(0, Int32.MaxValue); // FIXME animation name conflict
            clip.hideFlags = HideFlags.None;
            if (component.AssetContainer != null) AssetDatabase.AddObjectToAsset(clip, component.AssetContainer);
            return clip;
        }

        internal static BlendTree NewBlendTreeAsRaw(AacConfiguration component, string suffix)
        {
            var clip = new BlendTree();
            clip.name = "zAutogenerated__" + component.AssetKey + "__" + suffix + "_" + Random.Range(0, Int32.MaxValue); // FIXME animation name conflict
            clip.hideFlags = HideFlags.None;
            if (component.AssetContainer != null) AssetDatabase.AddObjectToAsset(clip, component.AssetContainer);
            return clip;
        }

        internal static T DuplicateAssetIntoContainer<T>(AacConfiguration component, T assetToDuplicate) where T : Object
        {
            var duplicated = (T)Object.Instantiate(assetToDuplicate);
            duplicated.name = "zAutogenerated__" + component.AssetKey + "__" + assetToDuplicate.name + "_" + Random.Range(0, Int32.MaxValue); // FIXME animation name conflict
            duplicated.hideFlags = HideFlags.None;
            if (component.AssetContainer != null) AssetDatabase.AddObjectToAsset(duplicated, component.AssetContainer);
            return duplicated;
        }

        internal static EditorCurveBinding Binding(AacConfiguration component, Type type, Transform transform, string propertyName)
        {
            return new EditorCurveBinding
            {
                path = ResolveRelativePath(component.AnimatorRoot, transform),
                type = type,
                propertyName = propertyName
            };
        }

        internal static AnimationCurve OneFrame(float desiredValue)
        {
            return AnimationCurve.Constant(0f, 1 / 60f, desiredValue);
        }

        internal static AnimationCurve ConstantSeconds(float seconds, float desiredValue)
        {
            return AnimationCurve.Constant(0f, seconds, desiredValue);
        }

        internal static string ResolveRelativePath(Transform avatar, Transform item)
        {
            if (avatar == item)
            {
                // TODO: Is this correct??
                return "";
            }
            
            if (item.parent != avatar && item.parent != null)
            {
                return ResolveRelativePath(avatar, item.parent) + "/" + item.name;
            }

            return item.name;
        }

        internal static EditorCurveBinding ToSubBinding(EditorCurveBinding binding, string suffix)
        {
            return new EditorCurveBinding {path = binding.path, type = binding.type, propertyName = binding.propertyName + "." + suffix};
        }

        internal static void NoUndo<T>(T obj, Action action)
        {
            try
            {
                UndoDisable(obj);
                action.Invoke();
            }
            finally
            {
                UndoEnable(obj);
            }
        }

        internal static TResult NoUndo<T, TResult>(T obj, Func<TResult> action)
        {
            try
            {
                UndoDisable(obj);
                return action.Invoke();
            }
            finally
            {
                UndoEnable(obj);
            }
        }

        private static void UndoDisable<T>(T state)
        {
            typeof(T)
                .GetProperty("pushUndo", BindingFlags.Instance | BindingFlags.NonPublic)
                .SetValue(state, false);
        }

        private static void UndoEnable<T>(T state)
        {
            typeof(T)
                .GetProperty("pushUndo", BindingFlags.Instance | BindingFlags.NonPublic)
                .SetValue(state, true);
        }
    }
}