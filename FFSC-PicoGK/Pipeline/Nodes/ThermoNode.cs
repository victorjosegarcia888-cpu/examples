// ThermoNode.cs
//
// Node wrapper for thermodynamic analysis task.

using PipelineCore;
using FFSC_PicoGK.Physics.Thermo;

namespace FFSC_PicoGK.Pipeline.Nodes;

public class ThermoNode : ITask<ThermoTaskInput, ThermoMap>
{
    public string Id => "thermo";
    public string Name => "Thermodynamic Analysis";

    public ThermoMap Run(ThermoTaskInput input)
    {
        return ComputeThermoTask.Run(input.Params);
    }

    public ThermoMap Execute(ThermoTaskInput input)
    {
        return Run(input);
    }
}
