// Task_CoolingAdaptive_v06.cs
//
// Task para refrigeracion adaptativa v06.

using PicoGK;
using FFSC_PicoGK.Models;
using EngineFFSC.Physics;

namespace EngineFFSC.Tasks
{
    public static class Task_CoolingAdaptive_v06
    {
        public static Voxels Task()
        {
            EngineParams p = new EngineParams
            {
                ThroatRadius = 0.12,
                ExitRadius = 0.80,
                ChamberPressure_bar = 350.0,
                Lstar = 1.2,
                ExpansionRatio = 45.0,
                Nz = 300,
                CoolantMassFlow = 50.0,
                CoolantInletTemp_C = 20.0,
                CoolantOutletTemp_C = 750.0
            };

            double Hg = 1e6;
            double Tw = 800.0;
            double Tcoolant = 20.0;
            Physics_Cooling_v06.HeatFlux(Hg, Tw, Tcoolant);
            return new Voxels();
        }
    }
}
