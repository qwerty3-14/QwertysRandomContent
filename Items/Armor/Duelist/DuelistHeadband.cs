using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Armor.Duelist
{
    [AutoloadEquip(EquipType.Head)]
    public class DuelistHeadband : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Duelist Headband");
            Tooltip.SetDefault("Attacking the same enemy continually with melee attaks increases damage dealt to that enemy \n6% reduced cooldown on quick morphs");

        }


        public override void SetDefaults()
        {

            item.value = 50000;
            item.rare = 1;


            item.width = 22;
            item.height = 14;
            item.defense = 1;



        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<DuelistEffects>().head = true;
            player.GetModPlayer<ShapeShifterPlayer>().coolDownDuration *= .94f;
        }
        public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
        {
            drawHair = true;

        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {

            return body.type == mod.ItemType("DuelistShirt") && legs.type == mod.ItemType("DuelistPants");

        }
        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = Language.GetTextValue("Mods.QwertysRandomContent.DuelistSet");
            player.GetModPlayer<DuelistEffects>().set = true;
        }






    }
    public class DuelistEffects : ModPlayer
    {
        public bool head;
        public bool body;
        public bool legs;
        public bool set;
        public NPC targeted;
        int targetCombo;
        float trigCounter;
        public override void ResetEffects()
        {
            head = false;
            body = false;
            legs = false;
            set = false;
        }
        void targetingLogic(NPC th, bool melee, ref int damage)
        {
            if (targeted != null && th == targeted)
            {
                if (head)
                {
                    damage = (int)(damage * (1f + targetCombo / 20f * .5f));

                }
                if (melee && targetCombo < 20)
                {
                    targetCombo++;
                }

            }
            else
            {
                targeted = th;
                targetCombo = 0;
            }




        }
        public override void ModifyHitNPC(Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
            if (head || body)
            {

                targetingLogic(target, item.melee, ref damage);

            }
            if (legs && item.melee && player.HasBuff(mod.BuffType("MorphCooldown")) && player.buffTime[player.FindBuffIndex(mod.BuffType("MorphCooldown"))] > 300)
            {
                player.buffTime[player.FindBuffIndex(mod.BuffType("MorphCooldown"))] -= damage / 2;
            }
        }
        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (head || body)
            {
                targetingLogic(target, proj.melee, ref damage);
                if (set && proj.GetGlobalProjectile<MorphProjectile>().morph && targetCombo == 20)
                {
                    crit = true;
                }
            }
            if (legs && proj.melee && player.HasBuff(mod.BuffType("MorphCooldown")) && player.buffTime[player.FindBuffIndex(mod.BuffType("MorphCooldown"))] > 300)
            {
                player.buffTime[player.FindBuffIndex(mod.BuffType("MorphCooldown"))] -= damage / 2;
            }
        }

        public override void ModifyHitByNPC(NPC npc, ref int damage, ref bool crit)
        {
            if (body && targeted != null && npc == targeted)
            {
                damage = damage - (int)(damage * .5 * targetCombo / 20f);
            }
        }
        public static readonly PlayerLayer DuelistIcon = new PlayerLayer("QwertysRandomContent", "DuelistIcon", PlayerLayer.MiscEffectsFront, delegate (PlayerDrawInfo drawInfo)
        {

            Mod mod = ModLoader.GetMod("QwertysRandomContent");
            Texture2D texture = mod.GetTexture("Items/Armor/Duelist/DuelistIcon_Shield");
            Player drawPlayer = drawInfo.drawPlayer;
            NPC targeted = drawPlayer.GetModPlayer<DuelistEffects>().targeted;
            drawPlayer.GetModPlayer<DuelistEffects>().trigCounter += (float)Math.PI / 30;
            float scale = 1f + .1f * (float)Math.Sin(drawPlayer.GetModPlayer<DuelistEffects>().trigCounter);

            if (targeted != null && targeted.active && (drawPlayer.GetModPlayer<DuelistEffects>().body))
            {
                DrawData value = new DrawData(texture, targeted.Center - new Vector2(0, targeted.height / 2 + 24) - Main.screenPosition, null, Color.White, 0f, texture.Size() * .5f, scale, SpriteEffects.None, 0);
                value.shader = drawInfo.legArmorShader;
                Main.playerDrawData.Add(value);
            }
            texture = mod.GetTexture("Items/Armor/Duelist/DuelistIcon_Sword");

            if (targeted != null && targeted.active && (drawPlayer.GetModPlayer<DuelistEffects>().head))
            {
                DrawData value = new DrawData(texture, targeted.Center - new Vector2(0, targeted.height / 2 + 24) - Main.screenPosition, null, Color.White, 0f, texture.Size() * .5f, scale, SpriteEffects.None, 0);
                value.shader = drawInfo.bodyArmorShader;
                Main.playerDrawData.Add(value);
            }
            texture = mod.GetTexture("Items/Armor/Duelist/DuelistIcon_Progress");

            if (targeted != null && targeted.active && (drawPlayer.GetModPlayer<DuelistEffects>().head || drawPlayer.GetModPlayer<DuelistEffects>().body))
            {

                DrawData value = new DrawData(texture, targeted.Center - new Vector2(0, targeted.height / 2 + 14) - Main.screenPosition, new Rectangle(0, 0 * texture.Height / 2, texture.Width, texture.Height / 2), Color.White, 0f, new Vector2(texture.Width * .5f, texture.Height * .25f), scale, SpriteEffects.None, 0);
                value.shader = drawInfo.bodyArmorShader;
                Main.playerDrawData.Add(value);
                value = new DrawData(texture, targeted.Center - new Vector2(0, targeted.height / 2 + 14) - Main.screenPosition, new Rectangle(0, 1 * texture.Height / 2, (int)(texture.Width * drawPlayer.GetModPlayer<DuelistEffects>().targetCombo / 20f), texture.Height / 2), Color.White, 0f, new Vector2(texture.Width * .5f, texture.Height * .25f), scale, SpriteEffects.None, 0);
                value.shader = drawInfo.legArmorShader;
                Main.playerDrawData.Add(value);



            }

        });

        public static readonly PlayerLayer Head = LayerDrawing.DrawHeadSimple("DuelistHeadband", "Items/Armor/Duelist/DuelistHeadband_HeadSimple", glowmask: false);
        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
            int headLayer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("Face"));

            if (headLayer != -1)
            {
                Head.visible = true;
                layers.Insert(headLayer + 1, Head);
            }


            int frontLayer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("MiscEffectsFront"));
            if (frontLayer != -1)
            {
                DuelistIcon.visible = true;
                layers.Insert(frontLayer + 1, DuelistIcon);
            }
        }
        public static readonly PlayerHeadLayer MapMask = LayerDrawing.DrawHeadLayer("DuelistHeadband", "Items/Armor/Duelist/DuelistHeadband_HeadSimple");
        public override void ModifyDrawHeadLayers(List<PlayerHeadLayer> layers)
        {
            int headLayer = layers.FindIndex(PlayerHeadLayer => PlayerHeadLayer.Name.Equals("Head"));
            if (headLayer != -1)
            {

                MapMask.visible = true;
                layers.Insert(headLayer + 1, MapMask);
            }
        }
    }

}

