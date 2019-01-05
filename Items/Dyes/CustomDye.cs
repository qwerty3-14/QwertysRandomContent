using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Dyes
{
	public class CustomDye : ModItem
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Custom Dye: Shirt");
            Tooltip.SetDefault("Based on your default clothes' shirt color");
        }
        public override void SetDefaults()
		{
			
			byte dye = item.dye;
			item.CloneDefaults(ItemID.GelDye);
			item.dye = dye;
            item.value = 150000;
		}
	}
    public class CustomArmorShader : ArmorShaderData
    {

        private static bool isInitialized = false;

        
        private static ArmorShaderData dustShaderData;
        public CustomArmorShader(Ref<Effect> shader, string passName) : base(shader, passName)
        {
            
            dustShaderData = new ArmorShaderData(shader, passName);
        }
        /*
        public override void Apply(Entity entity, DrawData? drawData)
        {
            
            Player player = entity as Player;
            
            dustShaderData.UseColor(player.shirtColor).Apply(player, drawData);

           

        }
        */
        public override void Apply(Entity entity, DrawData? drawData)
        {
            Player player = entity as Player;
            if (player == null)
            {
                dustShaderData.UseColor(player.shirtColor).UseSaturation(3f).Apply(player, drawData);
                return;
            }
            UseColor(player.shirtColor);
            UseSaturation(3f);
            base.Apply(player, drawData);
        }

        public override ArmorShaderData GetSecondaryShader(Entity entity)
        {
            Player player = entity as Player;
            return dustShaderData.UseColor(player.shirtColor).UseSaturation(3f);
        }




    }
}
