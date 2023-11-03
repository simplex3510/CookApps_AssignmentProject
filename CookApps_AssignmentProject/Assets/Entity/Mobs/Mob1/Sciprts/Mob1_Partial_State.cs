using Base.Entity;
using Base.State;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Mob1
{
    #region public State Method
    public void CheckNearestEnemy()
    {
        // 1. 모든 적들을 가져온다. -> BattleManager에서 json을 읽은 후 객체들을 반환하도록 개선
        // 1.1. SpawnManager에서 스폰된 몹들을 대상으로
        // 2. 가져온 적들의 거리를 계산한다.
        // 3. 이 시퀀스를 상태전이마다 반복하여 최근거리 적을 찾는다.

        float leastDistance = float.MaxValue;
        Vector2 enemyPos = new Vector2(transform.position.x, transform.position.y);
        foreach (var character in SpawnManager.Instance.spawnedCharList)
        {
            Vector2 charPos = new Vector2(character.transform.position.x, character.transform.position.y);
            float distance = Vector2.Distance(enemyPos, charPos);
            if (distance < leastDistance)
            {
                leastDistance = distance;
                target = character;
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
                    detectedTarget = Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y), new Vector2(1.2f, 1.2f), 0f, EnemyLayerMask);
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
                    detectedTarget = Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y), new Vector2(1.2f, 1.2f), 0f, EnemyLayerMask);
                    if (detectedTarget == null || target == null)
                    {
                        ChangeState(EState.Move);
                    }
                    break;

                case EState.Skill:
                    detectedTarget = Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y), new Vector2(1.2f, 1.2f), 0f, EnemyLayerMask);
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

public class Mob1_IdleState : BaseState
{
    public Mob1_IdleState(Mob1 entity) : base(entity) { }

    public override void OnStateEnter()
    {
        Mob1 mob1 = entity as Mob1;
        mob1.AnimatorCtrller.SetTrigger(mob1.AnimParam_Idle);
    }

    public override void OnStateExit()
    {
        Mob1 mob1 = entity as Mob1;
        mob1.CheckNearestEnemy();
    }

    public override void OnStateUpdate()
    {
        
    }
}

public class Mob1_MoveState : BaseState
{
    public Mob1_MoveState(Mob1 entity) : base(entity) { }

    public override void OnStateEnter()
    {
        Mob1 mob1 = entity as Mob1;
        mob1.AnimatorCtrller.SetTrigger(mob1.AnimParam_Move);
    }

    public override void OnStateExit()
    {
        Mob1 mob1 = entity as Mob1;
        mob1.CheckNearestEnemy();
    }

    public override void OnStateUpdate()
    {
        Mob1 mob1 = entity as Mob1;
        mob1.MoveToTarget();
    }
}

public class Mob1_AttackState : BaseState
{
    public Mob1_AttackState(Mob1 entity) : base(entity) { }

    public override void OnStateEnter()
    {
        Mob1 mob1 = entity as Mob1;
        mob1.AnimatorCtrller.SetBool(mob1.AnimParam_Attack, true);
    }

    public override void OnStateExit()
    {
        Mob1 mob1 = entity as Mob1;
        mob1.CheckNearestEnemy();
        mob1.AnimatorCtrller.SetBool(mob1.AnimParam_Attack, false);
    }

    public override void OnStateUpdate()
    {
         
    }
}

public class Mob1_SkillState : BaseState
{
    public Mob1_SkillState(Mob1 entity) : base(entity) { }

    public override void OnStateEnter()
    {
        Mob1 mob1 = entity as Mob1;
        mob1.AnimatorCtrller.SetTrigger(mob1.AnimParam_Skill);
    }

    public override void OnStateExit()
    {
        Mob1 mob1 = entity as Mob1;
        mob1.CheckNearestEnemy();
    }

    public override void OnStateUpdate()
    {
         
    }
}