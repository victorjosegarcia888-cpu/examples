using PicoGK;

namespace FractalKernel;

public class ViscosityBlendAgent : IFractalAgent
{
    public string Name => "ViscosityBlendAgent";
    
    public Voxels Process(Voxels input, object? parameters = null)
    {
        if (input is null)
            return new Voxels();
        
        Voxels result = new Voxels();
        
        float viscosity = 0.5f;
        float blendRadius = 0.01f;
        
        if (parameters is System.Collections.IDictionary dict)
        {
            if (dict.Contains("viscosity"))
                viscosity = System.Convert.ToSingle(dict["viscosity"]);
            if (dict.Contains("blendRadius"))
                blendRadius = System.Convert.ToSingle(dict["blendRadius"]);
        }
        
        return result;
    }
}
