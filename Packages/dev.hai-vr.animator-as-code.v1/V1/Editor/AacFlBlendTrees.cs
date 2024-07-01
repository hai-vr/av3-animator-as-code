﻿using System;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace AnimatorAsCode.V1
{
    public class AacFlBlendTree
    {
        protected AacFlBlendTree(BlendTree blendTree)
        {
            BlendTree = blendTree;
        }

        /// Exposes the underlying Unity BlendTree asset.
        public BlendTree BlendTree { get; }
    }

    public class AacFlNonInitializedBlendTree : AacFlBlendTree
    {
        public AacFlNonInitializedBlendTree(BlendTree blendTree) : base(blendTree)
        {
        }

        /// Define this BlendTree as being FreeformCartesian2D.
        public AacFlBlendTree2D FreeformCartesian2D(AacFlFloatParameter parameterX, AacFlFloatParameter parameterY)
        {
            return New2DBlendTree(parameterX, parameterY, BlendTreeType.FreeformCartesian2D);
        }

        /// Define this BlendTree as being FreeformDirectional2D.
        public AacFlBlendTree2D FreeformDirectional2D(AacFlFloatParameter parameterX, AacFlFloatParameter parameterY)
        {
            return New2DBlendTree(parameterX, parameterY, BlendTreeType.FreeformDirectional2D);
        }

        /// Define this BlendTree as being SimpleDirectional2D.
        public AacFlBlendTree2D SimpleDirectional2D(AacFlFloatParameter parameterX, AacFlFloatParameter parameterY)
        {
            return New2DBlendTree(parameterX, parameterY, BlendTreeType.SimpleDirectional2D);
        }

        /// Define this BlendTree as being Simple1D.
        public AacFlBlendTree1D Simple1D(AacFlFloatParameter parameter)
        {
            BlendTree.blendType = BlendTreeType.Simple1D;
            BlendTree.blendParameter = parameter.Name;
            BlendTree.useAutomaticThresholds = false;
            
            return new AacFlBlendTree1D(BlendTree);
        }

        /// Define this BlendTree as being Direct.
        public AacFlBlendTreeDirect Direct()
        {
            BlendTree.blendType = BlendTreeType.Direct;
            
            return new AacFlBlendTreeDirect(BlendTree);
        }

        private AacFlBlendTree2D New2DBlendTree(AacFlFloatParameter parameterX, AacFlFloatParameter parameterY, BlendTreeType blendTreeType)
        {
            BlendTree.blendType = blendTreeType;
            BlendTree.blendParameter = parameterX.Name;
            BlendTree.blendParameterY = parameterY.Name;

            return new AacFlBlendTree2D(BlendTree);
        }
    }

    public class AacFlBlendTree2D : AacFlBlendTree
    {
        public AacFlBlendTree2D(BlendTree blendTree) : base(blendTree)
        {
        }

        /// Add a BlendTree in the specified coordinates. By default, the timeScale is 1, cycle offset is 0, mirror is false.
        public AacFlBlendTree2D WithAnimation(AacFlBlendTree blendTree, Vector2 pos)
        {
            return WithAnimationInternal(blendTree.BlendTree, pos.x, pos.y, null);
        }

        /// Add a BlendTree in the specified `x` and `y` coordinates. By default, the timeScale is 1, cycle offset is 0, mirror is false.
        public AacFlBlendTree2D WithAnimation(AacFlBlendTree blendTree, float x, float y)
        {
            return WithAnimationInternal(blendTree.BlendTree, x, y, null);
        }

        /// Add a Clip in the specified coordinates. By default, the timeScale is 1, cycle offset is 0, mirror is false.
        public AacFlBlendTree2D WithAnimation(AacFlClip clip, Vector2 pos)
        {
            return WithAnimationInternal(clip.Clip, pos.x, pos.y, null);
        }

        /// Add a Clip in the specified `x` and `y` coordinates. By default, the timeScale is 1, cycle offset is 0, mirror is false.
        public AacFlBlendTree2D WithAnimation(AacFlClip clip, float x, float y)
        {
            return WithAnimationInternal(clip.Clip, x, y, null);
        }

        /// Add a raw motion in the specified coordinates. By default, the timeScale is 1, cycle offset is 0, mirror is false.
        public AacFlBlendTree2D WithAnimation(Motion motion, Vector2 pos)
        {
            return WithAnimationInternal(motion, pos.x, pos.y, null);
        }

        /// Add a raw motion in the specified `x` and `y` coordinates. By default, the timeScale is 1, cycle offset is 0, mirror is false.
        public AacFlBlendTree2D WithAnimation(Motion motion, float x, float y)
        {
            return WithAnimationInternal(motion, x, y, null);
        }

        /// Add a BlendTree in the specified coordinates. By default, the timeScale is 1, cycle offset is 0, mirror is false.
        public AacFlBlendTree2D WithAnimation(AacFlBlendTree blendTree, Vector2 pos, Action<AacFlBlendTreeChildMotion> furtherDefiningChild)
        {
            // Disallow null here: as nulls are allowed in the internal function,
            // we want to stop early if this specific non-null overload is invoked with a null param.
            if (furtherDefiningChild == null) throw new NullReferenceException();

            return WithAnimationInternal(blendTree.BlendTree, pos.x, pos.y, furtherDefiningChild);
        }

        /// Add a BlendTree in the specified `x` and `y` coordinates. By default, the timeScale is 1, cycle offset is 0, mirror is false.
        public AacFlBlendTree2D WithAnimation(AacFlBlendTree blendTree, float x, float y, Action<AacFlBlendTreeChildMotion> furtherDefiningChild)
        {
            // Disallow null here: as nulls are allowed in the internal function,
            // we want to stop early if this specific non-null overload is invoked with a null param.
            if (furtherDefiningChild == null) throw new NullReferenceException();

            return WithAnimationInternal(blendTree.BlendTree, x, y, furtherDefiningChild);
        }

        /// Add a Clip in the specified coordinates. By default, the timeScale is 1, cycle offset is 0, mirror is false.
        public AacFlBlendTree2D WithAnimation(AacFlClip clip, Vector2 pos, Action<AacFlBlendTreeChildMotion> furtherDefiningChild)
        {
            // Disallow null here: as nulls are allowed in the internal function,
            // we want to stop early if this specific non-null overload is invoked with a null param.
            if (furtherDefiningChild == null) throw new NullReferenceException();

            return WithAnimationInternal(clip.Clip, pos.x, pos.y, furtherDefiningChild);
        }

        /// Add a Clip in the specified `x` and `y` coordinates. By default, the timeScale is 1, cycle offset is 0, mirror is false.
        public AacFlBlendTree2D WithAnimation(AacFlClip clip, float x, float y, Action<AacFlBlendTreeChildMotion> furtherDefiningChild)
        {
            // Disallow null here: as nulls are allowed in the internal function,
            // we want to stop early if this specific non-null overload is invoked with a null param.
            if (furtherDefiningChild == null) throw new NullReferenceException();

            return WithAnimationInternal(clip.Clip, x, y, furtherDefiningChild);
        }

        /// Add a raw motion in the specified coordinates. By default, the timeScale is 1, cycle offset is 0, mirror is false.
        public AacFlBlendTree2D WithAnimation(Motion motion, Vector2 pos, Action<AacFlBlendTreeChildMotion> furtherDefiningChild)
        {
            // Disallow null here: as nulls are allowed in the internal function,
            // we want to stop early if this specific non-null overload is invoked with a null param.
            if (furtherDefiningChild == null) throw new NullReferenceException();

            return WithAnimationInternal(motion, pos.x, pos.y, furtherDefiningChild);
        }

        /// Add a raw motion in the specified `x` and `y` coordinates. By default, the timeScale is 1, cycle offset is 0, mirror is false.
        public AacFlBlendTree2D WithAnimation(Motion motion, float x, float y, Action<AacFlBlendTreeChildMotion> furtherDefiningChild)
        {
            // Disallow null here: as nulls are allowed in the internal function,
            // we want to stop early if this specific non-null overload is invoked with a null param.
            if (furtherDefiningChild == null) throw new NullReferenceException();

            return WithAnimationInternal(motion, x, y, furtherDefiningChild);
        }

        private AacFlBlendTree2D WithAnimationInternal(Motion motion, float x, float y, Action<AacFlBlendTreeChildMotion> furtherDefiningChildNullable)
        {
            var children = BlendTree.children ?? new ChildMotion[0];
            var childrenList = children.ToList();


            var childMotionModifier = new AacFlBlendTreeChildMotion();
            furtherDefiningChildNullable?.Invoke(childMotionModifier);
            var newChildMotion = new ChildMotion
            {
                motion = motion,
                position = new Vector2(x, y),
                timeScale = childMotionModifier.TimeScale,
                mirror = childMotionModifier.Mirror,
                cycleOffset = childMotionModifier.CycleOffset
            };

            childrenList.Add(newChildMotion);
            BlendTree.children = childrenList.ToArray();

            return this;
        }
    }

    public class AacFlBlendTree1D : AacFlBlendTree
    {
        public AacFlBlendTree1D(BlendTree blendTree) : base(blendTree)
        {
        }
        
        /// Add a BlendTree in the specified threshold. By default, the timeScale is 1, cycle offset is 0, mirror is false.
        public AacFlBlendTree1D WithAnimation(AacFlBlendTree blendTree, float threshold)
        {
            return WithAnimationInternal(blendTree.BlendTree, threshold, null);
        }

        /// Add a Clip in the specified threshold. By default, the timeScale is 1, cycle offset is 0, mirror is false.
        public AacFlBlendTree1D WithAnimation(AacFlClip clip, float threshold)
        {
            return WithAnimationInternal(clip.Clip, threshold, null);
        }

        /// Add a raw motion in the specified threshold. By default, the timeScale is 1, cycle offset is 0, mirror is false.
        public AacFlBlendTree1D WithAnimation(Motion motion, float threshold)
        {
            return WithAnimationInternal(motion, threshold, null);
        }

        /// Add a BlendTree in the specified threshold, and further define that motion. By default, the timeScale is 1, cycle offset is 0, mirror is false.
        public AacFlBlendTree1D WithAnimation(AacFlBlendTree blendTree, float threshold, Action<AacFlBlendTreeChildMotion> furtherDefiningChild)
        {
            // Disallow null here: as nulls are allowed in the internal function,
            // we want to stop early if this specific non-null overload is invoked with a null param.
            if (furtherDefiningChild == null) throw new NullReferenceException();
            
            return WithAnimationInternal(blendTree.BlendTree, threshold, furtherDefiningChild);
        }

        /// Add a Clip in the specified threshold, and further define that motion. By default, the timeScale is 1, cycle offset is 0, mirror is false.
        public AacFlBlendTree1D WithAnimation(AacFlClip clip, float threshold, Action<AacFlBlendTreeChildMotion> furtherDefiningChild)
        {
            // Disallow null here: as nulls are allowed in the internal function,
            // we want to stop early if this specific non-null overload is invoked with a null param.
            if (furtherDefiningChild == null) throw new NullReferenceException();
            
            return WithAnimationInternal(clip.Clip, threshold, furtherDefiningChild);
        }

        /// Add a raw motion in the specified threshold, and further define that motion. By default, the timeScale is 1, cycle offset is 0, mirror is false.
        public AacFlBlendTree1D WithAnimation(Motion motion, float threshold, Action<AacFlBlendTreeChildMotion> furtherDefiningChild)
        {
            // Disallow null here: as nulls are allowed in the internal function,
            // we want to stop early if this specific non-null overload is invoked with a null param.
            if (furtherDefiningChild == null) throw new NullReferenceException();
            
            return WithAnimationInternal(motion, threshold, furtherDefiningChild);
        }

        private AacFlBlendTree1D WithAnimationInternal(Motion motion, float threshold, Action<AacFlBlendTreeChildMotion> furtherDefiningChildNullable)
        {
            var children = BlendTree.children ?? new ChildMotion[0];
            var childrenList = children.ToList();

            var childMotionModifier = new AacFlBlendTreeChildMotion();
            furtherDefiningChildNullable?.Invoke(childMotionModifier);
            childrenList.Add(new ChildMotion
            {
                motion = motion,
                threshold = threshold,
                timeScale = childMotionModifier.TimeScale,
                mirror = childMotionModifier.Mirror,
                cycleOffset = childMotionModifier.CycleOffset
            });
            BlendTree.children = childrenList
                // 1D blend trees are capricious; if the thresholds are not in the correct order, the blend tree will:
                // - misbehave at runtime as it might only blend in the 0th item no matter what
                // - it will display in the animator UI in a confusing manner
                // Sort the 1D blend tree to avoid this.
                .OrderBy(childMotion => childMotion.threshold)
                .ToArray();

            return this;
        }
    }

    public class AacFlBlendTreeDirect : AacFlBlendTree
    {
        public AacFlBlendTreeDirect(BlendTree blendTree) : base(blendTree)
        {
        }

        /// Add a BlendTree driven by the specified parameter. By default, the timeScale is 1, cycle offset is 0, mirror is false.
        public AacFlBlendTreeDirect WithAnimation(AacFlBlendTree blendTree, AacFlFloatParameter parameter)
        {
            return WithAnimationInternal(blendTree.BlendTree, parameter, null);
        }

        /// Add a Clip driven by the specified parameter. By default, the timeScale is 1, cycle offset is 0, mirror is false.
        public AacFlBlendTreeDirect WithAnimation(AacFlClip clip, AacFlFloatParameter parameter)
        {
            return WithAnimationInternal(clip.Clip, parameter, null);
        }

        /// Add a raw motion driven by the specified parameter. By default, the timeScale is 1, cycle offset is 0, mirror is false.
        public AacFlBlendTreeDirect WithAnimation(Motion motion, AacFlFloatParameter parameter)
        {
            return WithAnimationInternal(motion, parameter, null);
        }

        /// Add a BlendTree driven by the specified parameter, and further define that motion. By default, the timeScale is 1, cycle offset is 0, mirror is false.
        public AacFlBlendTreeDirect WithAnimation(AacFlBlendTree blendTree, AacFlFloatParameter parameter, Action<AacFlBlendTreeChildMotion> furtherDefiningChild)
        {
            // Disallow null here: as nulls are allowed in the internal function,
            // we want to stop early if this specific non-null overload is invoked with a null param.
            if (furtherDefiningChild == null) throw new NullReferenceException();

            return WithAnimationInternal(blendTree.BlendTree, parameter, furtherDefiningChild);
        }

        /// Add a Clip driven by the specified parameter, and further define that motion. By default, the timeScale is 1, cycle offset is 0, mirror is false.
        public AacFlBlendTreeDirect WithAnimation(AacFlClip clip, AacFlFloatParameter parameter, Action<AacFlBlendTreeChildMotion> furtherDefiningChild)
        {
            // Disallow null here: as nulls are allowed in the internal function,
            // we want to stop early if this specific non-null overload is invoked with a null param.
            if (furtherDefiningChild == null) throw new NullReferenceException();

            return WithAnimationInternal(clip.Clip, parameter, furtherDefiningChild);
        }

        /// Add a raw motion driven by the specified parameter, and further define that motion. By default, the timeScale is 1, cycle offset is 0, mirror is false.
        public AacFlBlendTreeDirect WithAnimation(Motion motion, AacFlFloatParameter parameter, Action<AacFlBlendTreeChildMotion> furtherDefiningChild)
        {
            // Disallow null here: as nulls are allowed in the internal function,
            // we want to stop early if this specific non-null overload is invoked with a null param.
            if (furtherDefiningChild == null) throw new NullReferenceException();

            return WithAnimationInternal(motion, parameter, furtherDefiningChild);
        }

        private AacFlBlendTreeDirect WithAnimationInternal(Motion motion, AacFlFloatParameter parameter, Action<AacFlBlendTreeChildMotion> furtherDefiningChildNullable)
        {
            var children = BlendTree.children ?? new ChildMotion[0];
            var childrenList = children.ToList();

            var childMotionModifier = new AacFlBlendTreeChildMotion();
            furtherDefiningChildNullable?.Invoke(childMotionModifier);
            childrenList.Add(new ChildMotion
            {
                motion = motion,
                directBlendParameter = parameter.Name,
                timeScale = childMotionModifier.TimeScale,
                mirror = childMotionModifier.Mirror,
                cycleOffset = childMotionModifier.CycleOffset
            });
            BlendTree.children = childrenList.ToArray();

            return this;
        }
    }

    public class AacFlBlendTreeChildMotion
    {
        internal float TimeScale { get; set; } = 1f;
        internal bool Mirror { get; set; }
        internal float CycleOffset { get; set; }
        
        ///  Set the time scale. The time scale value is 1 by default.
        public AacFlBlendTreeChildMotion WithTimeScaleSetTo(float timeScale)
        {
            TimeScale = timeScale;
            return this;
        }

        /// Set the mirror option. The mirror option value is false by default.
        public AacFlBlendTreeChildMotion WithMirrorSetTo(bool mirror)
        {
            Mirror = mirror;
            return this;
        }

        /// Set the cycle offset. The cycle offset value is 0 by default.
        public AacFlBlendTreeChildMotion WithCycleOffsetSetTo(float cycleOffset)
        {
            CycleOffset = cycleOffset;
            return this;
        }
    }
}