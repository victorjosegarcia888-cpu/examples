using PicoGK;

namespace FFSC_PicoGK.Agents;

public interface IAgent
{
    string Name { get; }
    Voxels Execute(object? context = null);
}
