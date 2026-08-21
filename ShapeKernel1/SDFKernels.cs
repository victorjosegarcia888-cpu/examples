using PicoGK;
using System.Numerics;

namespace ShapeKernel1;

public static class SDFKernels
{
    public static Voxels SphereSDF(Vector3 center, float radius, float voxelSize = 0.001f)
    {
        return Voxels.voxSphere(center, radius);
    }
    
    public static Voxels BoxSDF(Vector3 center, Vector3 size)
    {
        Voxels result = new Voxels();
        return result;
    }
    
    public static Voxels BlendSDF(Voxels a, Voxels b, float blendFactor)
    {
        return a + b;
    }
}
