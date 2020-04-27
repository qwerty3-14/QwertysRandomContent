using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace QwertysRandomContent
{
    public class QwertyMethods
    {
        public static float SlowRotation(float currentRotation, float targetAngle, float speed)
        {
            int f = 1; //this is used to switch rotation direction
            float actDirection = new Vector2((float)Math.Cos(currentRotation), (float)Math.Sin(currentRotation)).ToRotation();
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
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    WorldGen.KillTile(i + x, j + y, false, false, true);
                    WorldGen.KillWall(i + x, j + y, false);
                    Main.tile[i + x, j + y].liquid = 0;
                }

            }
        }
        public delegate bool SpecialCondition(NPC possibleTarget);
        public static bool ClosestNPC(ref NPC target, float maxDistance, Vector2 position, bool ignoreTiles = false, int overrideTarget = -1, SpecialCondition specialCondition = null)
        {
            if (specialCondition == null)
            {
                specialCondition = delegate (NPC possibleTarget) { return true; };
            }
            bool foundTarget = false;
            if (overrideTarget != -1)
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
                if (distance < maxDistance && possibleTarget.active && possibleTarget.chaseable && !possibleTarget.dontTakeDamage && !possibleTarget.friendly && possibleTarget.lifeMax > 5 && !possibleTarget.immortal && (Collision.CanHit(position, 0, 0, possibleTarget.Center, 0, 0) || ignoreTiles) && specialCondition(possibleTarget))
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
                Main.NewText("Client says it's " + q, Color.Pink);
            }


            if (Main.netMode == 2) // Server
            {
                NetMessage.BroadcastChatMessage(Terraria.Localization.NetworkText.FromLiteral("Server says it's " + q), Color.Green);
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
                NetMessage.BroadcastChatMessage(Terraria.Localization.NetworkText.FromLiteral("Server says it's " + q), Color.Green);
            }
        }
        public static void ServerClientCheck(string q)
        {
            if (Main.netMode == 1)
            {
                Main.NewText("Client says  " + q, Color.Pink);
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
                Main.NewText("Client says it's" + q, Color.Pink);
            }


            if (Main.netMode == 2) // Server
            {
                NetMessage.BroadcastChatMessage(Terraria.Localization.NetworkText.FromLiteral("Server says it's" + q), Color.Green);
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
        public static float AngularDifference(float angle1, float angle2)
        {
            angle1 = PolarVector(1f, angle1).ToRotation();
            angle2 = PolarVector(1f, angle2).ToRotation();
            if (Math.Abs(angle1 - angle2) > Math.PI)
            {
                return (float)Math.PI * 2 - Math.Abs(angle1 - angle2);
            }
            return Math.Abs(angle1 - angle2);
        }
        public static void SpawnBoss(Player player, int type)
        {
            if(Main.netMode==0)
            {
                int num7 = NPC.NewNPC((int)player.Center.X, (int)player.Center.Y - 2000, type);
                Main.NewText(Language.GetTextValue("Announcement.HasAwoken", Main.npc[num7].TypeName), 175, 75, 255, false);
                
            }
            else if(Main.netMode ==1)
            {
                
                ModPacket packet = QwertysRandomContent.Instance.GetPacket();
                packet.Write((byte)ModMessageType.SummonBoss);
                packet.WriteVector2(player.Center);
                packet.Write(type);
                packet.Send();
            }
        }
        public static List<Projectile> ProjectileSpread(Vector2 position, int count, float speed, int type, int damage, float kb, int owner = 255, float ai0=0, float ai1=0, float rotation = 0f, float spread = (float)Math.PI*2 )
        {
            List<Projectile> me = new List<Projectile>();
            for(int r =0; r < count; r++)
            {
                float rot = rotation + r *(spread / count) - (spread/2) + (spread / (2*count));
                me.Add(Main.projectile[Projectile.NewProjectile(position, PolarVector(speed, rot).SafeNormalize(-Vector2.UnitY), type, damage, kb, owner, ai0, ai1)]);
            }
            return me;
        }
    }
    public static class StaticQwertyMethods
    {
        public static void FriendlyFire(this Projectile projectile) //allows friendly projectile to hit player and cause pvp death (like the grenade explosion)
        {
            Rectangle myRect = new Rectangle((int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.height);
            int myPlayer = projectile.owner;
            if (Main.player[myPlayer].active && !Main.player[myPlayer].dead && !Main.player[myPlayer].immune && (!projectile.ownerHitCheck || projectile.CanHit(Main.player[myPlayer])))
            {
                Rectangle value = new Rectangle((int)Main.player[myPlayer].position.X, (int)Main.player[myPlayer].position.Y, Main.player[myPlayer].width, Main.player[myPlayer].height);
                if (myRect.Intersects(value))
                {
                    if (Main.player[myPlayer].position.X + (float)(Main.player[myPlayer].width / 2) < projectile.position.X + (float)(projectile.width / 2))
                    {
                        projectile.direction = -1;
                    }
                    else
                    {
                        projectile.direction = 1;
                    }
                    int num4 = Main.DamageVar((float)projectile.damage);
                    projectile.StatusPlayer(myPlayer);
                    Main.player[myPlayer].Hurt(PlayerDeathReason.ByProjectile(projectile.owner, projectile.whoAmI), num4, projectile.direction, true, false, false, -1);
                }
            }
        }
        public static Vector2 to2(this Vector3 vector3)
        {
            return new Vector2(vector3.X, vector3.Y);
        }
        public static Vector3 add2(this Vector3 vector3, Vector2 vector2)
        {
            return new Vector3(vector3.X + vector2.X, vector3.Y + vector2.Y, vector3.Z);
        }
        public static void SlowRotation(this ref float currentRotation, float targetAngle, float speed)
        {

            int f = 1; //this is used to switch rotation direction
            float actDirection = new Vector2((float)Math.Cos(currentRotation), (float)Math.Sin(currentRotation)).ToRotation();
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

            if (actDirection <= targetAngle + speed * 2 && actDirection >= targetAngle - speed * 2)
            {
                actDirection = targetAngle;

            }
            else if (actDirection <= targetAngle)
            {
                actDirection += speed * f;
            }
            else if (actDirection >= targetAngle)
            {
                actDirection -= speed * f;
            }
            actDirection = new Vector2((float)Math.Cos(actDirection), (float)Math.Sin(actDirection)).ToRotation();
            /*
            if(float.IsNaN(actDirection))
            {
                actDirection = 0;
            }*/
            currentRotation = actDirection;

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
