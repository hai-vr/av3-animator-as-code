using System.Linq;
using UnityEditor.Animations;

// ReSharper disable once CheckNamespace
namespace AnimatorAsCode.Framework
{
    public partial class AacAnimatorRemoval
    {
        private readonly AnimatorController _animatorController;

        public AacAnimatorRemoval(AnimatorController animatorController)
        {
            _animatorController = animatorController;
        }

        public void RemoveLayer(string layerName)
        {
            var index = FindIndexOf(layerName);
            if (index == -1)
                return;

            _animatorController.RemoveLayer(index);
        }

        private int FindIndexOf(string layerName)
        {
            return _animatorController.layers.ToList().FindIndex(layer => layer.name == layerName);
        }
    }
}
