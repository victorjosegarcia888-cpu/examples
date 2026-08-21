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
    Voxels Physics,
    Voxels FractalPostProcess);

public class Nodo_AssemblyFFSC : ITask<AssemblyFFSCInput, Voxels>
{
    public string Id => "AssemblyFFSC";
    public string Name => "Nodo Assembly FFSC";
    
    public Voxels Run(AssemblyFFSCInput input)
    {
        var assemblyAgent = new AssemblyAgent();
        Voxels parts = assemblyAgent.Execute(new Voxels[] {
            input.Chamber, input.PreBurner, input.Manifold, input.Turbopump,
            input.Cooling, input.Lattice, input.Physics, input.FractalPostProcess
        });
        
        var interfaceAgent = new InterfaceAgent();
        Voxels withInterfaces = interfaceAgent.Execute(parts);
        
        var validationAgent = new ValidationAgent();
        return validationAgent.Execute(withInterfaces);
    }
    
    public Voxels Execute(AssemblyFFSCInput input) => Run(input);
}
