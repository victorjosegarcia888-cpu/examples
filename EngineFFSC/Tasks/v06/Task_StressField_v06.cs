// Task_StressField_v06.cs
//
// Task para campo de tensiones v06.

using PicoGK;
using FFSC_PicoGK.Models;
using EngineFFSC.Geometry;
using EngineFFSC.Physics;

namespace EngineFFSC.Tasks
{
    public static class Task_StressField_v06
    {
        public static Voxels Task()
        {
            var camara = Geometry_Chamber_v06.Create(new EngineParams
            {
                ChamberRadius = 0.35,
                ChamberLength = 0.50,
                ThroatRadius = 0.12,
                Lstar = 1.2,
                ContractionRatio = 6.0
            });

            var spike = Geometry_Aerospike_v06.Create(new EngineParams
            {
                ThroatRadius = 0.12,
                ExitRadius = 0.80,
                ChamberLength = 0.50
            });

            var manifold = Geometry_Manifold_FFSC_v06.Create(
                innerRadius: 0.25,
                outerRadius: 0.35,
                length: 0.6);

            double sigmaHoop = Physics_Stress_v06.HoopStress(350e5, 0.35, 0.012);
            return new Voxels();
        }
    }
}
