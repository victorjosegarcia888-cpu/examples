// FFSC_v03.cs
//
// Version v03 MultiObjetivo del motor FFSC.

using PicoGK;
using FFSC_PicoGK.EngineFFSC.Assembly;

namespace FFSC_PicoGK.EngineFFSC.Versions
{
    public static class FFSC_v03
    {
        public static Voxels Build()
        {
            return FFSC_Assembly_Modular.V03();
        }
    }
}
