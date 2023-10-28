using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Base.Mobs;

namespace Base.State
{
    public abstract class BaseState
    {
        protected BaseMobs targetMobs { get; set; }

        protected abstract void OnStateEnter();
        protected abstract void OnStateUpdate();
        protected abstract void OnStateExit();
    }
}