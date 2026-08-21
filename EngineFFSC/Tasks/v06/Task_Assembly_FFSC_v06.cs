// Task_Assembly_FFSC_v06.cs
//
// Task de ensamblado final del motor FFSC v06.

using PicoGK;
using FFSC_PicoGK.Models;
using EngineFFSC.Geometry;
using EngineFFSC.Physics;

namespace EngineFFSC.Tasks
{
    public static class Task_Assembly_FFSC_v06
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
                ContractionRatio = 6.0
            };

            Voxels result = new Voxels();

            result = result + Geometry_Chamber_v06.Create(p);
            result = result + Geometry_Nozzle_v06.Create(p);
            result = result + Geometry_Aerospike_v06.Create(p);
            result = result + Geometry_Manifold_LOX_v06.Create();
            result = result + Geometry_Manifold_CH4_v06.Create();
            result = result + Geometry_Manifold_FFSC_v06.Create();
            result = result + Geometry_Turbopump_Dual_v06.Create();
            result = result + Geometry_Injectors_v06.Create();

            var chamber = Geometry_Chamber_v06.Create(p);
            var spike = Geometry_Aerospike_v06.Create(p);
            Voxels primary = Geometry_Cooling_v06.Primary(chamber, spike, p, channelWidth: 0.006, channelHeight: 0.0015);
            Voxels secondary = Geometry_Cooling_v06.Secondary(chamber, spike, p, filmThickness: 0.002);
            result = result + primary + secondary;

            double sigmaHoop = Physics_Stress_v06.HoopStress(350e5, 0.35, 0.012);
            Voxels stress = new Voxels();
            result = result + Geometry_Lattice_v06.Generate(stress, minDensity: 0.3, maxDensity: 0.7);
            result = result + stress;

            var cfdResult = Physics_CFD_v06.Solve(p);
            Voxels cfd = new Voxels();
            result = result + cfd;

            return result;
        }
    }
}
