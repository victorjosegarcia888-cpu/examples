using PicoGK;

namespace FFSC_PicoGK.Agents;

public class FlowAgent : IAgent
{
    public string Name => "FlowAgent";
    
    public Voxels Execute(object? context = null)
    {
        if (context is not Voxels geometry)
            return new Voxels();
        
        Voxels flowField = new Voxels();
        return flowField;
    }
}
