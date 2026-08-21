// Task_Geometry_Cooling.cs
//
// Task para generar canales de refrigeracion.

using PicoGK;
using FFSC_PicoGK.Geometry.Cooling;
using FFSC_PicoGK.Geometry.Chamber;
using FFSC_PicoGK.Geometry.Aerospike;

namespace FFSC_PicoGK.Tasks.Geometry
{
    public static class Task_Geometry_Cooling
    {
        public static Voxels Task()
        {
            var chamber = Geometry_Chamber.Create(
                new FFSC_PicoGK.Models.EngineParams
                {
                    ChamberRadius = 0.35,
                    ChamberLength = 0.50,
                    ThroatRadius = 0.12,
                    Lstar = 1.2,
                    ContractionRatio = 6.0
                });

            var spike = Geometry_Aerospike.Create(0.55, 0.15);

            var primary = Geometry_Cooling.Primary(chamber, spike);
            var secondary = Geometry_Cooling.Secondary(chamber, spike);
            return primary + secondary;
        }
    }
}
