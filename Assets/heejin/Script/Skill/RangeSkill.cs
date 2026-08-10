using System.Collections;
using UnityEngine;

public class RangeSkill : SkillData
{
    [SerializeField] private float m_range;
    [SerializeField] private float m_damage;
    public override float Damage => m_damage;
    public float m_WarningDelay;
    private const float m_baseSpriteRadius = 0.5f;

    [SerializeField] private SOAudio m_audioRangeSkill;

    public override void Init(Skill _skill)
    {
        base.Init(_skill);
        _skill.CoolTimer.SetCool("Range_Skill", _skill.Data.CoolTime,0,false,null);
    }

    public override void GameUpdate(Skill _skill)
    {
        Monster monster = _skill.Owner as Monster;
        if (monster == null) return;

        float distance = (monster.Position - GameManager.m_Instance.Player.Position).magnitude;
        
        if(_skill.CoolTimer.IsCoolComp("Range_Skill") && distance <= m_range)
        {
            if (!monster.TryStartSkill()) return;

            monster.MonsterMove.MonsterAttackSkill();
            monster.StartCoroutine(SkillCoroutine(monster));
            _skill.CoolTimer.RefreshCool("Range_Skill");
        }
    }

    private IEnumerator SkillCoroutine(Monster monster)
    {
        // 스킬 중 이동 정지
        monster.StopMoving();
        
        // 범위 반경 오브젝트 활성화
        GameObject indicator = ObjectPoolManager.m_Instance.GetObject(Prefab);
        indicator.transform.position = monster.Position;
        monster.SetActiveIndicator(indicator);

        float scale = m_range / m_baseSpriteRadius;
        indicator.transform.localScale = Vector3.one * scale;
       
        // 딜레이
        yield return new WaitForSeconds(m_WarningDelay);
        
        // 범위 오브젝트 반납
        ObjectPoolManager.m_Instance.PushObject(indicator);
        monster.ClearActiveIndicator();
        SoundManager.m_Instance.PlaySfx(m_audioRangeSkill);

        
        // 데미지 판정
        float distance = (monster.Position - GameManager.m_Instance.Player.Position).magnitude;

        if (distance <= m_range)
        {
            Debug.Log($"[RangeSkill 데미지] 시각: {Time.time}");
            DamageInfo info = new DamageInfo { Dmg = Damage };
            GameManager.m_Instance.Player.TakeDamage(info);
        }

        // 이동 재개
        monster.ResumeMoving();
        monster.EndSkill();
    }
}
