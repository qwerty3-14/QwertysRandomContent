using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent
{
    public class QwertyMethods
    {
        public static float SlowRotation(float currentRotation, float targetAngle, float speed)
        {
            int f = 1; //this is used to switch rotation direction
            float actDirection =  new Vector2((float)Math.Cos(currentRotation), (float)Math.Sin(currentRotation)).ToRotation();
            targetAngle = new Vector2((float)Math.Cos(targetAngle), (float)Math.Sin(targetAngle)).ToRotation();

            //this makes f 1 or -1 to rotate the shorter distance
            if (Math.Abs(actDirection - targetAngle) > Math.PI)
            {
                f = -1;
            }
            else
            {
                f = 1;
            }

            if (actDirection <= targetAngle + MathHelper.ToRadians(speed * 2) && actDirection >= targetAngle - MathHelper.ToRadians(speed * 2))
            {
                actDirection = targetAngle;
            }
            else if (actDirection <= targetAngle)
            {
                actDirection += MathHelper.ToRadians(speed) * f;
            }
            else if (actDirection >= targetAngle)
            {
                actDirection -= MathHelper.ToRadians(speed) * f;
            }
            actDirection = new Vector2((float)Math.Cos(actDirection), (float)Math.Sin(actDirection)).ToRotation();
            
            return actDirection;
        }
        public static Vector2 PolarVector(float radius, float theta)
        {

            return new Vector2((float)Math.Cos(theta), (float)Math.Sin(theta)) * radius;
        }

        public static void BreakTiles(int i, int j, int width, int height)
        {
            for(int x =0; x < width; x++)
            {
                for(int y =0; y<height; y++)
                {
                    WorldGen.KillTile(i+x, j+y, false, false, true);
                    WorldGen.KillWall(i + x, j + y, false);
                    Main.tile[i + x, j + y].liquid = 0;
                }

            }
        }
        public static bool ClosestNPC(ref NPC target, float maxDistance, Vector2 position, bool ignoreTiles = false, int overrideTarget =-1)
        {
            bool foundTarget = false;
            if(overrideTarget != -1)
            {
                if ((Main.npc[overrideTarget].Center - position).Length() < maxDistance)
                {
                    target = Main.npc[overrideTarget];
                    return true;
                }
                
            }
            for (int k = 0; k < Main.npc.Length; k++)
            {
                NPC possibleTarget = Main.npc[k];
                float distance = (possibleTarget.Center - position).Length();
                if (distance < maxDistance && possibleTarget.active && possibleTarget.chaseable && !possibleTarget.dontTakeDamage && !possibleTarget.friendly && possibleTarget.lifeMax > 5 && !possibleTarget.immortal && (Collision.CanHit(position, 0, 0, possibleTarget.Center, 0, 0)|| ignoreTiles))
                {
                    target = Main.npc[k];
                    foundTarget = true;


                    maxDistance = (target.Center - position).Length();
                }

            }
            return foundTarget;
        }
        public static void ServerClientCheck()
        {
            if (Main.netMode == 1)
            {
                Main.NewText("Client says Hello!", Color.Pink);
            }


            if (Main.netMode == 2) // Server
            {
                NetMessage.BroadcastChatMessage(Terraria.Localization.NetworkText.FromLiteral("Server says Hello!"), Color.Green);
            }
        }
        public static void ServerClientCheck(bool q)
        {
            if (Main.netMode == 1)
            {
                Main.NewText("Client says It's " +  q, Color.Pink);
            }


            if (Main.netMode == 2) // Server
            {
                NetMessage.BroadcastChatMessage(Terraria.Localization.NetworkText.FromLiteral("Server says It's " + q), Color.Green);
            }
        }
        public static void ServerClientCheck(int q)
        {
            if (Main.netMode == 1)
            {
                Main.NewText("Client says It's " + q, Color.Pink);
            }


            if (Main.netMode == 2) // Server
            {
                NetMessage.BroadcastChatMessage(Terraria.Localization.NetworkText.FromLiteral("Server says It's " + q), Color.Green);
            }
        }
        public static void ServerClientCheck(string q)
        {
            if (Main.netMode == 1)
            {
                Main.NewText("Client says It's " + q, Color.Pink);
            }


            if (Main.netMode == 2) // Server
            {
                NetMessage.BroadcastChatMessage(Terraria.Localization.NetworkText.FromLiteral("Server says " + q), Color.Green);
            }
        }
        public static void ServerClientCheck(Vector2 q)
        {
            if (Main.netMode == 1)
            {
                Main.NewText("Client says It's" + q, Color.Pink);
            }


            if (Main.netMode == 2) // Server
            {
                NetMessage.BroadcastChatMessage(Terraria.Localization.NetworkText.FromLiteral("Server says " + q), Color.Green);
            }
        }
        
        public static Item MakeItemFromID(int type)
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
                item = new Item();
                item.SetDefaults(type, true);
                return item;
            }
        }
        public static Projectile PokeNPC(Player player, NPC npc, float damage, float knockback = 0f, bool melee = false, bool ranged = false, bool magic = false, bool summon = false, bool morph = false)
        {
            Projectile p = Main.projectile[Projectile.NewProjectile(npc.Center, Vector2.Zero, QwertysRandomContent.Instance.ProjectileType("Poke"), (int)damage, knockback, player.whoAmI)];
            for (int n = 0; n < 200; n++)
            {
                if (Main.npc[n] != npc)
                {
                    p.localNPCImmunity[n] = -1;
                }
            }
            p.melee = melee;
            p.ranged = ranged;
            p.magic = magic;
            p.minion = summon;
            p.GetGlobalProjectile<MorphProjectile>().morph = morph;
            if (Main.netMode == 1)
            {
                QwertysRandomContent.UpdateProjectileClass(p);
            }
            return p;
        }
    }
    public class Poke : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 2;
            projectile.height = 2;
            
            projectile.friendly = true;
            projectile.timeLeft = 2;
            projectile.usesLocalNPCImmunity = true;
            projectile.tileCollide = false;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            return false;
        }
    }

}
