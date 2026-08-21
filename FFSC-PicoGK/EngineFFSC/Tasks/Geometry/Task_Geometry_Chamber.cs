// Task_Geometry_Chamber.cs
//
// Task para generar geometria de camara de combustion.

using PicoGK;
using FFSC_PicoGK.Geometry.Chamber;

namespace FFSC_PicoGK.Tasks.Geometry
{
    public static class Task_Geometry_Chamber
    {
        public static Voxels Task()
        {
            return Geometry_Chamber.Create(
                new FFSC_PicoGK.Models.EngineParams
                {
                    ChamberRadius = 0.35,
                    ChamberLength = 0.50,
                    ThroatRadius = 0.12,
                    Lstar = 1.2,
                    ContractionRatio = 6.0
                });
        }
    }
}
