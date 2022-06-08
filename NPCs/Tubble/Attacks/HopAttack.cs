using Bossjam.NPCs.Attacks;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Utilities;

namespace Bossjam.NPCs.Tubble.Attacks
{
    class HopAttack : BaseAttack
    {
        public override void AI(BaseNPC npc)
        {
            npc.npc.ai[0]++;

            float timer = npc.npc.ai[0];

            if (timer == 60)
            {
                npc.npc.TargetClosest(false);
                npc.npc.noTileCollide = true;

                if ((npc as TubbleBoss).lastStunDir == 0)
                {
                    if (npc.npc.DistanceSQ(npc.Target().Center) < 350 * 350)
                        npc.npc.velocity.X = Main.rand.NextFloat(4.5f, 6f) * Math.Sign(npc.Target().Center.X - npc.npc.Center.X);
                    else
                        npc.npc.velocity.X = Main.rand.NextFloat(4.5f, 6f) * Main.rand.Next(new int[] { -1, 1 });
                }
                else
                    npc.npc.velocity.X = Main.rand.NextFloat(4.5f, 6f) * -(npc as TubbleBoss).lastStunDir;

                npc.npc.velocity.Y = -10;
                npc.melee = true;
            }
            else if (timer > 60 && npc.npc.ai[1] == 0)
            {
                npc.npc.noTileCollide = npc.npc.velocity.Y < 0 || !Collision.SolidCollision(npc.npc.Bottom - new Vector2(0, 8), npc.npc.width, 8);
                npc.npc.spriteDirection = Math.Sign(npc.npc.velocity.X);

                if (!npc.npc.noTileCollide && npc.npc.collideY && !(npc as TubbleBoss).OnPlatforms()) //Land
                {
                    npc.npc.ai[1] = 1;
                    npc.npc.velocity.X = 0;

                    (npc as TubbleBoss).lastStunDir = 0;

                    ScreenShakePlayer.ShakeAction(ScreenShakePlayer.DistanceShake(npc.npc.Center, 30, 3, 1000));
                }

                void SetStunnedValues(sbyte dir)
                {
                    npc.npc.ai[1] = -1;
                    npc.npc.ai[2] = 1;
                    npc.npc.velocity.X = -2;
                    npc.npc.noTileCollide = false;
                    npc.npc.StrikeNPCNoInteraction(Main.rand.Next(3, 8), 0f, -1, false, false);
                    npc.melee = false;

                    (npc as TubbleBoss).lastStunDir = dir;

                    ScreenShakePlayer.ShakeAction(ScreenShakePlayer.DistanceShake(npc.npc.Center, 30, 3, 1000));
                }

                if (npc.npc.velocity.X > 0 && Collision.SolidCollision(npc.npc.TopRight - new Vector2(42, 0), 4, npc.npc.height / 2)) //Right wall collision
                    SetStunnedValues(1);
                if (npc.npc.velocity.X < 0 && Collision.SolidCollision(npc.npc.position + new Vector2(42), 4, npc.npc.height / 2)) //Right wall collision
                    SetStunnedValues(-1);
            }
            else if (npc.npc.ai[1] > 0) //Landed pause
                npc.npc.ai[1]++;
            else if (npc.npc.ai[2] > 0) //Stunned pause
            {
                if (npc.npc.collideY)
                    npc.npc.velocity.X *= 0.75f;

                npc.npc.ai[2]++;

                if (npc.npc.ai[2] > 120)
                    npc.npc.ai[1] = 1;
            }
        }

        public override bool ChooseNextAttack(BaseNPC npc) => npc.npc.ai[1] == 60;

        public override BaseAttack GetNextAttack(BaseNPC npc)
        {
            WeightedRandom<BaseAttack> nextAttack = new WeightedRandom<BaseAttack>();

            if (npc.npc.ai[2] < 1) //Hop only if not stunned
                nextAttack.Add(new HopAttack());
            nextAttack.Add(new MorselAttack(), 666666660.5f);

            return nextAttack;
        }

        public override void ResetNPC(BaseNPC npc)
        {
            npc.npc.ai[0] = 0;
            npc.npc.ai[1] = 0;
            npc.npc.ai[2] = 0;
            npc.melee = false;
        }
    }
}
