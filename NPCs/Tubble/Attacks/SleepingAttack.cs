﻿using Bossjam.NPCs.Attacks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Bossjam.NPCs.Tubble.Attacks
{
    public class SleepingAttack : BaseAttack
    {
        const int AnimationTime = 60;

        private bool _jumpedOn = false;

        public override void AI(BaseNPC npc)
        {
            if (++npc.npc.frameCounter >= AnimationTime)
                npc.npc.frameCounter = 0;

            npc.frame = (int)(npc.npc.frameCounter / (AnimationTime * 0.5f));

            if (_jumpedOn)
                return;

            for (int i = 0; i < Main.maxPlayers; ++i)
            {
                Player p = Main.player[i];
                if (p.active && !p.dead && p.velocity.Y > 0)
                {
                    Rectangle groundBox = new Rectangle((int)npc.npc.position.X, (int)npc.npc.position.Y + 48, npc.npc.width, 8);
                    Rectangle jumpBox = new Rectangle((int)p.position.X, (int)p.Bottom.Y, p.width, 4);

                    if (groundBox.Intersects(jumpBox))
                    {
                        p.velocity.Y = -14;
                        _jumpedOn = true;
                    }
                }
            }
        }

        public override bool ChooseNextAttack(BaseNPC npc) => _jumpedOn;
        public override BaseAttack GetNextAttack(BaseNPC npc) => new WakeUpAttack();
        public override void ResetNPC(BaseNPC npc) => npc.npc.frameCounter = 0;

        public override bool PreDrawNPC(BaseNPC npc, Color drawColor)
        {
            Texture2D sleepyFrog = npc.mod.GetTexture("NPCs/Tubble/SleepyTubble");
            Point loc = new Point(0, npc.frame * 184);

            Main.spriteBatch.Draw(sleepyFrog, npc.npc.position - Main.screenPosition, new Rectangle(loc.X, loc.Y, 220, 180), npc.LightingAt());
            return false;
        }
    }
}
