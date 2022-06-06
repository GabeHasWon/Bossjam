using Microsoft.Xna.Framework;

namespace Bossjam.NPCs.Attacks
{
    public abstract class BaseAttack
    {
        public abstract void AI(BaseNPC npc);
        public abstract bool ChooseNextAttack();
        public abstract BaseAttack GetNextAttack();

        /// <summary>Return false to stop the npc's HitEffect from running. Runs after PreDeathEffect.</summary>
        public virtual bool PreHitEffect(BaseNPC npc, int hitDirection, double damage) => true;

        /// <summary>Return false to stop the npc's DeathEffect (HitEffect where npc.life is less than 0) from running. Runs before PreHitEffect.</summary>
        public virtual bool PreDeathEffect(BaseNPC npc, int hitDirection, double damage) => true;

        public virtual bool PreDrawNPC(BaseNPC npc, Color drawColor) => true;
        public virtual void PostDrawNPC(BaseNPC npc, Color drawColor) { }
    }
}
