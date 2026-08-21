// FFSC_v06.cs
//
// Version v06 - Completa avanzada del motor FFSC.

using PicoGK;
using FFSC_PicoGK.EngineFFSC.Assembly;

namespace FFSC_PicoGK.EngineFFSC.Versions
{
    public static class FFSC_v06
    {
        public static Voxels Build()
        {
            return FFSC_Assembly_Modular.V06();
        }
    }
}
