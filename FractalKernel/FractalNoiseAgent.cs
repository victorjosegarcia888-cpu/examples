using PicoGK;

namespace FractalKernel;

public class FractalNoiseAgent : IFractalAgent
{
    public string Name => "FractalNoiseAgent";
    
    public Voxels Process(Voxels input, object? parameters = null)
    {
        if (input is null)
            return new Voxels();
        
        Voxels result = new Voxels();
        
        float amplitude = 0.02f;
        float frequency = 2.0f;
        
        if (parameters is System.Collections.IDictionary dict)
        {
            if (dict.Contains("amplitude"))
                amplitude = System.Convert.ToSingle(dict["amplitude"]);
            if (dict.Contains("frequency"))
                frequency = System.Convert.ToSingle(dict["frequency"]);
        }
        
        return result;
    }
}
