public interface IDamageable
{
    public void TakeDamage(DamageInfo info);
    public bool IsDead();
    public FactionEnum Faction{get;}
}
public enum FactionEnum
{
    Player,
    Enemy,
}