using System;
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

        public BlendTree BlendTree { get; }
    }

    public class AacFlNonInitializedBlendTree : AacFlBlendTree
    {
        public AacFlNonInitializedBlendTree(BlendTree blendTree) : base(blendTree)
        {
        }

        // Define this BlendTree as being FreeformCartesian2D.
        public AacFlBlendTree2D FreeformCartesian2D(AacFlFloatParameter parameterX, AacFlFloatParameter parameterY)
        {
            return New2DBlendTree(parameterX, parameterY, BlendTreeType.FreeformCartesian2D);
        }

        // Define this BlendTree as being FreeformDirectional2D.
        public AacFlBlendTree2D FreeformDirectional2D(AacFlFloatParameter parameterX, AacFlFloatParameter parameterY)
        {
            return New2DBlendTree(parameterX, parameterY, BlendTreeType.FreeformDirectional2D);
        }

        // Define this BlendTree as being SimpleDirectional2D.
        public AacFlBlendTree2D SimpleDirectional2D(AacFlFloatParameter parameterX, AacFlFloatParameter parameterY)
        {
            return New2DBlendTree(parameterX, parameterY, BlendTreeType.SimpleDirectional2D);
        }

        // Define this BlendTree as being Simple1D.
        public AacFlBlendTree1D Simple1D(AacFlFloatParameter parameter)
        {
            BlendTree.blendType = BlendTreeType.Simple1D;
            BlendTree.blendParameter = parameter.Name;
            BlendTree.useAutomaticThresholds = false;
            
            return new AacFlBlendTree1D(BlendTree);
        }

        // Define this BlendTree as being Direct.
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

        // Add a BlendTree in the specified coordinates.
        public AacFlBlendTree2D WithAnimation(AacFlBlendTree blendTree, Vector2 pos, Action<AacFlBlendTreeChildMotion> furtherDefiningChild = null)
        {
            return WithAnimation(blendTree.BlendTree, pos, furtherDefiningChild);
        }

        // Add a BlendTree in the specified `x` and `y` coordinates.
        public AacFlBlendTree2D WithAnimation(AacFlBlendTree blendTree, float x, float y, Action<AacFlBlendTreeChildMotion> furtherDefiningChild = null)
        {
            return WithAnimation(blendTree.BlendTree, x, y, furtherDefiningChild);
        }

        // Add a Clip in the specified coordinates.
        public AacFlBlendTree2D WithAnimation(AacFlClip clip, Vector2 pos, Action<AacFlBlendTreeChildMotion> furtherDefiningChild = null)
        {
            return WithAnimation(clip.Clip, pos, furtherDefiningChild);
        }

        // Add a Clip in the specified `x` and `y` coordinates.
        public AacFlBlendTree2D WithAnimation(AacFlClip clip, float x, float y, Action<AacFlBlendTreeChildMotion> furtherDefiningChild = null)
        {
            return WithAnimation(clip.Clip, x, y, furtherDefiningChild);
        }

        // Add a raw motion in the specified coordinates.
        public AacFlBlendTree2D WithAnimation(Motion motion, Vector2 pos, Action<AacFlBlendTreeChildMotion> furtherDefiningChild = null)
        {
            return WithAnimation(motion, pos.x, pos.y, furtherDefiningChild);
        }

        // Add a raw motion in the specified `x` and `y` coordinates.
        public AacFlBlendTree2D WithAnimation(Motion motion, float x, float y, Action<AacFlBlendTreeChildMotion> furtherDefiningChild = null)
        {
            var children = BlendTree.children ?? new ChildMotion[0];
            var childrenList = children.ToList();
            
            
            var childMotionModifier = new AacFlBlendTreeChildMotion();
            furtherDefiningChild?.Invoke(childMotionModifier);
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

        // Add a BlendTree in the specified threshold.
        public AacFlBlendTree1D WithAnimation(AacFlBlendTree blendTree, float threshold, Action<AacFlBlendTreeChildMotion> furtherDefiningChild = null)
        {
            return WithAnimation(blendTree.BlendTree, threshold, furtherDefiningChild);
        }

        // Add a Clip in the specified threshold.
        public AacFlBlendTree1D WithAnimation(AacFlClip clip, float threshold, Action<AacFlBlendTreeChildMotion> furtherDefiningChild = null)
        {
            return WithAnimation(clip.Clip, threshold, furtherDefiningChild);
        }

        // Add a raw motion in the specified threshold.
        public AacFlBlendTree1D WithAnimation(Motion motion, float threshold, Action<AacFlBlendTreeChildMotion> furtherDefiningChild = null)
        {
            var children = BlendTree.children ?? new ChildMotion[0];
            var childrenList = children.ToList();
            
            var childMotionModifier = new AacFlBlendTreeChildMotion();
            furtherDefiningChild?.Invoke(childMotionModifier);
            childrenList.Add(new ChildMotion
            {
                motion = motion,
                threshold = threshold,
                timeScale = childMotionModifier.TimeScale,
                mirror = childMotionModifier.Mirror,
                cycleOffset = childMotionModifier.CycleOffset
            });
            BlendTree.children = childrenList.ToArray();

            return this;
        }
    }

    public class AacFlBlendTreeDirect : AacFlBlendTree
    {
        public AacFlBlendTreeDirect(BlendTree blendTree) : base(blendTree)
        {
        }

        // Add a BlendTree driven by the specified parameter.
        public AacFlBlendTreeDirect WithAnimation(AacFlBlendTree blendTree, AacFlFloatParameter parameter, Action<AacFlBlendTreeChildMotion> furtherDefiningChild = null)
        {
            return WithAnimation(blendTree.BlendTree, parameter, furtherDefiningChild);
        }

        // Add a Clip driven by the specified parameter.
        public AacFlBlendTreeDirect WithAnimation(AacFlClip clip, AacFlFloatParameter parameter, Action<AacFlBlendTreeChildMotion> furtherDefiningChild = null)
        {
            return WithAnimation(clip.Clip, parameter, furtherDefiningChild);
        }

        // Add a raw motion driven by the specified parameter.
        public AacFlBlendTreeDirect WithAnimation(Motion motion, AacFlFloatParameter parameter, Action<AacFlBlendTreeChildMotion> furtherDefiningChild = null)
        {
            var children = BlendTree.children ?? new ChildMotion[0];
            var childrenList = children.ToList();
            
            var childMotionModifier = new AacFlBlendTreeChildMotion();
            furtherDefiningChild?.Invoke(childMotionModifier);
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
        
        public AacFlBlendTreeChildMotion WithTimeScaleSetTo(float timeScale)
        {
            TimeScale = timeScale;
            return this;
        }

        public AacFlBlendTreeChildMotion WithMirrorSetTo(bool mirror)
        {
            Mirror = mirror;
            return this;
        }

        public AacFlBlendTreeChildMotion WithCycleOffsetSetTo(float cycleOffset)
        {
            CycleOffset = cycleOffset;
            return this;
        }
    }
}