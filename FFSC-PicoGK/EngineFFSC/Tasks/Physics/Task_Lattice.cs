// Task_Lattice.cs
//
// Task para generacion de lattice adaptativo.

using PicoGK;
using FFSC_PicoGK.Physics.Stress;

namespace FFSC_PicoGK.Tasks.Physics
{
    /// <summary>
    /// Task de generacion de lattice.
    /// </summary>
    public static class Task_Lattice
    {
        /// <summary>
        /// Genera lattice dual-layer basado en tensiones.
        /// </summary>
        public static Field3D Task()
        {
            // Geometrias base
            var camara = FFSC_PicoGK.Geometry.Chamber.Geometry_Chamber.Create(
                new FFSC_PicoGK.Models.EngineParams
                {
                    ChamberRadius = 0.35,
                    ChamberLength = 0.50,
                    ThroatRadius = 0.12,
                    Lstar = 1.2,
                    ContractionRatio = 6.0
                });

            var spike = FFSC_PicoGK.Geometry.Aerospike.Geometry_Aerospike.Create(0.55, 0.15);
            var manifold = FFSC_PicoGK.Geometry.Manifolds.Geometry_Manifold_FFSC.Create();

            var stress = StressField.Static(camara, spike, manifold);

            // Capa gruesa
            var latticeHigh = Lattice_DualLayer.Generate(stress, 0.6, 0.3, 0.015, 0.008);

            return latticeHigh;
        }
    }
}
