// Task_Geometry_Pipes.cs
//
// Task para generar tuberias de alta presion.

using PicoGK;
using FFSC_PicoGK.Geometry.Pipes;

namespace FFSC_PicoGK.Tasks.Geometry
{
    public static class Task_Geometry_Pipes
    {
        public static Voxels Task()
        {
            return Geometry_Pipes.Create(0.03, 0.03, 0.80, 0.60);
        }
    }
}
