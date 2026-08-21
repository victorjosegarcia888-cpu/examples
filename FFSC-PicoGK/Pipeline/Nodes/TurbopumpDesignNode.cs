// TurbopumpDesignNode.cs
//
// Node wrapper for turbopump parametric design.

using PipelineCore;
using FFSC_PicoGK.EngineFFSC.Turbopump;

namespace FFSC_PicoGK.Pipeline.Nodes;

public class TurbopumpDesignNode : ITask<TurbopumpTaskInput, TurbopumpDesign>
{
    public string Id => "turbopump_design";
    public string Name => "Turbopump Design";

    public TurbopumpDesign Execute(TurbopumpTaskInput input)
    {
        return TurbopumpDesigner.Run(input.Params, input.MassFlowOxidizer);
    }
}
