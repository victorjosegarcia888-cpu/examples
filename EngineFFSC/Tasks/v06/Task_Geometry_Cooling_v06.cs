// Task_Geometry_Cooling_v06.cs
//
// Task para generar canales de refrigeracion v06.

using PicoGK;
using EngineFFSC.Geometry;
using FFSC_PicoGK.Models;

namespace EngineFFSC.Tasks
{
    public static class Task_Geometry_Cooling_v06
    {
        public static Voxels Task()
        {
            var chamber = Geometry_Chamber_v06.Create(new EngineParams
            {
                ChamberRadius = 0.35,
                ChamberLength = 0.50,
                ThroatRadius = 0.12,
                Lstar = 1.2,
                ContractionRatio = 6.0
            });

            var spike = Geometry_Aerospike_v06.Create(new EngineParams
            {
                ThroatRadius = 0.12,
                ExitRadius = 0.80,
                ChamberLength = 0.50
            });

            EngineParams p = new EngineParams
            {
                ChamberLength = 0.50,
                ChamberRadius = 0.35
            };

            Voxels primary = Geometry_Cooling_v06.Primary(chamber, spike, p, channelWidth: 0.006, channelHeight: 0.0015);
            Voxels secondary = Geometry_Cooling_v06.Secondary(chamber, spike, p, filmThickness: 0.002);

            return primary + secondary;
        }
    }
}
