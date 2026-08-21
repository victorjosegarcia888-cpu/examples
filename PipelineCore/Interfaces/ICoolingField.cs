// ICoolingField.cs
//
// Interface for regenerative cooling analysis tasks.

using PicoGK;

namespace PipelineCore;

public interface ICoolingField
{
    string Id { get; }
    string Name { get; }
    object Compute(object engineParams, object thermo);
    double[] GetTemperatureProfile();
    double[] GetHeatFluxProfile();
    double GetCoolantOutletTemp();
    bool IsEfficient { get; }
}
