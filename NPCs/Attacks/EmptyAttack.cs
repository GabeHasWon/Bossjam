using Terraria.ModLoader;

namespace Bossjam.NPCs.Attacks
{
    class EmptyAttack : BaseAttack
    {
        public override void AI(BaseNPC npc) { }
        public override bool ChooseNextAttack(BaseNPC npc) => false;
        public override BaseAttack GetNextAttack(BaseNPC npc) => null;
    }
}
