using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.MiscBows
{
	public class TrueBloomingBow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("True Blooming Bow");
			Tooltip.SetDefault("Randomly picks 1-16 randomly random arrows to fire in randomly random directions at randomly random velocities randomly!");
			
		}
		public override void SetDefaults()
		{
			item.damage = 27;
			item.ranged = true;
			
			item.useTime = 50;
			item.useAnimation = 45;
			item.useStyle = 5;
			item.knockBack = 5;
            item.value = 496036;
            item.rare = 6;
			item.UseSound = SoundID.Item5;
			
			item.width = 32;
			item.height = 74;
			
			item.shoot = 40;
			item.useAmmo = 40;
			item.shootSpeed =4;
			item.noMelee=true;
            item.autoReuse = true;
            

        }
        public override bool ConsumeAmmo(Player player)
        {
            return false;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(mod.ItemType("BloomingBow"));
            recipe.AddIngredient(mod.ItemType("WornPrehistoricBow"));
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        //Thanks Mirsario for this chunk of code
        private static Dictionary<int, Item> vanillaItemCache = new Dictionary<int, Item>();
        public static Item GetReference(int type)
        {
            if (type <= 0)
            {
                return null;
            }
            if (type >= ItemID.Count)
            {
                return ItemLoader.GetItem(type).item;
            }
            else
            {
                Item item;
                if (!vanillaItemCache.TryGetValue(type, out item))
                {
                    item = new Item();
                    item.SetDefaults(type, true);
                    vanillaItemCache[type] = item;
                }
                return item;
            }
        }
        /*------------------------------------------------- */
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {

            

            int numberProjectiles = 1+ Main.rand.Next(16); 
            for (int i = 0; i < numberProjectiles; i++)
            {
                QwertyPlayer modPlayer = player.GetModPlayer<QwertyPlayer>(mod);
                Vector2 trueSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(20));
                float scale = Main.rand.NextFloat(.7f, 2.3f);
                trueSpeed = trueSpeed * scale;
                bool yes = true;
                float anotherSpeedVariable = trueSpeed.Length();
                
                modPlayer.PickRandomAmmo(GetReference(39), ref type, ref anotherSpeedVariable, ref yes, ref damage, ref knockBack, Main.rand.Next(2)==0);
                Projectile.NewProjectile(position.X + Main.rand.Next(-18,18), position.Y + Main.rand.Next(-18, 18), trueSpeed.X, trueSpeed.Y, type, damage, knockBack, player.whoAmI);
            }
            return false;
        }


    }
		
	
}

