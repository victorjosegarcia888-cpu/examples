// Task_Geometry_Aerospike_v06.cs
//
// Task para generar geometria de aerospike v06.

using PicoGK;
using EngineFFSC.Geometry;
using FFSC_PicoGK.Models;

namespace EngineFFSC.Tasks
{
    public static class Task_Geometry_Aerospike_v06
    {
        public static Voxels Task()
        {
            return Geometry_Aerospike_v06.Create(new EngineParams
            {
                ThroatRadius = 0.12,
                ExitRadius = 0.80,
                ChamberLength = 0.50
            });
        }
    }
}
