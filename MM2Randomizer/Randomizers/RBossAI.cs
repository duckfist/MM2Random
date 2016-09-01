using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MM2Randomizer.Randomizers
{
    public class RBossAI
    {
        public RBossAI(Random r)
        {
            Randomize(r);
        }

        private void Randomize(Random r)
        {
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
            }
        }

        public void ChangeHeat(Random r, FileStream stream)
        {
            int rChargeVel = 0;
            int rDistance = 0;
            int rDelay = 0;

            // Heatman AI 0x02C16E - 0x02C1FE

            // projectile y - distances
            //0x02C207 default 07, good from 03 - 08
            //0x02C208 default 05, good from 04 - 07
            //0x02C209 default 03, good from 03 - 05
            rDistance = r.Next(6) + 0x03;
            stream.Position = 0x02C207;
            stream.WriteByte((byte)rDistance);
            rDistance = r.Next(4) + 0x04;
            stream.Position = 0x02C208;
            stream.WriteByte((byte)rDistance);
            rDistance = r.Next(3) + 0x03;
            stream.Position = 0x02C209;
            stream.WriteByte((byte)rDistance);

            // projectile x-distances, 0x3A 0x2E 0x1C
            // - The lower value, the faster speed. Different for each fireball.
            //0x02C20A - 1st value should be 0x47 to hit megaman, Or, from 0x30 to 0x80
            //0x02C20B - 2nd value should be 0x2E to hit megaman. Or, from 0x22 to 0x40
            //0x02C20C - 3rd value should be 0x17 to hit megaman, Or, from 0x10 to 0x30
            rDistance = r.Next(0x80 - 0x30 + 1) + 0x30;
            stream.Position = 0x02C20A;
            stream.WriteByte((byte)rDistance);
            rDistance = r.Next(0x40 - 0x22 + 1) + 0x22;
            stream.Position = 0x02C20B;
            stream.WriteByte((byte)rDistance);
            rDistance = r.Next(0x30 - 0x10 + 1) + 0x10;
            stream.Position = 0x02C20C;
            stream.WriteByte((byte)rDistance);

            // 30/60/90 frame delay
            //0x02C29D - Delay 1 0x1F
            //0x02C29E - Delay 2 0x3E
            //0x02C29F - Delay 3 0x5D
            // Choose delay interval from 10-40 frames
            rDelay = r.Next(31) + 10;
            stream.Position = 0x02C29D;
            stream.WriteByte((byte)rDelay);
            stream.Position = 0x02C29E;
            stream.WriteByte((byte)(rDelay * 2));
            stream.Position = 0x02C29F;
            stream.WriteByte((byte)(rDelay * 3));

            //0x02C253 - Charge velocity(0x04, 0x08 or more usually puts him on side of screen)
            rChargeVel = r.Next(4) + 0x02;
            stream.Position = 0x02C253;
            stream.WriteByte((byte)rChargeVel);
        }

        public void ChangeAir(Random r, FileStream stream)
        {
            int rIndex = 0;
            int rFraction = 0;
            double rChance = 0;

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
            int A_tornadoTableLength = 0x1E;

            // Write y-vel fractions: 00-FF
            stream.Position = 0x02C393;
            for (int i = 0; i < A_tornadoTableLength; i++)
            {
                rFraction = r.Next(256);
                stream.WriteByte((byte)rFraction);
            }

            // Write y-vel integers: FF-03, rare 04
            stream.Position = 0x02C3B1;
            for (int i = 0; i < A_tornadoTableLength; i++)
            {
                byte A_yVelInt = 0;
                byte[] A_yVelInts = new byte[] { 0xFF, 0x00, 0x01, 0x02, 0x03 };
                rChance = r.NextDouble();
                if (rChance > 0.9)
                {
                    A_yVelInt = 0x04;
                }
                else
                {
                    rIndex = r.Next(A_yVelInts.Length);
                    A_yVelInt = A_yVelInts[rIndex];
                }
                stream.WriteByte(A_yVelInt);
            }

            // Write x-vel fractions: 00-FF
            stream.Position = 0x02C3CF;
            for (int i = 0; i < A_tornadoTableLength; i++)
            {
                rFraction = r.Next(256);
                stream.WriteByte((byte)rFraction);
            }

            // Write x-vel integers: 00-04, rare 04, common 03
            stream.Position = 0x02C3ED;
            for (int i = 0; i < A_tornadoTableLength; i++)
            {
                byte A_xVelInt = 0;
                byte[] A_xVelInts = new byte[] { 0x00, 0x01, 0x02 };
                rChance = r.NextDouble();
                if (rChance > 0.9)
                {
                    A_xVelInt = 0x04;
                }
                else if (rChance > 0.6)
                {
                    A_xVelInt = 0x03;
                }
                else
                {
                    rIndex = r.Next(A_xVelInts.Length);
                    A_xVelInt = A_xVelInts[rIndex];
                }
                stream.WriteByte(A_xVelInt);
            }

            // Write delays: 05-2A
            stream.Position = 0x02C40B;
            int rDelay = 0;
            for (int i = 0; i < A_tornadoTableLength; i++)
            {
                rDelay = r.Next(0x25) + 0x05;
                stream.WriteByte((byte)rDelay);
            }

            // 0x02C30C - Num patterns before jumping 0x03 (do 1-4)
            int rNumPatterns = r.Next(4) + 1;
            stream.Position = 0x02C30C;
            stream.WriteByte((byte)rNumPatterns);

            //0x02C4DD - First Jump y-vel frac, 0xE6
            //0x02C4DE - Second Jump y-vel frac, 0x76
            //0x02C4E0 - First Jump y-vel int, 0x04
            //0x02C4E1 - Second Jump y-vel int 0x07
            //0x02C4E3 - First Jump x-vel frac, 0x39
            //0x02C4E4 - Second Jump x-vel frac 0x9a
            //0x02C4E6 - First Jump x-vel int, 0x01
            //0x02C4E7 - Second Jump x-vel int 0x01
            // Pick x-vel integers for both jumps first. Must add up to 2 or 3.
            int rSum = r.Next(2) + 2;
            int jump1x = r.Next(rSum + 1);
            int jump2x = rSum - jump1x;
            stream.Position = 0x02C4E6;
            stream.WriteByte((byte)jump1x);
            stream.Position = 0x02C4E7;
            stream.WriteByte((byte)jump2x);
            // If a jump's x-int is 0, its corresponding y-int must be 6-7
            // If a jump's x-int is 1, its corresponding y-int must be 4-7
            // If a jump's x-int is 2, its corresponding y-int must be 3-5
            // If a jump's x-int is 3, its corresponding y-int must be 2-4
            int jump1y = AirmanGetJumpYVelocity(jump1x, r);
            int jump2y = AirmanGetJumpYVelocity(jump2x, r);
            stream.Position = 0x02C4E0;
            stream.WriteByte((byte)jump1y);
            stream.Position = 0x02C4E1;
            stream.WriteByte((byte)jump2y);

            // Random x and y-vel fractions for both jumps
            stream.Position = 0x02C4DD; // 1st jump y-vel frac
            rFraction = r.Next(0xF1); // If jump is 7 and fraction is > 0xF0, Airman gets stuck!
            stream.WriteByte((byte)rFraction);
            stream.Position = 0x02C4DE; // 2nd jump y-vel frac
            rFraction = r.Next(0xF1);
            stream.WriteByte((byte)rFraction);
            stream.Position = 0x02C4E3; // 1st jump x-vel frac
            rFraction = r.Next(256);
            stream.WriteByte((byte)rFraction);
            stream.Position = 0x02C4E4; // 2nd jump x-vel frac
            rFraction = r.Next(256);
            stream.WriteByte((byte)rFraction);
        }

        private byte AirmanGetJumpYVelocity(int xVelInt, Random r)
        {
            int jumpYMax = 0;
            int jumpYMin = 0;
            switch (xVelInt)
            {
                case 0:
                    jumpYMax = 7;
                    jumpYMin = 6;
                    break;
                case 1:
                    jumpYMax = 7;
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

            int yVelInt = r.Next(jumpYMax - jumpYMin + 1) + jumpYMin;
            return (byte)yVelInt;
        }

        public void ChangeWood(Random r, FileStream stream)
        {
            int rIndex = 0;
            int rByte = 0;
            double rChance = 0.0;
            byte[] xVels;

            // Woodman AI

            // Some unused addresses for later:
            //0x02C567 - Falling leaf y-pos start, 0x20
            //0x03DA34 - Leaf shield y-velocity while it's attached to woodman, lol.

            //0x02C537 - Delay between leaves 0x12. Do 0x06 to 0x20.
            rByte = r.Next(0x20 - 0x06 + 1) + 0x06;
            stream.Position = 0x02C537;
            stream.WriteByte((byte)rByte);

            //0x02C5DD - Jump height, 0x04. Do 0x03 to 0x08.
            rByte = r.Next(0x08 - 0x03 + 1) + 0x03;
            stream.Position = 0x02C5DD;
            stream.WriteByte((byte)rByte);

            //0x02C5E2 - Jump distance, 0x01. Do 0x01 to 0x04.
            rByte = r.Next(0x04 - 0x01 + 1) + 0x01;
            stream.Position = 0x02C5E2;
            stream.WriteByte((byte)rByte);

            //0x02C5A9 - Shield launch speed, 0x04. Do 0x01 to 0x08.
            rByte = r.Next(0x08 - 0x01 + 1) + 0x01;
            stream.Position = 0x02C5A9;
            stream.WriteByte((byte)rByte);

            //0x02C553 - Number of falling leaves, 0x03. Do 0x02 20% of the time.
            rChance = r.NextDouble();
            if (rChance > 0.8)
            {
                stream.Position = 0x02C553;
                stream.WriteByte(0x02);
            }

            //0x02C576 - Falling leaf x-vel, 0x02. Do 0x01 or 0x02, but with a 10% chance for 0x00 and 10% for 0x03
            xVels = new byte[] {
                0x00,
                0x03,
                0x01,0x01,0x01,0x01,
                0x02,0x02,0x02,0x02,
            };
            rIndex = r.Next(xVels.Length);
            stream.Position = 0x02C576;
            stream.WriteByte(xVels[rIndex]);

            //0x03D8F6 - 0x02, change to 0x06 for an interesting leaf shield pattern 25% of the time
            rChance = r.NextDouble();
            if (rChance > 0.75)
            {
                stream.Position = 0x02C553;
                stream.WriteByte(0x06);
            }

            //0x03B855 - Leaf fall speed(sort of ?) 0x20. 
            // Decrease value to increase speed. At 0x40, it doesn't fall. 
            // 20% of the time, change to a high number to instantly despawn leaves for a fast pattern. 
            // Do from 0x00 to 0x24.  Make less than 0x1A a lower chance.
            int xVel;
            rChance = r.NextDouble();
            if (rChance > 0.8)
            {
                xVel = 0xA0; // Leaves go upwards
            }
            else
            {
                xVels = new byte[]
                {
                    0x00, 0x04, 0x08, 0x0C, 0x10, 0x14, 0x18, 0x1C, // Fall faster
                    0x1D, 0x1E, 0x20, 0x21, 0x22, 0x23, 0x24        // Fall slower
                };
                rIndex = r.Next(xVels.Length);
                xVel = xVels[rIndex];
            }
            stream.Position = 0x03B855;
            stream.WriteByte((byte)xVel);
        }

        public void ChangeBubble(Random r, FileStream stream)
        {
            byte[] yVels;
            int rIndex;

            // Bubbleman's AI

            //0x02C70B - Falling speed integer, 0xFF.
            yVels = new byte[] { 0xF0, 0xF4, 0xF8, 0xFC, 0xFF };
            rIndex = r.Next(yVels.Length);
            stream.Position = 0x02C70B;
            stream.WriteByte(yVels[rIndex]);

            //0x02C6D3 - Rising speed integer, 0x01.
            yVels = new byte[] { 0x01, 0x02, 0x03, 0x05 };
            rIndex = r.Next(yVels.Length);
            stream.Position = 0x02C6D3;
            stream.WriteByte(yVels[rIndex]);
        }

        public void ChangeQuick(Random r, FileStream stream)
        {
            int rInt;

            // Other addresses with potential:
            //0x02C872 - Projectile type, 0x59

            // Quickman's AI
            //0x02C86E - Number of Boomerangs, 0x03, do from 1 - 0x0A
            rInt = r.Next(0x0B) + 0x01;
            stream.Position = 0x02C86E;
            stream.WriteByte((byte)rInt);

            //0x02C882 - Boomerang: delay before arc 0x25. 0 for no arc, or above like 0x35. do from 5 to 0x35.
            rInt = r.Next(0x35 - 0x05 + 1) + 0x05;
            stream.Position = 0x02C882;
            stream.WriteByte((byte)rInt);

            //0x02C887 - Boomerang speed when appearing, 0x04, do from 0x01 to 0x07
            rInt = r.Next(0x07 - 0x01 + 1) + 0x01;
            stream.Position = 0x02C887;
            stream.WriteByte((byte)rInt);

            //0x03B726 - Boomerang speed secondary, 0x04, does this affect anything else?
            rInt = r.Next(0x07 - 0x01 + 1) + 0x01;
            stream.Position = 0x03B726;
            stream.WriteByte((byte)rInt);

            // For all jumps, choose randomly from 0x02 to 0x0A
            //0x02C8A3 - Middle jump, 0x07
            rInt = r.Next(0x0A - 0x02 + 1) + 0x02;
            stream.Position = 0x02C8A3;
            stream.WriteByte((byte)rInt);

            //0x02C8A4 - High jump, 0x08
            rInt = r.Next(0x0A - 0x02 + 1) + 0x02;
            stream.Position = 0x02C8A4;
            stream.WriteByte((byte)rInt);

            //0x02C8A5 - Low jump, 0x04
            rInt = r.Next(0x0A - 0x02 + 1) + 0x02;
            stream.Position = 0x02C8A5;
            stream.WriteByte((byte)rInt);

            //0x02C8E4 - Running time, 0x3E, do from 0x18 to 0x50
            rInt = r.Next(0x50 - 0x18 + 1) + 0x18;
            stream.Position = 0x02C8E4;
            stream.WriteByte((byte)rInt);

            //0x02C8DF - Running speed, 0x02, do from 0x05 to 0x01
            rInt = r.Next(0x05 - 0x01 + 1) + 0x01;
            stream.Position = 0x02C8DF;
            stream.WriteByte((byte)rInt);
        }

        public void ChangeFlash(Random r, FileStream stream)
        {
            int rInt;

            // Unused addresses
            //0x02CA71 - Projectile type 0x35
            //0x02CA52 - "Length of time stopper / projectile frequency ?"

            // Flashman's AI

            //0x02C982 - Walk velocity integer 0x01, do from 0 to 3
            rInt = r.Next(0x04);
            stream.Position = 0x02C982;
            stream.WriteByte((byte)rInt);

            //0x02C97D - Walk velocity fraction 0x06, do 0x00-0xFF
            rInt = r.Next(256);
            stream.Position = 0x02C97D;
            stream.WriteByte((byte)rInt);

            //0x02C98B - Delay before time stopper 0xBB (187 frames). Do from 30 frames to 240 frames
            rInt = r.Next(211) + 30;
            stream.Position = 0x02C98B;
            stream.WriteByte((byte)rInt);

            //0x02CAC6 - Jump distance integer 0x00, do 0 to 3
            // TODO do fraction also
            rInt = r.Next(4);
            stream.Position = 0x02CAC6;
            stream.WriteByte((byte)rInt);

            //0x02CACE - Jump height 0x04, do 3 - 8
            rInt = r.Next(6) + 3;
            stream.Position = 0x02CACE;
            stream.WriteByte((byte)rInt);

            //0x02CA81 - Projectile speed 0x08, do 2 - 0A
            rInt = r.Next(0x0A - 0x02 + 1) + 0x02;
            stream.Position = 0x02CA81;
            stream.WriteByte((byte)rInt);

            //0x02CA09 - Number of projectiles to shoot 0x06, do 3 - 0x10
            rInt = r.Next(0x10 - 0x03 + 1) + 0x03;
            stream.Position = 0x02CA09;
            stream.WriteByte((byte)rInt);
        }

        public void ChangeMetal(Random r, FileStream stream)
        {
            int rInt;
            double rDbl;

            // Unused addresses
            //0x02CC2D - Projectile type
            //0x02CC29 - Metal Blade sound effect 0x20

            // Metalman AI

            //0x02CC3F - Speed of Metal blade 0x04, do 2 to 9
            rInt = r.Next(8) + 2;
            stream.Position = 0x02CC3F;
            stream.WriteByte((byte)rInt);

            //0x02CC1D - Odd change to attack behavior, 0x06, only if different than 6. Give 25% chance.
            rDbl = r.NextDouble();
            if (rDbl > 0.75)
            {
                stream.Position = 0x02CC3F;
                stream.WriteByte((byte)0x05);
            }

            //0x02CBB5 - Jump Height 1 0x06, do from 03 - 07 ? higher than 7 bonks ceiling
            //0x02CBB6 - Jump Height 2 0x05
            //0x02CBB7 - Jump Height 3 0x04
            List<int> jumpHeight = new List<int>(){ 0x03, 0x04, 0x05, 0x06, 0x07 };
            stream.Position = 0x02CBB5;
            for (int i = 0; i < 3; i ++)
            {
                // Pick a height at random and remove from list to get 3 different heights
                rInt = r.Next(jumpHeight.Count);
                stream.WriteByte((byte)jumpHeight[rInt]);
                jumpHeight.RemoveAt(rInt);
            }
        }

        public void ChangeClash(Random r, FileStream stream)
        {
            int rInt;
            double rDbl;

            // Unused addresses
            //0x02CDAF - Jump velocity fraction? 0x44 double check

            // Clashman AI

            // Crash Man's routine for attack
            //0x02CCf2 - Walk x-vel fraction 0x47
            rInt = r.Next(256);
            stream.Position = 0x02CCf2;
            stream.WriteByte((byte)rInt);

            //0x02CCF7 - Walk x-vel integer 0x01, do 0 to 3
            rInt = r.Next(4);
            stream.Position = 0x02CCF7;
            stream.WriteByte((byte)rInt);

            //0x02CD07 - Jump behavior 0x27. 0x17 = always jumping, any other value = doesn't react with a jump.
            // Give 25% chance for each unique behavior, and 50% for default.
            rDbl = r.NextDouble();
            byte jumpType = 0x27;
            if (rDbl > 0.75)
            {
                jumpType = 0x17;
            }
            else if (rDbl > 0.5)
            {
                jumpType = 0x00;
            }
            stream.Position = 0x02CD07;
            stream.WriteByte(jumpType);

            //0x02CD2A - Jump y-vel intger, 0x06, do from 0x02 to 0x0A
            rInt = r.Next(0x0A - 0x02 + 1) + 0x02;
            stream.Position = 0x02CD2A;
            stream.WriteByte((byte)rInt);
            
            //0x02CDD3 - Shot behavior, 0x5E, change to have him always shoot when jumping, 20% chance
            rDbl = r.NextDouble();
            if (rDbl > 0.20)
            {
                stream.Position = 0x02CDD3;
                stream.WriteByte(0x50);
            }

            //0x02CDEE - Clash Bomber velocity, 0x06, do from 2 to 8
            rInt = r.Next(7) + 2;
            stream.Position = 0x02CD2A;
            stream.WriteByte((byte)rInt);
        }
    }
}
