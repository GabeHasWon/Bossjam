using Bossjam.NPCs.Attacks;
using Bossjam.NPCs.Tubble.Attacks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace Bossjam.NPCs.Tubble
{
    class TubbleBoss : ModBoss
    {
        public override BaseAttack BaseAttack => new SleepingAttack();

        internal sbyte lastStunDir = 0;
        internal sbyte morselsEaten = 0;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tubble");
        }

        public override void Defaults()
        {
            npc.lifeMax = 500;
            npc.damage = 40;
            npc.defense = 15;
            npc.width = 220;
            npc.height = 180;
            npc.aiStyle = -1;
            npc.knockBackResist = 0f;
            npc.dontTakeDamage = true;
        }

        public override void OnHit(int hitDirection, double damage)
        {
            for (int i = 0; i < 6; ++i)
                Dust.NewDust(npc.position, npc.width, npc.height, DustID.Blood, hitDirection);
        }

        public bool OnGround()
        {
            if (npc.spriteDirection <= 0)
                return Collision.SolidCollision(new Vector2(npc.position.X, npc.Bottom.Y) + new Vector2(66, -8), npc.width - 66, 10);
            return Collision.SolidCollision(new Vector2(npc.position.X, npc.Bottom.Y) + new Vector2(0, -8), npc.width - 66, 10);
        }
    }
}
