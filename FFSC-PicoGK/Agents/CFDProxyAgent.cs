using PicoGK;

namespace FFSC_PicoGK.Agents;

public class CFDProxyAgent : IAgent
{
    public string Name => "CFDProxyAgent";
    
    public Voxels Execute(object? context = null)
    {
        if (context is not Voxels geometry)
            return new Voxels();
        
        return new Voxels();
    }
}
