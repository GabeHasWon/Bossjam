using Bossjam.NPCs.Tubble;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bossjam.Items
{
	public class TubbleArenaSpawner : ModItem
	{
		public override void SetStaticDefaults() 
		{
			Tooltip.SetDefault("Spawns the Tubble Arena + the boss.");
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
            StructureHelper.Generator.GenerateStructure("NPCs/Tubble/Structure/Arena", Main.MouseWorld.ToTileCoordinates16(), mod);
            ModContent.GetInstance<TubbleWorld>().bedPosition = Main.MouseWorld + new Vector2(63, 144).ToWorldCoordinates();

            Vector2 bed = ModContent.GetInstance<TubbleWorld>().bedPosition;
            NPC.NewNPC((int)bed.X, (int)bed.Y, ModContent.NPCType<TubbleBoss>());
            return true;
        }
    }
}