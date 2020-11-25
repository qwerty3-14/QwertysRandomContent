using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
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

        //used for homing projectile
        public static bool ClosestNPC(ref NPC target, float maxDistance, Vector2 position, bool ignoreTiles = false, int overrideTarget = -1, SpecialCondition specialCondition = null)
        {
            //very advance users can use a delegate to insert special condition into the function like only targetting enemies not currently having local iFrames, but if a special condition isn't added then just return it true
            if (specialCondition == null)
            {
                specialCondition = delegate (NPC possibleTarget) { return true; };
            }
            bool foundTarget = false;
            //If you want to prioritse a certain target this is where it's processed, mostly used by minions that haave a target priority
            if (overrideTarget != -1)
            {
                if ((Main.npc[overrideTarget].Center - position).Length() < maxDistance && !Main.npc[overrideTarget].immortal && (Collision.CanHit(position, 0, 0, Main.npc[overrideTarget].Center, 0, 0) || ignoreTiles) && specialCondition(Main.npc[overrideTarget]))
                {
                    target = Main.npc[overrideTarget];
                    return true;
                }
            }
            //this is the meat of the targetting logic, it loops through every NPC to check if it is valid the miniomum distance and target selected are updated so that the closest valid NPC is selected
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
        public static bool NPCsInRange(ref List<NPC> targets, float maxDistance, Vector2 position, bool ignoreTiles = false, SpecialCondition specialCondition = null)
        {
            //very advance users can use a delegate to insert special condition into the function like only targetting enemies not currently having local iFrames, but if a special condition isn't added then just return it true
            if (specialCondition == null)
            {
                specialCondition = delegate (NPC possibleTarget) { return true; };
            }
            bool foundTarget = false;
            targets = new List<NPC>();
            for (int k = 0; k < Main.npc.Length; k++)
            {
                NPC possibleTarget = Main.npc[k];
                float distance = (possibleTarget.Center - position).Length();
                if (distance < maxDistance && possibleTarget.active && possibleTarget.chaseable && !possibleTarget.dontTakeDamage && !possibleTarget.friendly && possibleTarget.lifeMax > 5 && !possibleTarget.immortal && (Collision.CanHit(position, 0, 0, possibleTarget.Center, 0, 0) || ignoreTiles) && specialCondition(possibleTarget))
                {
                    targets.Add(possibleTarget);
                    foundTarget = true;
                }
            }
            return foundTarget;
        }

        //used by minions to give each minion of the same type a unique identifier so they don't stack
        public static int MinionHordeIdentity(Projectile projectile)
        {
            int identity = 0;
            for (int p = 0; p < 1000; p++)
            {
                if (Main.projectile[p].active && Main.projectile[p].type == projectile.type && Main.projectile[p].owner == projectile.owner)
                {
                    if (projectile.whoAmI == p)
                    {
                        break;
                    }
                    else
                    {
                        identity++;
                    }
                }
            }
            return identity;
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

        public static List<Projectile> ProjectileSpread(Vector2 position, int count, float speed, int type, int damage, float kb, int owner = 255, float ai0 = 0, float ai1 = 0, float rotation = 0f, float spread = (float)Math.PI * 2)
        {
            List<Projectile> me = new List<Projectile>();
            for (int r = 0; r < count; r++)
            {
                float rot = rotation + r * (spread / count) - (spread / 2) + (spread / (2 * count));
                me.Add(Main.projectile[Projectile.NewProjectile(position, PolarVector(speed, rot), type, damage, kb, owner, ai0, ai1)]);
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

        //used for projectiles using ammo, the vanilla PickAmmo had a bunch of clutter we don't need
        public static bool UseAmmo(this Projectile projectile, int ammoID, ref int shoot, ref float speed, ref int Damage, ref float KnockBack, bool dontConsume = false)
        {
            Player player = Main.player[projectile.owner];
            Item item = new Item();
            bool hasFoundAmmo = false;
            for (int i = 54; i < 58; i++)
            {
                if (player.inventory[i].ammo == ammoID && player.inventory[i].stack > 0)
                {
                    item = player.inventory[i];
                    hasFoundAmmo = true;
                    break;
                }
            }
            if (!hasFoundAmmo)
            {
                for (int j = 0; j < 54; j++)
                {
                    if (player.inventory[j].ammo == ammoID && player.inventory[j].stack > 0)
                    {
                        item = player.inventory[j];
                        hasFoundAmmo = true;
                        break;
                    }
                }
            }

            if (hasFoundAmmo)
            {
                shoot = item.shoot;
                if (player.magicQuiver && (ammoID == AmmoID.Arrow || ammoID == AmmoID.Stake))
                {
                    KnockBack = (float)((int)((double)KnockBack * 1.1));
                    speed *= 1.1f;
                }
                speed += item.shootSpeed;
                if (item.ranged)
                {
                    if (item.damage > 0)
                    {
                        Damage += (int)((float)item.damage * player.rangedDamage);
                    }
                }
                else
                {
                    Damage += item.damage;
                }
                if (ammoID == AmmoID.Arrow && player.archery)
                {
                    if (speed < 20f)
                    {
                        speed *= 1.2f;
                        if (speed > 20f)
                        {
                            speed = 20f;
                        }
                    }
                    Damage = (int)((double)((float)Damage) * 1.2);
                }
                KnockBack += item.knockBack;
                bool flag2 = dontConsume;

                if (player.magicQuiver && ammoID == AmmoID.Arrow && Main.rand.Next(5) == 0)
                {
                    flag2 = true;
                }
                if (player.ammoBox && Main.rand.Next(5) == 0)
                {
                    flag2 = true;
                }
                if (player.ammoPotion && Main.rand.Next(5) == 0)
                {
                    flag2 = true;
                }

                if (player.ammoCost80 && Main.rand.Next(5) == 0)
                {
                    flag2 = true;
                }
                if (player.ammoCost75 && Main.rand.Next(4) == 0)
                {
                    flag2 = true;
                }
                if (!flag2 && item.consumable)
                {
                    item.stack--;
                    if (item.stack <= 0)
                    {
                        item.active = false;
                        item.TurnToAir();
                    }
                }
            }
            else
            {
                return false;
            }
            return true;
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
            currentRotation = actDirection;
        }
        public static void SlowRotWhileAvoid(this ref float currentRotation, float targetAngle, float speed, float avoid)
        {
            float actDirection = new Vector2((float)Math.Cos(currentRotation), (float)Math.Sin(currentRotation)).ToRotation();
            targetAngle = new Vector2((float)Math.Cos(targetAngle), (float)Math.Sin(targetAngle)).ToRotation();
            avoid = new Vector2((float)Math.Cos(avoid), (float)Math.Sin(avoid)).ToRotation();


            if (actDirection < 0)
            {
                actDirection += (float)Math.PI * 2;
            }
            if (targetAngle < 0)
            {
                targetAngle += (float)Math.PI * 2;
            }

            actDirection -= avoid;
            targetAngle -= avoid;

            actDirection = new Vector2((float)Math.Cos(actDirection), (float)Math.Sin(actDirection)).ToRotation();
            targetAngle = new Vector2((float)Math.Cos(targetAngle), (float)Math.Sin(targetAngle)).ToRotation();

            if (actDirection < 0)
            {
                actDirection += (float)Math.PI * 2;
            }
            if (targetAngle < 0)
            {
                targetAngle += (float)Math.PI * 2;
            }

            if (actDirection <= targetAngle + speed * 2 && actDirection >= targetAngle - speed * 2)
            {
                actDirection = targetAngle;
            }
            else if (actDirection <= targetAngle)
            {
                actDirection += speed;
            }
            else if (actDirection >= targetAngle)
            {
                actDirection -= speed;
            }

            actDirection += avoid;

            actDirection = new Vector2((float)Math.Cos(actDirection), (float)Math.Sin(actDirection)).ToRotation();
            currentRotation = actDirection;

        }
        public static Vector2 MoveToward(this Vector2 currentPosition, Vector2 here, float speed)
        {
            Vector2 dif = here - currentPosition;
            if(dif.Length() < speed)
            {
                return dif;
            }
            else
            {
                return dif.SafeNormalize(-Vector2.UnitY) * speed;
            }
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