// FFSC_v03.cs
//
// Version v03 MultiObjetivo del motor FFSC.
//
// Objetivo: demostrar arquitectura multi-objetivo
// con optimizacion de geometria y termica.
//
// Subsistemas:
// - Camara de combustion
// - Aerospike (sin tobera)
// - Manifold reforzado
// - Canales de refrigeracion primarios
// - Lattice dual-layer

using PicoGK;
using FFSC_PicoGK.EngineFFSC.Assembly;

namespace FFSC_PicoGK.EngineFFSC.Versions
{
    /// <summary>
    /// Version v03 MultiObjetivo.
    /// </summary>
    public static class FFSC_v03
    {
        public static Field3D Build()
        {
            return FFSC_Assembly_Modular.V03();
        }
    }
}
