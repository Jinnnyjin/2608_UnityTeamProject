using System.Collections;
using UnityEngine;

/// <summary>
/// 몬스터 액티브 스킬 공통 베이스.
/// 쿨타임 체크 -> 발동 조건 체크 -> 중복 실행 방지 -> 코루틴 실행 -> 종료 처리 흐름을 통일하고,
/// 실제 스킬 동작(SkillRoutine)만 하위 클래스에서 구현하도록 책임을 분리한다.
/// </summary>
public abstract class MonsterSkillData : SkillData
{
    protected abstract string SkillId { get; }

    public override void Init(Skill skill)
    {
        base.Init(skill);
        skill.CoolTimer.SetCool(SkillId, skill.Data.CoolTime, 0, false, null);
    }

    public override void GameUpdate(Skill skill)
    {
        Monster monster = skill.Owner as Monster;
        if (monster == null) return;

        if (!skill.CoolTimer.IsCoolComp(SkillId)) return;
        if (!CanUseSkill(monster)) return;
        if (!monster.TryStartSkill()) return;

        skill.CoolTimer.RefreshCool(SkillId);
        monster.StartCoroutine(RunSkill(monster, skill));
    }

    /// <summary>
    /// 쿨타임이 다 됐을 때 스킬을 발동할지 여부. (ex. RangeSkill의 사거리 체크)
    /// </summary>
    protected virtual bool CanUseSkill(Monster monster) => true;

    private IEnumerator RunSkill(Monster monster, Skill skill)
    {
        yield return SkillRoutine(monster, skill);
        monster.EndSkill();
    }

    /// <summary>
    /// 스킬의 실제 동작. 종료 시 EndSkill()은 베이스에서 자동 호출하므로 구현부에서 호출할 필요 없음.
    /// </summary>
    protected abstract IEnumerator SkillRoutine(Monster monster, Skill skill);
}
