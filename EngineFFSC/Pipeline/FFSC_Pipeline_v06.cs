// FFSC_Pipeline_v06.cs
//
// Pipeline de ejecucion secuencial para el motor FFSC v06.

using PicoGK;
using FFSC_PicoGK.Models;
using EngineFFSC.Tasks;
using EngineFFSC.Physics;

namespace EngineFFSC.Pipeline
{
    public static class FFSC_Pipeline_v06
    {
        public static Voxels Execute(EngineParams p)
        {
            Task_Thermo_v06.Task();
            Task_Thickness_v06.Task();
            Task_TurbopumpDesign_Dual_v06.Task();
            Task_Geometry_Manifold_LOX_v06.Task();
            Task_Geometry_Manifold_CH4_v06.Task();
            Task_Geometry_Manifold_FFSC_v06.Task();
            Task_LatticeAdaptive_v06.Task();
            Task_CoolingAdaptive_v06.Task();
            Task_StressField_v06.Task();
            Task_CFD_v06.Task();

            return Task_Assembly_FFSC_v06.Task();
        }
    }
}
