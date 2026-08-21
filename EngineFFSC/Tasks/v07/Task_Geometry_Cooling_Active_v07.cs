// Task_Geometry_Cooling_Active_v07.cs
//
// Task para generar canales de refrigeracion activa v07.

using PicoGK;
using EngineFFSC.Geometry;
using FFSC_PicoGK.Models;

namespace EngineFFSC.Tasks
{
    public static class Task_Geometry_Cooling_Active_v07
    {
        public static Voxels Task()
        {
            return Geometry_Cooling_Active_v07.Build(
                channelBaseWidth: 0.006,
                channelHeight: 0.0015,
                channelCount: 120,
                pitch: 0.02);
        }
    }
}
