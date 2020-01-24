using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.ShapeShifter
{
    public class MorphSickness : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Morph Sickness");
            Description.SetDefault("You're still adjusting to you new body!");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = true;
            longerExpertDebuff = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            for (int i = 0; i < 1 + (player.Size.Length() / 46.5f); i++)
            {
                Dust dust = Main.dust[Dust.NewDust(player.position, player.width, player.height, 54)];
                dust.noGravity = true;
            }

        }

    }

}