using Bossjam.NPCs.Tubble.Adds;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Bossjam.NPCs.Tubble.Projectiles
{
    public class TubbleTongue : ModProjectile
    {
        public Vector2 SpawnOrigin { get => new Vector2(projectile.ai[0], projectile.ai[1]); set => (projectile.ai[0], projectile.ai[1]) = (value.X, value.Y); }

        public NPC Target => Main.npc[target];
        public int target = 0;

        private bool _contact = false;
        private int _time = 0;
        private float _lastDistance = float.MaxValue;
        private bool _missed = false;
        private Vector2 _relativePos = Vector2.Zero;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tongue");
        }

        public override void SetDefaults()
        {
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.height = 28;
            projectile.alpha = 30;
            projectile.width = 18;
            projectile.penetrate = -1;
            projectile.timeLeft = 60000;
        }

        public override void AI()
        {
            projectile.rotation = projectile.AngleFrom(SpawnOrigin);

            if (!_contact && (!Target.active || Target.life < 0 || projectile.DistanceSQ(Target.Center) > _lastDistance))
                _missed = true;
            else
                _lastDistance = projectile.DistanceSQ(Target.Center);

            if (projectile.DistanceSQ(SpawnOrigin) > 3000 * 3000)
                projectile.Kill();

            if (_missed)
                return;

            if (_contact)
            {
                _time++;
                Target.Center = projectile.Center + _relativePos;

                if (_time > 30)
                    projectile.velocity = projectile.DirectionTo(SpawnOrigin) * 17;

                if (projectile.DistanceSQ(SpawnOrigin) < 20 * 20)
                {
                    projectile.Kill();
                    Target.StrikeNPCNoInteraction(Main.rand.Next(4520, 8730), 0, 0, true);
                }
            }
            else
            {
                for (int i = 0; i < Main.maxNPCs; ++i)
                {
                    NPC n = Main.npc[i];

                    if (n.active && n.life >= 1 && n.type == ModContent.NPCType<MorselFly>() && n.DistanceSQ(projectile.Center) < 10 * 10)
                    {
                        _contact = true;
                        _relativePos = n.Center - projectile.Center;

                        projectile.velocity = Vector2.Zero;
                        target = n.whoAmI;
                    }
                }
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float discard = 0;
            return Collision.CheckAABBvLineCollision(targetHitbox.Location.ToVector2(), targetHitbox.Size(), SpawnOrigin, projectile.Center, 24, ref discard);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D tex = Main.projectileTexture[projectile.type];

            Main.spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, new Rectangle(26, 0, 54, 24), lightColor, projectile.rotation, new Vector2(54, 24) / 2f, 1f, SpriteEffects.None, 0f);

            for (int i = 1; i < projectile.Distance(SpawnOrigin) / 54f; ++i)
            {
                Vector2 offset = projectile.DirectionTo(SpawnOrigin) * 54;
                Vector2 pos = projectile.Center + (offset * i);
                Color col = Lighting.GetColor((int)(pos.X / 16f), (int)(pos.Y / 16f));
                Main.spriteBatch.Draw(tex, pos - Main.screenPosition, new Rectangle(0, 0, 54, 24), col, projectile.rotation, new Vector2(54, 24) / 2f, 1f, SpriteEffects.None, 0f);
            }
            return false;
        }
    }
}
