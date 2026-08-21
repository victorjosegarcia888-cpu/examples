using PipelineCore;
using PicoGK;
using FFSC_PicoGK.Agents;
using FractalKernel;

namespace FFSC_PicoGK.Nodes.Noyron;

public record FractalPostProcessInput(
    Voxels Chamber,
    Voxels PreBurner,
    Voxels Manifold,
    Voxels Turbopump,
    Voxels Cooling,
    Voxels Lattice,
    Voxels Physics,
    Voxels OrganicManifold);

public class Nodo_FractalPostProcess : ITask<FractalPostProcessInput, Voxels>
{
    public string Id => "FractalPostProcess";
    public string Name => "Nodo Fractal Post Process";
    
    public Voxels Run(FractalPostProcessInput input)
    {
        Voxels combined = input.Chamber + input.PreBurner + input.Manifold + 
                          input.Turbopump + input.Cooling + input.Lattice + input.Physics +
                          input.OrganicManifold;
        
        var fractalNoise = new FractalKernel.FractalNoiseAgent();
        Voxels noise = fractalNoise.Process(combined);
        
        var turbulence = new FractalKernel.TurbulenceAgent();
        Voxels turb = turbulence.Process(combined);
        
        var viscosity = new FractalKernel.ViscosityBlendAgent();
        Voxels visc = viscosity.Process(combined);
        
        var multiScale = new FractalKernel.MultiScaleSDFAgent();
        Voxels multi = multiScale.Process(combined);
        
        var organic = new FractalKernel.OrganicPerturbationAgent();
        Voxels org = organic.Process(combined);
        
        return combined + noise + turb + visc + multi + org;
    }
    
    public Voxels Execute(FractalPostProcessInput input) => Run(input);
}
