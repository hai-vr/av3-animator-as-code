#if UNITY_EDITOR
using System;
using AnimatorAsCode.V0;
using UnityEditor;
using UnityEngine;
using VRC.SDK3.Avatars.Components;
using AnimatorController = UnityEditor.Animations.AnimatorController;

namespace AnimatorAsCodeFramework.Examples
{
    public static class AacExample
    {
        public static TemplateGenOptions Options()
        {
            return TemplateGenOptions.Defaults();
        }

        public struct TemplateGenOptions
        {
            public static TemplateGenOptions Defaults()
            {
                return new TemplateGenOptions()
                {
                    WriteDefaults = false
                };
            }

            public TemplateGenOptions WriteDefaultsOff()
            {
                WriteDefaults = false;
                return this;
            }

            public TemplateGenOptions WriteDefaultsOn()
            {
                WriteDefaults = true;
                return this;
            }

            public bool WriteDefaults;
        }

        public static void InspectorTemplate(Editor editor, SerializedObject serializedObj, string propName, Action createFn, Action removeFnOptional = null)
        {
            var prop = serializedObj.FindProperty(propName);
            if (prop.stringValue.Trim() == "")
            {
                prop.stringValue = GUID.Generate().ToString();
                serializedObj.ApplyModifiedProperties();
            }

            editor.DrawDefaultInspector();

            if (GUILayout.Button("Create"))
            {
                createFn.Invoke();
            }
            if (removeFnOptional != null && GUILayout.Button("Remove"))
            {
                removeFnOptional.Invoke();
            }
        }

        /// <summary>
        /// Creates an AAC base with default options (write defaults OFF). This function is provided as an example on how to invoke AAC internals.
        /// </summary>
        /// <param name="systemName">Prefix for layer names</param>
        /// <param name="avatar">Playable layers of this avatar to modify</param>
        /// <param name="assetContainer">Animation assets will be generated as sub-assets of that asset container</param>
        /// <param name="assetKey">Animation assets will be generated with this name in order to clean up previously generated assets of the same system</param>
        /// <param name="options">Some options, such as whether Write Defaults is ON or OFF</param>
        /// <returns>The AAC base.</returns>
        public static AacFlBase AnimatorAsCode(string systemName, VRCAvatarDescriptor avatar, AnimatorController assetContainer, string assetKey)
        {
            return AnimatorAsCode(systemName, avatar, assetContainer, assetKey, TemplateGenOptions.Defaults());
        }

        /// <summary>
        /// Creates an AAC base. This function is provided as an example on how to invoke AAC internals.
        /// </summary>
        /// <param name="systemName">Prefix for layer names</param>
        /// <param name="avatar">Playable layers of this avatar to modify</param>
        /// <param name="assetContainer">Animation assets will be generated as sub-assets of that asset container</param>
        /// <param name="assetKey">Animation assets will be generated with this name in order to clean up previously generated assets of the same system</param>
        /// <param name="options">Some options, such as whether Write Defaults is ON or OFF</param>
        /// <returns>The AAC base.</returns>
        public static AacFlBase AnimatorAsCode(string systemName, VRCAvatarDescriptor avatar, AnimatorController assetContainer, string assetKey, TemplateGenOptions options)
        {
            var aac = AacV0.Create(new AacConfiguration
            {
                SystemName = systemName,
                // In the examples, we consider the avatar to be also the animator root.
                AvatarDescriptor = avatar,
                // You can set the animator root to be different than the avatar descriptor,
                // if you want to apply an animator to a different avatar without redefining
                // all of the game object references which were relative to the original avatar.
                AnimatorRoot = avatar.transform,
                // DefaultValueRoot is currently unused in AAC. It is added here preemptively
                // in order to define an avatar root to sample default values from.
                // The intent is to allow animators to be created with Write Defaults OFF,
                // but mimicking the behaviour of Write Defaults ON by automatically
                // sampling the default value from the scene relative to the transform
                // defined in DefaultValueRoot.
                DefaultValueRoot = avatar.transform,
                AssetContainer = assetContainer,
                AssetKey = assetKey,
                DefaultsProvider = new AacDefaultsProvider(writeDefaults: options.WriteDefaults)
            });
            aac.ClearPreviousAssets();
            return aac;
        }
    }
}
#endif