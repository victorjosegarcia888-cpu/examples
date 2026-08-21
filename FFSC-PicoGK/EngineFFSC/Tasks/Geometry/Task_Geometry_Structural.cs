// Task_Geometry_Structural.cs
//
// Task para generar geometria estructural.

using PicoGK;
using FFSC_PicoGK.Geometry.Structural;

namespace FFSC_PicoGK.Tasks.Geometry
{
    public static class Task_Geometry_Structural
    {
        public static Voxels Task()
        {
            return Geometry_Structural.Create(0.08, 0.10, 0.20, 0.40);
        }
    }
}
