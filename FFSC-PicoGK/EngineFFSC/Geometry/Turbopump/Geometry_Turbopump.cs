// Geometry_Turbopump.cs
//
// Geometria completa de turbobomba FFSC.
// Usando PicoGK Voxels API.

using PicoGK;
using System.Numerics;

namespace FFSC_PicoGK.Geometry.Turbopump
{
    public static class Geometry_Turbopump
    {
        public static Voxels Create(
            double rotorRadius = 0.16,
            double hubRadius = 0.05,
            int bladeCount = 10,
            double bladeChord = 0.04,
            double bladeHeight = 0.06)
        {
            float RR = (float)rotorRadius;
            float HR = (float)hubRadius;
            float BH = (float)bladeHeight;

            // Cubo central (hub)
            Voxels hub = Voxels.voxSphere(new Vector3(0, 0, BH * 0.5f), HR);
            for (int i = 1; i < 4; i++)
            {
                float z = i * BH / 4.0f;
                hub += Voxels.voxSphere(new Vector3(0, 0, z), HR);
            }

            // Aspas
            Voxels blades = new Voxels();
            for (int i = 0; i < bladeCount; i++)
            {
                float ang = i * 2.0f * (float)Math.PI / bladeCount;
                float midR = (RR + HR) / 2.0f;
                float x = (float)Math.Cos(ang) * midR;
                float y = (float)Math.Sin(ang) * midR;

                // Aspa representada como esfera alargada
                blades += Voxels.voxSphere(new Vector3(x, y, BH * 0.5f), (float)bladeChord);
                blades += Voxels.voxSphere(new Vector3(x * 1.1f, y * 1.1f, BH * 0.5f), (float)(bladeChord * 0.8));
            }

            // Eje
            Voxels shaft = Voxels.voxSphere(new Vector3(0, 0, -0.15f), 0.03f);
            shaft += Voxels.voxSphere(new Vector3(0, 0, -0.05f), 0.03f);

            // Voluta espiral
            Voxels volute = Voxels.voxSphere(new Vector3(RR * 0.9f, 0, BH * 0.5f), 0.05f);
            for (int i = 1; i < 8; i++)
            {
                float ang = i * 2.0f * (float)Math.PI / 8.0f;
                float x = (float)Math.Cos(ang) * RR * 0.9f;
                float y = (float)Math.Sin(ang) * RR * 0.9f;
                volute += Voxels.voxSphere(new Vector3(x, y, BH * 0.5f), 0.04f);
            }

            // Carcasa
            Voxels housing = Voxels.voxSphere(new Vector3(0, 0, BH * 0.6f), RR * 1.2f);
            housing += Voxels.voxSphere(new Vector3(0, 0, BH * 1.0f), RR * 1.1f);

            return hub + blades + shaft + volute + housing;
        }

        public static Voxels Create(FFSC_PicoGK.Models.EngineParams p)
        {
            return Create(0.16, 0.05, 10, 0.04, 0.06);
        }
    }
}
