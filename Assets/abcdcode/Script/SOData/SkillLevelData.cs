using UnityEngine;
public class SkillLevelData : SOData, IStat
{
    [SerializeField]protected float m_Hp = 0;
    [SerializeField]protected float m_HpMult = 0;
    [SerializeField]protected float m_Damage = 0;
    [SerializeField]protected float m_DmgMult = 0;
    [SerializeField]protected float m_Speed = 0;
    [SerializeField]protected float m_SpeedMult = 0;
    [SerializeField]protected float m_ProjSpeed = 0;
    [SerializeField]protected float m_ProjSpeedMult = 0;
    [SerializeField]protected int m_ProjCount = 0;
    [SerializeField]protected float m_CoolTime = 0;
    [SerializeField]protected float m_Def = 0;
    [SerializeField]protected float m_ReduceDmg = 0;

    public float Hp => m_Hp;
    public float HpMult => m_HpMult;
    public float Damage => m_Damage;
    public float DmgMult => m_DmgMult;
    public float Speed => m_Speed;
    public float SpeedMult => m_SpeedMult;
    public float ProjSpeed => m_ProjSpeed;
    public float ProjSpeedMult => m_ProjSpeedMult;
    public int ProjCount => m_ProjCount;
    public float CoolTime => m_CoolTime;
    public float Def => m_Def;
    public float ReduceDmg => m_ReduceDmg;
}