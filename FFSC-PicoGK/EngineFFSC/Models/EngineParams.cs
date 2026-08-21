// EngineParams.cs
//
// Parametros fundamentales del motor FFSC.
// Todos en SI units a menos que se indique lo contrario.

namespace FFSC_PicoGK.Models
{
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

    public class EngineParams
    {
        public double Thrust { get; set; } = 2_500_000.0;
        public double ChamberPressure_bar { get; set; } = 350.0;
        public double Pc => ChamberPressure_bar * 1e5;
        public double ExpansionRatio { get; set; } = 45.0;
        public double MixtureRatio { get; set; } = 3.6;
        public double Cstar { get; set; } = 2400.0;
        public double Isp { get; set; } = 380.0;
        public double MassFlowOxidizer { get; set; } = 320.0;
        public double MassFlowFuel { get; set; } = 89.0;
        public double TotalMassFlow => MassFlowOxidizer + MassFlowFuel;
        public double ThroatRadius { get; set; } = 0.12;
        public double ExitRadius { get; set; } = 0.80;
        public double ChamberRadius { get; set; } = 0.35;
        public double ChamberLength { get; set; } = 0.50;
        public double Lstar { get; set; } = 1.2;
        public double At => Math.PI * ThroatRadius * ThroatRadius;
        public double Ae => At * ExpansionRatio;
        public double ContractionRatio { get; set; } = 6.0;
        public double TadInitialGuess { get; set; } = 3600.0;
        public double ChamberTemp_K { get; set; } = 3600.0;
        public double Gamma { get; set; } = 1.3;
        public double MolecularWeight { get; set; } = 23.0;
        public int Nz { get; set; } = 300;
        public int Nr { get; set; } = 100;
        public MaterialSpec Material { get; set; } = new MaterialSpec();
        public double TurbopumpRPM { get; set; } = 40000.0;
        public double TurbopumpShaftDiameter { get; set; } = 0.06;
        public double CoolantMassFlow { get; set; } = 50.0;
        public double CoolantInletTemp_C { get; set; } = 20.0;
        public double CoolantOutletTemp_C { get; set; } = 750.0;
        public double CoolantPressure_bar { get; set; } = 450.0;
        public double CoolingChannelWidth { get; set; } = 0.006;
        public double CoolingChannelHeight { get; set; } = 0.0015;
        public int CoolingChannelCount { get; set; } = 120;
        public string LatticeType { get; set; } = "DualLayer";
        public double StressThresholdHigh { get; set; } = 0.6;
        public double StressThresholdLow { get; set; } = 0.3;
        public double SafetyFactor { get; set; } = 1.5;
        public string EngineVersion { get; set; } = "v06";
        public double VoxelSize { get; set; } = 0.0005;
        public double FeatureAngle { get; set; } = 30.0;
    }
}
