// CFDNode.cs
//
// Node wrapper for simplified CFD thermal analysis.

using PipelineCore;
using PicoGK;
using FFSC_PicoGK.Physics.CFD;

namespace FFSC_PicoGK.Pipeline.Nodes;

public class CFDNode : ITask<CFDInput, Voxels>
{
    public string Id => "physics_cfd";
    public string Name => "CFD Thermal Analysis";

    public Voxels Run(CFDInput input)
    {
        Voxels combined = input.Chamber + input.Nozzle + input.Spike + input.Manifold;
        return CFDTask.Dynamic(combined);
    }

    public Voxels Execute(CFDInput input)
    {
        return Run(input);
    }
}
