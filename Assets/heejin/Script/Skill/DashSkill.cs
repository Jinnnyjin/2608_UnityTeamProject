using System.Collections;
using UnityEngine;

public class DashSkill : MonsterSkillData
{
    protected override string SkillId => "Dash_Skill";

    [SerializeField] private float m_moveweight = 1.0f;
    [SerializeField] private SOAudio m_audioDashSkill;

    protected override IEnumerator SkillRoutine(Monster monster, Skill skill)
    {
        MonsterAIMove move = monster.MonsterMove;
        float startWeight = move.MoveWeight;
        move.MoveWeight = m_moveweight;
        move.LockDir = true;

        monster.StartTrail();
        SoundManager.m_Instance.PlaySfx(m_audioDashSkill);

        float time = 0f;
        float prevDist = (monster.Position - GameManager.m_Instance.Player.Position).magnitude;

        // 지속시간동안 스킬
        while (time < m_moveweight)
        {
            float currentDist = (monster.Position - GameManager.m_Instance.Player.Position).magnitude;

            // 직전 거리보다 멀어졌다면?
            if (currentDist > prevDist)
            {
                // 0.3f만큼만 더 가고 멈춤
                yield return new WaitForSeconds(0.3f);
                // 대쉬스킬 중단
                break;
            }

            prevDist = currentDist;
            time += Time.deltaTime;

            // 1 프레임 쉬고 다음프레임에 while문 이어서
            yield return null;
        }

        // 가중치 원래 값으로
        move.MoveWeight = startWeight;
        move.LockDir = false;

        monster.StopTrail();
    }
}
