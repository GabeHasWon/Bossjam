using Terraria.ID;

namespace Bossjam
{
    public abstract class ModBoss : BaseNPC
    {
        public virtual int BossBag => ItemID.None;

        public override void SetDefaults()
        {
            npc.boss = true;

            bossBag = BossBag;

            base.SetDefaults();
        }
    }
}