using PipelineCore;
using PicoGK;
using FFSC_PicoGK.Agents;
using FFSC_PicoGK.Models;
using FFSC_PicoGK.Utils;

namespace FFSC_PicoGK.Nodes.Noyron;

public record LatticeAdaptativoInput(
    Voxels Chamber,
    Voxels PreBurner,
    Voxels PhysicsFields);

public class Nodo_LatticeAdaptativo : ITask<LatticeAdaptativoInput, Voxels>
{
    public string Id => "LatticeAdaptativo";
    public string Name => "Nodo Lattice Adaptativo";
    
    public Voxels Run(LatticeAdaptativoInput input)
    {
        Voxels result = new Voxels();
        
        // Agent 1: LatticeAgent - Gyroid/quasicrystal lattice based on thermal zones
        var latticeAgent = new LatticeAgent();
        Voxels latticeDual = latticeAgent.GenerateDualLayer(input.PhysicsFields, 0.6, 0.3, 0.015, 0.008);
        Voxels latticeQuasi = latticeAgent.GenerateQuasicrystal(input.PhysicsFields, 0.3, 0.5);
        result += latticeDual + latticeQuasi;
        
        // Agent 2: ThermalGradientAgent - Thermal gradient analysis
        var thermalGradientAgent = new ThermalGradientAgent();
        Voxels thermalGradient = thermalGradientAgent.Execute(input.Chamber);
        result += thermalGradient;
        
        // Validation
        var validationAgent = new ValidationAgent();
        return validationAgent.Execute(result);
    }
    
    public Voxels Execute(LatticeAdaptativoInput input) => Run(input);
}
