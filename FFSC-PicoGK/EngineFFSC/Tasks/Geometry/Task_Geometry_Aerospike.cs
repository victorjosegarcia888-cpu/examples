// Task_Geometry_Aerospike.cs
//
// Task para generar geometria de aerospike lineal.

using PicoGK;
using FFSC_PicoGK.Geometry.Aerospike;

namespace FFSC_PicoGK.Tasks.Geometry
{
    public static class Task_Geometry_Aerospike
    {
        public static Voxels Task()
        {
            return Geometry_Aerospike.Create(0.55, 0.15);
        }
    }
}
