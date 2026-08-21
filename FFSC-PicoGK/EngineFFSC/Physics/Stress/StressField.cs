// StressField.cs
//
// Campo de tensiones volumetrico para el motor FFSC.

using PicoGK;
using System.Numerics;

namespace FFSC_PicoGK.Physics.Stress
{
    public static class StressField
    {
        public static Voxels Static(Voxels camara, Voxels spike, Voxels manifold)
        {
            Voxels stress = new Voxels();
            Voxels combinado = camara + spike + manifold;

            // Generate stress field as spheres at grid positions
            int count = 30;
            for (int i = 0; i < count; i++)
            {
                float x = (float)((i / 10 - 1.5) * 0.1);
                float y = (float)((i % 5 - 2) * 0.05);
                float z = (float)((i % 3 - 1) * 0.1);
                double tension = 0.5 + (i % 5) * 0.1;
                stress += Voxels.voxSphere(new Vector3(x, y, z), (float)tension * 0.01f);
            }

            return stress;
        }

        public static Voxels Dynamic(Voxels camara, Voxels spike, Voxels manifold)
        {
            Voxels stress = new Voxels();

            // Dynamic stress field with oscillation
            int count = 40;
            for (int i = 0; i < count; i++)
            {
                float x = (float)((i / 10 - 1.5) * 0.12);
                float y = (float)((i % 8 - 4) * 0.04);
                float z = (float)((i % 5 - 2) * 0.12);

                double oscilacion = Math.Sin(z * 12.0) * 0.2;
                double tension = (0.6 + oscilacion) + (i % 3) * 0.1;
                tension = Math.Clamp(tension, 0.0, 1.0);

                if (tension > 0.1)
                {
                    stress += Voxels.voxSphere(new Vector3(x, y, z), (float)tension * 0.012f);
                }
            }

            return stress;
        }
    }
}
