using PicoGK;

namespace FFSC_PicoGK.Agents;

public class ThermalGradientAgent : IAgent
{
    public string Name => "ThermalGradientAgent";
    
    public Voxels Execute(object? context = null)
    {
        if (context is not Voxels geometry)
            return new Voxels();
        
        Voxels thermalGradient = new Voxels();
        return thermalGradient;
    }
}
