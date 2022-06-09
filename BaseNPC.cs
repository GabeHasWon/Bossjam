using Bossjam.NPCs.Attacks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Bossjam
{
    public abstract class BaseNPC : ModNPC
    {
        public abstract BaseAttack BaseAttack { get; }

        public BaseAttack CurrentAttack = new EmptyAttack();

        private bool _init = false;

        internal bool melee = false;
        internal int frame = 0;

        public override void SetDefaults()
        {
            npc.damage = 1;

            Defaults();
        }

        public virtual void Defaults() { }

        public sealed override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            if (CanHitPlayerExtra())
                return true;
            return melee;
        }

        public virtual bool CanHitPlayerExtra() => false;

        public override void AI()
        {
            if (!_init)
            {
                CurrentAttack = BaseAttack;

                OnInit();
                _init = true;
            }
            else
            {
                if (CurrentAttack.ChooseNextAttack(this))
                {
                    string oldAttackName = CurrentAttack.GetType().Name;

                    CurrentAttack.ResetNPC(this);
                    CurrentAttack = CurrentAttack.GetNextAttack(this);

                    if (CurrentAttack is null)
                    {
                        mod.Logger.Debug($"{npc.TypeName} gave null error after passing from {oldAttackName}\nWhat why");
                    }
                    return;
                }

                CurrentAttack.AI(this);
            }
        }

        public virtual void OnInit() { }

        public sealed override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
                if (!CurrentAttack.PreDeathEffect(this, hitDirection, damage))
                    return;

            if (!CurrentAttack.PreHitEffect(this, hitDirection, damage))
                return;

            OnHit(hitDirection, damage);
        }

        public virtual void OnHit(int hitDirection, double damage) { }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor) => CurrentAttack.PreDrawNPC(this, drawColor);

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor) => CurrentAttack.PostDrawNPC(this, drawColor);

        public Color LightingAt() => Lighting.GetColor((int)(npc.Center.X / 16f), (int)(npc.Center.Y / 16f));
        public Player Target() => Main.player[npc.target];
    }
}