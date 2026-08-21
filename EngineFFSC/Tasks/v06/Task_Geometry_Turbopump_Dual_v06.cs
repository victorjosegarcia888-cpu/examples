// Task_Geometry_Turbopump_Dual_v06.cs
//
// Task para generar geometria de turbobomba dual v06.

using PicoGK;
using EngineFFSC.Geometry;

namespace EngineFFSC.Tasks
{
    public static class Task_Geometry_Turbopump_Dual_v06
    {
        public static Voxels Task()
        {
            return Geometry_Turbopump_Dual_v06.Create(
                rotorRadius: 0.16,
                bladeCount: 10,
                bladeChord: 0.04,
                bladeHeight: 0.06);
        }
    }
}
