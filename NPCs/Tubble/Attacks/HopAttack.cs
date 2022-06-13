using Bossjam.NPCs.Attacks;
using Bossjam.NPCs.Tubble.Adds;
using Bossjam.NPCs.Tubble.Projectiles;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace Bossjam.NPCs.Tubble.Attacks
{
    class HopAttack : BaseAttack
    {
        public override void AI(BaseNPC npc)
        {
            npc.npc.ai[0]++;

            float timer = npc.npc.ai[0];
            TubbleBoss tubble = npc as TubbleBoss;

            if (timer == 60)
            {
                npc.npc.TargetClosest(false);
                npc.npc.noTileCollide = true;

                if (tubble.lastStunDir == 0)
                {
                    if (npc.npc.DistanceSQ(npc.Target().Center) < 350 * 350)
                        npc.npc.velocity.X = Main.rand.NextFloat(4.5f, 6f) * Math.Sign(npc.Target().Center.X - npc.npc.Center.X);
                    else
                        npc.npc.velocity.X = Main.rand.NextFloat(4.5f, 6f) * Main.rand.Next(new int[] { -1, 1 });
                }
                else
                    npc.npc.velocity.X = Main.rand.NextFloat(4.5f, 6f) * -tubble.lastStunDir;

                npc.npc.velocity.Y = -10;
            }
            else if (timer > 60 && npc.npc.ai[1] == 0)
            {
                npc.npc.noTileCollide = npc.npc.velocity.Y < 0 || !tubble.OnGround();
                npc.npc.spriteDirection = Math.Sign(npc.npc.velocity.X);
                npc.melee = npc.npc.velocity.Y > 0;

                if (npc.npc.velocity.Y < -2f)
                    tubble.SetFrame(1);
                else if (npc.npc.velocity.Y < 2)
                    tubble.SetFrame(0);
                else
                    tubble.SetFrame(3);

                if (!npc.npc.noTileCollide && tubble.OnGround()) //Land
                {
                    npc.npc.ai[1] = 1;
                    npc.npc.velocity.X = 0;

                    tubble.lastStunDir = 0;

                    (float left, float right) = tubble.GetAdjustedSides();
                    for (int i = 0; i < 8; ++i)
                    {
                        float x = MathHelper.Lerp(left, right, i / 8f);

                        Vector2 vel = new Vector2(Main.rand.NextFloat(-0.2f, 0.2f), Main.rand.NextFloat(-2f, 0));
                        Gore.NewGore(new Vector2(x, npc.npc.Bottom.Y), vel, Main.rand.Next(11, 14), Main.rand.NextFloat(0.2f, 0.5f));
                    }

                    Main.PlaySound(SoundID.DD2_OgreGroundPound, npc.npc.Bottom);
                    Collision.HitTiles(new Vector2(left, npc.npc.Bottom.Y), new Vector2(0, 2), npc.npc.width - 66, 10);
                    ScreenShakePlayer.ShakeAction(ScreenShakePlayer.DistanceShake(npc.npc.Center, 30, 3, 1000));
                }

                void SetStunnedValues(sbyte dir)
                {
                    npc.npc.ai[1] = -1;
                    npc.npc.ai[2] = 1;
                    npc.npc.velocity.X = -3 * dir;
                    npc.npc.noTileCollide = false;
                    npc.npc.StrikeNPCNoInteraction(Main.rand.Next(3, 8), 0f, -1, false, false);
                    npc.melee = false;

                    tubble.lastStunDir = dir;

                    SpawnDebris();
                    ScreenShakePlayer.ShakeAction(ScreenShakePlayer.DistanceShake(npc.npc.Center, 30, 3, 1000));
                }

                if (npc.npc.velocity.X < 0 && Collision.SolidCollision(npc.npc.position + new Vector2(36, 0), 8, npc.npc.height / 2)) //Left wall collision
                    SetStunnedValues(-1);
                if (npc.npc.velocity.X > 0 && Collision.SolidCollision(npc.npc.TopRight - new Vector2(28, 0), 8, npc.npc.height / 2)) //Right wall collision
                    SetStunnedValues(1);
            }
            else if (npc.npc.ai[1] > 0) //Landed pause
            {
                tubble.SetFrame(0);
                npc.npc.ai[1]++;
                npc.melee = false;
            }
            else if (npc.npc.ai[2] > 0) //Stunned pause
            {
                tubble.SetFrame(0);
                npc.npc.noTileCollide = !tubble.OnGround();

                if (npc.npc.collideY)
                    npc.npc.velocity.X *= 0.75f;
                else
                    npc.npc.velocity.Y += 0.2f;

                npc.npc.ai[2]++;

                if (npc.npc.ai[2] > 120)
                    npc.npc.ai[1] = 1;
            }
        }

        private void SpawnDebris()
        {
            Vector2 bed = ModContent.GetInstance<TubbleWorld>().bedPosition;
            Vector2 ceiling = bed - new Vector2(0, 1400);

            Vector2 RandomCeiling() => ceiling - new Vector2(Main.rand.NextFloat(-400, 500), Main.rand.NextFloat(-100, 200));

            for (int i = 0; i < 18; ++i)
                Gore.NewGore(RandomCeiling(), Vector2.Zero, GoreID.TreeLeaf_Jungle);

            for (int i = 0; i < 5; ++i)
                Projectile.NewProjectile(RandomCeiling(), Vector2.UnitY * Main.rand.NextFloat(0, 3f), ModContent.ProjectileType<SkullDrop>(), 0, 0);

            for (int i = 0; i < 3; ++i)
            {
                Vector2 p = RandomCeiling();
                int npc = NPC.NewNPC((int)p.X, (int)p.Y, ModContent.NPCType<Ladybug>());

                (Main.npc[npc].modNPC as Ladybug).falling = true;
            }
        }

        public override bool ChooseNextAttack(BaseNPC npc) => npc.npc.ai[1] == 60;

        public override BaseAttack GetNextAttack(BaseNPC npc)
        {
            WeightedRandom<BaseAttack> nextAttack = new WeightedRandom<BaseAttack>();

            if (npc.npc.ai[2] < 1) //Hop only if not stunned
                nextAttack.Add(new HopAttack());
            nextAttack.Add(new MorselAttack(), 0.85f);

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
