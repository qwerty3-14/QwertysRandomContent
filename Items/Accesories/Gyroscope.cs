using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Accesories
{
	public class Gyroscope : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gyroscope");
			Tooltip.SetDefault("Tops spin for longer");
		}

		public override void SetDefaults()
		{
			item.value = 10000;
			item.rare = 1;
			item.width = 38;
			item.height = 34;
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<QwertyPlayer>().TopFrictionMultiplier -= .3f;
		}
	}

	internal class GyroLoot : ModWorld
	{
		public override void PostWorldGen()
		{
			for (int c = 0; c < Main.chest.Length; c++)
			{
				if (Main.chest[c] != null)
				{
					if (Main.chest[c].item[0].type == ItemID.CloudinaBottle || Main.chest[c].item[0].type == ItemID.HermesBoots || Main.chest[c].item[0].type == ItemID.LavaCharm || Main.chest[c].item[0].type == ItemID.FlareGun || Main.chest[c].item[0].type == ItemID.EnchantedBoomerang || Main.chest[c].item[0].type == ItemID.Extractinator)
					{
						if (Main.rand.Next(5) == 0)
						{
							for (int i = 0; i < Main.chest[c].item.Length; i++)
							{
								if (Main.chest[c].item[i].IsAir)
								{
									Main.chest[c].item[i].SetDefaults(QwertysRandomContent.Instance.ItemType("Gyroscope"), false);
									break;
								}
							}
						}
					}
				}
			}
		}
	}
}