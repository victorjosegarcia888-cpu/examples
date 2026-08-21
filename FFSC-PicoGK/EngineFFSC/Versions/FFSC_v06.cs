// FFSC_v06.cs
//
// Version v06 del motor FFSC - version completa avanzada.
//
// Objetivo: motor completo con todos los subsistemas
// y campos fisicos integrados.
//
// Subsistemas:
// - Camara + tobera Rao + aerospike
// - Manifolds LOX, CH4, FFSC
// - Inyectores coaxiales
// - Turbobomba con perfil NACA
// - Canales regenerativos (primario + secundario)
// - Tuberias de alta presion
// - Thrust frame + gimbal + skirt
// - Soportes
// - Lattice dual-layer + quasicrystal
// - Campo de tensiones dinamico
// - CFD dinamico
// - Campo termico dinamico
// - Mapa de enfriamiento

using PicoGK;
using FFSC_PicoGK.EngineFFSC.Assembly;

namespace FFSC_PicoGK.EngineFFSC.Versions
{
    /// <summary>
    /// Version v06 - Completa avanzada.
    /// </summary>
    public static class FFSC_v06
    {
        public static Field3D Build()
        {
            return FFSC_Assembly_Modular.V06();
        }
    }
}
