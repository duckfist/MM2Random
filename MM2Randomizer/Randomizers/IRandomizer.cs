using MM2Randomizer.Patcher;
using System;

namespace MM2Randomizer.Randomizers
{
    public interface IRandomizer
    {
        void Randomize(Patch p, Random r);
    }
}
