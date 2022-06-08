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
        private readonly Queue<int> _morsels = new Queue<int>();

        private Projectile Tongue => Main.projectile[_tongueIndex];
        private int _tongueIndex = 0;

        public override void AI(BaseNPC npc)
        {
            npc.npc.ai[0]++;

            if (npc.npc.ai[0] == 60) //Spawn NPCs if they haven't been spawned yet
            {
                if (_morsels.Count == 0)
                {
                    for (int i = 0; i < 3; ++i)
                    {
                        Vector2 pos = ModContent.GetInstance<TubbleWorld>().bedPosition - new Vector2(Main.rand.NextFloat(-450, 650), Main.rand.NextFloat(140, 550));
                        int newNPC = NPC.NewNPC((int)pos.X, (int)pos.Y, ModContent.NPCType<MorselFly>());
                        _morsels.Enqueue(newNPC);
                    }
                }
            }
            else if (npc.npc.ai[0] == 120) //Shoot tongue
            {
                _tongueIndex = Projectile.NewProjectile(npc.npc.Center, Vector2.Zero, ModContent.ProjectileType<TubbleTongue>(), 20, 1f);

                var tongue = Tongue.modProjectile as TubbleTongue;
                tongue.SpawnOrigin = npc.npc.Center;
                tongue.target = _morsels.Dequeue();
                Tongue.velocity = Tongue.DirectionTo(tongue.Target.Center) * TubbleTongue.ShootSpeed;
            }
            else if (npc.npc.ai[0] > 120)
            {
                if (!Tongue.active && _morsels.Count > 0)
                {
                    (npc as TubbleBoss).morselsEaten++;
                    npc.npc.ai[0] = 50;
                }
            }
        }

        public override bool ChooseNextAttack(BaseNPC npc) => _morsels.Count == 0 && !Tongue.active && npc.npc.ai[0] > 120;
        public override BaseAttack GetNextAttack(BaseNPC npc) => new HopAttack();

        public override void ResetNPC(BaseNPC npc)
        {
            (npc as TubbleBoss).morselsEaten++;
            npc.npc.ai[0] = 0;
        }
    }
}
