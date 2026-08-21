// FFSC_Assembly_Modular.cs
//
// Ensamblado modular del motor FFSC.
//
// Permite:
// - Activar/desactivar subsistemas
// - Cambiar entre versiones (v03, v04, v05, v06)
// - Combinar subsistemas seleccionados
//
// Cita PDF:
// "El diseno modular permite iterar rapidamente entre
//  configuraciones sin reescribir el codigo base."

using PicoGK;
using FFSC_PicoGK.Models;
using FFSC_PicoGK.Geometry.Chamber;
using FFSC_PicoGK.Geometry.Nozzle;
using FFSC_PicoGK.Geometry.Aerospike;
using FFSC_PicoGK.Geometry.Manifolds;
using FFSC_PicoGK.Geometry.Injectors;
using FFSC_PicoGK.Geometry.Turbopump;
using FFSC_PicoGK.Geometry.Cooling;
using FFSC_PicoGK.Geometry.Pipes;
using FFSC_PicoGK.Geometry.Structural;
using FFSC_PicoGK.Geometry.Supports;
using FFSC_PicoGK.Physics.Stress;
using FFSC_PicoGK.Physics.CFD;

namespace FFSC_PicoGK.EngineFFSC.Assembly
{
    /// <summary>
    /// Configuracion modular del ensamblado FFSC.
    /// </summary>
    public class FFSC_Assembly_Config
    {
        public bool IncludeChamber { get; set; } = true;
        public bool IncludeNozzle { get; set; } = true;
        public bool IncludeAerospike { get; set; } = false;
        public bool IncludeManifold { get; set; } = true;
        public bool IncludeInjectors { get; set; } = true;
        public bool IncludeTurbopump { get; set; } = true;
        public bool IncludeCooling { get; set; } = true;
        public bool IncludePipes { get; set; } = true;
        public bool IncludeStructural { get; set; } = true;
        public bool IncludeSupports { get; set; } = true;
        public bool IncludeStress { get; set; } = false;
        public bool IncludeCFD { get; set; } = false;
        public string Version { get; set; } = "v06";
    }

    /// <summary>
    /// Ensamblador modular del motor FFSC.
    /// </summary>
    public static class FFSC_Assembly_Modular
    {
        /// <summary>
        /// Ensambla el motor segun la configuracion dada.
        /// </summary>
        public static Field3D Assemble(FFSC_Assembly_Config config)
        {
            EngineParams p = new EngineParams
            {
                Thrust = 2_500_000.0,
                ChamberPressure_bar = 350.0,
                ExpansionRatio = 45.0,
                Lstar = 1.2,
                ThroatRadius = 0.12,
                ExitRadius = 0.80,
                ChamberRadius = 0.35,
                ChamberLength = 0.50,
                ContractionRatio = 6.0
            };

            p.Material = new MaterialSpec
            {
                Name = "Inconel_718",
                YieldStrengthPa = 1.03e9
            };

            Field3D result = Field3D.Empty;

            if (config.IncludeChamber)
            {
                result = Field3D.Combine(result, Geometry_Chamber.Create(p));
            }

            if (config.IncludeNozzle)
            {
                result = Field3D.Combine(result, Geometry_Nozzle.Create(p));
            }

            if (config.IncludeAerospike)
            {
                result = Field3D.Combine(result, Geometry_Aerospike.Create(p));
            }

            if (config.IncludeManifold)
            {
                result = Field3D.Combine(result, Geometry_Manifold_FFSC.Create());
            }

            if (config.IncludeInjectors)
            {
                result = Field3D.Combine(result, Geometry_Injectors.Create());
            }

            if (config.IncludeTurbopump)
            {
                result = Field3D.Combine(result, Geometry_Turbopump.Create());
            }

            if (config.IncludeCooling)
            {
                var chamber = Geometry_Chamber.Create(p);
                var spike = Geometry_Aerospike.Create(p);
                result = Field3D.Combine(result, Geometry_Cooling.Primary(chamber, spike));
                result = Field3D.Combine(result, Geometry_Cooling.Secondary(chamber, spike));
            }

            if (config.IncludePipes)
            {
                result = Field3D.Combine(result, Geometry_Pipes.Create());
            }

            if (config.IncludeStructural)
            {
                result = Field3D.Combine(result, Geometry_Structural.Create());
            }

            if (config.IncludeSupports)
            {
                result = Field3D.Combine(result, Geometry_Supports.Create());
            }

            if (config.IncludeStress)
            {
                var chamber = Geometry_Chamber.Create(p);
                var spike = Geometry_Aerospike.Create(p);
                var manifold = Geometry_Manifold_FFSC.Create();
                result = Field3D.Combine(result, StressField.Dynamic(chamber, spike, manifold));
            }

            if (config.IncludeCFD)
            {
                var camara = Geometry_Chamber.Create(p);
                var nozzle = Geometry_Nozzle.Create(p);
                var manifold = Geometry_Manifold_FFSC.Create();
                result = Field3D.Combine(result, CFDTask.Dynamic(Field3D.Combine(camara, nozzle, manifold)));
            }

            return result;
        }

        /// <summary>
        /// Ensambla version v03 (multi-objetivo).
        /// </summary>
        public static Field3D V03()
        {
            var config = new FFSC_Assembly_Config
            {
                Version = "v03",
                IncludeChamber = true,
                IncludeNozzle = false,
                IncludeAerospike = true,
                IncludeManifold = true,
                IncludeInjectors = false,
                IncludeTurbopump = false,
                IncludeCooling = true,
                IncludePipes = false,
                IncludeStructural = false,
                IncludeSupports = false
            };
            return Assemble(config);
        }

        /// <summary>
        /// Ensambla version v04 (redundante).
        /// </summary>
        public static Field3D V04()
        {
            var config = new FFSC_Assembly_Config
            {
                Version = "v04",
                IncludeChamber = true,
                IncludeNozzle = false,
                IncludeAerospike = true,
                IncludeManifold = true,
                IncludeInjectors = true,
                IncludeTurbopump = true,
                IncludeCooling = true,
                IncludePipes = true,
                IncludeStructural = false,
                IncludeSupports = false
            };
            return Assemble(config);
        }

        /// <summary>
        /// Ensambla version v05 (adaptivo).
        /// </summary>
        public static Field3D V05()
        {
            var config = new FFSC_Assembly_Config
            {
                Version = "v05",
                IncludeChamber = true,
                IncludeNozzle = false,
                IncludeAerospike = true,
                IncludeManifold = true,
                IncludeInjectors = true,
                IncludeTurbopump = true,
                IncludeCooling = true,
                IncludePipes = true,
                IncludeStructural = true,
                IncludeSupports = true
            };
            return Assemble(config);
        }

        /// <summary>
        /// Ensambla version v06 (completa).
        /// </summary>
        public static Field3D V06()
        {
            var config = new FFSC_Assembly_Config
            {
                Version = "v06",
                IncludeChamber = true,
                IncludeNozzle = true,
                IncludeAerospike = true,
                IncludeManifold = true,
                IncludeInjectors = true,
                IncludeTurbopump = true,
                IncludeCooling = true,
                IncludePipes = true,
                IncludeStructural = true,
                IncludeSupports = true,
                IncludeStress = true,
                IncludeCFD = true
            };
            return Assemble(config);
        }
    }
}
