using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Base.Entity;
using Base.State;
using UnityEditor.U2D.Aseprite;

public class Mob1 : BaseEntity
{
    public EntityData StatData { get { return statData; } private set { statData = value; } }

    public override void AttackTarget()
    {

    }

    public override void DamagedCharacter(float damage)
    {
        if (IsDie())
        {
            Debug.Log($"{gameObject.name} is Died");
        }
        else
        {
            statData.CurHP -= (damage - StatData.DEF);
            Debug.Log($"CurHP: {statData.CurHP}, DMG: {damage}");
        }
    }

    public override bool IsDie()
    {
        if (statData.CurHP <= 0)
        {
            SpawnManager.Instance.spawnedMobList.Remove(this);
            Destroy(gameObject);
            return true;
        }
        else
        {
            return false;
        }
    }

    protected override void AssignAnimationParameters()
    {

    }

    protected override void ChangeState(EState eState)
    {

    }

    protected override void InitializeStateDict()
    {

    }

    protected override IEnumerator UpdateFSM()
    {
        yield return null;
    }
}
