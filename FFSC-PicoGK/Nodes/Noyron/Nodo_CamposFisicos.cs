using PipelineCore;
using PicoGK;
using FFSC_PicoGK.Agents;
using FFSC_PicoGK.Models;
using FFSC_PicoGK.Utils;

namespace FFSC_PicoGK.Nodes.Noyron;

public record CamposFisicosInput(
    Voxels Chamber,
    Voxels PreBurner,
    Voxels Manifold,
    Voxels Turbopump);

public class Nodo_CamposFisicos : ITask<CamposFisicosInput, Voxels>
{
    public string Id => "CamposFisicos";
    public string Name => "Nodo Campos Físicos";
    
    public Voxels Run(CamposFisicosInput input)
    {
        Voxels result = new Voxels();
        
        // Agent 1: PhysicsAgent - Thermal, pressure, vibration, flow fields
        var physicsAgent = new PhysicsAgent();
        Voxels physicsFields = physicsAgent.Execute(input.Chamber + input.PreBurner + input.Manifold + input.Turbopump);
        result += physicsFields;
        
        // Agent 2: CFDProxyAgent - CFD thermal analysis
        var cfdProxy = new CFDProxyAgent();
        Voxels cfdFields = cfdProxy.Execute(input.Chamber + input.PreBurner);
        result += cfdFields;
        
        return result;
    }
    
    public Voxels Execute(CamposFisicosInput input) => Run(input);
}
