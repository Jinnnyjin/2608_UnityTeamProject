# UnityTeamProject

## 프로젝트 소개

- **프로젝트 명**: 뱀서 모작
- **소개**: 4인 팀 프로젝트로 진행하는 뱀파이어 서바이버즈 장르 모작
- **기간**: 2026-08-01 ~ 2026-08-07

## 프로젝트 셋팅

- **Unity 버전**: 6000.3.6f1

## R&R (Roles & Responsibilities)

| 팀원 | 담당 |
| --- | --- |
| 혁찬 | 플레이어 & 스킬 |
| 혜성 | 매니저 |
| 희진 | 몬스터 |
| 운암 | UI & 사운드(효과음, BGM), 맵 |

## 핵심 게임 플레이

- **이동**: W/A/S/D 키로 플레이어 이동
- **스킬**: 보유한 스킬은 자동으로 발사(오토 어택)
- **몬스터 스폰**: 뷰포트 바깥 다방면에서 몬스터가 스폰되어 플레이어를 향해 이동
- **레벨업**: 플레이어가 레벨업하면 액티브/패시브 스킬 중 랜덤 3개가 제시되며, 그중 하나를 선택하여 획득/강화

## 코드 컨벤션

### 네이밍 규칙

| 대상 | 규칙 | 예시 |
| --- | --- | --- |
| `private` / `protected` 필드 | camelCase, 접두사 `m` | `mHealth`, `mMoveSpeed` |
| `public` 필드 / 프로퍼티 | PascalCase | `Health`, `MoveSpeed` |
| 매개변수(parameter) | 접두사 `_` | `_amount`, `_targetPosition` |
| 함수(메서드) | 항상 PascalCase (접근 제한자 무관) | `TakeDamage()`, `moveSpeed` 아님 |

### 예시

```csharp
public class Player : MonoBehaviour
{
    public float Health;
    private float mMoveSpeed;
    protected int mLevel;

    public void TakeDamage(float _amount)
    {
        Health -= _amount;
    }

    private void Move(Vector2 _direction)
    {
        transform.position += (Vector3)_direction * mMoveSpeed * Time.deltaTime;
    }
}
```


