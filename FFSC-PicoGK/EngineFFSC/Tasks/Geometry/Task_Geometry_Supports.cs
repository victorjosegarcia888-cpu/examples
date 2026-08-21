// Task_Geometry_Supports.cs
//
// Task para generar soportes del motor.

using PicoGK;
using FFSC_PicoGK.Geometry.Supports;

namespace FFSC_PicoGK.Tasks.Geometry
{
    public static class Task_Geometry_Supports
    {
        public static Voxels Task()
        {
            return Geometry_Supports.Create(4, 0.03, 0.40, 0.50);
        }
    }
}
