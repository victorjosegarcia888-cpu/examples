// LoadParamsNode.cs
//
// Node that loads engine parameters from JSON configuration.

using PipelineCore;
using FFSC_PicoGK.Utils;
using FFSC_PicoGK.Models;

namespace FFSC_PicoGK.Pipeline.Nodes;

public class LoadParamsNode : ITask<string, EngineParams>
{
    public string Id => "load_params";
    public string Name => "Load Engine Parameters";

    public EngineParams Execute(string configPath)
    {
        if (string.IsNullOrWhiteSpace(configPath))
            throw new PipelineException("Config path cannot be null or empty.");

        return EngineParamsLoader.Load(configPath);
    }
}
