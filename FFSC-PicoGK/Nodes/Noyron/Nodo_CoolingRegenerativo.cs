using PipelineCore;
using PicoGK;
using FFSC_PicoGK.Agents;
using FFSC_PicoGK.Models;
using FFSC_PicoGK.Utils;

namespace FFSC_PicoGK.Nodes.Noyron;

public record CoolingRegenerativoInput(
    Voxels Chamber,
    Voxels PreBurner,
    Voxels PhysicsFields);

public class Nodo_CoolingRegenerativo : ITask<CoolingRegenerativoInput, Voxels>
{
    public string Id => "CoolingRegenerativo";
    public string Name => "Nodo Cooling Regenerativo";
    
    public Voxels Run(CoolingRegenerativoInput input)
    {
        Voxels result = new Voxels();
        
        // Agent 1: CoolingAgent - Volumetric internal cooling channels
        var coolingAgent = new CoolingAgent();
        Voxels coolingPrimary = coolingAgent.GeneratePrimary(input.Chamber);
        Voxels coolingSecondary = coolingAgent.GenerateSecondary(input.Chamber);
        result += coolingPrimary + coolingSecondary;
        
        // Agent 2: ThermalAgent (using PhysicsAgent proxy) - Thermal analysis
        var physicsAgent = new PhysicsAgent();
        Voxels thermal = physicsAgent.Execute(result);
        result += thermal;
        
        // Agent 3: FlowAgent - Flow analysis
        var flowAgent = new FlowAgent();
        Voxels flow = flowAgent.Execute(result);
        result += flow;
        
        // Validation
        var validationAgent = new ValidationAgent();
        return validationAgent.Execute(result);
    }
    
    public Voxels Execute(CoolingRegenerativoInput input) => Run(input);
}
