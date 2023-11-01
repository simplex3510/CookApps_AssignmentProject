using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Base.State;

public class FiniteStateMachine
{
    private IStatable curState;

    public FiniteStateMachine(IStatable initState)
    {
        curState = initState;
        ChangeState(curState);
    }

    public void ChangeState(IStatable nextState)
    {
        if (curState == nextState)
        {
            return;
        }

        if (curState != null)
        {
            curState.OnStateExit();
        }

        curState = nextState;
        nextState.OnStateEnter();
    }

    public void UpdateState()
    {
        if (curState != null)
        {
            curState.OnStateUpdate();
        }
    }
}