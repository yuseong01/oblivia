public interface IProjectileModule
{
    IProjectileModule Clone();
    void OnFire(Projectile projectile);
    void OnUpdate(Projectile projectile);
}
