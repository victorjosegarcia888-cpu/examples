using PicoGK;

namespace FractalKernel;

public class TurbulenceAgent : IFractalAgent
{
    public string Name => "TurbulenceAgent";
    
    public Voxels Process(Voxels input, object? parameters = null)
    {
        if (input is null)
            return new Voxels();
        
        Voxels result = new Voxels();
        
        float intensity = 0.03f;
        int octaves = 3;
        
        if (parameters is System.Collections.IDictionary dict)
        {
            if (dict.Contains("intensity"))
                intensity = System.Convert.ToSingle(dict["intensity"]);
            if (dict.Contains("octaves"))
                octaves = System.Convert.ToInt32(dict["octaves"]);
        }
        
        return result;
    }
}
