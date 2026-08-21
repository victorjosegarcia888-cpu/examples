// FFSC_v06.cs
//
// Version v06 - Motor FFSC completo.

using PicoGK;
using EngineFFSC.Tasks;

namespace EngineFFSC.Versions
{
    public static class FFSC_v06
    {
        public static Voxels Build()
        {
            return Task_Assembly_FFSC_v06.Task();
        }
    }
}
