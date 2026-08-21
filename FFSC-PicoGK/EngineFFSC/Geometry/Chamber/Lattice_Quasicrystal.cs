// Lattice_Quasicrystal.cs
//
// Lattice cuasicristalino basado en campo de tensiones.
// Usando PicoGK Voxels API.

using PicoGK;
using System.Numerics;

namespace FFSC_PicoGK.Geometry.Chamber
{
    public static class Lattice_Quasicrystal
    {
        public static Voxels Generate(
            Voxels stressField,
            double escala = 0.3,
            double intensidad = 0.5)
        {
            Voxels lattice = new Voxels();

            var bbox = stressField.oCalculateBoundingBox();
            var center = bbox.vecCenter();
            var size = bbox.vecSize();
            float sc = (float)escala;

            // Quasicrystal pattern: non-periodic distribution
            int count = 60;
            for (int i = 0; i < count; i++)
            {
                float x = center.X + (float)((i / 15.0 - 1.0) * size.X * 0.4);
                float y = center.Y + (float)((i % 12 - 6) * 0.04);
                float z = center.Z + (float)((i % 5 - 2) * 0.08);

                // Quasicrystal-like pattern using golden ratio
                double qx = Math.Cos(x * sc) + Math.Cos(y * sc * 1.618);
                double qy = Math.Sin(y * sc) + Math.Sin(z * sc * 1.618);
                double magnitud = Math.Abs(qx + qy);

                if (magnitud > 1.2)
                {
                    lattice += Voxels.voxSphere(new Vector3(x, y, z), 0.006f);
                }
            }

            return lattice;
        }
    }
}
