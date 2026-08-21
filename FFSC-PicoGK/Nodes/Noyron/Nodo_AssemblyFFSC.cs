using PipelineCore;
using PicoGK;
using FFSC_PicoGK.Agents;

namespace FFSC_PicoGK.Nodes.Noyron;

public record AssemblyFFSCInput(
    Voxels Chamber,
    Voxels PreBurner,
    Voxels Manifold,
    Voxels Turbopump,
    Voxels Cooling,
    Voxels Lattice,
    Voxels Physics);

public class Nodo_AssemblyFFSC : ITask<AssemblyFFSCInput, Voxels>
{
    public string Id => "AssemblyFFSC";
    public string Name => "Nodo Assembly FFSC";
    
    public Voxels Run(AssemblyFFSCInput input)
    {
        // Agent 1: AssemblyAgent - Assembles all pieces
        var assemblyAgent = new AssemblyAgent();
        Voxels parts = assemblyAgent.Execute(new Voxels[] {
            input.Chamber, input.PreBurner, input.Manifold, input.Turbopump,
            input.Cooling, input.Lattice, input.Physics
        });
        
        // Agent 2: InterfaceAgent - Handles interfaces between subsystems
        var interfaceAgent = new InterfaceAgent();
        Voxels withInterfaces = interfaceAgent.Execute(parts);
        
        // Agent 3: ValidationAgent - Validates final assembly
        var validationAgent = new ValidationAgent();
        return validationAgent.Execute(withInterfaces);
    }
    
    public Voxels Execute(AssemblyFFSCInput input) => Run(input);
}
