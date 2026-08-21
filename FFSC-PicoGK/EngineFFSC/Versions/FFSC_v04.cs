// FFSC_v04.cs
//
// Version v04 Redundante del motor FFSC.

using PicoGK;
using FFSC_PicoGK.EngineFFSC.Assembly;

namespace FFSC_PicoGK.EngineFFSC.Versions
{
    public static class FFSC_v04
    {
        public static Voxels Build()
        {
            return FFSC_Assembly_Modular.V04();
        }
    }
}
