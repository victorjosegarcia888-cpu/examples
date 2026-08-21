// EngineParamsLoader.cs
//
// Cargador de parametros del motor desde JSON.

using System.IO;
using System.Text.Json;
using FFSC_PicoGK.Models;

namespace FFSC_PicoGK.Utils
{
    /// <summary>
    /// Cargador de parametros del motor.
    /// </summary>
    public static class EngineParamsLoader
    {
        private static readonly JsonSerializerOptions Options = new()
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true
        };

        /// <summary>
        /// Carga parametros desde archivo JSON.
        /// </summary>
        public static EngineParams Load(string ruta)
        {
            if (!File.Exists(ruta))
                throw new FileNotFoundException($"Archivo no encontrado: {ruta}");

            string json = File.ReadAllText(ruta);
            var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var p = new EngineParams
            {
                Thrust = root.GetProperty("Thrust").GetDouble(),
                ChamberPressure_bar = root.GetProperty("ChamberPressure_bar").GetDouble(),
                ExpansionRatio = root.GetProperty("ExpansionRatio").GetDouble(),
                Lstar = root.GetProperty("Lstar").GetDouble(),
                ThroatRadius = root.GetProperty("ThroatRadius").GetDouble(),
                ExitRadius = root.GetProperty("ExitRadius").GetDouble(),
                ChamberRadius = root.GetProperty("ChamberRadius").GetDouble(),
                ChamberLength = root.GetProperty("ChamberLength").GetDouble(),
                MixtureRatio = root.GetProperty("MixtureRatio").GetDouble(),
                Cstar = root.GetProperty("Cstar").GetDouble(),
                Isp = root.GetProperty("Isp").GetDouble(),
                MassFlowOxidizer = root.GetProperty("MassFlowOxidizer").GetDouble(),
                MassFlowFuel = root.GetProperty("MassFlowFuel").GetDouble(),
                ContractionRatio = root.GetProperty("ContractionRatio").GetDouble(),
                TadInitialGuess = root.GetProperty("TadInitialGuess").GetDouble(),
                Nz = root.GetProperty("Nz").GetInt32(),
                TurbopumpRPM = root.GetProperty("TurbopumpRPM").GetDouble(),
                CoolantMassFlow = root.GetProperty("CoolantMassFlow").GetDouble(),
                CoolantInletTemp_C = root.GetProperty("CoolantInletTemp_C").GetDouble(),
                CoolantOutletTemp_C = root.GetProperty("CoolantOutletTemp_C").GetDouble(),
                CoolantPressure_bar = root.GetProperty("CoolantPressure_bar").GetDouble(),
                CoolingChannelWidth = root.GetProperty("CoolingChannelWidth").GetDouble(),
                CoolingChannelHeight = root.GetProperty("CoolingChannelHeight").GetDouble(),
                CoolingChannelCount = root.GetProperty("CoolingChannelCount").GetInt32(),
                SafetyFactor = root.GetProperty("SafetyFactor").GetDouble(),
                VoxelSize = root.GetProperty("VoxelSize").GetDouble()
            };

            if (root.TryGetProperty("Material", out var mat))
            {
                p.Material = new MaterialSpec
                {
                    Name = mat.GetProperty("Name").GetString() ?? string.Empty,
                    YieldStrengthPa = mat.GetProperty("YieldStrengthPa").GetDouble(),
                    Density = mat.GetProperty("Density").GetDouble(),
                    ThermalConductivity = mat.GetProperty("ThermalConductivity").GetDouble(),
                    YoungsModulus = mat.GetProperty("YoungsModulus").GetDouble()
                };
            }

            return p;
        }

        /// <summary>
        /// Guarda parametros a JSON.
        /// </summary>
        public static void Save(EngineParams p, string ruta)
        {
            string json = JsonSerializer.Serialize(p, Options);
            File.WriteAllText(ruta, json);
        }
    }
}
