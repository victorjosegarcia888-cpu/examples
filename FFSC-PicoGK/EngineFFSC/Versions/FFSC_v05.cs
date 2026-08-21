// FFSC_v05.cs
//
// Version v05 Adaptive del motor FFSC.

using PicoGK;
using FFSC_PicoGK.EngineFFSC.Assembly;

namespace FFSC_PicoGK.EngineFFSC.Versions
{
    public static class FFSC_v05
    {
        public static Voxels Build()
        {
            return FFSC_Assembly_Modular.V05();
        }
    }
}
