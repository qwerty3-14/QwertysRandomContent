using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Graphics;

using Terraria.UI;
using Terraria.DataStructures;
using Terraria.GameContent.UI;
using System;
using Terraria.Graphics;
using QwertysRandomContent.Items.B4Items;
using QwertysRandomContent.Items.Dyes;
using Terraria.GameContent.Dyes;


namespace QwertysRandomContent
{
	public class QwertysRandomContent : Mod
	{
        public static Effect CustomEffect;
        public static ModHotKey YetAnotherSpecialAbility;
        public override void AddRecipeGroups()
        {
           

            RecipeGroup group = new RecipeGroup(() => Lang.misc[37].Value + " evil bow", new int[]
            {
                ItemID.DemonBow,
                ItemID.TendonBow,
            });
            RecipeGroup.RegisterGroup("QwertysrandomContent:EvilBows", group);

            group = new RecipeGroup(() => Lang.misc[37].Value + " Aerous Bow", new int[]
            {
                ItemType("AerousLongbow"),
                ItemType("AerousLongbowWithRandomEnchantment"),
                ItemType("AerousLongbowWithStormEnchantment"),

            });
            RecipeGroup.RegisterGroup("QwertysrandomContent:AerousBow", group);


            group = new RecipeGroup(() => Lang.misc[37].Value + " Evil Powder", new int[]
            {
                ItemID.VilePowder,
                ItemID.ViciousPowder,

            });
            RecipeGroup.RegisterGroup("QwertysrandomContent:EvilPowder", group);
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(this);
        }
        internal static QwertysRandomContent instance;
		public const string AncientMachineHead = "QwertysRandomContent/NPCs/AncientMachine/AncientMachine_Head_Boss";
		public const string Head1Head = "QwertysRandomContent/NPCs/HydraBoss/Head1_Head_Boss";
		public const string Head2Head = "QwertysRandomContent/NPCs/HydraBoss/Head2_Head_Boss";
		public const string Head3Head = "QwertysRandomContent/NPCs/HydraBoss/Head3_Head_Boss";
		public QwertysRandomContent()
		{
			Properties = new ModProperties()
			{
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};
			
			
		}

        
        public override void UpdateUI(GameTime gameTime)
        {
            
        }
        public override void UpdateMusic(ref int music, ref MusicPriority priority)
        {
            if (Main.myPlayer != -1 && !Main.gameMenu && Main.LocalPlayer.active)
            {
                // Make sure your logic here goes from lowest priority to highest so your intended priority is maintained.
                if (Main.LocalPlayer.GetModPlayer<FortressBiome>().TheFortress)
                {
                    music = GetSoundSlot(SoundType.Music, "Sounds/Music/HeavenlyFortress");
                    priority = MusicPriority.Event;
                }
                
            }
        }
        public override void PostSetupContent()
        {
            Mod bossChecklist = ModLoader.GetMod("BossChecklist");
            if (bossChecklist != null)
            {
                
                bossChecklist.Call("AddBossWithInfo", "Ancient Machine", 5.5f, (Func<bool>)(() => QwertyWorld.downedAncient), "Look into the [i:" + ItemType("AncientEmblem") + "]");
                bossChecklist.Call("AddBossWithInfo", "The Hydra", 6.000001f, (Func<bool>)(() => QwertyWorld.downedhydra), "Tempt with [i:" + ItemType("HydraSummon") + "]");
                bossChecklist.Call("AddBossWithInfo", "Rune Ghost", 12.2f, (Func<bool>)(() => QwertyWorld.downedRuneGhost), "Use a [i:" + ItemType("SummoningRune") + "] to challenge its power. [i:" + ItemType("SummoningRune") + "] drops from the dungeon's sneaking ghosts");
                bossChecklist.Call("AddBossWithInfo", "Oversized Laser-emitting Obliteration Radiation-emitting Destroyer", 13.9999f, (Func<bool>)(() => QwertyWorld.downedB4), "Use a [i:" + ItemType("B4Summon") + "]");
                bossChecklist.Call("AddEventWithInfo", "Dino Militia", 9.001f, (Func<bool>)(() => QwertyWorld.downedDinoMilitia), "Use a [i:" + ItemType("DinoEgg") + "] and they'll come to drive you extinct!");
                bossChecklist.Call("AddEventWithInfo", "Dino Militia (post Moonlord)", 15f, (Func<bool>)(() => QwertyWorld.downedDinoMilitiaHard), "Use a [i:" + ItemType("DinoEgg") + "] the militia will hold nothing back this time!");
                bossChecklist.Call("AddMiniBossWithInfo", "The Great Tyrannosaurus", 9.002f, (Func<bool>)(() => QwertyWorld.downedTyrant), "Fights with the dino militia, more likely to a apear near the end");
                bossChecklist.Call("AddBossWithInfo", "The Divine Light", 4f, (Func<bool>)(() => QwertyWorld.downedFortressBoss), "Use a [i:" + ItemType("FortressBossSummon") + "]" + " at the altar in the sky fortress");
                bossChecklist.Call("AddBossWithInfo", "Polar Exterminator", .7f, (Func<bool>)(() => QwertyWorld.downedBear), "Hibernates in the underground tundra, [i:" + ItemType("FrostCompass") + "]Helps find it");
                bossChecklist.Call("AddBossWithInfo", "Imperious", 9.4f, (Func<bool>)(() => QwertyWorld.downedBlade), "Use the [i:" + ItemType("BladeBossSummon") + "], and accept its challenge");
            }
        }
        public override void Load()
		{
            Config.Load();
        
        // Registers a new hotkey
        YetAnotherSpecialAbility = RegisterHotKey("Yet Another Special Ability", "Mouse3");
            if (!Main.dedServ)
            {
                // Add the female leg variants
                AddEquipTexture(GetItem("LuneLeggings"), EquipType.Legs, "LuneLeggings_Female", "QwertysRandomContent/Items/Armor/Lune/LuneLeggings_FemaleLegs");
                AddEquipTexture(GetItem("GaleSwiftRobes"), EquipType.Legs, "GaleSwiftRobes_Female", "QwertysRandomContent/Items/Fortress/GaleArmor/GaleSwiftRobes_FemaleLegs");
                AddEquipTexture(new CaeliteGreavesMale(), GetItem("CaeliteGreaves"), EquipType.Legs, "CaeliteGreaves_Legs", "QwertysRandomContent/Items/Fortress/CaeliteArmor/CaeliteGreaves_Legs");
                AddEquipTexture(new CaeliteGreavesFemale(), GetItem("CaeliteGreaves"), EquipType.Legs, "CaeliteGreaves_FemaleLegs", "QwertysRandomContent/Items/Fortress/CaeliteArmor/CaeliteGreaves_FemaleLegs");
                AddEquipTexture(new RhuthiniumGreavesMale(), GetItem("RhuthiniumGreaves"), EquipType.Legs, "RhuthiniumGreaves", "QwertysRandomContent/Items/Armor/Rhuthinium/RhuthiniumGreaves_Legs");
                AddEquipTexture(new RhuthiniumGreavesFemale(), GetItem("RhuthiniumGreaves"), EquipType.Legs, "RhuthiniumGreaves_FemaleLegs", "QwertysRandomContent/Items/Armor/Rhuthinium/RhuthiniumGreaves_FemaleLegs");
                AddEquipTexture(GetItem("HydraLeggings"), EquipType.Legs, "HydraLeggings_Female", "QwertysRandomContent/Items/HydraItems/HydraLeggings_FemaleLegs");
                CustomEffect = GetEffect("Effects/CustomEffect");
                Ref<Effect> CustomEffectRef = new Ref<Effect>();
                CustomEffectRef.Value = CustomEffect;
                /*
                LegacyHairShaderData coloredShader = new LegacyHairShaderData().UseLegacyMethod(delegate (Player player, Color newColor, ref bool lighting)
                {
                    newColor.R = player.shirtColor.R;
                    newColor.B = player.shirtColor.G;
                    newColor.G = player.shirtColor.B;
                    return newColor;
                })
                //GameShaders.Armor.BindShader<ArmorShaderData>(ItemType("CustomDye"), );
                */
                GameShaders.Armor.BindShader<CustomArmorShader>(ItemType("CustomDye"), new CustomArmorShader(Main.PixelShaderRef, "ArmorColored"));
                GameShaders.Armor.BindShader<CustomArmorShader2>(ItemType("CustomDye2"), new CustomArmorShader2(Main.PixelShaderRef, "ArmorColored"));
                GameShaders.Armor.BindShader<CustomArmorShader3>(ItemType("CustomDye3"), new CustomArmorShader3(Main.PixelShaderRef, "ArmorColored"));
                GameShaders.Armor.BindShader<CustomArmorShader4>(ItemType("CustomDye4"), new CustomArmorShader4(Main.PixelShaderRef, "ArmorColored"));
            }
            
            instance = this;
			
			ModTranslation text = CreateTranslation("DinoDefeat");
			text.SetDefault("You drove the Dinosaurs to extinction!");
            text.AddTranslation(GameCulture.Russian, "Ты довел динозавров до вымирания!");
			AddTranslation(text);

			text = CreateTranslation("DinoEventStart");
			text.SetDefault("The Dino Militia is coming!");
            text.AddTranslation(GameCulture.Russian, "Ополчение динозавров идет!");
            AddTranslation(text);

            text = CreateTranslation("DinoHardStart");
            text.SetDefault("The Dino Militia ready for a rematch!");
            text.AddTranslation(GameCulture.Russian, "Дино армия готово к реваншу!");
            AddTranslation(text);

            text = CreateTranslation("GhostAngry");
            text.SetDefault("I haven't even been trying until now!");
            text.AddTranslation(GameCulture.Russian, "Я даже не старался до сих пор! ");
            AddTranslation(text);

            text = CreateTranslation("GhostFurious");
            text.SetDefault("Ok, now it's personal!");
            text.AddTranslation(GameCulture.Russian, "Хорошо, теперь это личное! ");
            AddTranslation(text);

            text = CreateTranslation("RhuthiniumGenerates");
            text.SetDefault("Rhuthimis has blessed your world with Rhuthinium!");
            text.AddTranslation(GameCulture.Russian, "Ваш мир был благославлён Рутиниумом ");
            AddTranslation(text);

            text = CreateTranslation("DivineIntro");
            text.SetDefault("You have activated my sacred altar... but you are not one of my disciples... and thus I see you as an enemy. Leave this fortress! and never return!");
            text.AddTranslation(GameCulture.Russian, "Ты активировал мой священный алтарь... но ты не один из моих учеников... и поэтому я вижу в тебе врага. Покинь эту крепость! И никогда не возвращайся! ");
            AddTranslation(text);

            text = CreateTranslation("DivineRage");
            text.SetDefault("What part of 'NEVER RETURN' did you not understand? Such foolishness will bring forth your demise...");
            text.AddTranslation(GameCulture.Russian, "Какую часть фразы 'никогда не возвращайся' Ты не понял? Такая глупость приведет тебя к смерти... ");
            AddTranslation(text);

            text = CreateTranslation("DivineRage2");
            text.SetDefault("Fool, your pride shall bring your end...");
            text.AddTranslation(GameCulture.Russian, "Глупец, твоя гордость принесет тебе конец... ");
            AddTranslation(text);

            text = CreateTranslation("DivineMock");
            text.SetDefault("Either you're slower than a mollusket or you're trying to heal, you know I'll do that if you don't keep up...");
            text.AddTranslation(GameCulture.Russian, "Либо ты медленнее моллюска, либо ты пытаешься выздороветь, ты знаешь, я сделаю это, если ты не поспеешь ... ");
            AddTranslation(text);

            text = CreateTranslation("DivineLeave");
            text.SetDefault("now do not bother coming back!");
            text.AddTranslation(GameCulture.Russian, "И больше не возвращайся!");
            AddTranslation(text);

            text = CreateTranslation("DivineStart");
            text.SetDefault("... so be it");
            text.AddTranslation(GameCulture.Russian, "... Так тому и быть");
            AddTranslation(text);



            AddBossHeadTexture(AncientMachineHead);
			AddBossHeadTexture(Head1Head);
			AddBossHeadTexture(Head2Head);
			AddBossHeadTexture(Head3Head);
			if (!Main.dedServ)
			{
				// Register a new music box
				//AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/HydraBoss"), ItemType("HydraMusicBox"), TileType("HydraMusicBox"));
                AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/EnergisedPlanetaryIncinerationClimax"), ItemType("B4MusicBox"), TileType("B4MusicBox"));
                AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/BuiltToDestroy"), ItemType("AncientMusicBox"), TileType("MusicBoxBuiltToDestroy"));
                AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/HeavenlyFortress"), ItemType("MusicBoxHeavenlyFortress"), TileType("MusicBoxHeavenlyFortress"));
                AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/TheConjurer"), ItemType("MusicBoxTheConjurer"), TileType("MusicBoxTheConjurer"));
                AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/OldDinosNewGuns"), ItemType("MusicBoxOldDinosNewGuns"), TileType("MusicBoxOldDinosNewGuns"));
                AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/HigherBeing"), ItemType("MusicBoxHigherBeing"), TileType("MusicBoxHigherBeing"));
                AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/BladeOfAGod"), ItemType("MusicBoxBladeOfAGod"), TileType("MusicBoxBladeOfAGod"));
                AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/BeastOfThreeHeads"), ItemType("MusicBoxBeastOfThreeHeads"), TileType("MusicBoxBeastOfThreeHeads"));

                //Main.playerTextures[1, 10] = TextureManager.Load("Images/Player_6_6");
                //Main.playerTextures[1, PlayerTextureID.LegSkin] = GetTexture("Items/Vanity/AltLegs"); 


            }
			
			
		}
        public override void Unload()
        {

            // Unload static references
            // You need to clear static references to assets (Texture2D, SoundEffects, Effects). 
            CustomEffect = null;
            YetAnotherSpecialAbility = null;

        }
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            ModMessageType msgType = (ModMessageType)reader.ReadByte();
            switch (msgType)
            {
                case ModMessageType.ArrowMessage:
                    int identity = reader.ReadInt32();
                    byte owner = reader.ReadByte();
                    int realIdentity = Projectile.GetByUUID(owner, identity);
                    if (realIdentity != -1)
                    {
                        Main.projectile[realIdentity].GetGlobalProjectile<arrowHoming>().B4HomingArrow = true;
                    }
                    if (Main.netMode == 2)
                    {
                        ModPacket packet = GetPacket();
                        packet.Write((byte)ModMessageType.ArrowMessage);
                        packet.Write(identity);
                        packet.Write(owner);
                        packet.Send();
                    }
                    break;
                    
                case ModMessageType.FinnedRandomSwimMessage:
                    
                    int identity2 = reader.ReadInt32();
                    byte owner2 = reader.ReadByte();
                    float randomSwim = reader.Read();
                    int realIdentity2 = Projectile.GetByUUID(owner2, identity2);
                    
                    if (realIdentity2 != -1)
                    {
                        Main.projectile[realIdentity2].ai[1] = randomSwim;
                    }
                    
                    if (Main.netMode == 2)
                    {
                        ModPacket packet = GetPacket();
                        packet.Write((byte)ModMessageType.FinnedRandomSwimMessage);
                        packet.Write(identity2);
                        packet.Write(owner2);
                        packet.Write(randomSwim);
                        packet.Send();
                    }
                    break;
                case ModMessageType.ScaleMessage:

                    int identity3 = reader.ReadInt32();
                    byte owner3 = reader.ReadByte();
                    
                    int realIdentity3 = Projectile.GetByUUID(owner3, identity3);

                    if (realIdentity3 != -1)
                    {
                        Main.projectile[realIdentity3].scale = 3 ;
                    }

                    if (Main.netMode == 2)
                    {
                        ModPacket packet = GetPacket();
                        packet.Write((byte)ModMessageType.ScaleMessage);
                        packet.Write(identity3);
                        packet.Write(owner3);
                        
                        packet.Send();
                    }
                    break;

            }
        }
        
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            QwertyWorld modWorld = (QwertyWorld)GetModWorld("QwertyWorld");
            if (modWorld.DinoEvent)
            {
                int index = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
                LegacyGameInterfaceLayer orionProgress = new LegacyGameInterfaceLayer("Dino Militia",
                    delegate
                    {
                        DrawDinoEvent(Main.spriteBatch);
                        return true;
                    },
                    InterfaceScaleType.UI);
                layers.Insert(index, orionProgress);
            }
        }
        
        public void DrawDinoEvent(SpriteBatch spriteBatch)
        {
            QwertyWorld modWorld = (QwertyWorld)GetModWorld("QwertyWorld");
            if (modWorld.DinoEvent && !Main.gameMenu)
            {
                float scaleMultiplier = 0.5f + 1 * 0.5f;
                float alpha = 0.5f;
                Texture2D progressBg = Main.colorBarTexture;
                Texture2D progressColor = Main.colorBarTexture;
                Texture2D orionIcon = GetTexture("Items/DinoItems/DinoEgg");
                const string orionDescription = "Dino Militia";
                Color descColor = new Color(39, 86, 134);

                Color waveColor = new Color(255, 241, 51);
                Color barrierColor = new Color(255, 241, 51);

                try
                {
                    //draw the background for the waves counter
                    const int offsetX = 20;
                    const int offsetY = 20;
                    int width = (int)(200f * scaleMultiplier);
                    int height = (int)(46f * scaleMultiplier);
                    Rectangle waveBackground = Utils.CenteredRectangle(new Vector2(Main.screenWidth - offsetX - 100f, Main.screenHeight - offsetY - 23f), new Vector2(width, height));
                    Utils.DrawInvBG(spriteBatch, waveBackground, new Color(63, 65, 151, 255) * 0.785f);

                    //draw wave text

                    string waveText = "Cleared " + (int)(((float)modWorld.DinoKillCount / (float)modWorld.MaxDinoKillCount)*100) + "%";
                    Utils.DrawBorderString(spriteBatch, waveText, new Vector2(waveBackground.X + waveBackground.Width / 2, waveBackground.Y), Color.White, scaleMultiplier, 0.5f, -0.1f);

                    //draw the progress bar

                    if (modWorld.DinoKillCount == 0)
                    {

                    }
                   // Main.NewText(MathHelper.Clamp((modWorld.DinoKillCount/modWorld.MaxDinoKillCount), 0f, 1f));
                    Rectangle waveProgressBar = Utils.CenteredRectangle(new Vector2(waveBackground.X + waveBackground.Width * 0.5f, waveBackground.Y + waveBackground.Height * 0.75f), new Vector2(progressColor.Width, progressColor.Height));
                    Rectangle waveProgressAmount = new Rectangle(0, 0, (int)(progressColor.Width  * MathHelper.Clamp(((float)modWorld.DinoKillCount / (float)modWorld.MaxDinoKillCount), 0f, 1f)), progressColor.Height);
                    Vector2 offset = new Vector2((waveProgressBar.Width - (int)(waveProgressBar.Width * scaleMultiplier)) * 0.5f, (waveProgressBar.Height - (int)(waveProgressBar.Height * scaleMultiplier)) * 0.5f);

                    spriteBatch.Draw(progressBg, waveProgressBar.Location.ToVector2() + offset, null, Color.White * alpha, 0f, new Vector2(0f), scaleMultiplier, SpriteEffects.None, 0f);
                    spriteBatch.Draw(progressBg, waveProgressBar.Location.ToVector2() + offset, waveProgressAmount, waveColor, 0f, new Vector2(0f), scaleMultiplier, SpriteEffects.None, 0f);

                    //draw the icon with the event description

                    //draw the background
                    const int internalOffset = 6;
                    Vector2 descSize = new Vector2(154, 40) * scaleMultiplier;
                    Rectangle barrierBackground = Utils.CenteredRectangle(new Vector2(Main.screenWidth - offsetX - 100f, Main.screenHeight - offsetY - 19f), new Vector2(width, height));
                    Rectangle descBackground = Utils.CenteredRectangle(new Vector2(barrierBackground.X + barrierBackground.Width * 0.5f, barrierBackground.Y - internalOffset - descSize.Y * 0.5f), descSize);
                    Utils.DrawInvBG(spriteBatch, descBackground, descColor * alpha);

                    //draw the icon
                    int descOffset = (descBackground.Height - (int)(32f * scaleMultiplier)) / 2;
                    Rectangle icon = new Rectangle(descBackground.X + descOffset, descBackground.Y + descOffset, (int)(32 * scaleMultiplier), (int)(32 * scaleMultiplier));
                    spriteBatch.Draw(orionIcon, icon, Color.White);

                    //draw text

                    Utils.DrawBorderString(spriteBatch, orionDescription, new Vector2(barrierBackground.X + barrierBackground.Width * 0.5f, barrierBackground.Y - internalOffset - descSize.Y * 0.5f), Color.White, 0.80f, 0.3f, 0.4f);
                }
                catch (Exception e)
                {
                    ErrorLogger.Log(e.ToString());
                }
            }
        }

    }
    enum ModMessageType : byte
    {
        ArrowMessage,
        FinnedRandomSwimMessage,
        ScaleMessage,
        TurretSetupMessage
            
    }
    public class CaeliteGreavesMale : EquipTexture
    {
    }

    public class CaeliteGreavesFemale : EquipTexture
    {
        public override bool DrawLegs()
        {
            return false;
        }
    }
    public class RhuthiniumGreavesMale : EquipTexture
    {
        /*
        public override bool DrawLegs()
        {
            return false;
        }*/
    }

    public class RhuthiniumGreavesFemale : EquipTexture
    {
        
    }
}
