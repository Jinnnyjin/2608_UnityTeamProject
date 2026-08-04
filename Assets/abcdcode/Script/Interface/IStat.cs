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
    public float Damage{get;}
    public float DmgMult{get;}
    public float Speed{get;}
    public float CoolTime{get;}
    public float Def{get;}
    public float ReduceDmg{get;}
}