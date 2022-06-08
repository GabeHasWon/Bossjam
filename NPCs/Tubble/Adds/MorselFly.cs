using Bossjam.NPCs.Attacks;
using Terraria;
using Terraria.ID;

namespace Bossjam.NPCs.Tubble.Adds
{
    class MorselFly : BaseNPC
    {
        public override BaseAttack BaseAttack => new EmptyAttack();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Morsel Fly");
        }

        public override void Defaults()
        {
            npc.lifeMax = 50;
            npc.damage = 0;
            npc.defense = 0;
            npc.width = 46;
            npc.height = 42;
            npc.aiStyle = -1;
            npc.noGravity = true;
            npc.knockBackResist = 0f;
        }

        public override void OnHit(int hitDirection, double damage)
        {
            for (int i = 0; i < 6; ++i)
                Dust.NewDust(npc.position, npc.width, npc.height, DustID.Blood, hitDirection);
        }
    }
}
