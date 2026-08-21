// EngineParams.cs
//
// Parametros fundamentales del motor FFSC.
// Todos en SI units a menos que se indique lo contrario.
//
// Basado en:
// - Raptor 3 (SpaceX) - 2.5 MN, 350 bar
// - Termoquimica UC3M
// - Diseno de motores de cohete PDFs
//
// Cita:
// "L* es la longitud caracteristica de la camara de combustion.
//  Determina la eficiencia de combustion y la estabilidad."

namespace FFSC_PicoGK.Models
{
    /// <summary>
    /// Especificacion de material estructural.
    /// </summary>
    public class MaterialSpec
    {
        public string Name { get; set; } = "Inconel_718";
        public double YieldStrengthPa { get; set; } = 1.03e9;
        public double Density { get; set; } = 8190.0;
        public double ThermalConductivity { get; set; } = 11.4;
        public double YoungsModulus { get; set; } = 2.0e11;
        public double PoissonRatio { get; set; } = 0.3;
        public double MaxServiceTemp_C { get; set; } = 700.0;
        public double MeltingPoint_C { get; set; } = 1336.0;
    }

    /// <summary>
    /// Parametros completos del motor FFSC estilo Raptor-3.
    /// </summary>
    public class EngineParams
    {
        // --- Performance ---
        public double Thrust { get; set; } = 2_500_000.0;         // N
        public double ChamberPressure_bar { get; set; } = 350.0;   // bar
        public double Pc => ChamberPressure_bar * 1e5;             // Pa

        public double ExpansionRatio { get; set; } = 45.0;
        public double MixtureRatio { get; set; } = 3.6;            // O/F

        // --- Performance derived ---
        public double Cstar { get; set; } = 2400.0;                // m/s caracteristico
        public double Isp { get; set; } = 380.0;                   // s
        public double MassFlowOxidizer { get; set; } = 320.0;      // kg/s LOX
        public double MassFlowFuel { get; set; } = 89.0;           // kg/s CH4
        public double TotalMassFlow => MassFlowOxidizer + MassFlowFuel;

        // --- Geometry ---
        public double ThroatRadius { get; set; } = 0.12;           // m
        public double ExitRadius { get; set; } = 0.80;             // m
        public double ChamberRadius { get; set; } = 0.35;          // m
        public double ChamberLength { get; set; } = 0.50;          // m
        public double Lstar { get; set; } = 1.2;                   // m longitud caracteristica
        public double At => Math.PI * ThroatRadius * ThroatRadius; // m^2 area garganta
        public double Ae => At * ExpansionRatio;                    // m^2 area salida
        public double ContractionRatio { get; set; } = 6.0;        // Ac/At

        // --- Thermochemistry ---
        public double TadInitialGuess { get; set; } = 3600.0;      // K adivinanza inicial
        public double ChamberTemp_K { get; set; } = 3600.0;        // K temperatura camara
        public double Gamma { get; set; } = 1.3;                   // ratio de capacidades
        public double MolecularWeight { get; set; } = 23.0;        // g/mol peso molecular

        // --- Discretization ---
        public int Nz { get; set; } = 300;                        // puntos axiales
        public int Nr { get; set; } = 100;                        // puntos radiales

        // --- Material ---
        public MaterialSpec Material { get; set; } = new MaterialSpec();

        // --- Turbopump ---
        public double TurbopumpRPM { get; set; } = 40000.0;
        public double TurbopumpShaftDiameter { get; set; } = 0.06;

        // --- Cooling ---
        public double CoolantMassFlow { get; set; } = 50.0;        // kg/s CH4 regenerativo
        public double CoolantInletTemp_C { get; set; } = 20.0;
        public double CoolantOutletTemp_C { get; set; } = 750.0;
        public double CoolantPressure_bar { get; set; } = 450.0;
        public double CoolingChannelWidth { get; set; } = 0.006;
        public double CoolingChannelHeight { get; set; } = 0.0015;
        public int CoolingChannelCount { get; set; } = 120;

        // --- Lattice ---
        public string LatticeType { get; set; } = "DualLayer";
        public double StressThresholdHigh { get; set; } = 0.6;
        public double StressThresholdLow { get; set; } = 0.3;
        public double SafetyFactor { get; set; } = 1.5;

        // --- Simulation ---
        public string EngineVersion { get; set; } = "v06";
        public double VoxelSize { get; set; } = 0.0005;
        public double FeatureAngle { get; set; } = 30.0;
    }
}
