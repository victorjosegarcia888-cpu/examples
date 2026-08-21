// Geometry_Injectors_v07.cs
//
// Advanced coaxial injector array for FFSC engine.
// Theory: Shear coaxial design with LOX post and CH4 annular film.
// Using PicoGK Voxels API.

using PicoGK;
using System.Numerics;
using FFSC_PicoGK.Models;

namespace EngineFFSC.Geometry
{
    public static class Geometry_Injectors_v07
    {
        public static Voxels Build(
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

            Voxels geometry = new Voxels();

            // Injector plate
            Voxels plate = Voxels.voxSphere(new Vector3(0, 0, 0), pr);
            for (int i = 1; i < 3; i++)
            {
                float z = i * 0.01f;
                plate += Voxels.voxSphere(new Vector3(0, 0, z), pr);
            }
            geometry += plate;

            // Coaxial injectors
            for (int i = 0; i < count; i++)
            {
                float ang = i * 2.0f * (float)Math.PI / count;
                float r = pr * 0.7f;
                float x = (float)Math.Cos(ang) * r;
                float y = (float)Math.Sin(ang) * r;

                float zCenter = 0.01f + ilen * 0.5f;

                // LOX post
                geometry += Voxels.voxSphere(new Vector3(x, y, zCenter), loxR);

                // CH4 annulus
                geometry += Voxels.voxSphere(new Vector3(x, y, zCenter), ch4R);
            }

            return geometry;
        }

        public static Voxels Build(EngineParams p)
        {
            int count = p.MixtureRatio > 3.0 ? 32 : 24;
            return Build(count, p.ChamberRadius * 0.7);
        }
    }
}
