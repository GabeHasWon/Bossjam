using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bossjam.NPCs.Tubble.Projectiles
{
    public class BigTubbleBubble : ModProjectile
    {
        public const int NoContents = 0;
        public const int Skull = 1;
        public const int Ladybug = 2;

        internal byte contents = 0;

        public override void SetStaticDefaults() => DisplayName.SetDefault("Bubble");

        public override void SetDefaults()
        {
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.height = 64;
            projectile.width = 64;
            projectile.penetrate = -1;
            projectile.timeLeft = 10 * 60;
            projectile.scale = Main.rand.NextFloat(0.9f, 1.1f);
        }

        public override void AI()
        {
            projectile.velocity.X *= 0.98f;
            projectile.velocity.Y *= 0.998f;

            if (projectile.velocity.Y > -0.5f)
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

        public override void Kill(int timeLeft)
        {
            if (contents == Skull)
            {
                int proj = Projectile.NewProjectile(projectile.Center, projectile.velocity, ModContent.ProjectileType<SkullDrop>(), 0, 0);
                Main.projectile[proj].scale = Main.rand.NextFloat(0.9f, 1.1f);
            }
            else if (contents == Ladybug)
            {
                int npc = NPC.NewNPC((int)projectile.Center.X, (int)projectile.Center.Y, ModContent.NPCType<Adds.Ladybug>(), 0, 0, 0, 0, 0, Player.FindClosest(projectile.Center, 0, 0));
                Main.npc[npc].scale = Main.rand.NextFloat(0.9f, 1.1f);
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D contTex = mod.GetTexture("NPCs/Tubble/Projectiles/BigBubbleContents");

            Main.spriteBatch.Draw(contTex, projectile.position - Main.screenPosition, new Rectangle(66 * contents, 0, 64, 64), lightColor);
            return true;
        }
    }
}
