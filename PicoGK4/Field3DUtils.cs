using PicoGK;

namespace PicoGK4;

public static class Field3DUtils
{
    public static Voxels BooleanUnion(Voxels a, Voxels b)
    {
        return a + b;
    }
    
    public static Voxels BooleanSubtract(Voxels a, Voxels b)
    {
        return a - b;
    }
    
    public static Voxels BooleanIntersect(Voxels a, Voxels b)
    {
        Voxels result = new Voxels();
        return result;
    }
    
    public static Voxels SmoothBlend(Voxels a, Voxels b, float radius)
    {
        return a + b;
    }
    
    public static Voxels Field3DBlend(Voxels a, Voxels b, float blendFactor)
    {
        return a + b;
    }
}
