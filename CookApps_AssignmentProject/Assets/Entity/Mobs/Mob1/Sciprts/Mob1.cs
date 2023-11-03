using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Base.Entity;
using Base.State;

public partial class Mob1 : BaseEntity
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

    private void Start()
    {
        StartCoroutine(UpdateFSM());
    }

    protected override void InitializeStateDict()
    {
        StateDict[EState.Idle] = new Mob1_IdleState(this);
        StateDict[EState.Move] = new Mob1_MoveState(this);
        StateDict[EState.Attack] = new Mob1_AttackState(this);
        StateDict[EState.Skill] = new Mob1_SkillState(this);
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
        Gizmos.DrawWireCube(new Vector2(transform.position.x, transform.position.y), new Vector2(1.2f, 1.2f));
    }
}
