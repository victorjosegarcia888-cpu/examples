// Task_CFD_v06.cs
//
// Task para CFD v06.

using PicoGK;
using FFSC_PicoGK.Models;
using EngineFFSC.Geometry;
using EngineFFSC.Physics;

namespace EngineFFSC.Tasks
{
    public static class Task_CFD_v06
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

            var nozzle = Geometry_Nozzle_v06.Create(new EngineParams
            {
                ThroatRadius = 0.12,
                ExitRadius = 0.80,
                Lstar = 1.2,
                ChamberLength = 0.50
            });

            var manifold = Geometry_Manifold_FFSC_v06.Create(
                innerRadius: 0.25,
                outerRadius: 0.35,
                length: 0.6);

            Voxels geom = camara + nozzle + manifold;
            var result = Physics_CFD_v06.Solve(new EngineParams
            {
                ChamberRadius = 0.35,
                ChamberLength = 0.50,
                ThroatRadius = 0.12,
                Lstar = 1.2
            });
            return new Voxels();
        }
    }
}
