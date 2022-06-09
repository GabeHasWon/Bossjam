using Bossjam.NPCs.Attacks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace Bossjam.NPCs.Tubble.Adds
{
    class MorselFly : BaseNPC
    {
        public static readonly Vector2 Size = new Vector2(46, 42);

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
            npc.width = (int)Size.X;
            npc.height = (int)Size.Y;
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
