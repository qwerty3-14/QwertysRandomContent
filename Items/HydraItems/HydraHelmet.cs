using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.HydraItems
{
    [AutoloadEquip(EquipType.Head)]
    public class HydraHelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hydra Helmet");
            Tooltip.SetDefault("+0.5 life/sec regen rate" + "\n+10% summon damage");
        }

        public override bool Autoload(ref string name)
        {
            // All code below runs only if we're not loading on a server
            if (!Main.dedServ)
            {
                // Add certain equip textures
                mod.AddEquipTexture(new HydraHelmetGlow(), null, EquipType.Head, "HydraHelmet_Glow", "QwertysRandomContent/Items/HydraItems/HydraHelmet_Glow");
            }
            return true;
        }

        public override void SetDefaults()
        {
            item.value = 50000;
            item.rare = 5;

            item.width = 28;
            item.height = 22;
            item.defense = 13;
        }

        public override void UpdateEquip(Player player)
        {
            player.lifeRegen += 2;
            player.minionDamage += .1f;
        }

        public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
        {
            drawAltHair = true;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("HydraScalemail") && legs.type == mod.ItemType("HydraLeggings");
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = Language.GetTextValue("Mods.QwertysRandomContent.HydraSet");
            if (((player.statLife * 1.0f) / (player.statLifeMax2 * 1.0f)) < .01f)
            {
                player.maxMinions += 20;
            }
            else if (((player.statLife * 1.0f) / (player.statLifeMax2 * 1.0f)) < .2f)
            {
                player.maxMinions += 4;
            }
            else if (((player.statLife * 1.0f) / (player.statLifeMax2 * 1.0f)) < .4f)
            {
                player.maxMinions += 3;
            }
            else if (((player.statLife * 1.0f) / (player.statLifeMax2 * 1.0f)) < .6f)
            {
                player.maxMinions += 2;
            }
            else if (((player.statLife * 1.0f) / (player.statLifeMax2 * 1.0f)) < .8f)
            {
                player.maxMinions += 1;
            }
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("HydraScale"), 12);

            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }

    public class HydraHelmetGlow : EquipTexture
    {
        /*
        public override void DrawArmorColor(Player  drawPlayer, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor)
		{
			glowMask = mod.GetEquipSlot("HydraHelmet_Glow", EquipType.Head);
		}
        */
    }

    public class HydraHelmetGlowmask : ModPlayer
    {
        public static readonly PlayerLayer HydraEye = LayerDrawing.DrawOnHead("HydraHelmet", "Items/HydraItems/HydraHelmet_Glow");

        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
            int headLayer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("Head"));
            if (headLayer != -1)
            {
                HydraEye.visible = true;
                layers.Insert(headLayer + 1, HydraEye);
            }
        }
    }
}