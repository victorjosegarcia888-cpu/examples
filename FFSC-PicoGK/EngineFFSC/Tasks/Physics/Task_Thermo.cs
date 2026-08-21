// Task_Thermo.cs
//
// Task para calculo termoquimico del motor FFSC.

using FFSC_PicoGK.Models;
using FFSC_PicoGK.Physics.Thermo;

namespace FFSC_PicoGK.Tasks.Physics
{
    public static class Task_Thermo
    {
        public static ThermoMap Task()
        {
            EngineParams p = new EngineParams
            {
                ThroatRadius = 0.12,
                ExitRadius = 0.80,
                ChamberPressure_bar = 350.0,
                Lstar = 1.2,
                ExpansionRatio = 45.0,
                Nz = 300
            };

            return ComputeThermoTask.Run(p);
        }
    }
}
