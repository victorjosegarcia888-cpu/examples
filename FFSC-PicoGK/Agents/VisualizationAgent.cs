using PicoGK;

namespace FFSC_PicoGK.Agents;

public class VisualizationAgent : IAgent
{
    public string Name => "VisualizationAgent";
    
    public Voxels Execute(object? context = null)
    {
        if (context is not Voxels engine)
            return new Voxels();
        
        return engine;
    }
}
