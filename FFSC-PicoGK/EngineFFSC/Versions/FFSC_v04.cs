// FFSC_v04.cs
//
// Version v04 Redundante del motor FFSC.
//
// Objetivo: demostrar redundancia en subsistemas criticos.
//
// Subsistemas:
// - Camara + convergente
// - Aerospike
// - Manifold con valvulas redundantes
// - Canales primarios + secundarios + manifold
// - Lattice dual-layer
// - Inyectores
// - Turboboma

using PicoGK;
using FFSC_PicoGK.EngineFFSC.Assembly;

namespace FFSC_PicoGK.EngineFFSC.Versions
{
    /// <summary>
    /// Version v04 Redundante.
    /// </summary>
    public static class FFSC_v04
    {
        public static Field3D Build()
        {
            return FFSC_Assembly_Modular.V04();
        }
    }
}
