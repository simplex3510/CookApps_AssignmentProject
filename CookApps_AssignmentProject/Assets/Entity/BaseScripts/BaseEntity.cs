using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Base.State;

namespace Base.Entity
{
    public interface IDamagable
    {
        public void AttackTarget();
        public void DamagedCharacter(float damage);
    }

    public abstract class BaseEntity : MonoBehaviour, IDamagable
    {
        public Animator AnimatorCtrller { get; protected set; }

        protected EState curState;
        protected Dictionary<EState, IStatable> StateDict { get; set; }
        protected FiniteStateMachine FSM { get; set; }

        [SerializeField]
        protected BaseStatData statData;
        public BaseStatData StatData { get { return statData; } }

        [SerializeField]
        protected BaseEntity target;
        [SerializeField]
        protected LayerMask EnemyLayerMask;

        protected abstract void InitializeStateDict();
        protected abstract void AssignAnimationParameters();
        protected abstract IEnumerator UpdateFSM();
        protected abstract void ChangeState(EState eState);

        public abstract void AttackTarget();
        public abstract void DamagedCharacter(float damage);
    }
}
