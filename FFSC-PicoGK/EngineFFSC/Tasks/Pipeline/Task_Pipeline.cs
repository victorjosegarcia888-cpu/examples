// Task_Pipeline.cs
//
// Task del pipeline completo FFSC.

using PicoGK;
using FFSC_PicoGK.Models;
using FFSC_PicoGK.Physics.Thermo;
using FFSC_PicoGK.Physics.Structural;
using FFSC_PicoGK.Physics.Stress;
using FFSC_PicoGK.Physics.Cooling;
using FFSC_PicoGK.Physics.CFD;
using FFSC_PicoGK.Geometry.Chamber;
using FFSC_PicoGK.Geometry.Nozzle;
using FFSC_PicoGK.Geometry.Aerospike;
using FFSC_PicoGK.Geometry.Manifolds;
using FFSC_PicoGK.Geometry.Injectors;
using FFSC_PicoGK.Geometry.Turbopump;
using FFSC_PicoGK.Geometry.Cooling;
using FFSC_PicoGK.Geometry.Pipes;
using FFSC_PicoGK.Geometry.Structural;
using FFSC_PicoGK.Geometry.Supports;

namespace FFSC_PicoGK.Tasks.Pipeline
{
    /// <summary>
    /// Task del pipeline completo FFSC.
    /// </summary>
    public static class Task_Pipeline
    {
        /// <summary>
        /// Ejecuta el pipeline completo.
        /// </summary>
        public static Field3D Task()
        {
            EngineParams p = new EngineParams
            {
                Thrust = 2_500_000.0,
                ChamberPressure_bar = 350.0,
                ExpansionRatio = 45.0,
                Lstar = 1.2,
                ThroatRadius = 0.12,
                ExitRadius = 0.80,
                ChamberRadius = 0.35,
                ChamberLength = 0.50,
                MixtureRatio = 3.6,
                ContractionRatio = 6.0
            };

            p.Material = new MaterialSpec
            {
                Name = "Inconel_718",
                YieldStrengthPa = 1.03e9
            };

            // 1. Termoquimica
            var thermo = ComputeThermoTask.Run(p);

            // 2. Espesor estructural
            var thickness = ComputeThicknessTask.Run(p, thermo);

            // 3. Geometria
            var camara = Geometry_Chamber.Create(p);
            var nozzle = Geometry_Nozzle.Create(p);
            var spike = Geometry_Aerospike.Create(p);
            var manifold = Geometry_Manifold_FFSC.Create();
            var injectors = Geometry_Injectors.Create();
            var turbopump = Geometry_Turbopump.Create();
            var cooling = Geometry_Cooling.Primary(camara, spike);
            var pipes = Geometry_Pipes.Create();
            var structural = Geometry_Structural.Create();
            var supports = Geometry_Supports.Create();

            // 4. Campos fisicos
            var stress = StressField.Dynamic(camara, spike, manifold);
            var cfd = CFDTask.Dynamic(Field3D.Combine(camara, nozzle, spike, manifold));

            // 5. Ensamblado
            var geom = Field3D.Combine(
                camara, nozzle, spike, manifold,
                injectors, turbopump, cooling,
                pipes, structural, supports,
                stress, cfd
            );

            return geom;
        }
    }
}
