// FFSC_v07.cs
//
// Version v07 - Motor FFSC completo con todas las subsistemas avanzadas.

using PicoGK;
using EngineFFSC.Tasks;

namespace EngineFFSC.Versions
{
    public static class FFSC_v07
    {
        public static Voxels Build()
        {
            return Task_Assembly_FFSC_v07.Task();
        }
    }
}
