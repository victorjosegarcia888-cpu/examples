// IEngineSpec.cs
//
// Interface for engine specification and parameters.

using PicoGK;

namespace PipelineCore;

public interface IEngineSpec
{
    double Thrust { get; }
    double ChamberPressure_bar { get; }
    double Pc { get; }
    double ExpansionRatio { get; }
    double MixtureRatio { get; }
    double Cstar { get; }
    double Isp { get; }
    double MassFlowOxidizer { get; }
    double MassFlowFuel { get; }
    double TotalMassFlow { get; }
    double ThroatRadius { get; }
    double ExitRadius { get; }
    double ChamberRadius { get; }
    double ChamberLength { get; }
    double Lstar { get; }
    double At { get; }
    double Ae { get; }
    double ContractionRatio { get; }
    double TadInitialGuess { get; }
    double ChamberTemp_K { get; }
    double Gamma { get; }
    double MolecularWeight { get; }
    int Nz { get; }
    int Nr { get; }
    IMaterialSpec Material { get; }
    double TurbopumpRPM { get; }
    double TurbopumpShaftDiameter { get; }
    double CoolantMassFlow { get; }
    double CoolantInletTemp_C { get; }
    double CoolantOutletTemp_C { get; }
    double CoolantPressure_bar { get; }
    double CoolingChannelWidth { get; }
    double CoolingChannelHeight { get; }
    int CoolingChannelCount { get; }
    string LatticeType { get; }
    double StressThresholdHigh { get; }
    double StressThresholdLow { get; }
    double SafetyFactor { get; }
    string EngineVersion { get; }
    double VoxelSize { get; }
    double FeatureAngle { get; }
}

public interface IMaterialSpec
{
    string Name { get; }
    double YieldStrengthPa { get; }
    double Density { get; }
    double ThermalConductivity { get; }
    double YoungsModulus { get; }
    double PoissonRatio { get; }
    double MaxServiceTemp_C { get; }
    double MeltingPoint_C { get; }
}
