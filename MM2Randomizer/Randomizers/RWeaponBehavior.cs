using MM2Randomizer.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace MM2Randomizer.Randomizers
{
    public class RWeaponBehavior
    {
        public List<ESoundID> Sounds;

        // Buster Heat Air Wood Bubble Quick Metal Clash
        public static List<double> AmmoUsage;

        private StringBuilder debug;
        public override string ToString()
        {
            return debug.ToString();
        }

        public RWeaponBehavior(Random r)
        {
            debug = new StringBuilder();

            Sounds = new List<ESoundID>(new ESoundID[] {
                ESoundID.WeaponF,
                ESoundID.HeatmanUnused,
                ESoundID.WeaponM,
                ESoundID.WeaponP,
                ESoundID.Shotman,
                ESoundID.TakeDamage,
                ESoundID.QuickBeam,
                ESoundID.Refill,
                ESoundID.MegaLand,
                ESoundID.DamageEnemy,
                ESoundID.Dragon,
                ESoundID.Tink,
                ESoundID.ClashAttach,
                ESoundID.Cursor,
                ESoundID.TeleportIn,
                ESoundID.WeaponW,
                ESoundID.Pause,
                ESoundID.WeaponH_Charge0,
                ESoundID.WeaponH_Shoot,
                ESoundID.FlyBoy,
                ESoundID.TeleportOut,
                ESoundID.Splash,
                ESoundID.Yoku,
                ESoundID.Droplet1,
                ESoundID.WeaponA,
                ESoundID.Unknown1,
                ESoundID.Death,
                ESoundID.OneUp,
            });
            
            AmmoUsage = new List<double>();
            AmmoUsage.Add(0); // Buster is free

            debug.AppendLine("Weapon Behavior");
            using (var stream = new FileStream(RandomMM2.DestinationFileName, FileMode.Open, FileAccess.ReadWrite))
            {
                ChangeHeat(r, stream);
                ChangeAir(r, stream);
                ChangeWood(r, stream);
                ChangeBubble(r, stream);
                ChangeQuick(r, stream);
                ChangeFlash(r, stream);
                ChangeMetal(r, stream);
                ChangeClash(r, stream);
                ChangeItem1(r, stream);
            }

            debug.AppendLine("\nAmmo Usage");
            debug.AppendLine("P     H     A     W     B     Q     M     C");
            debug.AppendLine("-----------------------------------------------");
            foreach (double w in AmmoUsage)
            {
                debug.Append(String.Format("{0:0.00}  ", w));
            }
            debug.AppendLine("\n");
        }

        /// <summary>
        /// Get a random unique sound for a weapon to use
        /// </summary>
        /// <param name="r"></param>
        /// <returns>Sound ID byte</returns>
        private ESoundID GetRandomSound(Random r)
        {
            int i = r.Next(Sounds.Count);
            ESoundID sound = Sounds.ElementAt(i);
            Sounds.RemoveAt(i);

            // Pick a random charge level if charge sound is chosen
            if (sound == ESoundID.WeaponH_Charge0)
            {
                i = r.Next(3);
                sound = ESoundID.WeaponH_Charge0 + i;
            }

            debug.AppendLine(Enum.GetName(typeof(ESoundID), sound));
            return sound;
        }

        public void ChangeHeat(Random r, FileStream stream)
        {
            debug.AppendLine("-------- Atomic Fire ---------");

            //0x03DE55 - H L1 Ammo use(01)
            //0x03DE56 - H L2 Ammo use(06)
            //0x03DE57 - H L3 Ammo use(0A)
            // 50% chance for L1 to be free
            // L2 and L3 will cost 1-5 more each
            double rTestL1Ammo = r.NextDouble();
            if (rTestL1Ammo > 0.5)
            {
                debug.AppendLine("(H) | Shot L1 Ammo Cost: 00");
                stream.Position = 0x03DE55;
                stream.WriteByte(0x00);
            }
            else
            {
                debug.AppendLine("(H) | Shot L1 Ammo Cost: 01");
            }
            stream.Position = 0x03DE56;
            int ammoMax = 0;
            for (int i = 0; i < 2; i++)
            {
                int ammoUse = r.Next(0x05) + 0x01;
                ammoMax += ammoUse;
                stream.WriteByte((byte)ammoMax);
                debug.AppendLine(String.Format("(H) | Shot L{0} Ammo Cost: {1}", i + 2, ammoMax));
            }
            AmmoUsage.Add(ammoMax);

            //0x03DDEC - H shot sound effect(38)
            debug.Append("(H) | Shot Sound: ");
            stream.Position = 0x03DDEC;
            stream.WriteByte((byte)GetRandomSound(r));

            //0x03DDF1 - H x - speed (04, all levels)
            //    Do from 02 to 08
            int xVel = r.Next(0x07) + 0x02;
            debug.AppendLine("(H) | X-Velocity : " + xVel);
            stream.Position = 0x03DDF1;
            stream.WriteByte((byte)xVel);

            //0x03DE45 - H charge sound 1(35) Unused
            //0x03DE46 - H charge sound 2(35)
            stream.Position = 0x03DE46;
            debug.Append("(H) | L1 Sound: ");
            stream.WriteByte((byte)GetRandomSound(r));
            //0x03DE47 - H charge sound 3(36)
            stream.Position = 0x03DE47;
            debug.Append("(H) | L2 Sound: ");
            stream.WriteByte((byte)GetRandomSound(r));
            //0x03DE48 - H charge sound 4(37)
            stream.Position = 0x03DE48;
            debug.Append("(H) | L3 Sound: ");
            stream.WriteByte((byte)GetRandomSound(r));

            debug.AppendLine();
        }

        public void ChangeAir(Random r, FileStream stream)
        {
            debug.AppendLine("-------- Air Shooter ---------");

            //0x03DAD6 - A num projectiles, default 0x04
            //  Values 0x02 and 0x03 work, but larger values behave strangely
            int numProjectiles = 0x04;
            double rTestNumProjectiles = r.NextDouble();
            if (rTestNumProjectiles > 0.80)
                numProjectiles = 0x03;
            else if (rTestNumProjectiles > 0.60)
                numProjectiles = 0x02;
            debug.AppendLine(String.Format("(A) | Projectiles: {0:X2}", numProjectiles));
            stream.Position = 0x03DAD6;
            stream.WriteByte((byte)numProjectiles);

            //0x03DADA - A projectile type, default 0x02
            //  Can use this to change behavior completely!Buggy though.

            //0x03DAE6 - A sound effect (0x3F)
            stream.Position = 0x03DAE6;
            debug.Append("(A) | Sound: ");
            stream.WriteByte((byte)GetRandomSound(r));

            //0x03DAEE - A ammo used(0x02)
            int ammoUse = r.Next(0x02) + 0x01;
            stream.Position = 0x03DAEE;
            stream.WriteByte((byte)ammoUse);
            debug.AppendLine(String.Format("(A) | Ammo Use: {0:X2}", ammoUse));
            AmmoUsage.Add(ammoUse);

            //0x03DE6E - A projectile y-acceleration fraction(10)
            // Do 0x02 to 0x32, where values above 0x10 are less common
            int yAccFrac = r.Next(0x17) + 0x02;
            if (yAccFrac > 0x10)
            {
                // double the addition of any acceleration value chosen above 0x10
                yAccFrac += (yAccFrac - 0x10) * 2; 
            }
            debug.AppendLine(String.Format("(A) | Y-Acceleration (fraction): {0:X2}", yAccFrac));
            stream.Position = 0x03DE6E;
            stream.WriteByte((byte)yAccFrac);

            //0x03DE76 - A projectile y-acceleration integer(00)
            // 15% chance to be significantly faster
            int yAccInt = 0x00;
            double rYAcc = r.NextDouble();
            if (rYAcc > 0.85)
                yAccInt = 0x01;
            debug.AppendLine(String.Format("(A) | Y-Acceleration (integer): {0:X2}", yAccInt));
            stream.Position = 0x03DE76;
            stream.WriteByte((byte)yAccInt);

            //0x03DE7E - A x - speed fraction projectile 1(19)
            //0x03DE7F - A x - speed fraction projectile 2(99)
            //0x03DE80 - A x - speed fraction projectile 3(33)
            stream.Position = 0x03DE7E;
            for (int i = 0; i < 3; i++)
            {
                int xFracSpeed = r.Next(0xFF) + 0x01;
                debug.AppendLine(String.Format("(A) | Projectile {0} X-Velocity (fraction): {1:X2}", i, xFracSpeed));
                stream.WriteByte((byte)xFracSpeed);
            }

            //0x03DE81 - A x - speed integer projectile 1(01)
            //0x03DE82 - A x - speed integer projectile 2(01)
            //0x03DE83 - A x - speed integer projectile 3(02)
            int[] xIntSpeeds = new int[] {
                0x00, 0x01, 0x02, 0x04, 0x06
            };
            int rIndex = 0;
            int xIntSpeed = 0;
            stream.Position = 0x03DE81;
            for (int i = 0; i < 3; i++)
            {
                rIndex = r.Next(xIntSpeeds.Length);
                xIntSpeed = xIntSpeeds[rIndex];
                debug.AppendLine(String.Format("(A) | Projectile {0} X-Velocity (integer): {1:X2}", i, xIntSpeed));
                stream.WriteByte((byte)xIntSpeed);
            }

            debug.AppendLine();
        }

        public void ChangeWood(Random r, FileStream stream)
        {
            debug.AppendLine("-------- Leaf Shield ---------");

            //0x03DEDA - W deploy time (0C)
            //    Can change from 06 to 12
            //    Note: Shield glitches on odd numbers.  Use evens only.
            int[] deployDelays = new int[] {
                0x00, 0x02, 0x04, 0x06, 0x08, 0x0A, 0x0C, 0x10, 0x14, 0x1C, 0x22
            };
            int rIndex = r.Next(deployDelays.Length);
            int deployDelay = deployDelays[rIndex];
            stream.Position = 0x03DEDA;
            stream.WriteByte((byte)deployDelay);
            debug.AppendLine(String.Format("(W) | Deploy Delay: {0:X2}", deployDelay));

            //0x03DF0D - W spin animation? (01)

            //0x03DF1B - W delay between sounds(07)
            // TODO

            //0x03DF1F - W deploy sound effect
            stream.Position = 0x03DF1F;
            debug.Append("(W) | Deploy Sound: ");
            stream.WriteByte((byte)GetRandomSound(r));

            //0x03DF41 - W which directions the shield is allowed to launch in (F0)
            //    Can prevent launching left / right, up / down, etc

            //0x03DF4D - W launch directions (C0)
            //    Don't use this one

            //0x03DF50 - W launch x - direction subroutine
            //    50% chance to have inverted x controls
            //    Change "LSR AND #40" (4A 29 40) to simply "AND #40" to implement
            double rTestReverseX = r.NextDouble();
            if (rTestReverseX > 0.5)
            {
                debug.AppendLine(String.Format("(W) | Launch X-Direction: Reverse"));
                stream.Position = 0x03DF50;
                stream.WriteByte(0x29); // AND
                stream.WriteByte(0x40); // #$40

                stream.WriteByte(0xEA); // NOP (best thing i could find to simply "skip" a line)
            }
            else
            {
                debug.AppendLine(String.Format("(W) | Launch X-Direction: Normal"));
            }

            //0x03DF59 - W x - speed(04) (do 0x02-0x08)
            int launchVel = r.Next(0x06) + 0x02;
            stream.Position = 0x03DF59;
            stream.WriteByte((byte)launchVel);
            debug.AppendLine(String.Format("(W) | Launch X-Velocity: {0:X2}", launchVel));

            //0x03DF64 - W launch y - direction(10)
            //  Change to 0x20 to reverse, 50% chance
            int reverseY = 0x10;
            double rTestReverseY = r.NextDouble();
            if (rTestReverseY > 0.5)
                reverseY = 0x20;
            stream.Position = 0x03DF64;
            stream.WriteByte((byte)reverseY);
            debug.AppendLine(String.Format("(W) | Launch Y-Direction: {0:X2}", reverseY));

            //0x03DF72 - W ammo usage (3) (do from 1 to 3)
            int ammoUse = r.Next(0x03) + 0x01;
            stream.Position = 0x03DF72;
            stream.WriteByte((byte)ammoUse);
            AmmoUsage.Add(ammoUse);
            debug.AppendLine(String.Format("(W) | Ammo Usage: {0:X2}", ammoUse));

            //0x03DF7D - W y - speed(04)
            stream.Position = 0x03DF7D;
            stream.WriteByte((byte)launchVel);
            debug.AppendLine(String.Format("(W) | Launch Y-Velocity: {0:X2}\n", launchVel));
        }

        public void ChangeBubble(Random r, FileStream stream)
        {
            debug.AppendLine("-------- Bubble Lead ---------");

            //0x03D4AB - B x - speed on shoot (0x01) (do 1-3)
            int xVelShoot = r.Next(0x03) + 0x01;
            stream.Position = 0x03D4AB;
            stream.WriteByte((byte)xVelShoot);
            debug.AppendLine(String.Format("(B) | X-Velocity Shoot: {0:X2}", xVelShoot));

            //0x03D4CF - B y - speed on shoot(0x02) (do 0-6)
            int yVelShoot = r.Next(0x06);
            stream.Position = 0x03D4CF;
            stream.WriteByte((byte)yVelShoot);
            debug.AppendLine(String.Format("(B) | Y-Velocity Shoot: {0:X2}", yVelShoot));

            //0x03DB21 - B max shots (0x03) (do 2-5, i.e. 1-4 total projectiles)
            //    Valid from 0x02 - 0x0F. Lags a bunch >= 0x06.
            int maxShots = r.Next(0x04) + 0x02;
            stream.Position = 0x03DB21;
            stream.WriteByte((byte)maxShots);
            debug.AppendLine(String.Format("(B) | Max Shots: {0:X2}", maxShots));

            //0x03DB2F - B weapon type(0x04)
            // Don't do

            //0x03DB34 - B sound effect
            stream.Position = 0x03DB34;
            debug.Append("(B) | Sound: ");
            stream.WriteByte((byte)GetRandomSound(r));

            //0x03DB3D - B shots per ammo tick (0x02) (do 1-4)
            int magSize = r.Next(0x04) + 0x01;
            stream.Position = 0x03DB3D;
            stream.WriteByte((byte)magSize);
            AmmoUsage.Add(1d / (double)magSize);
            debug.AppendLine(String.Format("(B) | Shots Per Ammo Tick: {0:X2}", magSize));

            //0x03DFA4 - B y - pos to embed in surface(0xFF)
            // Dumb

            //0x03DFA9 - B x - speed on surface (0x02)
            //      0x01 - 0x04 ?
            int xVelRoll = r.Next(0x04) + 0x01;
            stream.Position = 0x03DFA9;
            stream.WriteByte((byte)xVelRoll);
            debug.AppendLine(String.Format("(B) | X-Velocity Surface: {0:X2}", xVelRoll));

            //0x03DFC0 - B x - speed after falling from ledge (0x00)
            //      Make 50% chance to be 0, or 1-5
            int xVelFall = 0x00;
            double rTestXFallSpeed = r.NextDouble();
            if (rTestXFallSpeed > 0.5)
                xVelFall = r.Next(0x05) + 0x01;
            stream.Position = 0x03DFC0;
            stream.WriteByte((byte)xVelFall);
            debug.AppendLine(String.Format("(B) | X-Velocity Fall: {0:X2}", xVelFall));

            //0x03DFC8 - B y - speed after falling(0xFE)
            //      Either 0xFA - 0xFF or 0x01 - 0x06
            int[] yFallVels = new int[] { 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06 };
            int rIndex = r.Next(yFallVels.Length);
            int yFallVel = yFallVels[rIndex];
            stream.Position = 0x03DFC8;
            stream.WriteByte((byte)yFallVel);
            debug.AppendLine(String.Format("(B) | Y-Velocity Fall: {0:X2}\n", yFallVel));
        }

        public void ChangeQuick(Random r, FileStream stream)
        {
            debug.AppendLine("------- Quick Boomerang --------");

            // Q autofire delay, default 0x0B
            //    Do from 0x05 to 0x12
            int autoFireDelay = r.Next(0x0D) + 0x05;
            stream.Position = 0x03DB54;
            stream.WriteByte((byte)autoFireDelay);
            debug.AppendLine(String.Format("(Q) | Autofire Delay: {0:X2}", autoFireDelay));

            // Q max shots, default 0x05
            //    Do from 0x03 to 0x07(2 to 6 shots)
            int maxShots = r.Next(0x04) + 0x03;
            stream.Position = 0x03DB5C;
            stream.WriteByte((byte)maxShots);
            debug.AppendLine(String.Format("(Q) | Max Shots: {0:X2}", maxShots));

            // 0x03DB6F - Q sound effect
            stream.Position = 0x03DB6F;
            debug.Append("(Q) | Sound: ");
            stream.WriteByte((byte)GetRandomSound(r));

            // Q shots per ammo tick, default 0x08
            //    Do from 0x04 to 0x0A ?
            int magSize = r.Next(0x06) + 0x04;
            stream.Position = 0x03DB78;
            stream.WriteByte((byte)magSize);
            AmmoUsage.Add(1d / (double)magSize);
            debug.AppendLine(String.Format("(Q) | Shots Per Ammo Tick: {0:X2}", magSize));

            // Q behavior, distance, default 0x12
            //    Do from 0x0A to 0x20 ?
            int distance = r.Next(0x16) + 0x0A;
            stream.Position = 0x03DFE2;
            stream.WriteByte((byte)distance);
            debug.AppendLine(String.Format("(Q) | Travel Distance: {0:X2}", distance));

            // Q behavior, initial angle, default 0x4B
            //    Do from 0x00 to 0x60 ?
            int angle1 = r.Next(0x60);
            stream.Position = 0x03DFEA;
            stream.WriteByte((byte)angle1);
            debug.AppendLine(String.Format("(Q) | Initial Angle: {0:X2}", angle1));

            //0x03DFF2 - Q behavior, weird, default 0x00
            //    Don't use, but change to 0x01 for dumb effect

            // Q behavior, angle, default 0x40
            //    0x40 - (GOOD)Normal
            //    0x80 - (GOOD / HARD) Disappears(doesn't return)
            //    0x00 - (GOOD)Sine wave
            //    0x03 - (GOOD)Float downwards(interesting behavior when changing other byte)
            //    0x04, 05 - Float downwards(short, not different enough from 03)
            //    0x06 - Float downwards(faster)
            int[] angle2s = new int[] { 0x40, 0x80, 0x00, 0x03 };
            int rIndex = r.Next(angle2s.Length);
            int angle2 = angle2s[rIndex];
            stream.Position = 0x03DFFF;
            stream.WriteByte((byte)angle2);
            debug.AppendLine(String.Format("(Q) | Secondary Angle: {0:X2}", angle2));

            // Q behavior, time before disappearing on return, default 0x23
            //    Do from 0x1E to 0x30
            int despawnDelay = r.Next(0x12) + 0x1E;
            stream.Position = 0x03E007;
            stream.WriteByte((byte)despawnDelay);
            debug.AppendLine(String.Format("(Q) | Despawn Delay: {0:X2}", despawnDelay));

            // Q behavior, return angle, default 0x4B
            //    Do from 0x00 to 0x90
            int angle3 = r.Next(0x90);
            stream.Position = 0x03E013;
            stream.WriteByte((byte)angle3);
            debug.AppendLine(String.Format("(Q) | Return Angle: {0:X2}\n", angle3));

            //0x03E01B - Q behavior, weird, default 0x00
            //    Change to 0x01 for interesting effects
        }

        public void ChangeFlash(Random r, FileStream stream)
        {
            debug.AppendLine("-------- Time Stopper ---------");

            //0x03DC59 - F sound (21)
            stream.Position = 0x03DC59;
            debug.Append("(F) | Sound: ");
            stream.WriteByte((byte)GetRandomSound(r));

            //0x03E172 - F custom subroutine for reusable weapon
            // 75% chance to occur
            debug.Append("(F) | Reusable Time-Stopper: ");
            double rTestFChange = r.NextDouble();
            if (rTestFChange > 0.25)
            {
                debug.AppendLine("Yes");

                // 0x03E175 - New ammo-usage address.
                byte[] ammos = new byte[] { 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08};
                int rIndex = r.Next(7);
                byte ammo = ammos[rIndex];
                debug.AppendLine(String.Format("    | >>> Ammo Usage: {0:X2}", ammos[rIndex]));

                // Change ammo-tick subroutine into one that resumes time
                stream.Position = 0x03E172;
                byte[] sub = new byte[]
                {
                    0xA5, 0xA1,         // LDA ammoFlash = #$0A
                    0xE9, ammo,         // SBC {ammo use}
                    0x85, 0xA1,         // STA ammoFlash = #$0A
                    0x5E, 0x20, 0x04,   // LSR megaXDir, X @ megaXDir = #$C0
                    0xA9, 0x00,         // LDA #$00
                    0x85, 0xAA,         // STA $00AA = #$00
                    0x85, 0x50,         // STA $0050 = #$00 ; we erased LDA #$01, but doesn't seem to hurt 
                };
                stream.Write(sub, 0, sub.Length);

                // 0x03E16E and 0x03D49D is the new duration, in frames. They must both be the same value.
                int duration = 0x01;
                switch (ammo)
                {
                    case 0x02:
                        duration = r.Next(70) + 20;
                        break;
                    case 0x03:
                        duration = r.Next(60) + 40;
                        break;
                    case 0x04:
                        duration = r.Next(60) + 60;
                        break;
                    case 0x05:
                        duration = r.Next(60) + 80;
                        break;
                    case 0x06:
                        duration = r.Next(60) + 100;
                        break;
                    case 0x07:
                        duration = r.Next(85) + 120;
                        break;
                    case 0x08:
                        duration = r.Next(115) + 140;
                        break;
                    default: break;
                }
                stream.Position = 0x03E16E;
                stream.WriteByte((byte)duration);
                stream.Position = 0x03D49D;
                stream.WriteByte((byte)duration);
                debug.AppendLine(String.Format("    | >>> Freeze Duration: {0:X2}", duration));

                // Finally, a fix is needed to prevent ammo underflow
                // 0x03DC41 - WpnMove_FStart
                stream.Position = 0x03DC41;
                sub = new byte[]
                {
                    0xA2, 0x02,         // LDX #$02
                    0xAD, 0x22, 0x04,   // LDA $0422 = #$61
                    0x30, 0x1E,         // BMI WpnMove_Done    
                    0xA5, 0xA1,         // LDA ammoFlash = #$02
                    0xC9, 0x07,         // CMP #$07            
                    0x90, 0x18,         // BCC WpnMove_Done    
                };
                stream.Write(sub, 0, sub.Length);
            }
            else
            {
                debug.AppendLine("No");

                // Standard Time Stopper, but modify the tick frequency
                // Default 0x0F. For 28 ticks, that's 7 seconds. Modify to be 4-10 seconds, which is about 0x09 to 0x16
                int tickDelay = r.Next(0x13) + 9;
                stream.Position = 0x03E16E;
                stream.WriteByte((byte)tickDelay);
                stream.Position = 0x03D49D;
                stream.WriteByte((byte)tickDelay);
                debug.AppendLine(String.Format("    | >>> Tick Delay: {0:X2}", tickDelay));
            }
            debug.AppendLine();
        }

        public void ChangeMetal(Random r, FileStream stream)
        {
            debug.AppendLine("-------- Metal Blade ---------");

            //0x03DBB6 - M max shots (04) (change to 0x02-0x05, or 1-4)
            int maxShots = r.Next(0x04) + 0x02;
            stream.Position = 0x03DBB6;
            stream.WriteByte((byte)maxShots);
            debug.AppendLine(String.Format("(M) | Max Shots: {0:X2}", maxShots));

            //0x03DBC9 - M sound effect(23)
            stream.Position = 0x03DBC9;
            debug.Append("(M) | Sound: ");
            stream.WriteByte((byte)GetRandomSound(r));

            //0x03DBD2 - M shots per ammo tick(04) (change to 1-5)
            int magSize = r.Next(0x05) + 0x01;
            stream.Position = 0x03DBD2;
            stream.WriteByte((byte)magSize);
            AmmoUsage.Add(1d / (double)magSize);
            debug.AppendLine(String.Format("(M) | Shots Per Ammo Tick: {0:X2}", magSize));

            // Speeds.  Change each to be 2-7.  Diagonal will be half each, rounded up.
            int velX = r.Next(0x06) + 0x02;
            int velY = r.Next(0x06) + 0x02;
            int halfY = (int)Math.Ceiling((double)velY / 2d);
            int halfX = (int)Math.Ceiling((double)velX / 2d);

            //0x03DC12 - M y - speed, holding up(04)
            stream.Position = 0x03DC12;
            stream.WriteByte((byte)velY);
            debug.AppendLine(String.Format("(M) | Y-Velocity Up: {0:X2}", velY));

            //0x03DC31 - M x - speed, no direction(04)
            stream.Position = 0x03DC31;
            stream.WriteByte((byte)velX);
            debug.AppendLine(String.Format("(M) | X-Velocity Neutral: {0:X2}", velX));

            //0x03DC35 - M x - speed, holding left(04)
            stream.Position = 0x03DC35;
            stream.WriteByte((byte)velX);
            debug.AppendLine(String.Format("(M) | X-Velocity Left: {0:X2}", velX));

            //0x03DC39 - M x - speed, holding right(04)
            stream.Position = 0x03DC39;
            stream.WriteByte((byte)velX);
            debug.AppendLine(String.Format("(M) | X-Velocity Right: {0:X2}", velX));

            //0x03DC13 - M y - speed, holding down(FC)
            stream.Position = 0x03DC13;
            stream.WriteByte((byte)((byte)0x00 - (byte)velY));
            debug.AppendLine(String.Format("(M) | Y-Velocity Down: {0:X2}", velY));

            //0x03DC16 - M y - speed, holding up + left(02)
            stream.Position = 0x03DC16;
            stream.WriteByte((byte)halfY);
            debug.AppendLine(String.Format("(M) | Y-Velocity Up+Left: {0:X2}", velY));

            //0x03DC17 - M y - speed, holding down + left(FD)
            stream.Position = 0x03DC17;
            stream.WriteByte((byte)(0x00 - (byte)halfY));
            debug.AppendLine(String.Format("(M) | Y-Velocity Down+Left: {0:X2}", velY));

            //0x03DC1A - M y - speed, holding up + right(02)
            stream.Position = 0x03DC1A;
            stream.WriteByte((byte)halfY);
            debug.AppendLine(String.Format("(M) | Y-Velocity Up+Right: {0:X2}", velY));

            //0x03DC1B - M y - speed, holding down + right(FD)
            stream.Position = 0x03DC1B;
            stream.WriteByte((byte)((byte)0x00 - (byte)halfY));
            debug.AppendLine(String.Format("(M) | Y-Velocity Down+Right: {0:X2}", velY));

            //0x03DC36 - M x - speed, holding up + left(02)
            stream.Position = 0x03DC36;
            stream.WriteByte((byte)halfX);
            debug.AppendLine(String.Format("(M) | X-Velocity Up+Left: {0:X2}", velX));

            //0x03DC37 - M x - speed, holding down + left(02)
            stream.Position = 0x03DC37;
            stream.WriteByte((byte)halfX);
            debug.AppendLine(String.Format("(M) | X-Velocity Down+Left: {0:X2}", velX));

            //0x03DC3A - M x - speed, holding up + right(02)
            stream.Position = 0x03DC3A;
            stream.WriteByte((byte)halfX);
            debug.AppendLine(String.Format("(M) | X-Velocity Up+Right: {0:X2}", velX));

            //0x03DC3B - M x - speed, holding down + right(02)
            stream.Position = 0x03DC3B;
            stream.WriteByte((byte)halfX);
            debug.AppendLine(String.Format("(M) | X-Velocity Down+Right: {0:X2}\n", velX));
        }

        public void ChangeClash(Random r, FileStream stream)
        {
            debug.AppendLine("-------- Clash Bomber ---------");

            //0x03D4AD - C x-speed on shoot (04) (do 2-7)
            int xVel = r.Next(0x06) + 0x02;
            stream.Position = 0x03D4AD;
            stream.WriteByte((byte)xVel);
            debug.AppendLine(String.Format("(C) | X-Velocity: {0:X2}", xVel));

            //0x03D4D7 - C y-speed integer for explosion (up only) (0)
            // TODO: Figure how this works more to apply in all directions
            // For now, 25% chance to make this move upward at 2px/fr
            int yVelExplode = 0x00;
            double rTestYVelExplode = r.NextDouble();
            if (rTestYVelExplode > 0.75)
                yVelExplode = r.Next(2) + 0x01;
            stream.Position = 0x03D4D7;
            stream.WriteByte((byte)yVelExplode);
            debug.AppendLine(String.Format("(C) | Y-Velocity (Explosion): {0:X2}", yVelExplode));

            //0x03DB99 - C ammo per shot (04) (do 1-4)
            int ammoUse = r.Next(0x04) + 0x01;
            stream.Position = 0x03DB99;
            stream.WriteByte((byte)ammoUse);
            AmmoUsage.Add(ammoUse);
            debug.AppendLine(String.Format("(C) | Ammo Usage: {0:X2}", ammoUse));

            // 0x03DB9F - C explosion type? (02)
            // Change to 03 to "single explosion" type. Most other values break the game.
            // For now, 50% chance to change
            int multiExplode = 0x02;
            double rTestMultiExplode = r.NextDouble();
            if (rTestMultiExplode > 0.50)
                multiExplode = 0x03;
            stream.Position = 0x03DB9F;
            stream.WriteByte((byte)multiExplode);
            debug.AppendLine(String.Format("(C) | Explosion Type: {0:X2}", multiExplode));

            //0x03DBA6 - C shoot sound effect (24)
            stream.Position = 0x03DBA6;
            debug.Append("(C) | Sound Shoot: ");
            stream.WriteByte((byte)GetRandomSound(r));

            //0x03E089 - C attach sound effect (2E)
            stream.Position = 0x03E089;
            debug.Append("(C) | Sound Attach: ");
            stream.WriteByte((byte)GetRandomSound(r));

            //0x03E09C - C delay before explosion (7E) (do 01 to C0)
            int delayExplosion = r.Next(0xBF) + 0x01;
            stream.Position = 0x03E09C;
            stream.WriteByte((byte)delayExplosion);
            debug.AppendLine(String.Format("(C) | Explode Delay: {0:X2}", delayExplosion));

            //0x03E0DA - C explode sound effect
            stream.Position = 0x03E0DA;
            debug.Append("(C) | Sound Explode: ");
            stream.WriteByte((byte)GetRandomSound(r));

            debug.AppendLine();
        }

        public void ChangeItem1(Random r, FileStream stream)
        {
            debug.AppendLine("----------- Item 1 ------------");


            int rInt;

            //0x03E1A0(0F:E190) - Item 1 Update Subroutine

            //0x03E1AC - Delay before Item 1 starts flashing 0xBB (make 0x00 for infinite) (do from 0x30 to 0xFF)
            rInt = r.Next(0xFF - 0x30 + 1) + 0x30;
            stream.Position = 0x03E1AC;
            stream.WriteByte((byte)rInt);
            debug.AppendLine(String.Format("(C) | Delay Before Flashing: {0:X2}", rInt));

            //0x03E1BF - Delay before Item 1 disappears after flashing 0x3E (make 0x00 for infinite) (do from 0x20 to 0x70)
            rInt = r.Next(0x70 - 0x20 + 1) + 0x20;
            stream.Position = 0x03E1BF;
            stream.WriteByte((byte)rInt);
            debug.AppendLine(String.Format("(C) | Delay Before Despawn: {0:X2}", rInt));

            //0x03E1E2 - y-pos offset for Mega Man once standing on Item 1(0x04)
            //0x03E1E7 - width of Item 1 surface for Mega Man to stand on(0x14)
            //0x03E1F0 - Distance above Item to check for despawning(0x1D)

            //0x03D4C2 - y-vel fraction(0x41)
            int[] rXVelFracs = new int[] { 0x00, 0x20, 0x41, 0x50 };
            rInt = r.Next(rXVelFracs.Length);
            stream.Position = 0x03D4C2;
            stream.WriteByte((byte)rXVelFracs[rInt]);
            debug.AppendLine(String.Format("(C) | Y-Velocity (Fraction): {0:X2}\n", rXVelFracs[rInt]));

        }
    }
}
