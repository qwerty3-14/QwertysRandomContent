using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Armor.Glass
{
	[AutoloadEquip(EquipType.Head)]
	public class GlassHelm : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Glass Helm");
			Tooltip.SetDefault("A glass prism orbits you zapping enemies");
			
		}
		

		public override void SetDefaults()
		{
            item.value = 10000;
            item.rare = 1;
            item.width = 22;
			item.height = 14;
			item.defense = 4;
        }
		
		public override void UpdateEquip(Player player)
		{
            player.GetModPlayer<HelmEffects>().helmEffect = true ;
        }
		public override void DrawHair(ref bool  drawHair, ref bool  drawAltHair )
		{
            drawAltHair = true;
			
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == mod.ItemType("GlassAbsorber") && legs.type == mod.ItemType("GlassLimbguards");
			
		}
		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Ranged attacks Inflict 'Arcanely tuned' \nMagic attacks chase enemies inflicted with 'Arcanely tuned'";
            player.GetModPlayer<HelmEffects>().setBonus = true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Glass, 15);
            recipe.AddRecipeGroup("QwertysrandomContent:SilverBar", 4);
            recipe.AddTile(TileID.GlassKiln);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }





    }
    public class HelmEffects : ModPlayer
    {

        public float PrismTrigonometryCounterOfAwsomenessWowThisIsAVeryLongVariableName;
        public bool helmEffect = false;
        public int prismDazzleCounter = 60;
        public bool setBonus = false;
        public override void ResetEffects()
        {
            setBonus = false;
            if (!helmEffect)
            {
                prismDazzleCounter = 60;
            }
            helmEffect = false;
        }
        public override void PreUpdate()
        {
            if(helmEffect)
            {
                PrismTrigonometryCounterOfAwsomenessWowThisIsAVeryLongVariableName += (float)MathHelper.Pi / 30;
                prismDazzleCounter--;
                NPC target = new NPC();
                Vector2 prismCenter = player.Center + new Vector2((float)Math.Sin(PrismTrigonometryCounterOfAwsomenessWowThisIsAVeryLongVariableName) * 40f, 0);
                if (QwertyMethods.ClosestNPC(ref target, 4000, prismCenter) && prismDazzleCounter <=0)
                {
                    Projectile.NewProjectile(prismCenter, QwertyMethods.PolarVector(1, (target.Center - prismCenter).ToRotation()), mod.ProjectileType("PrismDazzle"), (int)(10f * player.magicDamage), 0f, player.whoAmI);
                    prismDazzleCounter = 60;
                }
            }
            
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if(proj.ranged && setBonus)
            {
                target.AddBuff(mod.BuffType("ArcanelyTuned"), 360);
            }
            
        }
        public static readonly PlayerLayer Mask = new PlayerLayer("QwertysRandomContent", "Mask", PlayerLayer.Head, delegate (PlayerDrawInfo drawInfo)
        {
            if (drawInfo.shadow != 0f)
            {
                return;
            }
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("QwertysRandomContent");
            //ExamplePlayer modPlayer = drawPlayer.GetModPlayer<ExamplePlayer>(mod);
            if (drawPlayer.head == mod.GetEquipSlot("GlassHelm", EquipType.Head))
            {
                //Main.NewText("Pug!");
                //Main.NewText(drawPlayer.bodyFrame);

                int fHeight = 56;

                Texture2D texture = mod.GetTexture("Items/Armor/Glass/GlassHelm_HeadSimple");
                Color color12 = drawPlayer.GetImmuneAlphaPure(Lighting.GetColor((int)((double)drawInfo.position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)drawInfo.position.Y + (double)drawPlayer.height * 0.5) / 16, Microsoft.Xna.Framework.Color.White), 0f);
                int drawX = (int)(drawPlayer.position.X - Main.screenPosition.X);
                int drawY = (int)(drawPlayer.position.Y - Main.screenPosition.Y);
                Vector2 Position = drawInfo.position;



                Vector2 pos = new Vector2((float)((int)(Position.X - Main.screenPosition.X - (float)(drawPlayer.bodyFrame.Width / 2) + (float)(drawPlayer.width / 2))), (float)((int)(Position.Y - Main.screenPosition.Y + (float)drawPlayer.height - (float)drawPlayer.bodyFrame.Height + 4f))) + drawPlayer.bodyPosition + new Vector2((float)(drawPlayer.bodyFrame.Width / 2), (float)(drawPlayer.bodyFrame.Height / 2));
                if (drawPlayer.bodyFrame.Y == 7 * fHeight || drawPlayer.bodyFrame.Y == 8 * fHeight || drawPlayer.bodyFrame.Y == 9 * fHeight || drawPlayer.bodyFrame.Y == 14 * fHeight || drawPlayer.bodyFrame.Y == 15 * fHeight || drawPlayer.bodyFrame.Y == 16 * fHeight)
                {
                    if(drawPlayer.gravDir == -1)
                    {
                        pos.Y += 2;
                    }
                    else
                    {
                        pos.Y -= 2;
                    }
                    

                }
                Rectangle frame = new Rectangle(0, 0, 40, fHeight);
                Vector2 origin = new Vector2((float)frame.Width * 0.5f, (float)frame.Height * 0.5f);
                //pos.Y -= drawPlayer.mount.PlayerOffset;
                DrawData data = new DrawData(texture, pos, frame, color12, 0f, origin, 1f, drawInfo.spriteEffects, 0);
                data.shader = drawInfo.headArmorShader;
                Main.playerDrawData.Add(data);

                 texture = mod.GetTexture("Items/Armor/Glass/GlassHelm_HeadSimple_Glass");
                 
               
                data = new DrawData(texture, pos, frame, color12, 0f, origin, 1f, drawInfo.spriteEffects, 0);
                data.shader = (int)drawPlayer.dye[3].dye;
                Main.playerDrawData.Add(data);
            }
        });
        public static readonly PlayerLayer PrismFront = new PlayerLayer("QwertysRandomContent", "PrismFront", PlayerLayer.MiscEffectsFront, delegate (PlayerDrawInfo drawInfo)
        {
            if (drawInfo.shadow != 0f)
            {
                return;
            }
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("QwertysRandomContent");
            Texture2D texture = mod.GetTexture("Items/Armor/Glass/GlassPrism");
            Color color12 = drawPlayer.GetImmuneAlphaPure(Lighting.GetColor((int)((double)drawInfo.position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)drawInfo.position.Y + (double)drawPlayer.height * 0.5) / 16, Microsoft.Xna.Framework.Color.White), 0f);
            if (Math.Cos(drawPlayer.GetModPlayer<HelmEffects>().PrismTrigonometryCounterOfAwsomenessWowThisIsAVeryLongVariableName ) >0 && drawPlayer.GetModPlayer<HelmEffects>().helmEffect)
            {
                Vector2 Center = drawInfo.position + new Vector2(drawPlayer.width / 2, drawPlayer.height / 2) - Main.screenPosition;
                Center.X += (float)Math.Sin(drawPlayer.GetModPlayer<HelmEffects>().PrismTrigonometryCounterOfAwsomenessWowThisIsAVeryLongVariableName) * 40;
                DrawData data = new DrawData(texture, Center, texture.Frame(), color12, 0f, texture.Size() * .5f, 1f, drawInfo.spriteEffects, 0);
                data.shader = (int)drawPlayer.dye[3].dye;
                Main.playerDrawData.Add(data);
            }
        });
        public static readonly PlayerLayer PrismBack = new PlayerLayer("QwertysRandomContent", "PrismBack", PlayerLayer.MiscEffectsBack, delegate (PlayerDrawInfo drawInfo)
        {
            if (drawInfo.shadow != 0f)
            {
                return;
            }
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("QwertysRandomContent");
            Texture2D texture = mod.GetTexture("Items/Armor/Glass/GlassPrism");
            Color color12 = drawPlayer.GetImmuneAlphaPure(Lighting.GetColor((int)((double)drawInfo.position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)drawInfo.position.Y + (double)drawPlayer.height * 0.5) / 16, Microsoft.Xna.Framework.Color.White), 0f);
            if (Math.Cos(drawPlayer.GetModPlayer<HelmEffects>().PrismTrigonometryCounterOfAwsomenessWowThisIsAVeryLongVariableName) <= 0 && drawPlayer.GetModPlayer<HelmEffects>().helmEffect)
            {
                Vector2 Center = drawInfo.position + new Vector2(drawPlayer.width / 2, drawPlayer.height / 2) - Main.screenPosition;
                Center.X += (float)Math.Sin(drawPlayer.GetModPlayer<HelmEffects>().PrismTrigonometryCounterOfAwsomenessWowThisIsAVeryLongVariableName) * 40;
                DrawData data = new DrawData(texture, Center, texture.Frame(), color12, 0f, texture.Size() * .5f, 1f, drawInfo.spriteEffects, 0);
                data.shader = (int)drawPlayer.dye[3].dye;
                Main.playerDrawData.Add(data);
            }
        });
        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
            int headLayer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("Head"));
            if (headLayer != -1)
            {
                Mask.visible = true;
                layers.Insert(headLayer + 1, Mask);
            }
            int frontLayer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("MiscEffectsFront"));
            if (frontLayer != -1)
            {
                PrismFront.visible = true;
                layers.Insert(frontLayer + 1, PrismFront);
            }
            int backLayer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("MiscEffectsBack"));
            if (backLayer != -1)
            {
                PrismBack.visible = true;
                layers.Insert(backLayer + 1, PrismBack);
            }

        }
    }
    public class PrismDazzle : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 4;
            projectile.height = 4;
            projectile.extraUpdates = 99;
            projectile.timeLeft = 1200;
            projectile.friendly = true;
            projectile.magic = true;
            projectile.penetrate = 2;
            projectile.usesLocalNPCImmunity = true;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            return false;
        }
        public override void AI()
        {
            if (Main.rand.Next(8) == 0)
            {
                Dust d = Main.dust[Dust.NewDust(projectile.Center, 0, 0, mod.DustType("GlassSmoke"))];
                d.velocity *= .1f;
                d.noGravity = true;
                d.position = projectile.Center;
                d.shader = GameShaders.Armor.GetSecondaryShader(Main.player[projectile.owner].dye[3].dye, Main.player[projectile.owner]);
            }
            for (int k = 0; k < 200; k++)
            {
                
                if (!Collision.CheckAABBvAABBCollision(projectile.position, projectile.Size, Main.npc[k].position, Main.npc[k].Size))
                {
                    projectile.localNPCImmunity[k] = 0;
                }

            }

        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (!target.boss && Main.rand.Next(20)==0)
            {
                target.AddBuff(mod.BuffType("Stunned"), 120);
            }
            projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[projectile.owner] = 0;
        }
        
    }
    public class ArcanelyTunedHoming : GlobalProjectile
    {
        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }
        NPC target;
        bool foundTarget = false;
        public override void PostAI(Projectile projectile)
        {
            float maxDistance = 10000;
            foundTarget = false;
            if (projectile.magic && projectile.friendly)
            {
                
               
                for (int k = 0; k < 200; k++)
                {
                    NPC possibleTarget = Main.npc[k];
                    float distance = (possibleTarget.Center - projectile.Center).Length();
                    if (distance < maxDistance && possibleTarget.HasBuff(mod.BuffType("ArcanelyTuned")) && possibleTarget.active && !possibleTarget.dontTakeDamage && !possibleTarget.friendly && possibleTarget.lifeMax > 5  && (Collision.CanHit(projectile.Center, 0, 0, possibleTarget.Center, 0, 0) || !projectile.tileCollide))
                    {
                        target = Main.npc[k];
                        foundTarget = true;


                        maxDistance = (target.Center - projectile.Center).Length();
                    }

                }
                if (foundTarget)
                {
                    float aimToward = (target.Center - projectile.Center).ToRotation();
                    float dir = projectile.velocity.ToRotation();
                    dir = QwertyMethods.SlowRotation(dir, aimToward, 2);
                    projectile.velocity = QwertyMethods.PolarVector(projectile.velocity.Length(), dir);
                }
            }
            
        }

    }
}

