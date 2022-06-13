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

            if (NPC.AnyNPCs(ModContent.NPCType<TubbleBoss>()))
            {
                Vector2 bed = ModContent.GetInstance<TubbleWorld>().bedPosition;

                if (projectile.position.Y < bed.Y - 600)
                    projectile.velocity.Y += 0.07f;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Main.PlaySound(SoundID.Item54.SoundId, (int)projectile.Center.X, (int)projectile.Center.Y, SoundID.Item54.Style, 0.5f);

            if (projectile.velocity.X != oldVelocity.X)
                projectile.velocity.X = -oldVelocity.X;

            if (projectile.velocity.Y != oldVelocity.Y)
                projectile.velocity.Y = -oldVelocity.Y;

            return false;
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.Item54, projectile.position);

            int dustCount = Main.rand.Next(4, 9);
            for (int i = 0; i < dustCount; ++i)
            {
                Vector2 speed = Main.rand.NextVector2Circular(0.5f, 0.5f);
                Dust.NewDust(projectile.Center, 0, 0, DustID.BubbleBurst_Blue, speed.X * .5f, speed.Y * .5f, 0, default, Main.rand.NextFloat(0.5f, 1f));
            }
        }
    }
}
