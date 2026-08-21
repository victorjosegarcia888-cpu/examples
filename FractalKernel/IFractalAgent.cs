using PicoGK;

namespace FractalKernel;

public interface IFractalAgent
{
    string Name { get; }
    Voxels Process(Voxels input, object? parameters = null);
}
