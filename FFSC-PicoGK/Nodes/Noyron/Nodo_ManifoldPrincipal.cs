using PipelineCore;
using PicoGK;
using FFSC_PicoGK.Agents;
using FFSC_PicoGK.Models;
using FFSC_PicoGK.Utils;

namespace FFSC_PicoGK.Nodes.Noyron;

public class Nodo_ManifoldPrincipal : ITask<Unit, Voxels>
{
    public string Id => "ManifoldPrincipal";
    public string Name => "Nodo Manifold Principal";
    
    public Voxels Run(Unit input)
    {
        Voxels result = new Voxels();
        
        // Agent 1: GeometryAgent - Manifold geometry
        var geometryAgent = new GeometryAgent();
        Voxels manifold = geometryAgent.CreateGeometry(GeometryType.ManifoldFFSC);
        result += manifold;
        
        // Agent 2: FlowAgent - Flow channels
        var flowAgent = new FlowAgent();
        Voxels flow = flowAgent.Execute(manifold);
        result += flow;
        
        // Agent 3: ValidationAgent - Validation
        var validationAgent = new ValidationAgent();
        return validationAgent.Execute(result);
    }
    
    public Voxels Execute(Unit input) => Run(input);
}
