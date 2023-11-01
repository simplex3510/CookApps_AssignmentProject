using Base.Entity;
using Base.State;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public partial class SwordWarrior
{
    #region public State Method
    public void CheckNearestEnemy()
    {
        // 1. 모든 적들을 가져온다. -> BattleManager에서 json을 읽은 후 객체들을 반환하도록 개선
        // 1.1. SpawnManager에서 스폰된 몹들을 대상으로
        // 2. 가져온 적들의 거리를 계산한다.
        // 3. 이 시퀀스를 상태전이마다 반복하여 최근거리 적을 찾는다.

        float leastDistance = float.MaxValue;
        Vector2 charPos = new Vector2(transform.position.x, transform.position.y);
        foreach (var enemy in SpawnManager.Instance.spawnedMobList)
        {
            Vector2 enemyPos = new Vector2(enemy.transform.position.x, enemy.transform.position.y);
            float distance = Vector2.Distance(charPos, enemyPos);
            if (distance < leastDistance)
            {
                leastDistance = distance;
                target = enemy;
            }
        }

        return;
    }

    public void MoveToTarget()
    {
        if (target == null)
        {
            Debug.LogError("Can not Move to Target That is null");
            return;
        }

        transform.position = Vector2.MoveTowards(transform.position, target.transform.position, StatData.SPD * Time.deltaTime);
        //transform.Translate(StatData.SPD * Time.deltaTime * target.transform.position);
    }

    public override void AttackTarget()
    {
        target.DamagedCharacter(StatData.ATK);
    }

    public override void DamagedCharacter(float damage)
    {
        StatData.CurHP -= damage;
        Debug.Log($"CurHP: {StatData.CurHP}, DMG: {damage}");
    }
    #endregion

    #region protected Method

    protected override IEnumerator UpdateFSM()
    {
        Debug.Log("Wait UpdateFSM");
        yield return new WaitForSeconds(3f);
        Debug.Log("Start UpdateFSM");

        Collider2D detectedTarget;

        while (true)
        {
            switch (curState)
            {
                case EState.Idle:
                    if (SpawnManager.Instance.spawnedMobList.Count != 0)
                    {
                        ChangeState(EState.Move);
                    }
                    break;
                case EState.Move:
                    detectedTarget = Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y - 0.4f), new Vector2(2, 1), 0f, EnemyLayerMask);
                    if (detectedTarget != null)
                    {
                        ChangeState(EState.Attack);
                    }
                    break;
                case EState.Attack:
                    detectedTarget = Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y - 0.4f), new Vector2(2, 1), 0f, EnemyLayerMask);
                    if (detectedTarget == null)
                    {
                        ChangeState(EState.Move);
                    }
                    break;
                case EState.Skill:
                    detectedTarget = Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y - 0.4f), new Vector2(2, 1), 0f, EnemyLayerMask);
                    if (detectedTarget != null)
                    {
                        ChangeState(EState.Attack);
                    }
                    else
                    {
                        ChangeState(EState.Move);
                    }
                    break;
                default:
                    Debug.LogError("UpdateFSM Error");
                    break;
            }

            Debug.Log(curState.ToString());
            FSM.UpdateState();
            yield return null;
        }
    }

    protected override void ChangeState(EState nextState)
    {
        curState = nextState;

        switch (curState)
        {
            case EState.Idle:
                FSM.ChangeState(StateDict[EState.Idle]);
                break;
            case EState.Move:
                FSM.ChangeState(StateDict[EState.Move]);
                break;
            case EState.Attack:
                FSM.ChangeState(StateDict[EState.Attack]);
                break;
            case EState.Skill:
                FSM.ChangeState(StateDict[EState.Skill]);
                break;
            default:
                Debug.LogError("ChangeState Error");
                break;
        }
    }

    #endregion

    #region private Method

    #endregion
}

public class SwordWarrior_IdleState : BaseState
{
    public SwordWarrior_IdleState(SwordWarrior entity) : base(entity) { }

    public override void OnStateEnter()
    {
        GetEntity<SwordWarrior>().AnimatorCtrller.SetTrigger(GetEntity<SwordWarrior>().AnimParam_Idle);
    }

    public override void OnStateExit()
    {
        GetEntity<SwordWarrior>().CheckNearestEnemy();
    }

    public override void OnStateUpdate()
    {
        // Blank
    }
}

public class SwordWarrior_MoveState : BaseState
{
    public SwordWarrior_MoveState(SwordWarrior entity) : base(entity) { }

    public override void OnStateEnter()
    {
        GetEntity<SwordWarrior>().AnimatorCtrller.SetTrigger(GetEntity<SwordWarrior>().AnimParam_Move);
    }

    public override void OnStateExit()
    {
        GetEntity<SwordWarrior>().CheckNearestEnemy();
    }

    public override void OnStateUpdate()
    {
        Debug.Log("Moving");
        GetEntity<SwordWarrior>().MoveToTarget();
    }
}

public class SwordWarrior_AttackState : BaseState
{
    public SwordWarrior_AttackState(SwordWarrior entity) : base(entity) { }

    public override void OnStateEnter()
    {
        GetEntity<SwordWarrior>().AnimatorCtrller.SetBool(GetEntity<SwordWarrior>().AnimParam_Attack, true);
    }

    public override void OnStateExit()
    {
        GetEntity<SwordWarrior>().CheckNearestEnemy();
        GetEntity<SwordWarrior>().AnimatorCtrller.SetBool(GetEntity<SwordWarrior>().AnimParam_Attack, false);
    }

    public override void OnStateUpdate()
    {
        
    }
}

public class SwordWarrior_SkillState : BaseState
{
    public SwordWarrior_SkillState(SwordWarrior entity) : base(entity) { }

    public override void OnStateEnter()
    {
        GetEntity<SwordWarrior>().AnimatorCtrller.SetTrigger(GetEntity<SwordWarrior>().AnimParam_Skill);
    }

    public override void OnStateExit()
    {
        GetEntity<SwordWarrior>().CheckNearestEnemy();
    }

    public override void OnStateUpdate()
    {

    }
}
