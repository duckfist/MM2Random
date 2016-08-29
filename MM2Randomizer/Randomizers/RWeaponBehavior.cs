using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MM2Randomizer.Randomizers
{
    public class RWeaponBehavior
    {
        public RWeaponBehavior(Random r)
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
            // TODO: Find ammo use

            //0x03DDEC - H shot sound effect(38)
            //0x03DDF1 - H x - speed(04, all levels)
            //    Do from 02 to 08
            //0x03DE45 - H charge sound 1(35)
            //0x03DE46 - H charge sound 2(35)
            //0x03DE47 - H charge sound 3(36)
            //0x03DE48 - H charge sound 4(37)
        }

        public void ChangeAir(Random r, FileStream stream)
        {
            //0x03DAD6 - A num projectiles, default 0x04
            //  Values 0x02 and 0x03 work, but larger values behave strangely
            //0x03DADA - A projectile type, default 0x02
            //  Can use this to change behavior completely!Buggy though.
            //0x03DAE6 - A sound effect (0x3F)
            //0x03DAEE - A ammo used(0x02)
            //0x03DE6E - A projectile y-acceleration fraction(10)
            //0x03DE76 - A projectile y-acceleration integer(00)
            //0x03DE7E - A x - speed fraction projectile 1(19)
            //0x03DE7F - A x - speed fraction projectile 2(99)
            //0x03DE80 - A x - speed fraction projectile 3(33)
            //0x03DE81 - A x - speed integer projectile 1(01)
            //0x03DE82 - A x - speed integer projectile 2(01)
            //0x03DE83 - A x - speed integer projectile 3(02)
        }

        public void ChangeWood(Random r, FileStream stream)
        {
            //0x03DEDA - W deploy time (0C)
            //    Can change from 06 to 12
            //0x03DF0D - W spin sound effect
            //0x03DF1B - W delay between sounds(07)
            //0x03DF1F - W deploy sound effect
            //0x03DF41 - W which directions the shield is allowed to launch in (F0)
            //    Can prevent launching left / right, up / down, etc
            //0x03DF4D - W launch directions (C0)
            //    Don't use this one
            //0x03DF52 - W launch x - direction(40)
            //    Can't figure this out.
            //0x03DF59 - W x - speed(04)
            //0x03DF64 - W launch y - direction(10)
            //    Change to 0x20 to reverse y - direction
            //0x03DF7D - W y - speed(04)
        }

        public void ChangeBubble(Random r, FileStream stream)
        {
            //0x03D4AB - B x - speed on shoot (0x01) (do 1-3)
            int xVelShoot = r.Next(0x03) + 0x01;
            stream.Position = 0x03D4AB;
            stream.WriteByte((byte)xVelShoot);

            //0x03D4CF - B y - speed on shoot(0x02) (do 0-6)
            int yVelShoot = r.Next(0x06);
            stream.Position = 0x03D4CF;
            stream.WriteByte((byte)yVelShoot);

            //0x03DB21 - B max shots (0x03) (do 2-5, i.e. 1-4 total projectiles)
            //    Valid from 0x02 - 0x0F. Lags a bunch >= 0x06.
            int maxShots = r.Next(0x04) + 0x02;
            stream.Position = 0x03DB21;
            stream.WriteByte((byte)maxShots);

            //0x03DB2F - B weapon type(0x04)
            // Don't do

            //0x03DB34 - B sound effect
            // TODO

            //0x03DB3D - B shots per ammo tick (0x02) (do 1-4)
            int magSize = r.Next(0x04) + 0x01;
            stream.Position = 0x03DB3D;
            stream.WriteByte((byte)magSize);

            //0x03DFA4 - B y - pos to embed in surface(0xFF)
            // Dumb

            //0x03DFA9 - B x - speed on surface (0x02)
            //      0x01 - 0x04 ?
            int xVelRoll = r.Next(0x04) + 0x01;
            stream.Position = 0x03DFA9;
            stream.WriteByte((byte)xVelRoll);

            //0x03DFC0 - B x - speed after falling from ledge (0x00)
            //      Make 50% chance to be 0, or 1-5
            int xVelFall = 0x00;
            double rTestXFallSpeed = r.Next();
            if (rTestXFallSpeed > 0.5)
                xVelFall = r.Next(0x05) + 0x01;
            stream.Position = 0x03DFC0;
            stream.WriteByte((byte)xVelFall);

            //0x03DFC8 - B y - speed after falling(0xFE)
            //      Either 0xFA - 0xFF or 0x01 - 0x06
            int[] yFallVels = new int[] { 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06 };
            int rIndex = r.Next(yFallVels.Length);
            int yFallVel = yFallVels[rIndex];
            stream.Position = 0x03DFC8;
            stream.WriteByte((byte)yFallVel);
        }

        public void ChangeQuick(Random r, FileStream stream)
        {
            // Q autofire delay, default 0x0B
            //    Do from 0x05 to 0x12
            int autoFireDelay = r.Next(0x0D) + 0x05;
            stream.Position = 0x03DB54;
            stream.WriteByte((byte)autoFireDelay);

            // Q max shots, default 0x05
            //    Do from 0x03 to 0x07(2 to 6 shots)
            int maxShots = r.Next(0x04) + 0x03;
            stream.Position = 0x03DB5C;
            stream.WriteByte((byte)maxShots);

            // 0x03DB6F - Q sound effect

            // Q shots per ammo tick, default 0x08
            //    Do from 0x04 to 0x0A ?
            int magSize = r.Next(0x06) + 0x04;
            stream.Position = 0x03DB78;
            stream.WriteByte((byte)magSize);

            // Q behavior, distance, default 0x12
            //    Do from 0x0A to 0x20 ?
            int distance = r.Next(0x16) + 0x0A;
            stream.Position = 0x03DFE2;
            stream.WriteByte((byte)distance);

            // Q behavior, initial angle, default 0x4B
            //    Do from 0x00 to 0x60 ?
            int angle1 = r.Next(0x60);
            stream.Position = 0x03DFEA;
            stream.WriteByte((byte)angle1);

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

            // Q behavior, time before disappearing on return, default 0x23
            //    Do from 0x1E to 0x30
            int despawnDelay = r.Next(0x12) + 0x1E;
            stream.Position = 0x03E007;
            stream.WriteByte((byte)despawnDelay);

            // Q behavior, return angle, default 0x4B
            //    Do from 0x00 to 0x90
            int angle3 = r.Next(0x90);
            stream.Position = 0x03E013;
            stream.WriteByte((byte)angle3);

            //0x03E01B - Q behavior, weird, default 0x00
            //    Change to 0x01 for interesting effects
        }

        public void ChangeFlash(Random r, FileStream stream)
        {
            // TODO
        }

        public void ChangeMetal(Random r, FileStream stream)
        {
            //0x03DBB6 - M max shots (04)
            //0x03DBC9 - M sound effect(23)
            //0x03DBD2 - M shots per ammo tick(04)
            //0x03DC12 - M y - speed, holding up(04)
            //0x03DC13 - M y - speed, holding down(FC)
            //0x03DC16 - M y - speed, holding up + left(02)
            //0x03DC17 - M y - speed, holding down + left(FD)
            //0x03DC1A - M y - speed, holding up + right(02)
            //0x03DC1B - M y - speed, holding down + right(FD)

            //0x03DC31 - M x - speed, no direction(04)
            //0x03DC35 - M x - speed, holding left(04)
            //0x03DC36 - M x - speed, holding up + left(02)
            //0x03DC37 - M x - speed, holding down + left(02)
            //0x03DC39 - M x - speed, holding right(04)
            //0x03DC3A - M x - speed, holding up + right(02)
            //0x03DC3B - M x - speed, holding down + right(02)
        }

        public void ChangeClash(Random r, FileStream stream)
        {
            // TODO

            //0x03DB99 - C ammo per shot
            //0x03E089 - C attach sound effect
            //0x03E0DA - C explode sound effect
        }
    }
}
