// AssemblyNode.cs
//
// Node wrapper for final engine assembly combining all subsystems.

using PipelineCore;
using PicoGK;

namespace FFSC_PicoGK.Pipeline.Nodes;

public record AssemblyInput(
    Voxels Chamber,
    Voxels Nozzle,
    Voxels Aerospike,
    Voxels ManifoldFFSC,
    Voxels ManifoldLOX,
    Voxels ManifoldCH4,
    Voxels Injectors,
    Voxels Turbopump,
    Voxels Cooling,
    Voxels Pipes,
    Voxels Structural,
    Voxels Supports,
    Voxels Stress,
    Voxels CFD,
    Voxels LatticeDual,
    Voxels LatticeQuasi);

public class AssemblyNode : ITask<AssemblyInput, Voxels>
{
    public string Id => "final_assembly";
    public string Name => "Final Engine Assembly";

    public Voxels Run(AssemblyInput input)
    {
        return input.Chamber + input.Nozzle + input.Aerospike +
               input.ManifoldFFSC + input.ManifoldLOX + input.ManifoldCH4 +
               input.Injectors + input.Turbopump + input.Cooling +
               input.Pipes + input.Structural + input.Supports +
               input.LatticeDual + input.LatticeQuasi +
               input.Stress + input.CFD;
    }

    public Voxels Execute(AssemblyInput input)
    {
        return Run(input);
    }
}
