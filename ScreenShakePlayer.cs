using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace Bossjam
{
    public class ScreenShakePlayer : ModPlayer
    {
        public float magnitude = 0f;
        public int time = 0;

        public override void ModifyScreenPosition()
        {
            if (magnitude > 0 && time > 0)
            {
                Main.screenPosition += new Vector2(magnitude, 0).RotatedByRandom(MathHelper.TwoPi);
                time--;

                if (time <= 0)
                    magnitude = 0;
            }
        }

        public static void ShakeAction(Func<Player, (int, float)> playerAction)
        {
            for (int i = 0; i < Main.maxPlayers; ++i)
            {
                Player p = Main.player[i];
                if (p.active)
                {
                    var modPlayer = p.GetModPlayer<ScreenShakePlayer>();
                    (modPlayer.time, modPlayer.magnitude) = playerAction.Invoke(p);
                }
            }
        }

        public static Func<Player, (int, float)> DistanceShake(Vector2 center, int time, float maxMagnitude, float maxDistance)
        {
            return (player) =>
            {
                float dist = player.Distance(center);
                float factor = 1 - (dist / maxDistance);
                if (dist > maxDistance)
                    return (0, 0);
                return ((int)(time * factor), maxMagnitude * factor);
            };
        }
    }
}
