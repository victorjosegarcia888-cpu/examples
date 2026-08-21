// FFSCShowcase_Advanced.cs
//
// Punto de entrada avanzado para visualizar el motor FFSC.
//
// Permite visualizar:
// - Todos los subsistemas independientemente
// - Motor completo (version seleccionable)
// - Vista explotada
// - Campos fisicos

using PicoGK;
using FFSC_PicoGK.EngineFFSC.Assembly;
using FFSC_PicoGK.EngineFFSC.Versions;
using FFSC_PicoGK.Pipeline;

namespace FFSC_PicoGK.EngineFFSC
{
    /// <summary>
    /// Showcase avanzado del motor FFSC.
    /// </summary>
    public static class FFSCShowcase_Advanced
    {
        /// <summary>
        /// Visualiza el motor completo version v06.
        /// </summary>
        public static Field3D Task()
        {
            return FFSC_v06.Build();
        }

        /// <summary>
        /// Visualiza una version especifica.
        /// </summary>
        public static Field3D Task(string version)
        {
            return version.ToLower() switch
            {
                "v03" => FFSC_v03.Build(),
                "v04" => FFSC_v04.Build(),
                "v05" => FFSC_v05.Build(),
                "v06" or _ => FFSC_v06.Build()
            };
        }

        /// <summary>
        /// Visualiza subsistema especifico.
        /// </summary>
        public static Field3D Task_Subsystem(string subsystem)
        {
            var config = new FFSC_Assembly_Config();

            // Reset all
            config.IncludeChamber = false;
            config.IncludeNozzle = false;
            config.IncludeAerospike = false;
            config.IncludeManifold = false;
            config.IncludeInjectors = false;
            config.IncludeTurbopump = false;
            config.IncludeCooling = false;
            config.IncludePipes = false;
            config.IncludeStructural = false;
            config.IncludeSupports = false;

            switch (subsystem.ToLower())
            {
                case "chamber": config.IncludeChamber = true; break;
                case "nozzle": config.IncludeNozzle = true; break;
                case "aerospike": config.IncludeAerospike = true; break;
                case "manifold": config.IncludeManifold = true; break;
                case "injectors": config.IncludeInjectors = true; break;
                case "turbopump": config.IncludeTurbopump = true; break;
                case "cooling": config.IncludeCooling = true; break;
                case "pipes": config.IncludePipes = true; break;
                case "structural": config.IncludeStructural = true; break;
                case "supports": config.IncludeSupports = true; break;
            }

            return FFSC_Assembly_Modular.Assemble(config);
        }

        /// <summary>
        /// Visualiza el motor via pipeline completo.
        /// </summary>
        public static Field3D Task_Pipeline()
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

            return FFSC_Pipeline_Advanced.Execute(p);
        }
    }
}
