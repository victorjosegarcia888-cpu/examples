using PicoGK;

namespace FractalKernel;

public class OrganicPerturbationAgent : IFractalAgent
{
    public string Name => "OrganicPerturbationAgent";
    
    public Voxels Process(Voxels input, object? parameters = null)
    {
        if (input is null)
            return new Voxels();
        
        Voxels result = new Voxels();
        
        float perturbation = 0.015f;
        float smoothness = 0.8f;
        
        if (parameters is System.Collections.IDictionary dict)
        {
            if (dict.Contains("perturbation"))
                perturbation = System.Convert.ToSingle(dict["perturbation"]);
            if (dict.Contains("smoothness"))
                smoothness = System.Convert.ToSingle(dict["smoothness"]);
        }
        
        return result;
    }
}
