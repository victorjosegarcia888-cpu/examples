// ThicknessNode.cs
//
// Node wrapper for structural thickness calculation.

using PipelineCore;
using FFSC_PicoGK.Physics.Structural;

namespace FFSC_PicoGK.Pipeline.Nodes;

public class ThicknessNode : ITask<ThicknessTaskInput, ThicknessMap>
{
    public string Id => "thickness";
    public string Name => "Structural Thickness Calculation";

    public ThicknessMap Execute(ThicknessTaskInput input)
    {
        return ComputeThicknessTask.Run(input.Params, input.Thermo);
    }
}
