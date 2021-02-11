using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MM2Randomizer.Enums;
using MM2Randomizer.Patcher;

namespace MM2Randomizer.Randomizers
{
    public class RWeaponBehavior : IRandomizer
    {
        private List<ESoundID> sounds;

        // Buster Heat Air Wood Bubble Quick Metal Clash
        public static List<Double> AmmoUsage;

        public static Double GetAmmoUsage(EDmgVsEnemy weapon)
        {
            if (weapon == EDmgVsEnemy.DamageP)
            {
                return AmmoUsage[0];
            }
            else if (weapon == EDmgVsEnemy.DamageH)
            {
                return AmmoUsage[1];
            }
            else if (weapon == EDmgVsEnemy.DamageA)
            {
                return AmmoUsage[2];
            }
            else if (weapon == EDmgVsEnemy.DamageW)
            {
                return AmmoUsage[3];
            }
            else if (weapon == EDmgVsEnemy.DamageB)
            {
                return AmmoUsage[4];
            }
            else if (weapon == EDmgVsEnemy.DamageQ)
            {
                return AmmoUsage[5];
            }
            else if (weapon == EDmgVsEnemy.DamageM)
            {
                return AmmoUsage[6];
            }
            else if (weapon == EDmgVsEnemy.DamageC)
            {
                return AmmoUsage[7];
            }
            else
            {
                return 0;
            }
        }

        private StringBuilder debug;
        public override String ToString()
        {
            return debug.ToString();
        }

        public RWeaponBehavior() { }

        private static List<ESoundID> GetSoundList()
        {
            return new List<ESoundID>(new ESoundID[] {
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
        }

        public void Randomize(Patch p, Random r)
        {
            debug = new StringBuilder();
            sounds = GetSoundList();
            AmmoUsage = new List<Double>();
            AmmoUsage.Add(0); // Buster is free

            ChangeHeat(p, r);
            ChangeAir(p, r);
            ChangeWood(p, r);
            ChangeBubble(p, r);
            ChangeQuick(p, r);
            ChangeFlash(p, r);
            ChangeMetal(p, r);
            ChangeClash(p, r);
            ChangeItem1(p, r);

            debug.AppendLine("Ammo Usage");
            debug.AppendLine("P     H     A     W     B     Q     M     C");
            debug.AppendLine("-----------------------------------------------");
            foreach (Double w in AmmoUsage)
            {
                debug.Append(String.Format("{0:0.00}  ", w));
            }
            debug.Append(Environment.NewLine);
        }

        /// <summary>
        /// Get a random unique sound for a weapon to use
        /// </summary>
        /// <param name="r"></param>
        /// <returns>Sound ID Byte</returns>
        private ESoundID GetRandomSound(Random r)
        {
            Int32 i = r.Next(sounds.Count);
            ESoundID sound = sounds.ElementAt(i);
            sounds.RemoveAt(i);

            // Pick a random charge level if charge sound is chosen
            if (sound == ESoundID.WeaponH_Charge0)
            {
                i = r.Next(3);
                sound = ESoundID.WeaponH_Charge0 + i;
            }
            return sound;
        }

        protected void ChangeHeat(Patch Patch, Random r)
        {
            //0x03DE55 - H L1 Ammo use(01)
            //0x03DE56 - H L2 Ammo use(06)
            //0x03DE57 - H L3 Ammo use(0A)
            // 90% chance for L1 to be free, else cost 1 ammo
            // L2 ammo cost = L1 ammo cost
            // L3 will cost 1-4 ammo
            Double rTestL1Ammo = r.NextDouble();
            Byte L1Ammo = (rTestL1Ammo > 0.1) ? (Byte)0 : (Byte)1;
            Patch.Add(0x03DE55, L1Ammo, $"(H) | Shot L1 Ammo Cost: {L1Ammo}");
            Patch.Add(0x03DE56, L1Ammo, $"(H) | Shot L2 Ammo Cost: {L1Ammo}");

            Byte[] bytes = new Byte[] { 0x01, 0x01, 0x02, 0x02, 0x02, 0x02, 0x03, 0x03, };
            Int32 rInt = r.Next(bytes.Length);
            Patch.Add(0x03DE57, bytes[rInt], $"(H) | Shot L3 Ammo Cost: {bytes[rInt]}");
            AmmoUsage.Add(bytes[rInt]);

            // Charge Behavior
            // 20% to have old charge behavior, 40% to skip 1 level, 40% to shoot L3 immediately
            Double rTestChargeType = r.NextDouble();
            //0x03DD66 - H change these 4 bytes to 0xEA to skip L2 charge
            if (rTestChargeType < 0.8)
            {
                for (Int32 i = 0; i < 4; i++)
                {
                    Patch.Add(0x03DD66 + i, 0xEA, $"(H) | Skip L2 Charge");
                }
            }
            //0x03DD62 - H after doing the previous, change these 2 bytes to 0xEA to skip charge altogether
            if (rTestChargeType < 0.4)
            {
                Patch.Add(0x03DD62, 0xEA, $"(H) | Skip L1 Charge");
                Patch.Add(0x03DD63, 0xEA, $"(H) | Skip L1 Charge");
                //0x03DD95 - H change this from 0x0D to 0x14 to fire the charge on press instead of on release
                Patch.Add(0x03DD95, 0x14, $"(H) | Fire Shot On Press");
            }

            // Charge delay for L2 will be between 0x10 and 0x80 frames
            //0x03DD61 - H L2 charge delay (0x7D)
            Int32 chargeDelayL2 = r.Next(0x70) + 0x09;
            Patch.Add(0x03DD61, (Byte)chargeDelayL2, "(H) | L2 Charge Delay");

            // Charge delay for L3 will L2 plus a value between 0x10 and 0x40 frames
            //0x03DD67 - H L3 charge delay(0xBB)
            Int32 chargeDelayL3 = r.Next(0x30) + 0x09 + chargeDelayL2;
            if (rTestChargeType >= 0.8)
            {
                Patch.Add(0x03DD67, (Byte)chargeDelayL3, "(H) | L3 Charge Delay");
            }

            //0x03DDEC - H shot sound effect(38)
            ESoundID sound = GetRandomSound(r);
            Patch.Add(0x03DDEC, (Byte)sound, "(H) | Shot Sound");

            //0x03DDF1 - H x - speed (04, all levels)
            //    Do from 02 to 08
            Int32 xVel = r.Next(0x07) + 0x02;
            Patch.Add(0x03DDF1, (Byte)xVel, "(H) | X-Velocity");

            //0x03DE45 - H charge sound 1(35) Unused
            //0x03DE46 - H charge sound 2(35)
            sound = GetRandomSound(r);
            Patch.Add(0x03DE46, (Byte)sound, "(H) | L1 Sound");

            //0x03DE47 - H charge sound 3(36)
            sound = GetRandomSound(r);
            Patch.Add(0x03DE47, (Byte)sound, "(H) | L2 Sound");

            //0x03DE48 - H charge sound 4(37)
            sound = GetRandomSound(r);
            Patch.Add(0x03DE48, (Byte)sound, "(H) | L3 Sound");
        }

        protected void ChangeAir(Patch Patch, Random r)
        {
            //0x03DAD6 - A num projectiles, default 0x04
            //  Values 0x02 and 0x03 work, but larger values behave strangely
            Int32 numProjectiles = 0x04;
            Double rTestNumProjectiles = r.NextDouble();

            if (rTestNumProjectiles > 0.80)
            {
                numProjectiles = 0x03;
            }
            else if (rTestNumProjectiles > 0.60)
            {
                numProjectiles = 0x02;
            }

            Patch.Add(0x03DAD6, (Byte)numProjectiles, "(A) | Number of Projectiles");

            //0x03DADA - A projectile type, default 0x02
            //  Can use this to change behavior completely!Buggy though.

            //0x03DAE6 - A sound effect (0x3F)
            ESoundID sound = GetRandomSound(r);
            Patch.Add(0x03DAE6, (Byte)sound, "(A) | Sound");

            //0x03DAEE - A ammo used(0x02)
            Int32 ammoUse = r.Next(0x02) + 0x01;
            Patch.Add(0x03DAEE, (Byte)ammoUse, "(A) | Ammo Use");
            AmmoUsage.Add(ammoUse);

            //0x03DE6E - A projectile y-acceleration fraction(10)
            // Do 0x02 to 0x32, where values above 0x10 are less common
            Int32 yAccFrac = r.Next(0x17) + 0x02;
            if (yAccFrac > 0x10)
            {
                // Double the addition of any acceleration value chosen above 0x10
                yAccFrac += (yAccFrac - 0x10) * 2; 
            }
            Patch.Add(0x03DE6E, (Byte)yAccFrac, "(A) | Y-Acceleration (fraction)");

            //0x03DE76 - A projectile y-acceleration integer(00)
            // 15% chance to be significantly faster
            Int32 yAccInt = 0x00;
            Double rYAcc = r.NextDouble();

            if (rYAcc > 0.85)
            {
                yAccInt = 0x01;
            }

            Patch.Add(0x03DE76, (Byte)yAccInt, "(A) | Y-Acceleration (integer)");

            //0x03DE7E - A x - speed fraction projectile 1(19)
            //0x03DE7F - A x - speed fraction projectile 2(99)
            //0x03DE80 - A x - speed fraction projectile 3(33)
            for (Int32 i = 0; i < 3; i++)
            {
                Int32 xFracSpeed = r.Next(0xFF) + 0x01;
                Patch.Add(0x03DE7E + i, (Byte)xFracSpeed, String.Format("(A) | Projectile {0} X-Velocity (fraction)", i + 1));
            }

            //0x03DE81 - A x - speed integer projectile 1(01)
            //0x03DE82 - A x - speed integer projectile 2(01)
            //0x03DE83 - A x - speed integer projectile 3(02)
            Int32[] xIntSpeeds = new Int32[] {
                0x00, 0x01, 0x02, 0x04, 0x06
            };
            Int32 rIndex = 0;
            Int32 xIntSpeed = 0;
            for (Int32 i = 0; i < 3; i++)
            {
                rIndex = r.Next(xIntSpeeds.Length);
                xIntSpeed = xIntSpeeds[rIndex];
                Patch.Add(0x03DE81 + i, (Byte)xIntSpeed, String.Format("(A) | Projectile {0} X-Velocity (integer)", i + 1));
            }
        }

        protected void ChangeWood(Patch Patch, Random r)
        {
            //0x03DEDA - W deploy time (0C)
            //    Can change from 06 to 12
            //    Note: Shield glitches on odd numbers.  Use evens only.
            Int32[] deployDelays = new Int32[] {
                0x00, 0x02, 0x04, 0x06, 0x08, 0x0A, 0x0C, 0x10, 0x14, 0x1C, 0x22
            };
            Int32 rIndex = r.Next(deployDelays.Length);
            Int32 deployDelay = deployDelays[rIndex];
            Patch.Add(0x03DEDA, (Byte)deployDelay, "(W) | Deploy Delay");

            //0x03DF0D - W spin animation? (01)

            //0x03DF1B - W delay between sounds(07)
            // TODO

            //0x03DF1F - W deploy sound effect
            ESoundID sound = GetRandomSound(r);
            Patch.Add(0x03DF1F, (Byte)sound, "(W) | Deploy Sound");

            //0x03DF41 - W which directions the shield is allowed to launch in (F0)
            //    Can prevent launching left / right, up / down, etc

            //0x03DF4D - W launch directions (C0)
            //    Don't use this one

            //0x03DF50 - W launch x - direction subroutine
            //    50% chance to have inverted x controls
            //    Change "LSR AND #40" (4A 29 40) to simply "AND #40" to implement
            Double rTestReverseX = r.NextDouble();
            if (rTestReverseX > 0.5)
            {
                Patch.Add(0x03DF50, 0x29, "(W) | Reverse X-Direction Code AND");
                Patch.Add(0x03DF51, 0x40, "(W) | Reverse X-Direction Code #$40");
                Patch.Add(0x03DF52, 0xEA, "(W) | Reverse X-Direction Code NOP"); // (best thing i could find to simply "skip" a line)
            }
            
            //0x03DF59 - W x - speed(04) (do 0x02-0x08)
            Int32 launchVel = r.Next(0x06) + 0x02;
            Patch.Add(0x03DF59, (Byte)launchVel, "(W) | Launch X-Velocity Integer");

            //0x03DF64 - W launch y - direction(10)
            //  Change to 0x20 to reverse, 50% chance
            Int32 reverseY = 0x10;
            Double rTestReverseY = r.NextDouble();

            if (rTestReverseY > 0.5)
            {
                reverseY = 0x20;
            }

            Patch.Add(0x03DF64, (Byte)reverseY, "(W) | Launch Y-Direction");

            //0x03DF72 - W ammo usage (3) (do from 1 to 3)
            Int32 ammoUse = r.Next(0x03) + 0x01;
            Patch.Add(0x03DF72, (Byte)ammoUse, "(W) | Ammo Usage");
            AmmoUsage.Add(ammoUse);

            //0x03DF7D - W y - speed(04)
            Patch.Add(0x03DF7D, (Byte)launchVel, "(W) | Launch Y-Velocity Integer");
        }

        protected void ChangeBubble(Patch Patch, Random r)
        {
            //0x03D4AB - B x - speed on shoot (0x01) (do 1-3)
            Int32 xVelShoot = r.Next(0x03) + 0x01;
            Patch.Add(0x03D4AB, (Byte)xVelShoot, "(B) | X-Velocity Shoot (Integer)");

            //0x03D4CF - B y - speed on shoot(0x02) (do 0-6)
            Int32 yVelShoot = r.Next(0x06);
            Patch.Add(0x03D4CF, (Byte)yVelShoot, "(B) | Y-Velocity Shoot (Integer)");

            //0x03DB21 - B max shots (0x03) (do 2-5, i.e. 1-4 total projectiles)
            //    Valid from 0x02 - 0x0F. Lags a bunch >= 0x06.
            Int32 maxShots = r.Next(0x04) + 0x02;
            Patch.Add(0x03DB21, (Byte)maxShots, "(B) | Max Shots");

            //0x03DB2F - B weapon type(0x04)
            // Don't do

            //0x03DB34 - B sound effect
            ESoundID sound = GetRandomSound(r);
            Patch.Add(0x03DB34, (Byte)sound, "(B) | Sound");

            //0x03DB3D - B shots per ammo tick (0x02) (do 1-4)
            Int32 magSize = r.Next(0x04) + 0x01;
            Patch.Add(0x03DB3D, (Byte)magSize, "(B) | Shots Per Ammo Tick");
            AmmoUsage.Add(1d / (Double)magSize);

            //0x03DFA4 - B y - pos to embed in surface(0xFF)
            // Dumb

            //0x03DFA9 - B x - speed on surface (0x02)
            //      0x01 - 0x04 ?
            Int32 xVelRoll = r.Next(0x04) + 0x01;
            Patch.Add(0x03DFA9, (Byte)xVelRoll, "(B) | X-Velocity Surface (Integer)");

            //0x03DFC0 - B x - speed after falling from ledge (0x00)
            //      Make 50% chance to be 0, or 1-5
            Int32 xVelFall = 0x00;
            Double rTestXFallSpeed = r.NextDouble();

            if (rTestXFallSpeed > 0.5)
            {
                xVelFall = r.Next(0x05) + 0x01;
            }

            Patch.Add(0x03DFC0, (Byte)xVelFall, "(B) | X-Velocity Fall (Integer)");

            //0x03DFC8 - B y - speed after falling(0xFE)
            //      Either 0xFA - 0xFF or 0x01 - 0x06
            Int32[] yFallVels = new Int32[] { 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06 };
            Int32 rIndex = r.Next(yFallVels.Length);
            Int32 yFallVel = yFallVels[rIndex];
            Patch.Add(0x03DFC8, (Byte)yFallVel, "(B) | Y-Velocity Fall (Integer)");
        }

        protected void ChangeQuick(Patch Patch, Random r)
        {
            // Q autofire delay, default 0x0B
            //    Do from 0x05 to 0x12
            Int32 autoFireDelay = r.Next(0x0D) + 0x05;
            Patch.Add(0x03DB54, (Byte)autoFireDelay, "(Q) | Autofire Delay");

            // Q max shots, default 0x05
            //    Do from 0x03 to 0x07(2 to 6 shots)
            Int32 maxShots = r.Next(0x04) + 0x03;
            Patch.Add(0x03DB5C, (Byte)maxShots, "(Q) | Max Shots");

            // 0x03DB6F - Q sound effect
            ESoundID sound = GetRandomSound(r);
            Patch.Add(0x03DB6F, (Byte)sound, "(Q) | Sound");
            
            // Q shots per ammo tick, default 0x08
            //    Do from 0x04 to 0x0A ?
            Int32 magSize = r.Next(0x06) + 0x04;
            Patch.Add(0x03DB78, (Byte)magSize, "(Q) | Shots Per Ammo Tick");
            AmmoUsage.Add(1d / (Double)magSize);

            // Q behavior, distance, default 0x12
            //    Do from 0x0A to 0x20 ?
            Int32 distance = r.Next(0x16) + 0x0A;
            Patch.Add(0x03DFE2, (Byte)distance, "(Q) | Travel Distance");

            // Q behavior, initial angle, default 0x4B
            //    Do from 0x00 to 0x60 ?
            Int32 angle1 = r.Next(0x60);
            Patch.Add(0x03DFEA, (Byte)angle1, "(Q) | Initial Angle");

            //0x03DFF2 - Q behavior, weird, default 0x00
            //    Don't use, but change to 0x01 for dumb effect

            // Q behavior, angle, default 0x40
            //    0x40 - (GOOD)Normal
            //    0x80 - (GOOD / HARD) Disappears(doesn't return)
            //    0x00 - (GOOD)Sine wave
            //    0x03 - (GOOD)Float downwards(interesting behavior when changing other Byte)
            //    0x04, 05 - Float downwards(short, not different enough from 03)
            //    0x06 - Float downwards(faster)
            Int32[] angle2s = new Int32[] { 0x40, 0x80, 0x00, 0x03 };
            Int32 rIndex = r.Next(angle2s.Length);
            Int32 angle2 = angle2s[rIndex];
            Patch.Add(0x03DFFF, (Byte)angle2, "(Q) | Secondary Angle");

            // Q behavior, time before disappearing on return, default 0x23
            //    Do from 0x1E to 0x30
            Int32 despawnDelay = r.Next(0x12) + 0x1E;
            Patch.Add(0x03E007, (Byte)despawnDelay, "(Q) | Despawn Delay");

            // Q behavior, return angle, default 0x4B
            //    Do from 0x00 to 0x90
            Int32 angle3 = r.Next(0x90);
            Patch.Add(0x03E013, (Byte)angle3, "(Q) | Return Angle");

            //0x03E01B - Q behavior, weird, default 0x00
            //    Change to 0x01 for interesting effects
        }

        protected void ChangeFlash(Patch Patch, Random r)
        {
            //0x03DC59 - F sound (21)
            ESoundID sound = GetRandomSound(r);
            Patch.Add(0x03DC59, (Byte)sound, "(F) | Sound");

            //0x03E172 - F custom subroutine for reusable weapon
            // 75% chance to occur
            Double rTestFChange = r.NextDouble();
            if (rTestFChange > 0.25)
            {
                // 0x03E175 - New ammo-usage address.
                Byte[] ammos = new Byte[] { 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08};
                Int32 rIndex = r.Next(7);
                Byte ammo = ammos[rIndex];
                
                // Change ammo-tick subroutine into one that resumes time
                Byte[] sub = new Byte[]
                {
                    0xA5, 0xA1,         // LDA ammoFlash = #$0A
                    0xE9, ammo,         // SBC {ammo use}
                    0x85, 0xA1,         // STA ammoFlash = #$0A
                    0x5E, 0x20, 0x04,   // LSR megaXDir, X @ megaXDir = #$C0
                    0xA9, 0x00,         // LDA #$00
                    0x85, 0xAA,         // STA $00AA = #$00
                    0x85, 0x50,         // STA $0050 = #$00 ; we erased LDA #$01, but doesn't seem to hurt 
                };
                for (Int32 i = 0; i < sub.Length; i++)
                {
                    Patch.Add(0x03E172 + i, sub[i], "(F) | Reusable Time-Stopper Subroutine");
                }

                // 0x03E16E and 0x03D49D is the new duration, in frames. They must both be the same value.
                Int32 duration = 0x01;
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
                Patch.Add(0x03E16E, (Byte)duration, "(F) | Freeze Duration (1)");
                Patch.Add(0x03D49D, (Byte)duration, "(F) | Freeze Duration (2)");

                // Finally, a fix is needed to prevent ammo underflow
                // 0x03DC41 - WpnMove_FStart
                sub = new Byte[]
                {
                    0xA2, 0x02,         // LDX #$02
                    0xAD, 0x22, 0x04,   // LDA $0422 = #$61
                    0x30, 0x1E,         // BMI WpnMove_Done    
                    0xA5, 0xA1,         // LDA ammoFlash = #$02
                    0xC9, 0x07,         // CMP #$07            
                    0x90, 0x18,         // BCC WpnMove_Done    
                };
                for (Int32 i = 0; i < sub.Length; i++)
                {
                    Patch.Add(0x03DC41 + i, sub[i], "(F) | Reusable Time-Stopper Ammo-Underflow Fix");
                }
            }
            else
            {
                // Standard Time Stopper, but modify the tick frequency
                // Default 0x0F. For 28 ticks, that's 7 seconds. Modify to be 4-10 seconds, which is about 0x09 to 0x16
                Int32 tickDelay = r.Next(0x13) + 9;
                Patch.Add(0x03E16E, (Byte)tickDelay, "(F) | Time-Stopper Ammo Tick Delay (1)");
                Patch.Add(0x03D49D, (Byte)tickDelay, "(F) | Time-Stopper Ammo Tick Delay (2)");
            }
        }

        protected void ChangeMetal(Patch Patch, Random r)
        {
            //0x03DBB6 - M max shots (04) (change to 0x02-0x05, or 1-4)
            Int32 maxShots = r.Next(0x04) + 0x02;
            Patch.Add(0x03DBB6, (Byte)maxShots, "(M) | Max Shots");

            //0x03DBC9 - M sound effect(23)
            ESoundID sound = GetRandomSound(r);
            Patch.Add(0x03DBC9, (Byte)sound, "(M) | Sound");

            //0x03DBD2 - M shots per ammo tick(04) (change to 1-5)
            Int32 magSize = r.Next(0x05) + 0x01;
            Patch.Add(0x03DBD2, (Byte)magSize, "(M) | Shots Per Ammo Tick");
            AmmoUsage.Add(1d / magSize);

            // Speeds.  Change each to be 2-7.  Diagonal will be half each, rounded up.
            Int32 velX = r.Next(0x06) + 0x02;
            Int32 velY = r.Next(0x06) + 0x02;
            Int32 halfY = (Int32)Math.Ceiling((Double)velY / 2d);
            Int32 halfX = (Int32)Math.Ceiling((Double)velX / 2d);

            //0x03DC12 - M y - speed, holding up(04)
            Patch.Add(0x03DC12, (Byte)velY, "(M) | Y-Velocity Up");

            //0x03DC31 - M x - speed, no direction(04)
            Patch.Add(0x03DC31, (Byte)velX, "(M) | X-Velocity Neutral");

            //0x03DC35 - M x - speed, holding left(04)
            Patch.Add(0x03DC35, (Byte)velX, "(M) | X-Velocity Left");

            //0x03DC39 - M x - speed, holding right(04)
            Patch.Add(0x03DC39, (Byte)velX, "(M) | X-Velocity Right");

            //0x03DC13 - M y - speed, holding down(FC)
            Patch.Add(0x03DC13, (Byte)(0x00 - (Byte)velY), "(M) | Y-Velocity Down");

            //0x03DC16 - M y - speed, holding up + left(02)
            Patch.Add(0x03DC16, (Byte)halfY, "(M) | Y-Velocity Up+Left");

            //0x03DC17 - M y - speed, holding down + left(FD)
            Patch.Add(0x03DC17, (Byte)(0x00 - (Byte)halfY), "(M) | Y-Velocity Down+Left");

            //0x03DC1A - M y - speed, holding up + right(02)
            Patch.Add(0x03DC1A, (Byte)halfY, "(M) | Y-Velocity Up+Right");

            //0x03DC1B - M y - speed, holding down + right(FD)
            Patch.Add(0x03DC1B, (Byte)(0x00 - (Byte)halfY), "(M) | Y-Velocity Down+Right");

            //0x03DC36 - M x - speed, holding up + left(02)
            Patch.Add(0x03DC36, (Byte)halfX, "(M) | X-Velocity Up+Left");

            //0x03DC37 - M x - speed, holding down + left(02)
            Patch.Add(0x03DC37, (Byte)halfX, "(M) | X-Velocity Down+Left");

            //0x03DC3A - M x - speed, holding up + right(02)
            Patch.Add(0x03DC3A, (Byte)halfX, "(M) | X-Velocity Up+Right");

            //0x03DC3B - M x - speed, holding down + right(02)
            Patch.Add(0x03DC3B, (Byte)halfX, "(M) | X-Velocity Down+Right");
        }

        protected void ChangeClash(Patch Patch, Random r)
        {
            //0x03D4AD - C x-speed on shoot (04) (do 2-7)
            Int32 xVel = r.Next(0x06) + 0x02;
            Patch.Add(0x03D4AD, (Byte)xVel, "(C) | X-Velocity (Integer)");

            //0x03D4D7 - C y-speed integer for explosion (up only) (0)
            // TODO: Figure how this works more to apply in all directions
            // For now, 25% chance to make this move upward at 2px/fr
            Int32 yVelExplode = 0x00;
            Double rTestYVelExplode = r.NextDouble();

            if (rTestYVelExplode > 0.75)
            {
                yVelExplode = r.Next(2) + 0x01;
            }

            Patch.Add(0x03D4D7, (Byte)yVelExplode, "(C) | X-Velocity (Explosion)");

            //0x03DB99 - C ammo per shot (04) (do 1-3)
            Int32 ammoUse = r.Next(0x03) + 0x01;
            Patch.Add(0x03DB99, (Byte)ammoUse, "(C) | Ammo Usage");
            AmmoUsage.Add(ammoUse);

            // 0x03DB9F - C explosion type? (02)
            // Change to 03 to "single explosion" type. Most other values break the game.
            // For now, 50% chance to change
            Int32 multiExplode = 0x02;
            Double rTestMultiExplode = r.NextDouble();

            if (rTestMultiExplode > 0.50)
            {
                multiExplode = 0x03;
            }

            Patch.Add(0x03DB9F, (Byte)multiExplode, "(C) | Explosion Type");

            //0x03DBA6 - C shoot sound effect (24)
            ESoundID sound = GetRandomSound(r);
            Patch.Add(0x03DBA6, (Byte)sound, "(C) | Sound Shoot");

            //0x03E089 - C attach sound effect (2E)
            sound = GetRandomSound(r);
            Patch.Add(0x03E089, (Byte)sound, "(C) | Sound Attach");

            //0x03E09C - C delay before explosion (7E) (do 01 to C0)
            Int32 delayExplosion = r.Next(0xBF) + 0x01;
            Patch.Add(0x03E09C, (Byte)delayExplosion, "(C) | Explode Delay");

            //0x03E0DA - C explode sound effect
            sound = GetRandomSound(r);
            Patch.Add(0x03E0DA, (Byte)sound, "(C) | Sound Explode");
        }

        protected void ChangeItem1(Patch Patch, Random r)
        {
            Int32 rInt;

            //0x03E1A0(0F:E190) - Item 1 Update Subroutine

            //0x03E1AC - Delay before Item 1 starts flashing 0xBB (make 0x00 for infinite) (do from 0x30 to 0xFF)
            rInt = r.Next(0xFF - 0x30 + 1) + 0x30;
            Patch.Add(0x03E1AC, (Byte)rInt, "(1) | Delay Before Flashing");

            //0x03E1BF - Delay before Item 1 disappears after flashing 0x3E (make 0x00 for infinite) (do from 0x20 to 0x70)
            rInt = r.Next(0x70 - 0x20 + 1) + 0x20;
            Patch.Add(0x03E1BF, (Byte)rInt, "(1) | Delay Before Despawn");

            //0x03E1E2 - y-pos offset for Mega Man once standing on Item 1(0x04)
            //0x03E1E7 - width of Item 1 surface for Mega Man to stand on(0x14)
            //0x03E1F0 - Distance above Item to check for despawning(0x1D)

            //0x03D4C2 - y-vel fraction(0x41)
            Int32[] rXVelFracs = new Int32[] { 0x00, 0x20, 0x41, 0x50 };
            rInt = r.Next(rXVelFracs.Length);
            Patch.Add(0x03D4C2, (Byte)rXVelFracs[rInt], "(1) | Y-Velocity (Fraction)");
        }


    }
}
