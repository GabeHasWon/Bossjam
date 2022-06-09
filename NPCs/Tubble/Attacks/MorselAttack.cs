using Bossjam.NPCs.Attacks;
using Bossjam.NPCs.Tubble.Adds;
using Bossjam.NPCs.Tubble.Projectiles;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace Bossjam.NPCs.Tubble.Attacks
{
    public class MorselAttack : BaseAttack
    {
        private const int SpawnNPCTick = 50;
        private const int SpawnTongueTick = 90;

        private readonly Queue<int> _morsels = new Queue<int>();

        private Projectile Tongue => Main.projectile[_tongueIndex];
        private int _tongueIndex = 0;

        public override void AI(BaseNPC npc)
        {

            npc.npc.ai[0]++;

            if (npc.npc.ai[0] == SpawnNPCTick) //Spawn NPCs if they haven't been spawned yet
            {
                if (_morsels.Count == 0)
                {
                    for (int i = 0; i < 3; ++i)
                    {
                        Vector2 bed = ModContent.GetInstance<TubbleWorld>().bedPosition;
                        Vector2 pos = bed - new Vector2(Main.rand.NextFloat(-420, 620), Main.rand.NextFloat(140, 550));

                        bool abovePos = i == 2; //Alternate position
                        if (i == 1)
                        {
                            int initialDist = 500;

                            pos = npc.Target().Center + (npc.npc.DirectionTo(npc.Target().Center) * initialDist);
                            while (Collision.SolidCollision(pos - MorselFly.Size / 2f, (int)MorselFly.Size.X, (int)MorselFly.Size.Y) || pos.Y > bed.Y)
                            {
                                pos = npc.Target().Center + (npc.npc.DirectionTo(npc.Target().Center) * initialDist);

                                initialDist -= 50;

                                if (initialDist <= 0)
                                {
                                    abovePos = true;
                                    break;
                                }
                            }
                        }

                        if (abovePos)
                            pos = npc.Target().Center - new Vector2(0, Main.rand.NextFloat(80, 200));

                        int newNPC = NPC.NewNPC((int)pos.X, (int)pos.Y, ModContent.NPCType<MorselFly>());
                        _morsels.Enqueue(newNPC);
                    }
                }
            }
            else if (npc.npc.ai[0] == SpawnTongueTick) //Shoot tongue
            {
                _tongueIndex = Projectile.NewProjectile(npc.npc.Center, Vector2.Zero, ModContent.ProjectileType<TubbleTongue>(), 20, 1f);

                var tongue = Tongue.modProjectile as TubbleTongue;
                tongue.SpawnOrigin = npc.npc.Center;
                tongue.target = _morsels.Dequeue();

                while (!tongue.Target.active) //Check for missing flies
                {
                    if (_morsels.Count == 0)
                    {
                        Tongue.Kill();
                        return;
                    }
                    else
                        tongue.target = _morsels.Dequeue();
                }

                Tongue.velocity = Tongue.DirectionTo(tongue.Target.Center) * TubbleTongue.ShootSpeed;
            }
            else if (npc.npc.ai[0] > SpawnTongueTick)
            {
                npc.npc.spriteDirection = System.Math.Sign(Tongue.position.X - npc.npc.position.X);

                if (!Tongue.active && _morsels.Count > 0)
                {
                    (npc as TubbleBoss).morselsEaten++;
                    npc.npc.ai[0] = 50;
                }
            }
        }

        public override bool ChooseNextAttack(BaseNPC npc) => _morsels.Count == 0 && !Tongue.active && npc.npc.ai[0] > SpawnTongueTick;

        public override BaseAttack GetNextAttack(BaseNPC npc)
        {
            BaseAttack t = new HopAttack();
            if ((npc as TubbleBoss).morselsEaten <= 6)
                t = new SpewBubbleAttack();
            return t;
        }

        public override void ResetNPC(BaseNPC npc)
        {
            (npc as TubbleBoss).morselsEaten++;
            npc.npc.ai[0] = 0;
        }
    }
}
