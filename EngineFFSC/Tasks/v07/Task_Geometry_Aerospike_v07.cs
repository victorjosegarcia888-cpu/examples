// Task_Geometry_Aerospike_v07.cs
//
// Task para generar geometria de aerospike v07.

using PicoGK;
using FFSC_PicoGK.Geometry.Aerospike;
using FFSC_PicoGK.Models;

namespace EngineFFSC.Tasks
{
    public static class Task_Geometry_Aerospike_v07
    {
        public static Voxels Task()
        {
            return Geometry_Aerospike.Create(new EngineParams
            {
                ChamberLength = 0.55,
                ExitRadius = 0.80
            });
        }
    }
}
