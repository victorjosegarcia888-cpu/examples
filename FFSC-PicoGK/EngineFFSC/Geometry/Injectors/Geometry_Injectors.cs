// Geometry_Injectors.cs
//
// Geometria de inyectores coaxiales FFSC.
// Usando PicoGK Voxels API.

using PicoGK;
using System.Numerics;

namespace FFSC_PicoGK.Geometry.Injectors
{
    public static class Geometry_Injectors
    {
        public static Voxels Create(
            int count = 32,
            double plateRadius = 0.24,
            double loxPostRadius = 0.008,
            double ch4AnnulusWidth = 0.004,
            double injectorLength = 0.06)
        {
            float pr = (float)plateRadius;
            float loxR = (float)loxPostRadius;
            float ch4R = loxR + (float)ch4AnnulusWidth;
            float ilen = (float)injectorLength;

            // Placa base
            Voxels plate = Voxels.voxSphere(new Vector3(0, 0, 0), pr);
            for (int i = 1; i < 3; i++)
            {
                float z = i * 0.01f;
                plate += Voxels.voxSphere(new Vector3(0, 0, z), pr);
            }

            // Inyectores distribuidos radialmente
            Voxels injectors = new Voxels();
            for (int i = 0; i < count; i++)
            {
                float ang = i * 2.0f * (float)Math.PI / count;
                float r = pr * 0.7f;
                float x = (float)Math.Cos(ang) * r;
                float y = (float)Math.Sin(ang) * r;

                // Post LOX central
                injectors += Voxels.voxSphere(new Vector3(x, y, 0.01f + ilen * 0.5f), loxR);

                // Anillo CH4 (representado como esfera exterior)
                injectors += Voxels.voxSphere(new Vector3(x, y, 0.01f + ilen * 0.5f), ch4R);
            }

            return plate + injectors;
        }

        public static Voxels Create(FFSC_PicoGK.Models.EngineParams p)
        {
            return Create(p.MixtureRatio > 3.0 ? 32 : 24, p.ChamberRadius * 0.7);
        }
    }
}
