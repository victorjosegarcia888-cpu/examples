// Task_Geometry_Lattice_Intelligent_v07.cs
//
// Task para generar lattice inteligente v07.

using PicoGK;
using EngineFFSC.Geometry;
using FFSC_PicoGK.Models;
using FFSC_PicoGK.Geometry.Chamber;
using FFSC_PicoGK.Geometry.Aerospike;
using FFSC_PicoGK.Geometry.Manifolds;

namespace EngineFFSC.Tasks
{
    public static class Task_Geometry_Lattice_Intelligent_v07
    {
        public static Voxels Task()
        {
            var camara = Geometry_Chamber.Create(new EngineParams
            {
                ChamberRadius = 0.35,
                ChamberLength = 0.50,
                ThroatRadius = 0.12,
                Lstar = 1.2,
                ContractionRatio = 6.0
            });

            var spike = Geometry_Aerospike.Create(new EngineParams
            {
                ChamberLength = 0.55,
                ExitRadius = 0.80
            });

            var manifold = Geometry_Manifold_FFSC.Create();

            Voxels stress = new Voxels();
            return Geometry_Lattice_Intelligent_v07.Build(stress, new EngineParams
            {
                ChamberRadius = 0.35,
                ChamberLength = 0.50,
                ThroatRadius = 0.12,
                Lstar = 1.2
            });
        }
    }
}
