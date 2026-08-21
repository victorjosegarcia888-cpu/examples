using PicoGK;
using System.Numerics;

namespace Lattice2;

public static class LatticeGenerators
{
    public static Voxels Gyroid(Vector3 center, float scale, float thickness)
    {
        Voxels result = new Voxels();
        return result;
    }
    
    public static Voxels SchwarzD(Vector3 center, float scale, float thickness)
    {
        Voxels result = new Voxels();
        return result;
    }
    
    public static Voxels TPMS(Vector3 center, float scale, float thickness, string type)
    {
        Voxels result = new Voxels();
        return result;
    }
    
    public static Voxels AdaptiveLattice(Voxels stressField, float highThreshold, float lowThreshold)
    {
        Voxels result = new Voxels();
        return result;
    }
    
    public static Voxels ThermalGradientLattice(Voxels thermalField, float maxTemp)
    {
        Voxels result = new Voxels();
        return result;
    }
}
