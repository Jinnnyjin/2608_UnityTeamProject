/// <summary>
/// 스탯 인터페이스.
/// 배율 증감은 1 : 100%.
/// </summary>
public interface IStat
{
    /// <summary>
    /// 최대 HP. 패시브 스킬이면 수치만큼 최대 Hp 증감
    /// </summary>
    public float Hp{get;}
    /// <summary>
    /// 최대 HP 배율. 패시브 스킬이면 수치만큼 최대 Hp 배율 증감
    /// </summary>
    public float HpMult{get;}
    /// <summary>
    /// 초당 체력 재생. 패시브 스킬이면 수치만큼 체력 재생 증감
    /// </summary>
    public float HPGen{get;}
    /// <summary>
    /// 피해량. 액티브 스킬이면 스킬의 자체 피해량. 패시브 스킬이면 수치만큼 고정값으로 피해량 증감
    /// </summary>
    public float Damage{get;}
    /// <summary>
    /// 피해량 배율. 액티브는 미사용. 패시브 스킬이면 수치만큼 최종 피해량 배율 증감
    /// </summary>
    public float DmgMult{get;}
    /// <summary>
    /// 이동속도. 액티브는 미사용. 패시브 스킬이면 수치만큼 고정값으로 이동속도 증감
    /// </summary>
    public float Speed{get;}
    /// <summary>
    /// 이동속도 배율. 패시브 스킬이면 수치만큼 최종 이동속도 배율 증감
    /// </summary>
    public float SpeedMult{get;}
    /// <summary>
    /// 투사체 속도. 액티브 스킬이면 자체 투사체 속도. 패시브는 사용하지 않음
    /// </summary>
    public float ProjSpeed{get;}
    /// <summary>
    /// 투사체 속도 배율. 패시브 스킬이면 수치만큼 최종 투사체 속도 배율 증감
    /// </summary>
    public float ProjSpeedMult{get;}
    /// <summary>
    /// 투사체 개수. 액티브 스킬이면 자체 투사체 개수. 패시브 스킬이면 수치만큼 최종 투사체 개수 증감
    /// </summary>
    public int ProjCount{get;}
    /// <summary>
    /// 액티브 스킬 쿨타임. 액티브 스킬이면 자체 스킬 쿨타임. 패시브 스킬이면 수치만큼 최종 쿨타임 배율 증감
    /// </summary>
    public float CoolTime{get;}
    /// <summary>
    /// 방어력. 수치만큼 피해량 고정으로 감소
    /// </summary>
    public float Def{get;}
    /// <summary>
    /// 피해감소. 수치만큼 최종 피해량 배율 증감
    /// </summary>
    public float ReduceDmg{get;}
}