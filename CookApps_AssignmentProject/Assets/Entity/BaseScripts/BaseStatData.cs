using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseStatData
{
    [SerializeField] private float skillCoolTime;
    public float SkillCoolTime { get { return skillCoolTime; } }

    [SerializeField] private float maxHP;
    public float MaxHP { get { return maxHP; } }

    [SerializeField] private float curHP;
    public float CurHP
    {
        get { return curHP; }
        set { curHP = MaxHP < value ? maxHP : value; }
    }

    [SerializeField] private float attack;
    public float ATK { get { return attack; } }

    [SerializeField] private float defensive;
    public float DEF { get { return defensive; } }

    [SerializeField] private float speed;
    public float SPD { get { return speed; } }

    [SerializeField] private bool dead;
    public bool DEAD
    {
        get { return dead = (curHP <= 0) ? true : false; }
    }

    public void InitializeStatData()
    {
        curHP = maxHP;
        dead = false;
    }
}
