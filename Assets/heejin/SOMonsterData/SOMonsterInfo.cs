using System.Collections.Generic;
using UnityEngine;

public enum MonsterType
{
   Common,
   Named,
   Boss,
   End 
}

[CreateAssetMenu(fileName = "SO_", menuName = "Game/Monster/MonsterInfo")]
public class SOMonsterInfo : ScriptableObject
{
    [SerializeField] MonsterType m_monsterType;
    public MonsterType MonsterType => m_monsterType;

    [Min(1.0f)]
    [SerializeField] private float m_maxHp;
    public float Max_HP => m_maxHp;


    [SerializeField] private float m_baseSpeed;
    public float BaseSpeed => m_baseSpeed;

    [SerializeField] private float m_baseAttack;
    public float BaseAttack => m_baseAttack;

    [SerializeField] private float m_baseAttackRange;
    public float BaseAttackRange => m_baseAttackRange;

    [SerializeField] private int m_expReward;
    public int ExpReward => m_expReward;

    [SerializeField] private List<SkillData> m_skills;
    public List<SkillData> Skills => m_skills;
}

public class MonsterInfo
{
    public float Speed { get; set; }

    public float HP { get; set; }

    public float Attack { get; set; }
}