// FFSC_v05.cs
//
// Version v05 Adaptive del motor FFSC.
//
// Objetivo: demostrar adaptabilidad con lattice adaptativo
// y campos fisicos dinamicos.
//
// Subsistemas:
// - Camara + aerospike
// - Falda modular
// - Manifold completo FFSC
// - Preburner
// - Turbobomba
// - Turbina
// - Inyectores
// - Red de tuberias
// - Canales regenerativos (primario + secundario + manifold)
// - Lattice dual-layer + quasicrystal
// - Structural + supports

using PicoGK;
using FFSC_PicoGK.EngineFFSC.Assembly;

namespace FFSC_PicoGK.EngineFFSC.Versions
{
    /// <summary>
    /// Version v05 Adaptive.
    /// </summary>
    public static class FFSC_v05
    {
        public static Field3D Build()
        {
            return FFSC_Assembly_Modular.V05();
        }
    }
}
