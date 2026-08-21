using PicoGK;
using FFSC_PicoGK.Geometry.Cooling;

namespace FFSC_PicoGK.Agents;

public class CoolingAgent : IAgent
{
    public string Name => "CoolingAgent";
    
    public Voxels Execute(object? context = null)
    {
        if (context is not Voxels geometry)
            return new Voxels();
        
        return Geometry_Cooling.Primary(geometry, null);
    }
    
    public Voxels GeneratePrimary(Voxels chamber, Voxels? spike = null)
    {
        return Geometry_Cooling.Primary(chamber, spike);
    }
    
    public Voxels GenerateSecondary(Voxels chamber, Voxels? spike = null)
    {
        var s = spike ?? new Voxels();
        return Geometry_Cooling.Secondary(chamber, s);
    }
    
    public Voxels GenerateManifoldCooling(Voxels manifold)
    {
        return Geometry_Cooling.Manifold(manifold);
    }
}
