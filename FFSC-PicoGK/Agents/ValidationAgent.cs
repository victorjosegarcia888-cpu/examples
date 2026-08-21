using PicoGK;

namespace FFSC_PicoGK.Agents;

public class ValidationAgent : IAgent
{
    public string Name => "ValidationAgent";
    
    public Voxels Execute(object? context = null)
    {
        if (context is not Voxels geometry)
            return new Voxels();
        
        return geometry;
    }
}
