using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.HydraItems
{
	
	
	public class HydraCharm : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hydra Charm");
			Tooltip.SetDefault("Allows most minions to summon more minions to fill empty minion slots");
			
		}
		
		public override void SetDefaults()
		{
			
			item.value = 10000;
			item.rare = 5;
			
			
			item.width = 14;
			item.height = 22;
			
			item.accessory = true;
			
			
			
		}
		
		public override void UpdateEquip(Player player)
		{
            var modPlayer = player.GetModPlayer<QwertyPlayer>(mod);
            modPlayer.hydraCharm = true;

        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("HydraScale"), 6);

            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

    }
    public class minionDuplication : GlobalProjectile
    {
        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }

        public int wait;
        public override void AI(Projectile projectile)
        {
            Player player = Main.player[projectile.owner];
            QwertyPlayer modPlayer = player.GetModPlayer<QwertyPlayer>(mod);
            if (player.maxMinions - player.numMinions >= projectile.minionSlots && Main.netMode != 2 && projectile.minionSlots > 0 && projectile.active && modPlayer.hydraCharm)
            {
                if (wait >= 20 && projectile.active)
                {
                    Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, -4, projectile.type, projectile.damage, projectile.knockBack, Main.myPlayer, 0f, 0f);
                    wait = 0;
                }
                else
                {
                    wait++;
                }
            }
            else
            {
                wait = 0;
            }
        }

    }


}

