// Task_Assembly_FFSC_v07.cs
//
// Task de ensamblado final del motor FFSC v07.

using PicoGK;
using FFSC_PicoGK.Models;
using FFSC_PicoGK.Geometry.Chamber;
using FFSC_PicoGK.Geometry.Nozzle;
using FFSC_PicoGK.Geometry.Aerospike;
using FFSC_PicoGK.EngineFFSC.Geometry;
using FFSC_PicoGK.Geometry.Manifolds;
using FFSC_PicoGK.Geometry.Injectors;
using FFSC_PicoGK.Geometry.Cooling;
using FFSC_PicoGK.Geometry.Pipes;
using FFSC_PicoGK.Geometry.Structural;
using FFSC_PicoGK.Geometry.Supports;
using FFSC_PicoGK.Physics.Stress;
using FFSC_PicoGK.Physics.CFD;

namespace EngineFFSC.Tasks
{
    public static class Task_Assembly_FFSC_v07
    {
        public static Voxels Task()
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
                ContractionRatio = 6.0,
                MixtureRatio = 3.6
            };

            p.Material = new MaterialSpec
            {
                Name = "Inconel_718",
                YieldStrengthPa = 1.03e9
            };

            Voxels result = new Voxels();

            result = result + Geometry_Chamber.Create(p);
            result = result + Geometry_Nozzle.Create(p);
            result = result + Geometry_Aerospike.Create(p);
            result = result + Geometry_Manifold_Fractal_LOX_v07.Build();
            result = result + Geometry_Manifold_Fractal_CH4_v07.Build();
            result = result + Geometry_Manifold_Fractal_FFSC_v07.Build();
            result = result + Geometry_Turbopump_Triple_v07.Build(p);
            result = result + Geometry_Injectors_v07.Build(p);
            result = result + Geometry_Cooling_Active_v07.Build();
            result = result + Geometry_Lattice_Intelligent_v07.Build(new Voxels(), p);

            var chamber = Geometry_Chamber.Create(p);
            var spike = Geometry_Aerospike.Create(p);
            result = result + Geometry_Cooling.Primary(chamber, spike);
            result = result + Geometry_Cooling.Secondary(chamber, spike);

            result = result + Geometry_Pipes.Create();
            result = result + Geometry_Structural.Create();
            result = result + Geometry_Supports.Create();

            var manifold = Geometry_Manifold_FFSC.Create();
            result = result + StressField.Dynamic(chamber, spike, manifold);

            var nozzle = Geometry_Nozzle.Create(p);
            result = result + CFDTask.Dynamic(chamber + nozzle + manifold);

            return result;
        }
    }
}
