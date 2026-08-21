// Geometry_Structural.cs
//
// Geometria estructural del motor FFSC.
// Usando PicoGK Voxels API.

using PicoGK;
using System.Numerics;

namespace FFSC_PicoGK.Geometry.Structural
{
    public static class Geometry_Structural
    {
        public static Voxels Create(
            double frameWidth = 0.08,
            double gimbalRadius = 0.10,
            double skirtHeight = 0.20,
            double skirtRadius = 0.40)
        {
            float fw = (float)frameWidth;
            float gr = (float)gimbalRadius;
            float sh = (float)skirtHeight;
            float sr = (float)skirtRadius;

            Voxels structural = new Voxels();

            // Thrust frame vertical
            structural += Voxels.voxSphere(new Vector3(sr * 0.4f, 0, -sh * 0.5f), fw * 0.5f);
            structural += Voxels.voxSphere(new Vector3(-sr * 0.4f, 0, -sh * 0.5f), fw * 0.5f);
            for (float z = -sh; z <= 0; z += 0.05f)
            {
                structural += Voxels.voxSphere(new Vector3(sr * 0.4f, 0, z), fw * 0.4f);
                structural += Voxels.voxSphere(new Vector3(-sr * 0.4f, 0, z), fw * 0.4f);
            }

            // Horizontal beam
            for (float x = -sr * 0.8f; x <= sr * 0.8f; x += 0.05f)
            {
                structural += Voxels.voxSphere(new Vector3(x, 0, -sh), fw * 0.3f);
            }

            // Gimbal mounts
            structural += Voxels.voxSphere(new Vector3(sr * 0.3f, 0, -sh * 0.9f), gr * 0.3f);
            structural += Voxels.voxSphere(new Vector3(-sr * 0.3f, 0, -sh * 0.9f), gr * 0.3f);

            // Skirt
            for (float z = -sh; z <= 0; z += 0.05f)
            {
                structural += Voxels.voxSphere(new Vector3(0, 0, z), sr);
            }

            return structural;
        }
    }
}
