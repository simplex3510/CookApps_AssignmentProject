using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Base.Entity;
using Base.State;

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
        if (target.StatData.DEAD == true)
        {
            CheckNearestEnemy();
        }
        else
        {
            target.DamagedCharacter(StatData.ATK);
        }
    }

    public override void DamagedCharacter(float damage)
    {
        StatData.CurHP -= (damage - StatData.DEF);
        
        if (StatData.DEAD)
        {
            Debug.Log($"{gameObject.name} is Died");
            SpawnManager.Instance.spawnedMobList.Remove(this);
            Destroy(gameObject);
        }
        else
        {
            Debug.Log($"CurHP: {StatData.CurHP}, DMG: {damage}");
        }
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
                    if (SpawnManager.Instance.spawnedMobList.Count == 0)
                    {
                        ChangeState(EState.Idle);
                        break;
                    }
                    detectedTarget = Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y - 0.4f), new Vector2(2, 1), 0f, EnemyLayerMask);
                    if (detectedTarget != null && detectedTarget.GetComponent<BaseEntity>() == target)
                    {   
                        ChangeState(EState.Attack);
                    }
                    break;

                case EState.Attack:
                    if (SpawnManager.Instance.spawnedMobList.Count == 0)
                    {
                        ChangeState(EState.Idle);
                        break;
                    }
                    detectedTarget = Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y - 0.4f), new Vector2(2, 1), 0f, EnemyLayerMask);
                    if (detectedTarget == null || target == null)
                    {
                        ChangeState(EState.Move);
                    }
                    break;

                case EState.Skill:
                    detectedTarget = Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y - 0.4f), new Vector2(2, 1), 0f, EnemyLayerMask);
                    if (detectedTarget != null && detectedTarget.GetComponent<BaseEntity>() == target)
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

            Debug.Log($"{curState}");
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
}

public class SwordWarrior_IdleState : BaseState
{
    public SwordWarrior_IdleState(SwordWarrior entity) : base(entity) { }

    public override void OnStateEnter()
    {
        SwordWarrior swordWarrior = entity as SwordWarrior;
        swordWarrior.AnimatorCtrller.SetTrigger(swordWarrior.AnimParam_Idle);
    }

    public override void OnStateExit()
    {
        SwordWarrior swordWarrior = entity as SwordWarrior;
        swordWarrior.CheckNearestEnemy();
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
        SwordWarrior swordWarrior = entity as SwordWarrior;
        swordWarrior.AnimatorCtrller.SetTrigger(swordWarrior.AnimParam_Move);
    }

    public override void OnStateExit()
    {
        SwordWarrior swordWarrior = entity as SwordWarrior;
        swordWarrior.CheckNearestEnemy();
    }

    public override void OnStateUpdate()
    {
        SwordWarrior swordWarrior = entity as SwordWarrior;
        swordWarrior.MoveToTarget();
    }
}

public class SwordWarrior_AttackState : BaseState
{
    public SwordWarrior_AttackState(SwordWarrior entity) : base(entity) { }

    public override void OnStateEnter()
    {
        SwordWarrior swordWarrior = entity as SwordWarrior;
        swordWarrior.AnimatorCtrller.SetBool(swordWarrior.AnimParam_Attack, true);
    }

    public override void OnStateExit()
    {
        SwordWarrior swordWarrior = entity as SwordWarrior;
        swordWarrior.CheckNearestEnemy();
        swordWarrior.AnimatorCtrller.SetBool(swordWarrior.AnimParam_Attack, false);
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
        SwordWarrior swordWarrior = entity as SwordWarrior;
        swordWarrior.AnimatorCtrller.SetTrigger(swordWarrior.AnimParam_Skill);
    }

    public override void OnStateExit()
    {
        SwordWarrior swordWarrior = entity as SwordWarrior;
        swordWarrior.CheckNearestEnemy();
    }

    public override void OnStateUpdate()
    {

    }
}
