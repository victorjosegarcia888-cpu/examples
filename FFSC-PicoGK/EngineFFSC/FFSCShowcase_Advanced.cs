// FFSCShowcase_Advanced.cs
//
// Punto de entrada avanzado para visualizar el motor FFSC.
// La tarea retorna void y agrega el motor directamente al viewer.

using PicoGK;
using FFSC_PicoGK.EngineFFSC.Assembly;
using FFSC_PicoGK.EngineFFSC.Versions;
using FFSC_PicoGK.Pipeline;
using FFSC_PicoGK.Models;

namespace FFSC_PicoGK.EngineFFSC
{
    public static class FFSCShowcase_Advanced
    {
        public static void Task()
        {
            Voxels engine = FFSC_v06.Build();
            Library.oViewer().Add(engine);
        }

        public static void Task(string version)
        {
            Voxels engine = version.ToLower() switch
            {
                "v03" => FFSC_v03.Build(),
                "v04" => FFSC_v04.Build(),
                "v05" => FFSC_v05.Build(),
                "v06" or _ => FFSC_v06.Build()
            };
            Library.oViewer().Add(engine);
        }

        public static void Task_Subsystem(string subsystem)
        {
            var config = new FFSC_Assembly_Config();
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

            Voxels engine = FFSC_Assembly_Modular.Assemble(config);
            Library.oViewer().Add(engine);
        }

        public static void Task_Pipeline()
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

            Voxels engine = FFSC_Pipeline_Advanced.Execute(p);
            Library.oViewer().Add(engine);
        }
    }
}
