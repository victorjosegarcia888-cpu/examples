// CFDTask.cs
//
// Tarea CFD simplificada para el motor FFSC.

using PicoGK;
using System.Numerics;

namespace FFSC_PicoGK.Physics.CFD
{
    public static class CFDTask
    {
        public static Voxels Static(Voxels geom)
        {
            Voxels campo = new Voxels();

            int count = 40;
            for (int i = 0; i < count; i++)
            {
                float x = (float)((i / 10 - 1.5) * 0.1);
                float y = (float)((i % 5 - 2) * 0.05);
                float z = (float)((i % 4 - 2) * 0.1);

                double dist = Math.Sqrt(x * x + y * y);
                double vel = 1.0 - dist;
                double pres = Math.Sin(z * 8.0) * 0.5 + 0.5;
                double intensidad = Math.Clamp((vel + pres) * 0.5, 0.0, 1.0);

                if (intensidad > 0.1)
                {
                    campo += Voxels.voxSphere(new Vector3(x, y, z), (float)intensidad * 0.01f);
                }
            }

            return campo;
        }

        public static Voxels Dynamic(Voxels geom)
        {
            Voxels campo = new Voxels();

            int count = 50;
            for (int i = 0; i < count; i++)
            {
                float x = (float)((i / 10 - 1.5) * 0.12);
                float y = (float)((i % 7 - 3) * 0.04);
                float z = (float)((i % 5 - 2) * 0.12);

                double dist = Math.Sqrt(x * x + y * y);
                double vel = (1.0 - dist) + Math.Sin(z * 12.0) * 0.2;
                double pres = Math.Cos(z * 9.0) * 0.3 + 0.7;
                double intensidad = Math.Clamp((vel + pres) * 0.5, 0.0, 1.0);

                if (intensidad > 0.1)
                {
                    campo += Voxels.voxSphere(new Vector3(x, y, z), (float)intensidad * 0.012f);
                }
            }

            return campo;
        }
    }
}
