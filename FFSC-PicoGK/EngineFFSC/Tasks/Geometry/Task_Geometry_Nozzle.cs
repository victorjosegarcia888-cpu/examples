// Task_Geometry_Nozzle.cs
//
// Task para generar geometria de tobera Rao-optimizada.

using PicoGK;
using FFSC_PicoGK.Models;
using FFSC_PicoGK.Geometry.Nozzle;

namespace FFSC_PicoGK.Tasks.Geometry
{
    /// <summary>
    /// Task de generacion de geometria de tobera.
    /// </summary>
    public static class Task_Geometry_Nozzle
    {
        /// <summary>
        /// Genera la tobera Rao-optimizada.
        /// </summary>
        public static Field3D Task()
        {
            EngineParams p = new EngineParams
            {
                ThroatRadius = 0.12,
                ExitRadius = 0.80,
                ExpansionRatio = 45.0,
                Lstar = 1.2
            };

            return Geometry_Nozzle.Create(p);
        }

        /// <summary>
        /// Genera la tobera con parametros especificados.
        /// </summary>
        public static Field3D Task(EngineParams p)
        {
            return Geometry_Nozzle.Create(p);
        }
    }
}
