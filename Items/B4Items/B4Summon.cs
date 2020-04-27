using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.B4Items
{

    public class B4Summon : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("O.L.O.R.D. summon");
            Tooltip.SetDefault("Summons the Oversized Laser-emitting Obliteration Radiation-emitting Destroyer");
            Item.staff[item.type] = true;
            ItemID.Sets.SortingPriorityBossSpawns[item.type] = 13; // This helps sort inventory know this is a boss summoning item.
        }

        public override void SetDefaults()
        {
            item.width = 78;
            item.height = 78;
            item.maxStack = 20;
            item.rare = 3;
            item.useAnimation = 45;
            item.useTime = 45;
            item.useStyle = 5;
            item.UseSound = SoundID.Item44;
            item.consumable = true;
        }


        public override bool CanUseItem(Player player)
        {
            if (!NPC.AnyNPCs(mod.NPCType("OLORDv2")))
            {
                NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("OLORDv2"));
                Main.PlaySound(SoundID.Roar, player.position, 0);
                return true;
            }
            return false;
        }


        
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.MartianConduitPlating, 20);
            recipe.AddIngredient(mod.ItemType("CraftingRune"), 4);

            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }


    }
}