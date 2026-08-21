using PicoGK;

namespace FractalKernel;

public class MultiScaleSDFAgent : IFractalAgent
{
    public string Name => "MultiScaleSDFAgent";
    
    public Voxels Process(Voxels input, object? parameters = null)
    {
        if (input is null)
            return new Voxels();
        
        Voxels result = new Voxels();
        
        int scales = 4;
        float baseFrequency = 1.0f;
        
        if (parameters is System.Collections.IDictionary dict)
        {
            if (dict.Contains("scales"))
                scales = System.Convert.ToInt32(dict["scales"]);
            if (dict.Contains("baseFrequency"))
                baseFrequency = System.Convert.ToSingle(dict["baseFrequency"]);
        }
        
        return result;
    }
}
