using PicoGK;
using FFSC_PicoGK.Utils;
using FFSC_PicoGK.Geometry.Chamber;

namespace FFSC_PicoGK.Agents;

public class LatticeAgent : IAgent
{
    public string Name => "LatticeAgent";
    
    public Voxels Execute(object? context = null)
    {
        if (context is not Voxels stressField)
            return new Voxels();
        
        var p = EngineParamsLoader.Load("config/engine_params.json");
        return Lattice_DualLayer.Generate(stressField, p.StressThresholdHigh, p.StressThresholdLow, 0.015, 0.008);
    }
    
    public Voxels GenerateDualLayer(Voxels stressField, double highThreshold, double lowThreshold, double highRadius, double lowRadius)
    {
        return Lattice_DualLayer.Generate(stressField, highThreshold, lowThreshold, highRadius, lowRadius);
    }
    
    public Voxels GenerateQuasicrystal(Voxels stressField, double scale, double intensity)
    {
        return Lattice_Quasicrystal.Generate(stressField, scale, intensity);
    }
}
