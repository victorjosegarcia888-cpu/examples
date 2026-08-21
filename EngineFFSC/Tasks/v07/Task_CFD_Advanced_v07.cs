// Task_CFD_Advanced_v07.cs
//
// Task para CFD avanzado v07.

using PicoGK;
using EngineFFSC.Physics;

namespace EngineFFSC.Tasks
{
    public static class Task_CFD_Advanced_v07
    {
        public static Voxels Task()
        {
            Physics_CFD_Advanced_v07.Run(50);
            return new Voxels();
        }
    }
}
