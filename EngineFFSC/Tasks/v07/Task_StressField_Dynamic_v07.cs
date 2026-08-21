// Task_StressField_Dynamic_v07.cs
//
// Task para campo de tensiones dinamico v07.

using PicoGK;
using EngineFFSC.Geometry;
using EngineFFSC.Physics;
using FFSC_PicoGK.Models;
using FFSC_PicoGK.Geometry.Chamber;
using FFSC_PicoGK.Geometry.Aerospike;
using FFSC_PicoGK.Geometry.Manifolds;

namespace EngineFFSC.Tasks
{
    public static class Task_StressField_Dynamic_v07
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

            var thermo = Physics_Thermo_v07.Run(new EngineParams
            {
                ChamberPressure_bar = 350.0,
                MixtureRatio = 3.6,
                Lstar = 1.2
            });
            var thickness = Physics_Thickness_v07.Run(new EngineParams
            {
                ChamberPressure_bar = 350.0,
                SafetyFactor = 1.5
            }, thermo);
            Physics_Stress_Dynamic_v07.Run(new EngineParams
            {
                ChamberPressure_bar = 350.0,
                Material = new MaterialSpec { YieldStrengthPa = 1.03e9 }
            }, thermo, thickness);
            return new Voxels();
        }
    }
}
