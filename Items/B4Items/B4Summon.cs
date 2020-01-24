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

            return !NPC.AnyNPCs(mod.NPCType("BossFour"));
        }


        public override bool UseItem(Player player)
        {
            //NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("BossFour"));
            NPC npc = Main.npc[NPC.NewNPC((int)(player.position.X), (int)(player.position.Y - 3000), mod.NPCType("OLORDv2"))];

            //NPC.NewNPC((int)(npc.position.X), (int)(npc.position.Y), mod.NPCType("Turret"), 0, 0, 0, npc.whoAmI, 1);
            //NPC.NewNPC((int)(npc.position.X), (int)(npc.position.Y), mod.NPCType("Turret"), 0, 0, 0, npc.whoAmI, 2);
            //NPC.NewNPC((int)(npc.position.X), (int)(npc.position.Y), mod.NPCType("Turret"), 0, 0, 0, npc.whoAmI, 3);
            // NPC.NewNPC((int)(npc.position.X), (int)(npc.position.Y), mod.NPCType("Turret"), 0, 0, 0, npc.whoAmI, 4);


            // NPC.NewNPC((int)(npc.position.X), (int)(npc.position.Y), mod.NPCType("WeakPoint"), 0, 0, npc.whoAmI, 1);

            // NPC.NewNPC((int)(npc.position.X), (int)(npc.position.Y), mod.NPCType("BackGround"), 0, 0, 0, npc.whoAmI, 1);

            Main.PlaySound(SoundID.Roar, player.position, 0);
            return true;
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