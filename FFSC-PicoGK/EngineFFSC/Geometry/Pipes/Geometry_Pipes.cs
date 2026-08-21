// Geometry_Pipes.cs
//
// Geometria de tuberias de alta presion FFSC.
// Usando PicoGK Voxels API.

using PicoGK;
using System.Numerics;

namespace FFSC_PicoGK.Geometry.Pipes
{
    public static class Geometry_Pipes
    {
        public static Voxels Create(
            double loxRadius = 0.03,
            double ch4Radius = 0.03,
            double mainLength = 0.80,
            double branchLength = 0.60)
        {
            float lr = (float)loxRadius;
            float cr = (float)ch4Radius;
            float ml = (float)mainLength;
            float bl = (float)branchLength;

            Voxels pipes = new Voxels();

            // Tuberia principal LOX (horizontal, esferas a lo largo de X)
            for (float x = -ml / 2; x <= ml / 2; x += 0.05f)
            {
                pipes += Voxels.voxSphere(new Vector3(x, 0.30f, 0), lr);
            }

            // Derivacion superior LOX
            for (float z = 0; z <= bl; z += 0.05f)
            {
                pipes += Voxels.voxSphere(new Vector3(0.30f, 0.20f, z), lr);
            }

            // Derivacion inferior LOX
            for (float z = 0; z <= bl; z += 0.05f)
            {
                pipes += Voxels.voxSphere(new Vector3(0.30f, -0.20f, z), lr);
            }

            // Tuberia principal CH4
            for (float x = -ml / 2; x <= ml / 2; x += 0.05f)
            {
                pipes += Voxels.voxSphere(new Vector3(x, -0.30f, 0), cr);
            }

            // Derivacion superior CH4
            for (float z = 0; z <= bl; z += 0.05f)
            {
                pipes += Voxels.voxSphere(new Vector3(-0.30f, 0.20f, z), cr);
            }

            // Derivacion inferior CH4
            for (float z = 0; z <= bl; z += 0.05f)
            {
                pipes += Voxels.voxSphere(new Vector3(-0.30f, -0.20f, z), cr);
            }

            return pipes;
        }
    }
}
