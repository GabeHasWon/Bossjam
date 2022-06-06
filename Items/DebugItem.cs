using Bossjam.NPCs.Tubble;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bossjam.Items
{
	public class DebugItem : ModItem
	{
		public override void SetStaticDefaults() 
		{
			Tooltip.SetDefault("This is a basic modded sword.");
		}

        public override void SetDefaults()
        {
            item.damage = 50;
            item.melee = true;
            item.width = 40;
            item.height = 40;
            item.useTime = 1;
            item.useAnimation = 1;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 6;
            item.value = 10000;
            item.rare = ItemRarityID.Green;
            item.UseSound = SoundID.Item1;
            item.autoReuse = false;
        }

        public override bool UseItem(Player player)
        {
            //ModContent.GetInstance<TubbleWorld>().bedPosition = Main.MouseWorld;
            //Dust.NewDustPerfect(Main.MouseWorld, DustID.Cloud);
            //Dust.NewDustPerfect(Main.MouseWorld + new Vector2(110, 90), DustID.Cloud);

            Vector2 bed = ModContent.GetInstance<TubbleWorld>().bedPosition;
            NPC.NewNPC((int)bed.X, (int)bed.Y, ModContent.NPCType<TubbleBoss>());

            //Point p = Main.MouseWorld.ToTileCoordinates();
            //Tile t = Framing.GetTileSafely(p.X, p.Y);
            //if (t.type == TileID.Dirt)
            //    t.type = TileID.LivingWood;
            return true;
        }
    }
}