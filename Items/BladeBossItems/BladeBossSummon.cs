using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.BladeBossItems
{

    public class BladeBossSummon : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Icon of the Conqueror");
            Tooltip.SetDefault("Summons Imperious");
            ItemID.Sets.SortingPriorityBossSpawns[item.type] = 13; // This helps sort inventory know this is a boss summoning item.
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 22;
            item.maxStack = 20;
            item.rare = 3;
            item.useAnimation = 45;
            item.useTime = 45;
            item.useStyle = 4;
            item.UseSound = SoundID.Item44;
            item.consumable = true;
        }


        public override bool CanUseItem(Player player)
        {

            return !NPC.AnyNPCs(mod.NPCType("BladeBoss"));
        }

        public override bool UseItem(Player player)
        {
            //NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("BladeBoss"));
            if (item.owner == Main.myPlayer)
            {
                QwertyMethods.SpawnBoss(player, mod.NPCType("BladeBoss"));
            }
            Main.PlaySound(SoundID.Roar, player.position, 0);
            return true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("RhuthiniumBar"), 8);
            recipe.AddIngredient(ItemID.SoulofMight, 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }


    }
}