// Task_Thermo_v07.cs
//
// Task para calculo termoquimico avanzado v07.

using PicoGK;
using FFSC_PicoGK.Models;
using EngineFFSC.Physics;

namespace EngineFFSC.Tasks
{
    public static class Task_Thermo_v07
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
                MixtureRatio = 3.6,
                TadInitialGuess = 3600.0
            };

            Physics_Thermo_v07.Run(p);
            return new Voxels();
        }
    }
}
