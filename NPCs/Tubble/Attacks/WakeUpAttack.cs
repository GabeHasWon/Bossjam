using Bossjam.NPCs.Attacks;
using Microsoft.Xna.Framework;
using System;
using Terraria;

namespace Bossjam.NPCs.Tubble.Attacks
{
    public class WakeUpAttack : BaseAttack
    {
        private float _xVel = 0;

        public override void AI(BaseNPC npc)
        {
            npc.npc.ai[0]++;

            float timer = npc.npc.ai[0];

            if (timer == 60)
            {
                npc.npc.velocity.Y = -20;
                _xVel = Main.rand.NextFloat(-1f, 1f);
            }
            else if (timer > 60 && npc.npc.ai[1] == 0)
            {
                npc.npc.noTileCollide = npc.npc.velocity.Y < 0 || !Collision.SolidCollision(npc.npc.Bottom - new Vector2(0, 8), npc.npc.width, 8);
                npc.npc.velocity.X = _xVel;
                npc.npc.spriteDirection = Math.Sign(npc.npc.velocity.X);

                if (npc.npc.velocity.Y > 0)
                    npc.npc.velocity.Y *= 1.05f;

                if (npc.npc.collideY && !(npc as TubbleBoss).OnPlatforms())
                {
                    npc.npc.ai[1] = 1;
                    npc.npc.velocity.X = 0;

                    ScreenShakePlayer.ShakeAction(ScreenShakePlayer.DistanceShake(npc.npc.Center, 30, 3.5f, 1000));
                }
            }
            else if (npc.npc.ai[1] > 0)
                npc.npc.ai[1]++;
        }

        public override bool ChooseNextAttack(BaseNPC npc) => npc.npc.ai[1] == 120;
        public override BaseAttack GetNextAttack(BaseNPC npc) => new HopAttack();

        public override void ResetNPC(BaseNPC npc)
        {
            npc.npc.ai[0] = 0;
            npc.npc.ai[1] = 0;
            npc.npc.dontTakeDamage = false;
        }
    }
}
