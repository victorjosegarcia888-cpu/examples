// CoolingAnalysisNode.cs
//
// Node wrapper for regenerative cooling heat transfer analysis.

using PipelineCore;
using FFSC_PicoGK.Physics.Cooling;
using FFSC_PicoGK.Physics.Thermo;

namespace FFSC_PicoGK.Pipeline.Nodes;

public class CoolingAnalysisNode : ITask<CoolingTaskInput, CoolingMap>
{
    public string Id => "cooling_analysis";
    public string Name => "Cooling Heat Transfer Analysis";

    public CoolingMap Run(CoolingTaskInput input)
    {
        return CoolingTask.Run(input.Params, input.Thermo);
    }

    public CoolingMap Execute(CoolingTaskInput input)
    {
        return Run(input);
    }
}
