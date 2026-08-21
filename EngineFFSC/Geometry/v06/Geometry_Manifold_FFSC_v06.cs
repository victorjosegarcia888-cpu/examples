// Geometry_Manifold_FFSC_v06.cs
//
// Geometria del manifold de recirculacion FFSC v06.
// Usando PicoGK Voxels API: voxSphere para lazo de recirculacion.
//
// Teoria:
// - Recirculacion de gases para estabilizacion de combustion
// - Dinamica de lazo de combustion escalonada
// - Re circulacion de productos calientes hacia inyectores

using PicoGK;
using System.Numerics;

namespace EngineFFSC.Geometry
{
    public static class Geometry_Manifold_FFSC_v06
    {
        public static Voxels Create(double innerRadius = 0.25, double outerRadius = 0.35, double length = 0.6)
        {
            Voxels manifold = new Voxels();

            float Ri = (float)innerRadius;
            float Ro = (float)outerRadius;
            float L = (float)length;
            float zCenter = L * 0.5f;

            // Anillo toroidal exterior (colector de recirculacion)
            int segments = 48;
            for (int i = 0; i < segments; i++)
            {
                float theta = (float)(i * 2.0 * Math.PI / segments);
                float x = Ro * (float)Math.Cos(theta);
                float y = Ro * (float)Math.Sin(theta);
                
                manifold += Voxels.voxSphere(new Vector3(x, y, zCenter), 0.03f);
            }

            // Anillo toroidal interior
            for (int i = 0; i < segments; i++)
            {
                float theta = (float)(i * 2.0 * Math.PI / segments);
                float x = Ri * (float)Math.Cos(theta);
                float y = Ri * (float)Math.Sin(theta);
                
                manifold += Voxels.voxSphere(new Vector3(x, y, zCenter), 0.025f);
            }

            // Conexiones radiales entre anillos (pasos de recirculacion)
            int connections = 12;
            for (int i = 0; i < connections; i++)
            {
                float theta = (float)(i * 2.0 * Math.PI / connections);
                float x1 = Ri * (float)Math.Cos(theta);
                float y1 = Ri * (float)Math.Sin(theta);
                float x2 = Ro * (float)Math.Cos(theta);
                float y2 = Ro * (float)Math.Sin(theta);

                for (int j = 0; j <= 4; j++)
                {
                    float t = j / 4.0f;
                    float x = x1 + (x2 - x1) * t;
                    float y = y1 + (y2 - y1) * t;
                    float r = 0.02f * (1.0f - (float)Math.Abs(t - 0.5f) * 0.5f);
                    manifold += Voxels.voxSphere(new Vector3(x, y, zCenter), r);
                }
            }

            return manifold;
        }
    }
}
