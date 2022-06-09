using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
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
            projectile.timeLeft = 8 * 60;
        }

        public override void AI()
        {
            projectile.velocity.X *= 0.99f;
            projectile.velocity.Y *= 0.999f;

            if (projectile.velocity.Y > -2)
                projectile.velocity.Y -= projectile.velocity.X * 0.001f;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Main.PlaySound(SoundID.Item54, projectile.position);

            if (projectile.velocity.X != oldVelocity.X)
                projectile.velocity.X = -oldVelocity.X;

            if (projectile.velocity.Y != oldVelocity.Y)
                projectile.velocity.Y = -oldVelocity.Y;

            return false;
        }
    }
}
