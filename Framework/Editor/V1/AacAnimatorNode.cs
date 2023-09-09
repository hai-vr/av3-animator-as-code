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

        protected AacAnimatorNode(AacFlStateMachine parentMachine, IAacDefaultsProvider defaultsProvider)
        {
            ParentMachine = parentMachine;
            DefaultsProvider = defaultsProvider;
        }

        public TNode LeftOf(AacAnimatorNode otherNode) => MoveNextTo(otherNode, -1, 0);
        public TNode RightOf(AacAnimatorNode otherNode) => MoveNextTo(otherNode, 1, 0);
        public TNode Over(AacAnimatorNode otherNode) => MoveNextTo(otherNode, 0, -1);
        public TNode Under(AacAnimatorNode otherNode) => MoveNextTo(otherNode, 0, 1);

        public TNode LeftOf() => MoveNextTo(null, -1, 0);
        public TNode RightOf() => MoveNextTo(null, 1, 0);
        public TNode Over() => MoveNextTo(null, 0, -1);
        public TNode Under() => MoveNextTo(null, 0, 1);

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

        public TNode Shift(Vector3 otherPosition, int shiftX, int shiftY)
        {
            SetPosition(otherPosition + new Vector3(shiftX * DefaultsProvider.Grid().x, shiftY * DefaultsProvider.Grid().y, 0));
            return (TNode) this;
        }

        public abstract TBehaviour EnsureBehaviour<TBehaviour>() where TBehaviour : StateMachineBehaviour;
    }
}
