namespace QwertysRandomContent
{
    /*
    //Silhouette Stuff
    public class SilhouetteWorld : ModWorld
    {
        public static bool SilouetteMode = false;

        public override void PreUpdate()
        {

            if (SilouetteMode)
            {
                foreach (Dust dust in Main.dust)
                {
                    dust.shader = GameShaders.Armor.GetShaderFromItemId(ItemID.ShadowDye);
                }
            }
        }
    }
    public class SilhouetteProjectile : GlobalProjectile
    {
        public override void PostDraw(Projectile projectile, SpriteBatch spriteBatch, Color lightColor)
        {

            if (SilhouetteWorld.SilouetteMode)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.Transform);
            }
        }
        public override bool PreDraw(Projectile projectile, SpriteBatch spriteBatch, Color lightColor)
        {

            if (SilhouetteWorld.SilouetteMode)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
                GameShaders.Armor.GetShaderFromItemId(ItemID.ShadowDye).Apply(null);

            }
            return true;
        }
    }
    public class SilhouetteNPC : GlobalNPC
    {
        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Color lightColor)
        {

            if (SilhouetteWorld.SilouetteMode)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.Transform);
            }
        }
        public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Color lightColor)
        {

            if (SilhouetteWorld.SilouetteMode)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
                GameShaders.Armor.GetShaderFromItemId(ItemID.ShadowDye).Apply(null);

            }
            return true;
        }
    }
    public class SilhouetteItem : GlobalItem
    {
        public override void PostDrawInInventory(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            if (SilhouetteWorld.SilouetteMode)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.Transform);
            }
        }
        public override bool PreDrawInInventory(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            if (SilhouetteWorld.SilouetteMode)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
                GameShaders.Armor.GetShaderFromItemId(ItemID.ShadowDye).Apply(null);

            }
            return true;
        }
        public override void PostDrawInWorld(Item item, SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            if (SilhouetteWorld.SilouetteMode)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.Transform);
            }
        }
        public override bool PreDrawInWorld(Item item, SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            if (SilhouetteWorld.SilouetteMode)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
                GameShaders.Armor.GetShaderFromItemId(ItemID.ShadowDye).Apply(null);

            }
            return true;
        }
    }
    public class SilhouetteTile : GlobalTile
    {
        public override void PostDraw(int i, int j, int type, SpriteBatch spriteBatch)
        {
            if (i * 16 > Main.screenPosition.X - 16 && i * 16 < Main.screenPosition.X + Main.screenWidth + 16 && j * 16 > Main.screenPosition.Y - 16 && j * 16 < Main.screenPosition.Y + Main.screenHeight + 16)
            {
                if (SilhouetteWorld.SilouetteMode)
                {
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.Transform);
                }
            }

        }
        public override bool PreDraw(int i, int j, int type, SpriteBatch spriteBatch)
        {
            if (i * 16 > Main.screenPosition.X - 16 && i * 16 < Main.screenPosition.X + Main.screenWidth + 16 && j * 16 > Main.screenPosition.Y - 16 && j * 16 < Main.screenPosition.Y + Main.screenHeight + 16)
            {
                if (SilhouetteWorld.SilouetteMode)
                {
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
                    GameShaders.Armor.GetShaderFromItemId(ItemID.ShadowDye).Apply(null);

                }
            }


            return true;
        }
    }
    public class SilhouetteWall : GlobalWall
    {
        public override void PostDraw(int i, int j, int type, SpriteBatch spriteBatch)
        {
            if (i * 16 > Main.screenPosition.X - 16 && i * 16 < Main.screenPosition.X + Main.screenWidth + 16 && j * 16 > Main.screenPosition.Y - 16 && j * 16 < Main.screenPosition.Y + Main.screenHeight + 16)
            {
                if (SilhouetteWorld.SilouetteMode)
                {
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.Transform);
                }
            }

        }
        public override bool PreDraw(int i, int j, int type, SpriteBatch spriteBatch)
        {
            if (i * 16 > Main.screenPosition.X - 16 && i * 16 < Main.screenPosition.X + Main.screenWidth + 16 && j * 16 > Main.screenPosition.Y - 16 && j * 16 < Main.screenPosition.Y + Main.screenHeight + 16)
            {
                if (SilhouetteWorld.SilouetteMode)
                {
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
                    GameShaders.Armor.GetShaderFromItemId(ItemID.ShadowDye).Apply(null);

                }
            }


            return true;
        }
    }
    */
}
