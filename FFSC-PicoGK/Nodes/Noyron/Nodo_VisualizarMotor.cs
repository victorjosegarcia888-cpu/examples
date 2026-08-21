using PipelineCore;
using PicoGK;
using FFSC_PicoGK.Agents;

namespace FFSC_PicoGK.Nodes.Noyron;

public record VisualizarMotorInput(Voxels Engine);

public class Nodo_VisualizarMotor : ITask<VisualizarMotorInput, Voxels>
{
    public string Id => "VisualizarMotor";
    public string Name => "Nodo Visualizar Motor";
    
    public Voxels Run(VisualizarMotorInput input)
    {
        // Agent: VisualizationAgent - Converts EngineFFSC to PicoGK viewer format
        var visualizationAgent = new VisualizationAgent();
        return visualizationAgent.Execute(input.Engine);
    }
    
    public Voxels Execute(VisualizarMotorInput input) => Run(input);
}
