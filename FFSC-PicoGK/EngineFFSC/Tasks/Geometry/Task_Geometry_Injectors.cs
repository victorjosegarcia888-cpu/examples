// Task_Geometry_Injectors.cs
//
// Task para generar geometria de inyectores.

using PicoGK;
using FFSC_PicoGK.Geometry.Injectors;

namespace FFSC_PicoGK.Tasks.Geometry
{
    public static class Task_Geometry_Injectors
    {
        public static Voxels Task()
        {
            return Geometry_Injectors.Create(32, 0.24, 0.008, 0.004, 0.06);
        }
    }
}
