using System;
using System.Collections.Generic;
using MM2Randomizer.Patcher;

namespace MM2Randomizer.Randomizers
{
    public class RBossAI : IRandomizer
    {
        public RBossAI() { }

        public void Randomize(Patch p, Random r)
        {
            ChangeHeat(p, r);
            ChangeAir(p, r);
            ChangeWood(p, r);
            ChangeBubble(p, r);
            ChangeQuick(p, r);
            ChangeFlash(p, r);
            ChangeMetal(p, r);
            ChangeClash(p, r);
        }

        protected void ChangeHeat(Patch Patch, Random r)
        {
            Int32 rInt = 0;
            // Heatman AI 0x02C16E - 0x02C1FE

            // projectile y - distances
            //0x02C207 default 07, good from 03 - 08
            //0x02C208 default 05, good from 04 - 07
            //0x02C209 default 03, good from 03 - 05
            rInt = r.Next(6) + 0x03;
            Patch.Add(0x02C207, (Byte)rInt, "Heatman Projectile 1 Y-Distance");
            rInt = r.Next(4) + 0x04;
            Patch.Add(0x02C208, (Byte)rInt, "Heatman Projectile 2 Y-Distance");
            rInt = r.Next(3) + 0x03;
            Patch.Add(0x02C209, (Byte)rInt, "Heatman Projectile 3 Y-Distance");

            // projectile x-distances, 0x3A 0x2E 0x1C
            // - The lower value, the faster speed. Different for each fireball.
            //0x02C20A - 1st value should be 0x47 to hit megaman, Or, from 0x30 to 0x80
            //0x02C20B - 2nd value should be 0x2E to hit megaman. Or, from 0x22 to 0x40
            //0x02C20C - 3rd value should be 0x17 to hit megaman, Or, from 0x10 to 0x30
            rInt = r.Next(0x80 - 0x30 + 1) + 0x30;
            Patch.Add(0x02C20A, (Byte)rInt, "Heatman Projectile 1 X-Distance");
            rInt = r.Next(0x40 - 0x22 + 1) + 0x22;
            Patch.Add(0x02C20B, (Byte)rInt, "Heatman Projectile 2 X-Distance");
            rInt = r.Next(0x30 - 0x10 + 1) + 0x10;
            Patch.Add(0x02C20C, (Byte)rInt, "Heatman Projectile 3 X-Distance");

            // 30/60/90 frame delay
            //0x02C29D - Delay 1 0x1F
            //0x02C29E - Delay 2 0x3E
            //0x02C29F - Delay 3 0x5D
            // Choose delay interval from 10-40 frames
            rInt = r.Next(31) + 10;
            Patch.Add(0x02C29D, (Byte)rInt, "Heatman Invuln Delay 1");
            Patch.Add(0x02C29E, (Byte)(rInt * 2), "Heatman Invuln Delay 2");
            Patch.Add(0x02C29F, (Byte)(rInt * 3), "Heatman Invuln Delay 3");

            //0x02C253 - Charge velocity(0x04, 0x08 or more usually puts him on side of screen)
            rInt = r.Next(4) + 0x02;
            Patch.Add(0x02C253, (Byte)rInt, "Heatman Charge Velocity");
        }

        protected void ChangeAir(Patch Patch, Random r)
        {
            Int32 rInt = 0;
            Double rDbl = 0;

            // Airman AI 0x02C2F3 - 0x02C50A

            // Create random Air Shooter patterns

            //0x02C393 - Tornado 0 Pattern 0 y-vel fraction
            //0x02C395 - Tornado 1 Pattern 0 y-vel frac
            //...
            //0x02C39A - Tornado 0 Pattern 1 y-vel frac
            //... ...
            //0x02C3B1 - Tornado 0 Pattern 0 y-vel integer
            //0x02C3CF - Tornado 0 Pattern 0 x-vel fraction
            //0x02C3ED - Tornado 0 Pattern 0 x-vel integer
            //0x02C40B - Tornado 0 Pattern 0 delay before stop
            const Int32 A_tornadoTableLength = 0x1E;

            // Write y-vel fractions: 00-FF
            for (Int32 i = 0; i < A_tornadoTableLength; i++)
            {
                rInt = r.Next(256);
                Patch.Add(0x02C393 + i, (Byte)rInt, String.Format("Airman Tornado {0} Y-Vel Frac", i));
            }

            // Write y-vel integers: FF-03, rare 04
            for (Int32 i = 0; i < A_tornadoTableLength; i++)
            {
                Byte A_yVelInt = 0;
                Byte[] A_yVelInts = new Byte[] { 0xFF, 0x00, 0x01, 0x02, 0x03 };
                rDbl = r.NextDouble();
                if (rDbl > 0.9)
                {
                    A_yVelInt = 0x04;
                }
                else
                {
                    rInt = r.Next(A_yVelInts.Length);
                    A_yVelInt = A_yVelInts[rInt];
                }
                Patch.Add(0x02C3B1 + i, A_yVelInt, String.Format("Airman Tornado {0} Y-Vel Int", i));
            }

            // Write x-vel fractions: 00-FF
            for (Int32 i = 0; i < A_tornadoTableLength; i++)
            {
                rInt = r.Next(256);
                Patch.Add(0x02C3CF + i, (Byte)rInt, String.Format("Airman Tornado {0} X-Vel Frac", i));
            }

            // Write x-vel integers: 00-04, rare 04, common 03
            for (Int32 i = 0; i < A_tornadoTableLength; i++)
            {
                Byte A_xVelInt = 0;
                Byte[] A_xVelInts = new Byte[] { 0x00, 0x01, 0x02 };
                rDbl = r.NextDouble();
                if (rDbl > 0.9)
                {
                    A_xVelInt = 0x04;
                }
                else if (rDbl > 0.6)
                {
                    A_xVelInt = 0x03;
                }
                else
                {
                    rInt = r.Next(A_xVelInts.Length);
                    A_xVelInt = A_xVelInts[rInt];
                }
                Patch.Add(0x02C3ED + i, A_xVelInt, String.Format("Airman Tornado {0} X-Vel Int", i));
            }

            // Write delays: 05-2A
            for (Int32 i = 0; i < A_tornadoTableLength; i++)
            {
                rInt = r.Next(0x25) + 0x05;
                Patch.Add(0x02C40B + i, (Byte)rInt, String.Format("Airman Tornado {0} Delay Time", i));
            }

            // 0x02C30C - Num patterns before jumping 0x03 (do 1-4)
            rInt = r.Next(4) + 1;
            Patch.Add(0x02C30C, (Byte)rInt, "Airman Patterns Before Jump");

            //0x02C4DD - First Jump y-vel frac, 0xE6
            //0x02C4DE - Second Jump y-vel frac, 0x76
            //0x02C4E0 - First Jump y-vel Int32, 0x04
            //0x02C4E1 - Second Jump y-vel Int32 0x07
            //0x02C4E3 - First Jump x-vel frac, 0x39
            //0x02C4E4 - Second Jump x-vel frac 0x9a
            //0x02C4E6 - First Jump x-vel Int32, 0x01
            //0x02C4E7 - Second Jump x-vel Int32 0x01
            // Pick x-vel integers for both jumps first. Must add up to 2 or 3.
            Int32 rSum = r.Next(2) + 2;
            Int32 jump1x = r.Next(rSum + 1);
            Int32 jump2x = rSum - jump1x;
            Patch.Add(0x02C4E6, (Byte)jump1x, "Airman X-Velocity Integer Jump 1");
            Patch.Add(0x02C4E7, (Byte)jump2x, "Airman X-Velocity Integer Jump 2");

            // If a jump's x-Int32 is 0, its corresponding y-Int32 must be 5-6
            // If a jump's x-Int32 is 1, its corresponding y-Int32 must be 4-5
            // If a jump's x-Int32 is 2, its corresponding y-Int32 must be 3-5
            // If a jump's x-Int32 is 3, its corresponding y-Int32 must be 2-4
            Int32 jump1y = AirmanGetJumpYVelocity(jump1x, r);
            Int32 jump2y = AirmanGetJumpYVelocity(jump2x, r);
            Patch.Add(0x02C4E0, (Byte)jump1y, "Airman Y-Velocity Integer Jump 1");
            Patch.Add(0x02C4E1, (Byte)jump2y, "Airman Y-Velocity Integer Jump 2");

            // Random x and y-vel fractions for both jumps
            //stream.Position = 0x02C4DD; // 1st jump y-vel frac
            rInt = r.Next(0xF1); // If jump is 7 and fraction is > 0xF0, Airman gets stuck!
            Patch.Add(0x02C4DD, (Byte)rInt, "Airman Y-Velocity Fraction Jump 1");
            rInt = r.Next(0xF1);
            Patch.Add(0x02C4DE, (Byte)rInt, "Airman Y-Velocity Fraction Jump 2");
            rInt = r.Next(256);
            Patch.Add(0x02C4E3, (Byte)rInt, "Airman X-Velocity Fraction Jump 1");
            rInt = r.Next(256);
            Patch.Add(0x02C4E4, (Byte)rInt, "Airman X-Velocity Fraction Jump 2");
        }

        private Byte AirmanGetJumpYVelocity(Int32 xVelInt, Random r)
        {
            Int32 jumpYMax = 0;
            Int32 jumpYMin = 0;
            switch (xVelInt)
            {
                case 0:
                    jumpYMax = 6;
                    jumpYMin = 5;
                    break;
                case 1:
                    jumpYMax = 5;
                    jumpYMax = 4;
                    break;
                case 2:
                    jumpYMax = 5;
                    jumpYMin = 3;
                    break;
                case 3:
                    jumpYMax = 4;
                    jumpYMin = 2;
                    break;
                default: break;
            }

            Int32 yVelInt = r.Next(jumpYMax - jumpYMin + 1) + jumpYMin;
            return (Byte)yVelInt;
        }

        protected void ChangeWood(Patch Patch, Random r)
        {
            Int32 rInt = 0;
            Double rDbl = 0.0;
            Byte[] xVels;

            // Woodman AI

            // Some unused addresses for later:
            //0x02C567 - Falling leaf y-pos start, 0x20
            //0x03DA34 - Leaf shield y-velocity while it's attached to woodman, lol.

            //0x02C537 - Delay between leaves 0x12. Do 0x06 to 0x20.
            rInt = r.Next(0x20 - 0x06 + 1) + 0x06;
            Patch.Add(0x02C537, (Byte)rInt, "Woodman Leaf Spacing Delay");

            //0x02C5DD - Jump height, 0x04. Do 0x03 to 0x07.
            rInt = r.Next(0x07 - 0x03 + 1) + 0x03;
            Patch.Add(0x02C5DD, (Byte)rInt, "Woodman Jump Y-Velocity");

            //0x02C5E2 - Jump distance, 0x01. Do 0x01 to 0x04.
            rInt = r.Next(0x04 - 0x01 + 1) + 0x01;
            Patch.Add(0x02C5E2, (Byte)rInt, "Woodman Jump X-Velocity");

            //0x02C5A9 - Shield launch speed, 0x04. Do 0x01 to 0x08.
            rInt = r.Next(0x08 - 0x01 + 1) + 0x01;
            Patch.Add(0x02C5A9, (Byte)rInt, "Woodman Shield Launch X-Velocity");

            //0x02C553 - Number of falling leaves, 0x03. Do 0x02 20% of the time.
            rDbl = r.NextDouble();
            if (rDbl > 0.8)
            {
                Patch.Add(0x02C553, 0x02, "Woodman Falling Leaf Quantity");
            }

            //0x02C576 - Falling leaf x-vel, 0x02. Do 0x01 or 0x02, but with a low chance for 0x00 and lower chance for 0x03
            xVels = new Byte[] {
                0x00,0x00,0x00,
                0x03,
                0x01,0x01,0x01,0x01,0x01,0x01,
                0x02,0x02,0x02,0x02,
            };
            rInt = r.Next(xVels.Length);
            Patch.Add(0x02C576, xVels[rInt], "Woodman Falling Leaf X-Velocity");

            //0x03D8F6 - 0x02, change to 0x06 for an interesting leaf shield pattern 20% of the time
            rDbl = r.NextDouble();
            if (rDbl > 0.8)
            {
                Patch.Add(0x03D8F6, 0x06, "Woodman Leaf Shield Pattern");
            }

            //0x03B855 - Leaf fall speed(sort of ?) 0x20. 
            // Decrease value to increase speed. At 0x40, it doesn't fall. 
            // 20% of the time, change to a high number to instantly despawn leaves for a fast pattern. 
            // Do from 0x00 to 0x24.  Make less than 0x1A a lower chance.
            Int32 yVel;
            rDbl = r.NextDouble();
            if (rDbl > 0.8)
            {
                yVel = 0xA0; // Leaves go upwards
            }
            else
            {
                Byte[] yVels = new Byte[]
                {
                    0x08, 0x18, 0x1C, // Fall faster
                    0x1D, 0x1E, 0x20, 0x21, 0x22, 0x23, 0x24 // Fall slower
                };
                rInt = r.Next(yVels.Length);
                yVel = yVels[rInt];
            }
            Patch.Add(0x03B855, (Byte)yVel, "Woodman Falling Leaf Y-Velocity");
        }

        protected void ChangeBubble(Patch Patch, Random r)
        {
            Byte[] bytes;
            Int32 rInt;

            // Bubbleman's AI

            //0x02C707 - Y-pos to reach before falling, 0x50.
            bytes = new Byte[] { 0x50, 0x40, 0x50, 0x40, 0x60, 0x80 };
            rInt = r.Next(bytes.Length);
            Patch.Add(0x02C707, bytes[rInt], "Bubbleman Y Max Height");

            //0x02C70B - Falling speed integer, 0xFF.
            bytes = new Byte[] { 0xFE, 0xFF, 0xFF, 0xFF, 0xFF };
            rInt = r.Next(bytes.Length);
            Patch.Add(0x02C70B, bytes[rInt], "Bubbleman Y-Velocity Falling");

            //0x02C710 - Landing x-tracking speed, integer, 0x00.
            bytes = new Byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 };
            rInt = r.Next(bytes.Length);
            Patch.Add(0x02C710, bytes[rInt], "Bubbleman X-Velocity Falling");

            //0x02C6D3 - Rising speed integer, 0x01.
            bytes = new Byte[] { 0x01, 0x01, 0x02, 0x02, 0x03, };
            rInt = r.Next(bytes.Length);
            Patch.Add(0x02C6D3, bytes[rInt], "Bubbleman Y-Velocity Rising");

            //0x02C745 - Delay between water gun shots, 0x12
            bytes = new Byte[] { 0x04, 0x08, 0x0C, 0x10, 0x10, 0x14, 0x18, 0x1C };
            rInt = r.Next(bytes.Length);
            Patch.Add(0x02C745, bytes[rInt], "Bubbleman Water Gun Cooldown");

            // WARNING: THIS ADDRESS IS SHARED WITH THE VELOCITY OF THE DEATH BEAMS IN QUICKMAN STAGE!!
            ////0x03DA19 - X-Vel water gun, Int 0x04
            //bytes = new Byte[] { 0x02, 0x03, 0x04, 0x05, };
            //rInt = r.Next(bytes.Length);
            //Patch.Add(0x03DA19, bytes[rInt], "Bubbleman X-Vel Water Gun, Int");

            ////0x03DA1A - X-Vel water gun, Frac 0x40
            //bytes = new Byte[] { 0x40, 0x80, 0xC0, 0x00, };
            //rInt = r.Next(bytes.Length);
            //Patch.Add(0x03DA1A, bytes[rInt], "Bubbleman X-Vel Water Gun, Frac");

            //0x03DA25 - X-Vel bubble shot, Int 0x01
            bytes = new Byte[] { 0x00, 0x00, 0x01, 0x01, 0x02, };
            rInt = r.Next(bytes.Length);
            Patch.Add(0x03DA25, bytes[rInt], "Bubbleman X-Vel Bubble, Int");

            //0x03DA26 - X-Vel bubble shot, Frac 0x00
            bytes = new Byte[] { 0x80, 0xC0, 0xFF };
            rInt = r.Next(bytes.Length);
            Patch.Add(0x03DA26, bytes[rInt], "Bubbleman X-Vel Bubble, Frac");

            //0x03DA4D - Y-Vel bubble shot initial, Int (0x03)
            bytes = new Byte[] { 0x02, 0x03, 0x04, 0x05 };
            rInt = r.Next(bytes.Length);
            Patch.Add(0x03DA4D, bytes[rInt], "Bubbleman Y-Vel Bubble Initial, Int");

            //0x03DA4E - Y-Vel bubble shot initial, Frac (0x76)
            bytes = new Byte[] { 0x00, 0x40, 0x80, 0xC0 };
            rInt = r.Next(bytes.Length);
            Patch.Add(0x03DA4E, bytes[rInt], "Bubbleman Y-Vel Bubble Initial, Frac");

            //0x03B747 - Y-Vel bubble shot bounce, Int (0x03)
            bytes = new Byte[] { 0x02, 0x03, 0x04, 0x05 };
            rInt = r.Next(bytes.Length);
            Patch.Add(0x03B747, bytes[rInt], "Bubbleman Y-Vel Bubble Bounce, Int");

            //0x03B74C - Y-Vel bubble shot bounce, Frac (0x76)
            bytes = new Byte[] { 0x00, 0x40, 0x80, 0xC0 };
            rInt = r.Next(bytes.Length);
            Patch.Add(0x03B74C, bytes[rInt], "Bubbleman Y-Vel Bubble Bounce, Frac");
        }

        protected void ChangeQuick(Patch Patch, Random r)
        {
            Int32 rInt;

            // Other addresses with potential:
            //0x02C872 - Projectile type, 0x59

            // Quickman's AI
            //0x02C86E - Number of Boomerangs, 0x03, do from 1 - 0x0A
            rInt = r.Next(0x0B) + 0x01;
            Patch.Add(0x02C86E, (Byte)rInt, "Quickman Number of Boomerangs");

            //0x02C882 - Boomerang: delay before arc 0x25. 0 for no arc, or above like 0x35. do from 5 to 0x35.
            rInt = r.Next(0x35 - 0x05 + 1) + 0x05;
            Patch.Add(0x02C882, (Byte)rInt, "Quickman Boomerang Delay 1");

            //0x02C887 - Boomerang speed when appearing, 0x04, do from 0x01 to 0x07
            rInt = r.Next(0x07 - 0x01 + 1) + 0x01;
            Patch.Add(0x02C887, (Byte)rInt, "Quickman Boomerang Velocity Integer 1");

            //0x03B726 - Boomerang speed secondary, 0x04, does this affect anything else?
            rInt = r.Next(0x07 - 0x01 + 1) + 0x01;
            Patch.Add(0x03B726, (Byte)rInt, "Quickman Boomerang Velocity Integer 2");

            // For all jumps, choose randomly from 0x02 to 0x0A
            //0x02C8A3 - Middle jump, 0x07
            rInt = r.Next(0x0A - 0x02 + 1) + 0x02;
            Patch.Add(0x02C8A3, (Byte)rInt, "Quickman Jump Height 1 Integer");

            //0x02C8A4 - High jump, 0x08
            rInt = r.Next(0x0A - 0x02 + 1) + 0x02;
            Patch.Add(0x02C8A4, (Byte)rInt, "Quickman Jump Height 2 Integer");

            //0x02C8A5 - Low jump, 0x04
            rInt = r.Next(0x0A - 0x02 + 1) + 0x02;
            Patch.Add(0x02C8A5, (Byte)rInt, "Quickman Jump Height 3 Integer");

            //0x02C8E4 - Running time, 0x3E, do from 0x18 to 0x50
            rInt = r.Next(0x50 - 0x18 + 1) + 0x18;
            Patch.Add(0x02C8E4, (Byte)rInt, "Quickman Running Time");

            //0x02C8DF - Running speed, 0x02, do from 0x05 to 0x01
            rInt = r.Next(0x05 - 0x01 + 1) + 0x01;
            Patch.Add(0x02C8DF, (Byte)rInt, "Quickman Running Velocity Integer");
        }

        protected void ChangeFlash(Patch Patch, Random r)
        {
            Int32 rInt;

            // Unused addresses
            //0x02CA71 - Projectile type 0x35
            //0x02CA52 - "Length of time stopper / projectile frequency ?"

            // Flashman's AI

            //0x02C982 - Walk velocity integer 0x01, do from 0 to 3
            rInt = r.Next(0x04);
            Patch.Add(0x02C982, (Byte)rInt, "Flashman Walk Velocity Integer");

            //0x02C97D - Walk velocity fraction 0x06, do 0x00-0xFF
            rInt = r.Next(256);
            Patch.Add(0x02C97D, (Byte)rInt, "Flashman Walk Velocity Fraction");

            //0x02C98B - Delay before time stopper 0xBB (187 frames). Do from 30 frames to 240 frames
            rInt = r.Next(211) + 30;
            Patch.Add(0x02C98B, (Byte)rInt, "Flashman Delay Before Time Stopper");

            //0x02CAC6 - Jump distance integer 0x00, do 0 to 3
            // TODO do fraction also
            rInt = r.Next(4);
            Patch.Add(0x02CAC6, (Byte)rInt, "Flashman Jump X-Velocity Integer");

            //0x02CACE - Jump height 0x04, do 3 - 8
            rInt = r.Next(6) + 3;
            Patch.Add(0x02CACE, (Byte)rInt, "Flashman Jump Y-Velocity Integer");

            //0x02CA81 - Projectile speed 0x08, do 2 - 0A
            rInt = r.Next(0x0A - 0x02 + 1) + 0x02;
            Patch.Add(0x02CA81, (Byte)rInt, "Flashman Projectile Velocity Integer");

            //0x02CA09 - Number of projectiles to shoot 0x06, do 3 - 0x10
            rInt = r.Next(0x10 - 0x03 + 1) + 0x03;
            Patch.Add(0x02CA09, (Byte)rInt, "Flashman Number of Projectiles");
        }

        protected void ChangeMetal(Patch Patch, Random r)
        {
            Int32 rInt;
            Double rDbl;

            // Unused addresses
            //0x02CC2D - Projectile type
            //0x02CC29 - Metal Blade sound effect 0x20

            // Metalman AI

            //0x02CC3F - Speed of Metal blade 0x04, do 2 to 9
            rInt = r.Next(8) + 2;
            Patch.Add(0x02CC3F, (Byte)rInt, "Metalman Projectile Velocity Integer");

            //0x02CC1D - Odd change to attack behavior, 0x06, only if different than 6. Give 25% chance.
            rDbl = r.NextDouble();
            if (rDbl > 0.75)
            {
                Patch.Add(0x02CC1D, 0x05, "Metalman Alternate Attack Behavior");
            }

            //0x02CBB5 - Jump Height 1 0x06, do from 03 - 07 ? higher than 7 bonks ceiling
            //0x02CBB6 - Jump Height 2 0x05
            //0x02CBB7 - Jump Height 3 0x04
            List<Int32> jumpHeight = new List<Int32>(){ 0x03, 0x04, 0x05, 0x06, 0x07 };
            for (Int32 i = 0; i < 3; i ++)
            {
                // Pick a height at random and remove from list to get 3 different heights
                rInt = r.Next(jumpHeight.Count);
                Patch.Add(0x02CBB5 + i, (Byte)jumpHeight[rInt], String.Format("Metalman Jump {0} Y-Velocity Integer", i + 1));
                jumpHeight.RemoveAt(rInt);
            }
        }

        protected void ChangeClash(Patch Patch, Random r)
        {
            Int32 rInt;
            Double rDbl;

            // Unused addresses
            //0x02CDAF - Jump velocity fraction? 0x44 Double check

            // Clashman AI

            // Crash Man's routine for attack
            //0x02CCf2 - Walk x-vel fraction 0x47
            rInt = r.Next(256);
            Patch.Add(0x02CCf2, (Byte)rInt, "Clashman Walk X-Velocity Fraction");

            //0x02CCF7 - Walk x-vel integer 0x01, do 0 to 2
            rInt = r.Next(2);
            Patch.Add(0x02CCF7, (Byte)rInt, "Clashman Walk X-Velocity Integer");

            //0x02CD07 - Jump behavior 0x27. 0x17 = always jumping, any other value = doesn't react with a jump.
            // Give 25% chance for each unique behavior, and 50% for default.
            // UPDATE: One of these two behaviors breaks and clashman goes crazy. I think it's 0x17. disable.
            rDbl = r.NextDouble();
            Byte jumpType = 0x27;
            if (rDbl > 0.75)
            //{
            //    jumpType = 0x17;
            //}
            //else if (rDbl > 0.5)
            {
                jumpType = 0x26;
            }
            Patch.Add(0x02CD07, jumpType, "Clashman Special Jump Behavior");

            //0x02CD2A - Jump y-vel intger, 0x06, do from 0x02 to 0x0A
            rInt = r.Next(0x0A - 0x02 + 1) + 0x02;
            Patch.Add(0x02CD2A, (Byte)rInt, "Clashman Jump Y-Velocity Integer");

            //0x02CDD3 - Shot behavior, 0x5E, change to have him always shoot when jumping, 20% chance
            rDbl = r.NextDouble();
            if (rDbl > 0.80)
            {
                Patch.Add(0x02CDD3, 0x50, "Clashman Disable Single Shot");
            }

            //0x02CDEE - Clash Bomber velocity, 0x06, do from 2 to 8
            rInt = r.Next(7) + 2;
            Patch.Add(0x02CDEE, (Byte)rInt, "Clashman Clash Bomber X-Velocity");
        }
    }
}
