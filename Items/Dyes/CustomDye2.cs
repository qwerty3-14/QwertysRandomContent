using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Dyes
{
	public class CustomDye2 : ModItem
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Custom Dye: Underhirt");
            Tooltip.SetDefault("Based on your default clothes' undershirt color");
        }
        public override void SetDefaults()
		{
			
			byte dye = item.dye;
			item.CloneDefaults(ItemID.GelDye);
			item.dye = dye;
            item.value = 150000;
        }
	}
    public class CustomArmorShader2 : ArmorShaderData
    {

        private static bool isInitialized = false;

        
        private static ArmorShaderData dustShaderData;
        public CustomArmorShader2(Ref<Effect> shader, string passName) : base(shader, passName)
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
                dustShaderData.UseColor(player.underShirtColor).UseSaturation(3f).Apply(player, drawData);
                return;
            }
            UseColor(player.underShirtColor);
            UseSaturation(3f);
            base.Apply(player, drawData);
        }

        public override ArmorShaderData GetSecondaryShader(Entity entity)
        {
            Player player = entity as Player;
            return dustShaderData.UseColor(player.underShirtColor).UseSaturation(3f);
        }




    }
}
