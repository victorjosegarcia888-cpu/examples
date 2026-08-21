// Task_Geometry_Chamber.cs
//
// Task para generar geometria de camara de combustion.
//
// Patron: toma EngineParams, devuelve Field3D.
// Compatible con Library.Go.

using PicoGK;
using FFSC_PicoGK.Models;
using FFSC_PicoGK.Geometry.Chamber;

namespace FFSC_PicoGK.Tasks.Geometry
{
    /// <summary>
    /// Task de generacion de geometria de camara.
    /// </summary>
    public static class Task_Geometry_Chamber
    {
        /// <summary>
        /// Genera la camara de combustion.
        /// </summary>
        public static Field3D Task()
        {
            EngineParams p = new EngineParams
            {
                ChamberRadius = 0.35,
                ChamberLength = 0.50,
                ThroatRadius = 0.12,
                Lstar = 1.2,
                ContractionRatio = 6.0
            };

            return Geometry_Chamber.Create(p);
        }

        /// <summary>
        /// Genera la camara con parametros especificados.
        /// </summary>
        public static Field3D Task(EngineParams p)
        {
            return Geometry_Chamber.Create(p);
        }
    }
}
