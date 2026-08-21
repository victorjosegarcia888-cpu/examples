// Task_CoolingActive_v07.cs
//
// Task para refrigeracion activa v07.

using PicoGK;
using FFSC_PicoGK.Models;
using EngineFFSC.Physics;

namespace EngineFFSC.Tasks
{
    public static class Task_CoolingActive_v07
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

            var thermo = Physics_Thermo_v07.Run(p);
            Physics_Cooling_Active_v07.Run(p, thermo);
            return new Voxels();
        }
    }
}
