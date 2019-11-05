using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace QwertysRandomContent.Items.DinoItems
{
	public class DinoBone : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dino Bone");
			Tooltip.SetDefault("No this isn't a bone from a dinosaur... it's a bone dinosaurs like to chew on." + "\nSummons a Trex mount!" + "\nWhen mounted the dino vulcan's speed penalty is removed");

        }

		public override void SetDefaults()
		{
			item.width = 42;
			item.height = 42;
			item.useTime = 20;
			item.useAnimation = 20;
			item.useStyle = 1;
			item.value = 30000;
			item.rare = 6;
			item.UseSound = SoundID.Item79;
			item.noMelee = true;
			item.mountType = mod.MountType("TrexMount");
		}

		
	}
	public class TrexMountB : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Trex Mount");
			Description.SetDefault("Look who's king of the dinosaurs now!");
			Main.buffNoTimeDisplay[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.mount.SetMount(mod.MountType("TrexMount"), player);
			player.buffTime[buffIndex] = 10;
		}
	}
	public class TrexMount : ModMountData
	{
		public override void SetDefaults()
		{
			
			mountData.buff = mod.BuffType("TrexMountB");
			mountData.heightBoost = 80;
			mountData.fallDamage = 0f;
			mountData.runSpeed = 6f;
			mountData.dashSpeed = 8f;
			mountData.flightTimeMax = 0;
			mountData.fatigueMax = 0;
			mountData.jumpHeight = 20;
			mountData.acceleration = 0.19f;
			mountData.jumpSpeed = 4f;
			mountData.blockExtraJumps = false;
			mountData.totalFrames = 4;
			
			mountData.constantJump = true;
			int[] array = new int[mountData.totalFrames];
			for (int l = 0; l < array.Length; l++)
			{
				array[l] = 65;
			}
			mountData.playerYOffsets = array;
			mountData.xOffset = 3;
			mountData.bodyFrame = 3;
			mountData.yOffset = 18;
			mountData.playerHeadOffset = 22;
			mountData.standingFrameCount = 1;
			mountData.standingFrameDelay = 12;
			mountData.standingFrameStart = 0;
			mountData.runningFrameCount = 4;
			mountData.runningFrameDelay = 36;
			mountData.runningFrameStart = 0;
			mountData.flyingFrameCount = 0;
			mountData.flyingFrameDelay = 0;
			mountData.flyingFrameStart = 0;
			mountData.inAirFrameCount = 1;
			mountData.inAirFrameDelay = 12;
			mountData.inAirFrameStart = 0;
			mountData.idleFrameCount = 1;
			mountData.idleFrameDelay = 12;
			mountData.idleFrameStart = 0;
			mountData.idleFrameLoop = true;
			mountData.swimFrameCount = mountData.inAirFrameCount;
			mountData.swimFrameDelay = mountData.inAirFrameDelay;
			mountData.swimFrameStart = mountData.inAirFrameStart;
			
			if (Main.netMode != 2)
			{
				mountData.textureWidth = mountData.backTexture.Width + 20;
				mountData.textureHeight = mountData.backTexture.Height;
			}
		}
		
		public override void UpdateEffects(Player player)
		{
			
		}
	}
}