using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Base.Entity;
using Base.State;

public class Mob1 : BaseEntity
{
    public override void AttackTarget()
    {

    }

    public override void DamagedCharacter(float damage)
    {
        Debug.Log("¶Ñ±î¶Ñ±î");
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
