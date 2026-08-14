using System.Collections;
using UnityEngine;

public class RangeSkill : MonsterSkillData
{
    protected override string SkillId => "Range_Skill";

    [SerializeField] private float m_range;
    [SerializeField] private float m_damage;
    public override float Damage => m_damage;
    public float m_WarningDelay;
    private const float m_baseSpriteRadius = 0.5f;

    [SerializeField] private SOAudio m_audioRangeSkill;

    protected override bool CanUseSkill(Monster monster)
    {
        Player player = GameManager.m_Instance.Player;
        if (player == null) return false;

        float distance = (monster.Position - player.Position).magnitude;
        return distance <= m_range;
    }

    protected override IEnumerator SkillRoutine(Monster monster, Skill skill)
    {

        // 스킬 중 이동 정지
        monster.MonsterMove.MonsterAttackSkill();
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

        if (GameManager.m_Instance.Player == null)
        {
            monster.ResumeMoving();
            yield break;
        }

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
    }
}
