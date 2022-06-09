using Terraria.ModLoader;

namespace Bossjam.NPCs.Tubble.Projectiles
{
    public class SkullDrop : ModProjectile
    {
        public override void SetStaticDefaults() => DisplayName.SetDefault("Skull");

        public override void SetDefaults()
        {
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.height = 32;
            projectile.width = 32;
            projectile.penetrate = -1;
            projectile.timeLeft = 200000;
        }

        public override void AI()
        {
            projectile.rotation += 0.01f;
            projectile.velocity.Y += 0.2f;
        }
    }
}
