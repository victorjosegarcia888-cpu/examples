using PicoGK;

namespace FFSC_PicoGK.Agents;

public class ImpellerAgent : IAgent
{
    public string Name => "ImpellerAgent";
    
    public Voxels Execute(object? context = null)
    {
        if (context is not Voxels baseGeometry)
            return new Voxels();
        
        Voxels impeller = new Voxels();
        return impeller;
    }
}
