using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.Dungeon 
{
	public class BurstMiner : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Burst Miner");
			Tooltip.SetDefault("Mines quickly with a cooldown");
			
		}
		public override void SetDefaults()
		{
			item.damage = 11;
			item.melee = true;


            item.useTime = 4;
            item.useAnimation = 4;
           
			item.useStyle = 1;
			item.knockBack = 3;
            item.value = Item.sellPrice(silver: 54);
            item.rare = 2;
			item.UseSound = SoundID.Item1;
			
			item.width = 30;
			item.height = 30;
            item.noMelee = true;
            item.autoReuse = true;
			item.pick = 95;
			
			
			
			
			
		}
        int useCounter;
        const int maxUses = 20;
        const int delay = 60;
        public override bool CanUseItem(Player player)
        {
            if (player.selectedItem == 58)
            {
                return false;
            }
            if (useCounter >= maxUses)
            {
                return false;
            }
            useCounter++;

            return true;
        }
        public override void UpdateInventory(Player player)
        {
            if(useCounter>= maxUses)
            {
                useCounter++;
                if(useCounter>= maxUses + delay)
                {
                    useCounter = 0;
                    Main.PlaySound(25, -1, -1, 1, 1f, 0f);
                    for (int num71 = 0; num71 < 5; num71++)
                    {
                        int num72 = Dust.NewDust(player.position, player.width, player.height, 45, 0f, 0f, 255, default(Color), (float)Main.rand.Next(20, 26) * 0.1f);
                        Main.dust[num72].noLight = true;
                        Main.dust[num72].noGravity = true;
                        Main.dust[num72].velocity *= 0.5f;
                    }
                }
            }
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            int num292 = Dust.NewDust(new Vector2((float)hitbox.X, (float)hitbox.Y), hitbox.Width, hitbox.Height, 172, player.velocity.X * 0.2f + (float)(player.direction * 3), player.velocity.Y * 0.2f, 100, default(Color), 0.9f);
            Main.dust[num292].noGravity = true;
            Main.dust[num292].velocity *= 0.1f;
        }

    }
		
	
}

