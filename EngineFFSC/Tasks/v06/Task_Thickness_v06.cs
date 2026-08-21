// Task_Thickness_v06.cs
//
// Task para calculo de espesor estructural v06.

using PicoGK;
using FFSC_PicoGK.Models;
using EngineFFSC.Physics;

namespace EngineFFSC.Tasks
{
    public static class Task_Thickness_v06
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
                SafetyFactor = 1.5
            };

            Physics_Thickness_v06.WallThickness_Barlow(p.Pc, p.ChamberRadius, p.Material.YieldStrengthPa, p.SafetyFactor);
            return new Voxels();
        }
    }
}
