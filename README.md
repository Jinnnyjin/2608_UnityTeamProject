# UnityTeamProject
<img width="898" height="460" alt="image" src="https://github.com/user-attachments/assets/3779a815-494d-4b30-a2c2-fedd4541db69" />

## 프로젝트 소개

- **프로젝트 명**: 뱀서류 모작
- **소개**: 4인 팀 프로젝트로 진행하는 뱀파이어 서바이버즈 장르 모작
- **기간**: 2026-08-01 ~ 2026-08-07

- **노션 링크**: [1조 Unity 팀프로젝트 노션 링크](https://app.notion.com/p/1-178fe1fddf628207955501ec66e19fc8)

## 프로젝트 셋팅

- **Unity 버전**: 6000.3.6f1
- **실행 환경**: Windows 10/11
- **권장 해상도**: 1920 * 1080

## R&R (Roles & Responsibilities)

| 팀원 | 담당 |
| --- | --- |
| 혁찬 | 플레이어 & 스킬 |
| 혜성 | 매니저 |
| 희진 | 몬스터 |
| 운암 | UI & 사운드(효과음, BGM), 맵 |

## 게임 플레이 방법

- **이동**: W/A/S/D 키로 플레이어 이동
- **클리어 방법**: 맵 내 모든 몬스터 처치 시 클리어

### 핵심 기능
<img width="899" height="461" alt="image" src="https://github.com/user-attachments/assets/e21919f5-5a8b-4f51-95cc-dd30deb182e6" />

- **레벨업**: 플레이어가 레벨업하면 액티브/패시브 스킬 중 랜덤 3개가 제시되며, 그중 하나를 선택하여 획득/강화

<img width="314" height="57" alt="image" src="https://github.com/user-attachments/assets/4f672979-759d-421e-8e64-d12f1cfbc6d0" />

- **스킬**: 보유한 스킬은 자동으로 발사(오토 어택)

<img width="519" height="321" alt="image" src="https://github.com/user-attachments/assets/8bbd7724-06e4-4ac9-8669-18672dd81faa" />

- **몬스터 스폰**: 뷰포트 바깥 다방면에서 몬스터가 스폰되어 플레이어를 향해 이동

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


