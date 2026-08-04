using UnityEngine;

[CreateAssetMenu(fileName = "SO_Skill_", menuName = "Game/Monster/MonsterSkill")]
public class SOMonsterSkillData : SOData
{
    public SkillType Type;
    public float SkillAttackPower;
    public float SkillRange;
    public float SkillCoolTime;

}
