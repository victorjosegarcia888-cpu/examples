// FFSC_Pipeline_v07.cs
//
// Pipeline de ejecucion secuencial para el motor FFSC v07.

using PicoGK;
using FFSC_PicoGK.Models;
using EngineFFSC.Tasks;

namespace EngineFFSC.Pipeline
{
    public static class FFSC_Pipeline_v07
    {
        public static Voxels Execute(EngineParams p)
        {
            Task_Thermo_v07.Task();
            Task_Thickness_v07.Task();
            Task_TurbopumpDesign_Triple_v07.Task();
            Task_Geometry_Manifold_Fractal_LOX_v07.Task();
            Task_Geometry_Manifold_Fractal_CH4_v07.Task();
            Task_Geometry_Manifold_Fractal_FFSC_v07.Task();
            Task_LatticeAdaptive_Intelligent_v07.Task();
            Task_CoolingActive_v07.Task();
            Task_StressField_Dynamic_v07.Task();
            Task_CFD_Advanced_v07.Task();

            return Task_Assembly_FFSC_v07.Task();
        }
    }
}
