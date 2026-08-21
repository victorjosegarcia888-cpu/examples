// Task_CFD.cs
//
// Task para CFD simplificado.

using PicoGK;
using FFSC_PicoGK.Physics.CFD;
using FFSC_PicoGK.Geometry.Chamber;
using FFSC_PicoGK.Geometry.Nozzle;
using FFSC_PicoGK.Geometry.Manifolds;

namespace FFSC_PicoGK.Tasks.Physics
{
    /// <summary>
    /// Task de CFD simplificado.
    /// </summary>
    public static class Task_CFD
    {
        /// <summary>
        /// Ejecuta CFD estatico.
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
            var geom = Field3D.Combine(camara, nozzle, manifold);

            return CFDTask.Static(geom);
        }
    }
}
