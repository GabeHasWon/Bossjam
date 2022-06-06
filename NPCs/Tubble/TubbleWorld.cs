using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Bossjam.NPCs.Tubble
{
    public class TubbleWorld : ModWorld
    {
        public Vector2 bedPosition = Vector2.Zero;

        public override TagCompound Save()
        {
            return new TagCompound()
            {
                { "bed", bedPosition }
            };
        }

        public override void Load(TagCompound tag) => bedPosition = tag.Get<Vector2>("bed");
    }
}
