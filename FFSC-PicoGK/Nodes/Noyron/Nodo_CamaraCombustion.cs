using PipelineCore;
using PicoGK;
using FFSC_PicoGK.Agents;
using FFSC_PicoGK.Models;
using FFSC_PicoGK.Utils;

namespace FFSC_PicoGK.Nodes.Noyron;

public class Nodo_CamaraCombustion : ITask<Unit, Voxels>
{
    public string Id => "CamaraCombustion";
    public string Name => "Nodo Cámara de Combustión";
    
    public Voxels Run(Unit input)
    {
        Voxels result = new Voxels();
        
        // Agent 1: GeometryAgent - Generates chamber geometry
        var geometryAgent = new GeometryAgent();
        Voxels chamber = geometryAgent.CreateGeometry(GeometryType.Chamber);
        result += chamber;
        
        // Agent 2: CoolingAgent - Generates cooling channels
        var coolingAgent = new CoolingAgent();
        Voxels cooling = coolingAgent.GeneratePrimary(chamber);
        result += cooling;
        
        // Agent 3: LatticeAgent - Generates adaptive lattice
        var latticeAgent = new LatticeAgent();
        Voxels lattice = latticeAgent.GenerateDualLayer(chamber, 0.6, 0.3, 0.015, 0.008);
        result += lattice;
        
        // Agent 4: PhysicsAgent - Generates physics fields
        var physicsAgent = new PhysicsAgent();
        Voxels physics = physicsAgent.Execute(result);
        result += physics;
        
        // Agent 5: ValidationAgent - Validates assembly
        var validationAgent = new ValidationAgent();
        Voxels validated = validationAgent.Execute(result);
        
        return validated;
    }
    
    public Voxels Execute(Unit input) => Run(input);
}
