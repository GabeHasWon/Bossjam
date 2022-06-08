using Terraria.ModLoader;

namespace Bossjam.NPCs.Tubble.Projectiles
{
    public class TubbleBubble : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bubble");
        }

        public override void SetDefaults()
        {
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.height = 32;
            projectile.width = 32;
            projectile.penetrate = -1;
            projectile.timeLeft = 60000;
        }

        public override void AI()
        {
            projectile.velocity.X *= 0.99f;
            projectile.velocity.Y *= 0.999f;

            if (projectile.velocity.Y > -1)
                projectile.velocity.Y -= projectile.velocity.X * 0.001f;
        }
    }
}
