using PipelineCore;
using PicoGK;
using FFSC_PicoGK.Agents;
using FFSC_PicoGK.Models;
using FFSC_PicoGK.Utils;

namespace FFSC_PicoGK.Nodes.Noyron;

public class Nodo_Turbobomba : ITask<Unit, Voxels>
{
    public string Id => "Turbobomba";
    public string Name => "Nodo Turbobomba";
    
    public Voxels Run(Unit input)
    {
        Voxels result = new Voxels();
        
        // Agent 1: GeometryAgent - Turbopump base geometry
        var geometryAgent = new GeometryAgent();
        Voxels turbopump = geometryAgent.CreateGeometry(GeometryType.Turbopump);
        result += turbopump;
        
        // Agent 2: ImpellerAgent - Impeller design (r1, r2, h, U2, Cu2, omega)
        var impellerAgent = new ImpellerAgent();
        Voxels impeller = impellerAgent.Execute(turbopump);
        result += impeller;
        
        // Agent 3: ShaftAgent - Shaft design
        var shaftAgent = new ShaftAgent();
        Voxels shaft = shaftAgent.Execute(turbopump);
        result += shaft;
        
        // Agent 4: CoolingAgent - Cooling channels
        var coolingAgent = new CoolingAgent();
        Voxels cooling = coolingAgent.GeneratePrimary(turbopump);
        result += cooling;
        
        // Agent 5: PhysicsAgent - Physics fields
        var physicsAgent = new PhysicsAgent();
        Voxels physics = physicsAgent.ComputeStress(turbopump);
        result += physics;
        
        // Validation
        var validationAgent = new ValidationAgent();
        return validationAgent.Execute(result);
    }
    
    public Voxels Execute(Unit input) => Run(input);
}
