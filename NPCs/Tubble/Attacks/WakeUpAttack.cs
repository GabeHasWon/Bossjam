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
            TubbleBoss tubble = npc as TubbleBoss;

            if (timer == 60)
            {
                npc.npc.velocity.Y = -20;
                npc.npc.noTileCollide = true;
                _xVel = Main.rand.NextFloat(-1f, 1f);
            }
            else if (timer > 60 && npc.npc.ai[1] == 0)
            {
                npc.npc.noTileCollide = npc.npc.velocity.Y < 0 || !tubble.OnGround();
                npc.npc.velocity.X = _xVel;
                npc.npc.spriteDirection = Math.Sign(npc.npc.velocity.X);

                if (npc.npc.velocity.Y < 2)
                    tubble.SetFrame(4);
                else
                    tubble.SetFrame(3);

                if (npc.npc.velocity.Y > 0)
                    npc.npc.velocity.Y *= 1.05f;

                if (!npc.npc.noTileCollide && tubble.OnGround())
                {
                    npc.npc.ai[1] = 1;
                    npc.npc.velocity.X = 0;

                    (float left, float right) = tubble.GetAdjustedSides();
                    for (int i = 0; i < 8; ++i)
                    {
                        float x = MathHelper.Lerp(left, right, i / 8f);

                        Vector2 vel = new Vector2(Main.rand.NextFloat(-0.2f, 0.2f), Main.rand.NextFloat(-2f, 0));
                        Gore.NewGore(new Vector2(x, npc.npc.Bottom.Y), vel, Main.rand.Next(11, 14), Main.rand.NextFloat(0.2f, 0.5f));
                    }

                    Main.PlaySound(Terraria.ID.SoundID.DD2_OgreGroundPound, npc.npc.Bottom);
                    ScreenShakePlayer.ShakeAction(ScreenShakePlayer.DistanceShake(npc.npc.Center, 30, 3.5f, 1000));
                }
            }
            else if (npc.npc.ai[1] > 0)
            {
                npc.npc.ai[1]++;
                tubble.SetFrame(0);
            }
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
