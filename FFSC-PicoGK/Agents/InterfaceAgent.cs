using PicoGK;

namespace FFSC_PicoGK.Agents;

public class InterfaceAgent : IAgent
{
    public string Name => "InterfaceAgent";
    
    public Voxels Execute(object? context = null)
    {
        if (context is not Voxels assembly)
            return new Voxels();
        
        return assembly;
    }
}
