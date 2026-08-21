// ShapeKernel_Turbopump.cs
//
// Exportacion parametrica para ShapeKernel.

using FFSC_PicoGK.Models;
using System.Collections.Generic;

namespace FFSC_PicoGK.EngineFFSC.Turbopump
{
    public static class ShapeKernel_Turbopump
    {
        public static Dictionary<string, double> Export(TurbopumpDesign spec)
        {
            return new Dictionary<string, double>
            {
                { "r1", spec.R1 },
                { "r2", spec.R2 },
                { "bladeHeight", spec.BladeHeight },
                { "omega", spec.Omega },
                { "U2", spec.U2 },
                { "Cu2", spec.ShapeParams["Cu2"] }
            };
        }
    }
}
