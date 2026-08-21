using PicoGK;
using FFSC_PicoGK.Models;
using FFSC_PicoGK.Utils;
using FFSC_PicoGK.Geometry.Chamber;
using FFSC_PicoGK.Geometry.Nozzle;
using FFSC_PicoGK.Geometry.Manifolds;
using FFSC_PicoGK.Geometry.Injectors;
using FFSC_PicoGK.Geometry.Turbopump;

namespace FFSC_PicoGK.Agents;

public enum GeometryType { Chamber, Nozzle, ManifoldFFSC, ManifoldLOX, ManifoldCH4, Injectors, Turbopump, PreBurner }

public class GeometryAgent : IAgent
{
    public string Name => "GeometryAgent";
    
    public Voxels Execute(object? context = null)
    {
        return context switch
        {
            GeometryType gt => gt switch
            {
                GeometryType.Chamber => Geometry_Chamber.Create(EngineParamsLoader.Load("config/engine_params.json")),
                GeometryType.Nozzle => Geometry_Nozzle.Create(EngineParamsLoader.Load("config/engine_params.json")),
                GeometryType.ManifoldFFSC => Geometry_Manifold_FFSC.Create(),
                GeometryType.ManifoldLOX => Geometry_Manifold_LOX.Create(),
                GeometryType.ManifoldCH4 => Geometry_Manifold_CH4.Create(),
                GeometryType.Injectors => Geometry_Injectors.Create(),
                GeometryType.Turbopump => Geometry_Turbopump.Create(),
                GeometryType.PreBurner => Geometry_Chamber.Create(EngineParamsLoader.Load("config/engine_params.json")),
                _ => new Voxels()
            },
            EngineParams p => Geometry_Chamber.Create(p),
            _ => new Voxels()
        };
    }
    
    public Voxels CreateGeometry(GeometryType type, EngineParams? p = null)
    {
        return type switch
        {
            GeometryType.Chamber => Geometry_Chamber.Create(p ?? EngineParamsLoader.Load("config/engine_params.json")),
            GeometryType.Nozzle => Geometry_Nozzle.Create(p ?? EngineParamsLoader.Load("config/engine_params.json")),
            GeometryType.PreBurner => Geometry_Chamber.Create(p ?? EngineParamsLoader.Load("config/engine_params.json")),
            GeometryType.ManifoldFFSC => Geometry_Manifold_FFSC.Create(),
            GeometryType.Turbopump => Geometry_Turbopump.Create(),
            _ => new Voxels()
        };
    }
}
