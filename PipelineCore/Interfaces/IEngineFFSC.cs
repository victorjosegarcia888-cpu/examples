// IEngineFFSC.cs
//
// Interface for FFSC engine assembly tasks.

using PicoGK;

namespace PipelineCore;

public interface IEngineFFSC
{
    string Id { get; }
    string Name { get; }
    string Version { get; }
    Voxels Assemble(object engineParams, AssemblyConfig config);
    Voxels AssembleDefault();
    AssemblyConfig GetConfig();
    bool IsSubsystemIncluded(string subsystem);
}

public class AssemblyConfig
{
    public bool IncludeChamber { get; set; } = true;
    public bool IncludeNozzle { get; set; } = true;
    public bool IncludeAerospike { get; set; } = false;
    public bool IncludeManifold { get; set; } = true;
    public bool IncludeInjectors { get; set; } = true;
    public bool IncludeTurbopump { get; set; } = true;
    public bool IncludeCooling { get; set; } = true;
    public bool IncludePipes { get; set; } = true;
    public bool IncludeStructural { get; set; } = true;
    public bool IncludeSupports { get; set; } = true;
    public bool IncludeStress { get; set; } = false;
    public bool IncludeCFD { get; set; } = false;
    public string Version { get; set; } = "v06";
}
