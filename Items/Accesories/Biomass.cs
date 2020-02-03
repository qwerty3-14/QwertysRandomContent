using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Accesories
{


    public class Biomass : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Biomass");
            Tooltip.SetDefault("Stacks of throwing weapons will slowly grow in size" + "\nGrowth rate is lower for higher value throwables");

        }

        public override void SetDefaults()
        {

            item.value = 10000;
            item.rare = 1;
            item.width = 24;
            item.height = 24;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {

            player.GetModPlayer<BiomassEffect>().effect += 1;
        }



    }
    public class BiomassEffect : ModPlayer
    {
        public float effect;
        public override void ResetEffects()
        {
            effect = 00;

        }
        public override void PreUpdate()
        {
            if (effect > 0)
            {
                Item item = new Item();
                List<int> possibleStacks = new List<int>();
                bool attemptGrowth = false;
                for (int i = 0; i < 58; i++)
                {
                    if (!player.inventory[i].IsAir)
                    {
                        if (player.inventory[i].thrown && player.inventory[i].stack < player.inventory[i].maxStack)
                        {

                            possibleStacks.Add(i);
                            attemptGrowth = true;


                        }
                    }
                }
                if (attemptGrowth)
                {
                    item = player.inventory[possibleStacks[Main.rand.Next(possibleStacks.Count)]];
                    int valueStat = 60;
                    if (item.value > valueStat)
                    {
                        valueStat = item.value;
                        valueStat = (int)(valueStat / effect);
                    }
                    if (Main.rand.Next(valueStat * 2) == 0)
                    {
                        item.stack++;
                    }
                }
                attemptGrowth = false;
            }
        }
    }
    public class BiomassGrowth : ModWorld
    {
        public override void PostWorldGen()
        {
            for (int c = 0; c < Main.chest.Length; c++)
            {
                if (Main.chest[c] != null)
                {
                    if (Main.chest[c].item[0].type == ItemID.LivingWoodWand || Main.chest[c].item[0].type == ItemID.LeafWand)
                    {
                        for (int i = 0; i < Main.chest[c].item.Length; i++)
                        {
                            if (Main.chest[c].item[i].type == ItemID.LivingLoom)
                            {
                                break;
                            }
                            if (Main.chest[c].item[i].type == 0)
                            {

                                Main.chest[c].item[i].SetDefaults(mod.ItemType("Biomass"), false);
                                break;
                            }
                        }
                    }
                }

            }
        }
    }

}

