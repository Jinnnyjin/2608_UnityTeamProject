public class DefaultPassiveSkillData : SkillData
{
    protected float p_Hp = 0;
    protected float p_HpMult = 1;
    protected float p_Damage = 0;
    protected float p_Speed = 0;
    protected float p_CoolTime = 1;
    protected float p_Def = 0;
    protected float p_ReduceDmg = 1;
    protected float p_DmgMult = 1;
    protected float p_SpeedMult = 1;
    protected float p_ProjSpeedMult = 1;
    protected int p_ProjCount = 0;
    public override float Hp => p_Hp;
    public override float HpMult => p_HpMult;
    public override float Damage => p_Damage;
    public override float Speed => p_Speed;
    public override float CoolTime => m_CoolTime;
    public override float Def => p_Def;
    public override float ReduceDmg => p_ReduceDmg;
    public override float DmgMult => p_DmgMult;
    public override float SpeedMult => p_SpeedMult;
    public override float ProjSpeedMult => p_ProjSpeedMult;
    public override int ProjCount => p_ProjCount;
}