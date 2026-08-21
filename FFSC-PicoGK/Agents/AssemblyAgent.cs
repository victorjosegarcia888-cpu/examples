using PicoGK;

namespace FFSC_PicoGK.Agents;

public class AssemblyAgent : IAgent
{
    public string Name => "AssemblyAgent";
    
    public Voxels Execute(object? context = null)
    {
        if (context is not Voxels[] parts)
            return new Voxels();
        
        Voxels result = new Voxels();
        foreach (var part in parts)
            result += part;
        
        return result;
    }
}
