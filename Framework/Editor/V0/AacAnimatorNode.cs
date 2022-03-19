using UnityEngine;
using UnityEditor.Animations;
using System.Linq;

namespace AnimatorAsCode.V0
{
    public abstract class AacAnimatorNode
    {
        protected internal abstract Vector3 GetPosition();
        protected internal abstract void SetPosition(Vector3 position);
    }
    
    public abstract class AacAnimatorNode<T> : AacAnimatorNode where T: AacAnimatorNode<T>
    {
        protected readonly AacFlStateMachine ParentMachine;
        protected readonly IAacDefaultsProvider DefaultsProvider;

        protected AacAnimatorNode(AacFlStateMachine parentMachine, IAacDefaultsProvider defaultsProvider)
        {
            ParentMachine = parentMachine;
            DefaultsProvider = defaultsProvider;
        }

        public T LeftOf(T otherNode) => MoveNextTo(otherNode, -1, 0);
        public T RightOf(T otherNode) => MoveNextTo(otherNode, 1, 0);
        public T Over(T otherNode) => MoveNextTo(otherNode, 0, -1);
        public T Under(T otherNode) => MoveNextTo(otherNode, 0, 1);

        public T LeftOf() => MoveNextTo(null, -1, 0);
        public T RightOf() => MoveNextTo(null, 1, 0);
        public T Over() => MoveNextTo(null, 0, -1);
        public T Under() => MoveNextTo(null, 0, 1);
        
        public T Shift(T otherState, int shiftX, int shiftY) => MoveNextTo(otherState, shiftX, shiftY);
        
        private T MoveNextTo(T otherStateOrSecondToLastWhenNull, int x, int y)
        {
            if (otherStateOrSecondToLastWhenNull == null)
            {
                var siblings = ParentMachine.GetChildNodes();
                var other = siblings[siblings.Count - 2];
                Shift(other.GetPosition(), x, y);

                return (T) this;
            }

            Shift(otherStateOrSecondToLastWhenNull.GetPosition(), x, y);
            
            return (T) this;
        }

        public T Shift(Vector3 otherPosition, int shiftX, int shiftY)
        {
            SetPosition(otherPosition + new Vector3(shiftX * DefaultsProvider.Grid().x, shiftY * DefaultsProvider.Grid().y, 0));
            return (T) this;
        }
    }
}