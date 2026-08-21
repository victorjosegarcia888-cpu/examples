// Task_Geometry_Turbopump.cs
//
// Task para generar geometria de turbobomba.

using PicoGK;
using FFSC_PicoGK.Geometry.Turbopump;

namespace FFSC_PicoGK.Tasks.Geometry
{
    public static class Task_Geometry_Turbopump
    {
        public static Voxels Task()
        {
            return Geometry_Turbopump.Create(0.16, 0.05, 10, 0.04, 0.06);
        }
    }
}
