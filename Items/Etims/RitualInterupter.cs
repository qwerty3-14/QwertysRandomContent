using QwertysRandomContent.Config;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Etims
{
    public class RitualInterupter : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Summons Noehtnap");
            DisplayName.SetDefault("Ritual Interrupter");
            ItemID.Sets.SortingPriorityBossSpawns[item.type] = 13; // This helps sort inventory know this is a boss summoning item.
        }
        public override string Texture => ModContent.GetInstance<SpriteSettings>().ClassicNoehtnap ? base.Texture + "_Old" : base.Texture;
        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 24;
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
            if (!NPC.AnyNPCs(mod.NPCType("CloakedDarkBoss")))
            {
                NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("CloakedDarkBoss"));
                Main.PlaySound(SoundID.Roar, player.position, 0);
                item.stack--;
                return true;
            }
            return false;
        }
        

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("CaeliteCore"), 2);
            recipe.AddIngredient(mod.ItemType("LuneBar"), 2);
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
