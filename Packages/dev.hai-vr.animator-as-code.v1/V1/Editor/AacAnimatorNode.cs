using System;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace AnimatorAsCode.V1
{
    public abstract class AacAnimatorNode
    {
        protected internal abstract Vector3 GetPosition();
        protected internal abstract void SetPosition(Vector3 position);
    }

    public abstract class AacAnimatorNode<TNode> : AacAnimatorNode where TNode : AacAnimatorNode<TNode>
    {
        protected readonly AacFlStateMachine ParentMachine;
        protected readonly IAacDefaultsProvider DefaultsProvider;
        protected readonly Transform AnimatorRoot;

        protected AacAnimatorNode(AacFlStateMachine parentMachine, IAacDefaultsProvider defaultsProvider, Transform animatorRoot)
        {
            ParentMachine = parentMachine;
            DefaultsProvider = defaultsProvider;
            AnimatorRoot = animatorRoot;
        }

        /// Move the node the left of the other node in the graph.
        public TNode LeftOf(AacAnimatorNode otherNode) => MoveNextTo(otherNode, -1, 0);
        
        /// Move the node the right of the other node in the graph.
        public TNode RightOf(AacAnimatorNode otherNode) => MoveNextTo(otherNode, 1, 0);
        
        /// Move the node to be over the other node in the graph.
        public TNode Over(AacAnimatorNode otherNode) => MoveNextTo(otherNode, 0, -1);
        
        /// Move the node to be under the other node in the graph.
        public TNode Under(AacAnimatorNode otherNode) => MoveNextTo(otherNode, 0, 1);

        /// Move the node to the left of the last created node of the state machine this belongs to in the graph.
        public TNode LeftOf() => MoveNextTo(null, -1, 0);
        
        /// Move the node to the right of the last created node of the state machine this belongs to in the graph.
        public TNode RightOf() => MoveNextTo(null, 1, 0);
        
        /// Move the node to be over the last created node of the state machine this belongs to in the graph.
        public TNode Over() => MoveNextTo(null, 0, -1);
        
        /// Move the node to be under the last created node of the state machine this belongs to in the graph.
        public TNode Under() => MoveNextTo(null, 0, 1);
        
        /// Move the node to be at a specific position in grid units, where x positive goes right, and y positive goes down.
        public TNode At(int x, int y)
        {
            SetPosition(new Vector3(x * DefaultsProvider.Grid().x, y * DefaultsProvider.Grid().y, 0));
            return (TNode) this;
        }

        /// Move the state to be shifted next to the other state in the graph, in grid units. shiftX positive goes right, shiftY positive goes down.
        public TNode Shift(AacAnimatorNode otherState, int shiftX, int shiftY) => MoveNextTo(otherState, shiftX, shiftY);

        private TNode MoveNextTo(AacAnimatorNode otherStateOrSecondToLastWhenNull, int x, int y)
        {
            if (otherStateOrSecondToLastWhenNull == null)
            {
                var siblings = ParentMachine.GetChildNodes();
                var other = siblings[siblings.Count - 2];
                Shift(other.GetPosition(), x, y);

                return (TNode) this;
            }

            Shift(otherStateOrSecondToLastWhenNull.GetPosition(), x, y);

            return (TNode) this;
        }
        
        // FIXME API: Vector3 is really odd as a type.
        /// Given another position in non-grid units, move the state to be shifted next to that position, in grid units. shiftX positive goes right, shiftY positive goes down.
        public TNode Shift(Vector3 otherPosition, int shiftX, int shiftY)
        {
            SetPosition(otherPosition + new Vector3(shiftX * DefaultsProvider.Grid().x, shiftY * DefaultsProvider.Grid().y, 0));
            return (TNode) this;
        }

        /// Resolve the path of an item relative to the AnimatorRoot.
        public string ResolveRelativePath(Transform item)
        {
            return AacInternals.ResolveRelativePath(AnimatorRoot, item);
        }

        /// Create a behaviour of the given type if it doesn't exist, or returns the first behaviour of that type.
        public abstract TBehaviour EnsureBehaviour<TBehaviour>() where TBehaviour : StateMachineBehaviour;

        /// Create a behaviour of the given type.
        public abstract TBehaviour CreateNewBehaviour<TBehaviour>() where TBehaviour : StateMachineBehaviour;
    }
}
