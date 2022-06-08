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

        public bool OnPlatforms()
        {
            Point pos = new Vector2(npc.position.X, npc.Bottom.Y).ToTileCoordinates();
            for (int i = 0; i < npc.width / 16f; ++i)
            {
                Tile t = Framing.GetTileSafely(pos.X + i, pos.Y + 1);
                if (t.active() && Main.tileSolid[t.type] && !TileID.Sets.Platforms[t.type])
                {
                    Dust.NewDust(pos.ToWorldCoordinates() + new Vector2(i * 16, 16), 1, 1, DustID.Cloud);
                    return false;
                }
            }
            return true;
        }
    }
}
