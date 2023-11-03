using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Base.Entity;
using Base.State;

public partial class SwordWarrior : BaseEntity
{

    public int AnimParam_Idle { get; private set; }
    public int AnimParam_Move { get; private set; }
    public int AnimParam_Attack { get; private set; }
    public int AnimParam_Skill { get; private set; }

    private void Awake()
    {
        curState = EState.Idle;
        AnimatorCtrller = GetComponent<Animator>();
        StateDict = new Dictionary<EState, IStatable>();
        InitializeStateDict();
        FSM = new FiniteStateMachine(StateDict[curState]);

        AssignAnimationParameters();

        StatData.InitializeStatData();
    }

    void Start()
    {
        StartCoroutine(UpdateFSM());
    }

    protected override void InitializeStateDict()
    {
        StateDict[EState.Idle] = new SwordWarrior_IdleState(this);
        StateDict[EState.Move] = new SwordWarrior_MoveState(this);
        StateDict[EState.Attack] = new SwordWarrior_AttackState(this);
        StateDict[EState.Skill] = new SwordWarrior_SkillState(this);
    }

    protected override void AssignAnimationParameters()
    {
        AnimParam_Idle = Animator.StringToHash("idle");
        AnimParam_Move = Animator.StringToHash("move");
        AnimParam_Attack = Animator.StringToHash("canAttack");
        AnimParam_Skill = Animator.StringToHash("skill");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector2(transform.position.x, transform.position.y - 0.4f), new Vector2(2, 1));
    }
}