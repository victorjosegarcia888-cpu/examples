using PicoGK;
using FFSC_PicoGK.Models;
using FFSC_PicoGK.Utils;
using FFSC_PicoGK.Physics.Thermo;
using FFSC_PicoGK.Physics.Stress;
using FFSC_PicoGK.Physics.CFD;

namespace FFSC_PicoGK.Agents;

public class PhysicsAgent : IAgent
{
    public string Name => "PhysicsAgent";
    
    public Voxels Execute(object? context = null)
    {
        if (context is not Voxels geometry)
            return new Voxels();
        
        var p = EngineParamsLoader.Load("config/engine_params.json");
        
        Voxels stress = StressField.Dynamic(geometry, new Voxels(), new Voxels());
        Voxels thermal = CFDTask.Static(geometry);
        
        return stress + thermal;
    }
    
    public object ComputeThermo(EngineParams p)
    {
        return ComputeThermoTask.Run(p);
    }
    
    public Voxels ComputeStress(Voxels geometry)
    {
        return StressField.Dynamic(geometry, new Voxels(), new Voxels());
    }
    
    public Voxels ComputeCFD(Voxels geometry)
    {
        return CFDTask.Static(geometry);
    }
}
