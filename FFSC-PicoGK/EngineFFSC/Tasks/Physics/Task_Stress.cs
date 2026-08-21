// Task_Stress.cs
//
// Task para generacion de campo de tensiones.

using PicoGK;
using FFSC_PicoGK.Physics.Stress;
using FFSC_PicoGK.Geometry.Chamber;
using FFSC_PicoGK.Geometry.Nozzle;
using FFSC_PicoGK.Geometry.Manifolds;
using FFSC_PicoGK.Geometry.Aerospike;

namespace FFSC_PicoGK.Tasks.Physics
{
    /// <summary>
    /// Task de campo de tensiones.
    /// </summary>
    public static class Task_Stress
    {
        /// <summary>
        /// Ejecuta la generacion del campo de tensiones.
        /// </summary>
        public static Field3D Task()
        {
            var camara = Geometry_Chamber.Create(
                new FFSC_PicoGK.Models.EngineParams
                {
                    ChamberRadius = 0.35,
                    ChamberLength = 0.50,
                    ThroatRadius = 0.12,
                    Lstar = 1.2,
                    ContractionRatio = 6.0
                });

            var nozzle = Geometry_Nozzle.Create(
                new FFSC_PicoGK.Models.EngineParams
                {
                    ThroatRadius = 0.12,
                    ExitRadius = 0.80,
                    ExpansionRatio = 45.0,
                    Lstar = 1.2
                });

            var manifold = Geometry_Manifold_FFSC.Create();
            var spike = Geometry_Aerospike.Create(0.55, 0.15);

            return StressField.Dynamic(camara, nozzle, manifold);
        }
    }
}
