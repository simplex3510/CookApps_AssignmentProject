using Base.Entity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base.State
{
    public enum EState : int
    {
        None = 0,
        Idle,
        Move,
        Attack,
        Skill,
        Size
    }

    public interface IStatable
    {
        public void OnStateEnter();
        public void OnStateUpdate();
        public void OnStateExit();
    }

    public abstract class BaseState : IStatable
    {
        protected BaseEntity entity;

        protected BaseState(BaseEntity entity)
        {
            this.entity = entity;
        }

        public abstract void OnStateEnter();
        public abstract void OnStateUpdate();
        public abstract void OnStateExit();

        public T GetEntity<T>() where T : BaseEntity => (T)entity;

    }
}