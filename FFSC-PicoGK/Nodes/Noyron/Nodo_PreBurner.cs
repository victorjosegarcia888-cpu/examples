using PipelineCore;
using PicoGK;
using FFSC_PicoGK.Agents;
using FFSC_PicoGK.Models;
using FFSC_PicoGK.Utils;

namespace FFSC_PicoGK.Nodes.Noyron;

public class Nodo_PreBurner : ITask<Unit, Voxels>
{
    public string Id => "PreBurner";
    public string Name => "Nodo PreBurner";
    
    public Voxels Run(Unit input)
    {
        Voxels result = new Voxels();
        
        // Agent 1: GeometryAgent - Preburner geometry
        var geometryAgent = new GeometryAgent();
        Voxels preburner = geometryAgent.CreateGeometry(GeometryType.PreBurner);
        result += preburner;
        
        // Agent 2: CoolingAgent - Cooling channels
        var coolingAgent = new CoolingAgent();
        Voxels cooling = coolingAgent.GeneratePrimary(preburner);
        result += cooling;
        
        // Agent 3: LatticeAgent - Adaptive lattice
        var latticeAgent = new LatticeAgent();
        Voxels lattice = latticeAgent.GenerateDualLayer(preburner, 0.6, 0.3, 0.015, 0.008);
        result += lattice;
        
        // Agent 4: PhysicsAgent - Physics fields
        var physicsAgent = new PhysicsAgent();
        Voxels physics = physicsAgent.Execute(result);
        result += physics;
        
        // Validation
        var validationAgent = new ValidationAgent();
        return validationAgent.Execute(result);
    }
    
    public Voxels Execute(Unit input) => Run(input);
}
