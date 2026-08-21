using PipelineCore;
using PicoGK;
using FFSC_PicoGK.Agents;
using FractalKernel;

namespace FFSC_PicoGK.Nodes.Noyron;

public record OrganicManifoldInput(
    Voxels Manifold,
    Voxels PhysicsFields);

public class Nodo_OrganicManifold : ITask<OrganicManifoldInput, Voxels>
{
    public string Id => "OrganicManifold";
    public string Name => "Nodo Organic Manifold";
    
    public Voxels Run(OrganicManifoldInput input)
    {
        Voxels result = input.Manifold;
        
        var geometryAgent = new GeometryAgent();
        Voxels geometry = geometryAgent.CreateGeometry(GeometryType.ManifoldFFSC);
        result += geometry;
        
        var organic = new FractalKernel.OrganicPerturbationAgent();
        Voxels organicVoxels = organic.Process(result);
        result += organicVoxels;
        
        var multiScale = new FractalKernel.MultiScaleSDFAgent();
        Voxels multi = multiScale.Process(result);
        result += multi;
        
        var validationAgent = new ValidationAgent();
        return validationAgent.Execute(result);
    }
    
    public Voxels Execute(OrganicManifoldInput input) => Run(input);
}
