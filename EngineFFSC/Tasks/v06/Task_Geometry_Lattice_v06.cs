// Task_Geometry_Lattice_v06.cs
//
// Task para generar lattice estructural v06.

using PicoGK;
using EngineFFSC.Geometry;
using FFSC_PicoGK.Physics.Stress;
using FFSC_PicoGK.Models;

namespace EngineFFSC.Tasks
{
    public static class Task_Geometry_Lattice_v06
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

            Voxels stress = StressField.Dynamic(camara, spike, manifold);
            return Geometry_Lattice_v06.Generate(stress, minDensity: 0.3, maxDensity: 0.7);
        }
    }
}
