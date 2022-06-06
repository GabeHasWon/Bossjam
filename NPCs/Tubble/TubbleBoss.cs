using Bossjam.NPCs.Attacks;
using Bossjam.NPCs.Tubble.Attacks;
using Terraria;
using Terraria.ID;

namespace Bossjam.NPCs.Tubble
{
    class TubbleBoss : ModBoss
    {
        public override BaseAttack BaseAttack => new SleepingAttack();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tubble");
        }

        public override void Defaults()
        {
            npc.lifeMax = 9000;
            npc.damage = 40;
            npc.defense = 15;
            npc.width = 110;
            npc.height = 90;
        }

        public override void OnHit(int hitDirection, double damage)
        {
            Dust.NewDust(npc.position, npc.width, npc.height, DustID.Blood, hitDirection);
        }
    }
}
