using Bossjam.NPCs.Attacks;
using Bossjam.NPCs.Tubble.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace Bossjam.NPCs.Tubble.Attacks
{
    public class SpewBubbleAttack : BaseAttack
    {
        public override void AI(BaseNPC npc)
        {
            npc.npc.ai[0]++;

            if (npc.npc.ai[0] > 60 && npc.npc.ai[0] % 5 == 0 && npc.npc.ai[0] <= 100)
            {
                npc.npc.TargetClosest();
                Vector2 vel = npc.npc.DirectionTo(npc.Target().Center).RotatedByRandom(MathHelper.ToRadians(5)) * Main.rand.NextFloat(8f, 12f);
                int type = ModContent.ProjectileType<TubbleBubble>();

                if (Main.rand.NextBool(3))
                {
                    type = ModContent.ProjectileType<BigTubbleBubble>();
                    vel *= 0.9f;
                }

                int proj = Projectile.NewProjectile(npc.npc.Center, vel, type, type == ModContent.ProjectileType<TubbleBubble>() ? 30 : 40, 0f);

                if (type == ModContent.ProjectileType<BigTubbleBubble>())
                {
                    Projectile bubble = Main.projectile[proj];
                    BigTubbleBubble bigBubble = bubble.modProjectile as BigTubbleBubble;

                    WeightedRandom<byte> contents = new WeightedRandom<byte>();
                    contents.Add(BigTubbleBubble.NoContents);
                    contents.Add(BigTubbleBubble.Skull, 0.7f);
                    contents.Add(BigTubbleBubble.Ladybug, 0.5f);

                    bigBubble.contents = contents;
                }
            }
        }

        public override bool ChooseNextAttack(BaseNPC npc) => npc.npc.ai[0] == 140;
        public override BaseAttack GetNextAttack(BaseNPC npc) => new HopAttack();

        public override void ResetNPC(BaseNPC npc)
        {
            (npc as TubbleBoss).morselsEaten = 0;
            npc.npc.ai[0] = 0;
        }
    }
}
