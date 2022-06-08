﻿using Bossjam.NPCs.Attacks;
using Bossjam.NPCs.Tubble.Projectiles;
using Terraria;
using Terraria.ModLoader;

namespace Bossjam.NPCs.Tubble.Attacks
{
    public class SpewBubbleAttack : BaseAttack
    {
        public override void AI(BaseNPC npc)
        {
            npc.npc.ai[0]++;

            if (npc.npc.ai[0] > 60 && npc.npc.ai[0] % 10 == 0 && npc.npc.ai[0] <= 120)
            {
                npc.npc.TargetClosest();
                Projectile.NewProjectile(npc.npc.Center, npc.npc.DirectionTo(npc.Target().Center) * Main.rand.NextFloat(6f, 10f), ModContent.ProjectileType<TubbleBubble>(), 30, 0f);
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