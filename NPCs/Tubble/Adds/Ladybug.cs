using Bossjam.NPCs.Attacks;
using Terraria;
using Terraria.ID;

namespace Bossjam.NPCs.Tubble.Adds
{
    public class Ladybug : BaseNPC
    {
        public override BaseAttack BaseAttack => new EmptyAttack();

        internal bool falling = false;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ladybug");

            Main.npcFrameCount[npc.type] = 6;
        }

        public override void Defaults()
        {
            npc.CloneDefaults(NPCID.MushiLadybug);
            npc.damage = 30;
            npc.defense = 0;
            npc.lifeMax = 75;
            npc.friendly = false;

            animationType = NPCID.MushiLadybug;
            aiType = NPCID.MushiLadybug;
        }

        public override bool PreAI()
        {
            if (Collision.SolidCollision(npc.position, npc.width, npc.height + 4))
                falling = false;

            if (falling)
                npc.rotation += 0.2f;
            else
                npc.rotation *= 0.98f;

            return !falling || System.Math.Abs(npc.rotation) < 0.5f;
        }

        public override void OnInit() => melee = true;
    }
}
