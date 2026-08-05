using System.Collections;
using UnityEngine;

public class RangeSkill : SkillData
{
    [SerializeField] private float m_Range;
    [SerializeField] private float m_Damage;
    public override float Damage => m_Damage;

    public float m_WarningDelay;
    private const float BaseSpriteRadius = 0.5f;


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
        
        if(_skill.CoolTimer.IsCoolComp("Range_Skill") && distance <= m_Range)
        {
            monster.MonsterMove.MonsterAttackSkill();
            monster.StartCoroutine(SkillCoroutine(monster));
            _skill.CoolTimer.RefreshCool("Range_Skill");
        }
    }

    private IEnumerator SkillCoroutine(Monster monster)
    {
        monster.StopMoving();
        
        // 범위 반경 오브젝트 활성화
        GameObject indicator = ObjectPoolManager.m_Instance.GetObject(Prefab);
        indicator.transform.position = monster.Position;

        float scale = m_Range / BaseSpriteRadius;
        indicator.transform.localScale = Vector3.one * scale;
        
        yield return new WaitForSeconds(m_WarningDelay);
        
        // 범위 오브젝트 반납
        ObjectPoolManager.m_Instance.PushObject(indicator);

        float distance = (monster.Position - GameManager.m_Instance.Player.Position).magnitude;
        

        // TakeDamage 슬라이더바 오류로 코루틴 진행이안됨
        // 테스트용 try
        if (distance <= m_Range)
        {
            try
            {
                DamageInfo info = new DamageInfo { Dmg = Damage };
                GameManager.m_Instance.Player.TakeDamage(info);
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"TakeDamage 오류: {e.Message}");
            }
        }


        monster.ResumeMoving();
    }
}
