using PicoGK;

namespace FFSC_PicoGK.Agents;

public class ShaftAgent : IAgent
{
    public string Name => "ShaftAgent";
    
    public Voxels Execute(object? context = null)
    {
        if (context is not Voxels baseGeometry)
            return new Voxels();
        
        Voxels shaft = new Voxels();
        return shaft;
    }
}
